using UnityEngine;
using System.Collections;

public class BossEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [Header("Fire Rain Settings")]
    [SerializeField] private float fireRainInterval = 6f;
    [SerializeField] private int fireballWavesCount = 3;
    [SerializeField] private float waveInterval = 1f;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;

    private float cooldownTimer = Mathf.Infinity;
    private float fireRainTimer = 0;

    // References
    private Animator anim;
    private Health playerHealth;
    private EnemyPatrol enemyPatrol;
    private bool isFireRainActive = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        fireRainTimer += Time.deltaTime;

        if (fireRainTimer >= fireRainInterval && !isFireRainActive)
        {
            StartFireRain();
            fireRainTimer = 0;
        }

        if (PlayerInSight() && !isFireRainActive)
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("meleeAttack");
            }
        }

        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = !isFireRainActive && !PlayerInSight();
        }
    }


    private void StartFireRain()
    {
        isFireRainActive = true;
        anim.SetTrigger("rangedAttack");
        FireballManager.Instance.StartFireballRain();
        
        isFireRainActive = false;
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
                                  new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
                                  0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
        {
            playerHealth = hit.transform.GetComponent<Health>();
            return true;
        }

        return false;
    }


    private void DamagePlayer()
    {
        if (PlayerInSight())
            playerHealth.TakeDamage(damage);
    }
}
