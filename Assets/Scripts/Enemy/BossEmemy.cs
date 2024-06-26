using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    [Header("Melee Attack Parameters")]
    [SerializeField] private float meleeAttackCooldown;
    [SerializeField] private float meleeRange;
    [SerializeField] private int meleeDamage;

    [Header("Ranged Attack Parameters")]
    [SerializeField] private float rangedAttackCooldown;
    [SerializeField] private float rangedRange;
    [SerializeField] private int rangedDamage;

    [Header("Ranged Attack")]
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject[] fireballs;

    [Header("Collider Parameters")]
    [SerializeField] private float meleeColliderDistance;
    [SerializeField] private float rangedColliderDistance;
    [SerializeField] private BoxCollider2D meleeCollider;
    [SerializeField] private BoxCollider2D rangedCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float meleeCooldownTimer = Mathf.Infinity;
    private float rangedCooldownTimer = Mathf.Infinity;

    [Header("Fireball Sound")]
    [SerializeField] private AudioClip fireballSound;

    // References
    private Animator anim;
    private EnemyPatrol enemyPatrol;
    private Health playerHealth;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void Update()
    {
        meleeCooldownTimer += Time.deltaTime;
        rangedCooldownTimer += Time.deltaTime;

        bool isPlayerInMeleeRange = PlayerInMeleeRange();
        bool isPlayerInRangedRange = PlayerInRangedRange();

        if (isPlayerInMeleeRange)
        {
            if (meleeCooldownTimer >= meleeAttackCooldown)
            {
                meleeCooldownTimer = 0;
                anim.SetTrigger("meleeAttack");
            }
        }
        else if (isPlayerInRangedRange)
        {
            if (rangedCooldownTimer >= rangedAttackCooldown)
            {
                rangedCooldownTimer = 0;
                anim.SetTrigger("rangedAttack");
            }
        }

        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = !(isPlayerInMeleeRange || isPlayerInRangedRange);
        }
    }

    private bool PlayerInMeleeRange()
    {
        RaycastHit2D hit = Physics2D.BoxCast(meleeCollider.bounds.center + transform.right * meleeRange * transform.localScale.x * meleeColliderDistance,
                                  new Vector3(meleeCollider.bounds.size.x * meleeRange, meleeCollider.bounds.size.y, meleeCollider.bounds.size.z),
                                  0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
        {
            playerHealth = hit.transform.GetComponent<Health>();
            return true;
        }

        return false;
    }

    private bool PlayerInRangedRange()
    {
        RaycastHit2D hit = Physics2D.BoxCast(rangedCollider.bounds.center + transform.right * rangedRange * transform.localScale.x * rangedColliderDistance,
                                  new Vector3(rangedCollider.bounds.size.x * rangedRange, rangedCollider.bounds.size.y, rangedCollider.bounds.size.z),
                                  0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
        {
            playerHealth = hit.transform.GetComponent<Health>();
            return true;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        if (meleeCollider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(meleeCollider.bounds.center + transform.right * meleeRange * transform.localScale.x * meleeColliderDistance,
                new Vector3(meleeCollider.bounds.size.x * meleeRange, meleeCollider.bounds.size.y, meleeCollider.bounds.size.z));
        }

        if (rangedCollider != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(rangedCollider.bounds.center + transform.right * rangedRange * transform.localScale.x * rangedColliderDistance,
                new Vector3(rangedCollider.bounds.size.x * rangedRange, rangedCollider.bounds.size.y, rangedCollider.bounds.size.z));
        }
    }

    private void DamagePlayer()
    {
        if (PlayerInMeleeRange() && playerHealth != null)
            playerHealth.TakeDamage(meleeDamage);
    }

    private void RangedAttack()
    {
        SoundManager.instance.PlaySound(fireballSound);

        int fireballIndex = FindFireball();
        if (fireballs[fireballIndex] != null)
        {
            fireballs[fireballIndex].transform.position = firepoint.position;
            fireballs[fireballIndex].GetComponent<EnemyProjectile>().ActivateProjectile();
        }
        else
        {
            Debug.LogError("Fireball не найден или не активен");
        }
    }

    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
}
