using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementStateMachine : EnemyStateMachine
{
    [HideInInspector] public Animator animator;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public GameObject player;
    [HideInInspector] public EnemyStatus enemyStatus;
    [HideInInspector] public PlayerMovementStateMachine _pmsm;
    [HideInInspector] public EnemyAttackCollider attackCollider;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public EnemySpawner enemySpawner;

    [Header("Movement Settings")]
    public LayerMask whatIsGround, whatIsPlayer;
    public float attackRange, sightRange;
    public bool justSpawned = true;

    [Header("Trigger Settings")]
    public bool canSeePlayer;
    public bool isNearPlayer;
    public bool isHurt;

    [Header("Attack Settings")]
    public float attackCooldown;
    public float attackDamage;
    public float lungeStrength;
    public bool isAttacking = false;

    [Header("Audio Clips")]
    public AudioClip[] hurtSound;
    public AudioClip[] hurtBladeSound;
    public AudioClip[] deathSound;
    public AudioClip[] runSound;
    public AudioClip[] attackSound;
    public AudioClip[] alertSound;

    private bool velocityReset = false;
    [HideInInspector] public int accumulatedScore;

    [HideInInspector] public EnemyChasing chasingState;
    [HideInInspector] public EnemyPatrolling patrollingState;
    [HideInInspector] public EnemyAttacking attackingState;
    [HideInInspector] public EnemyDying dyingState;
    [HideInInspector] public EnemyHurt hurtState;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        enemyStatus = GetComponent<EnemyStatus>();
        player = GameObject.FindGameObjectWithTag("Player");
        _pmsm = player.GetComponent<PlayerMovementStateMachine>();
        attackCollider = GetComponent<EnemyAttackCollider>();
        rb = GetComponent<Rigidbody>();
        whatIsGround = LayerMask.GetMask("Terrain");
        whatIsPlayer = LayerMask.GetMask("Player");
        enemySpawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<EnemySpawner>();

        #region Initialized Variables
        attackRange = 2f;
        sightRange = Random.Range(4, 12);
        attackCooldown = Random.Range(1.5f, 2.5f);
        attackDamage = Random.Range(-20, -10);
        lungeStrength = Random.Range(25, 40);
        agent.speed = Random.Range(1f, 1.5f);
        agent.angularSpeed = Random.Range(180, 220);

        accumulatedScore = 0;
        #endregion

        #region Enemy Movement States
        chasingState = new EnemyChasing(this);
        patrollingState = new EnemyPatrolling(this);
        attackingState = new EnemyAttacking(this);
        dyingState = new EnemyDying(this);
        hurtState = new EnemyHurt(this);
        #endregion

        // Spats out from Spawner
        UpgradeEnemyStats(enemySpawner.tier - 1);
        StartCoroutine(ResetVelocityOnce());
    }

    protected override EnemyBaseState GetInitialState()
    {
        return patrollingState;
    }

    private void OnDestroy()
    {
        _pmsm.playerStatus.enemiesKilled++;
        _pmsm.playerStatus.score += accumulatedScore;
        _pmsm.playerStatus.currentEnemyCount--;
    }

    public IEnumerator WaitBeforeDying()
    {
        while (enemyStatus.isDead)
        {
            rb.detectCollisions = false;
            yield return new WaitForSeconds(2f); // TODO: Replace duration with fade vfx duration
            // TODO: Play VFX Fade
            GetComponent<LootBag>().InstantiateLoot(transform.position);
            GameObject.Destroy(gameObject);
        }
    }

    public IEnumerator ResetStunDuration()
    {
        while (isHurt)
        {
            animator.SetTrigger("TriggerHurt");
            yield return new WaitForSeconds(0.3f);
            isHurt = false;
            rb.velocity = Vector3.zero;
        }
    }

    private IEnumerator ResetVelocityOnce()
    {
        if (!velocityReset)
        {
            yield return new WaitForSeconds(1.5f);
            rb.velocity = Vector3.zero;
            velocityReset = true;
        }
    }

    public void UpgradeEnemyStats(int tier)
    {
        switch (tier)
        {
            case 0:
                enemyStatus.maxHealth += Random.Range(10, 21);
                sightRange += 2.0f;
                attackDamage -= Random.Range(-3, 0);
                attackCooldown -= 0.2f;
                agent.speed += Random.Range(0.2f, 0.5f);
                agent.angularSpeed += Random.Range(10, 16);
                break;
            case 1:
                enemyStatus.maxHealth += Random.Range(20, 41);
                sightRange += 2.5f;
                attackDamage -= Random.Range(-6, -3);
                attackCooldown -= 0.3f;
                agent.speed += Random.Range(0.5f, 1.0f);
                agent.angularSpeed += Random.Range(15, 21);
                break;
            case 2:
                enemyStatus.maxHealth += Random.Range(40, 81);
                sightRange += 3.0f;
                attackDamage -= Random.Range(-11, -8);
                attackCooldown -= 0.4f;
                agent.speed += Random.Range(1.0f, 1.5f);
                agent.angularSpeed += Random.Range(20, 31);
                break;
            case 3:
                enemyStatus.maxHealth += Random.Range(80, 161);
                sightRange += 3.5f;
                attackDamage -= Random.Range(-18, -15);
                attackCooldown -= 0.5f;
                agent.speed += Random.Range(1.5f, 2.0f);
                agent.angularSpeed += Random.Range(30, 41);
                break;
            case 4:
                enemyStatus.maxHealth += Random.Range(160, 321);
                attackDamage -= Random.Range(-27, -24);
                agent.speed += Random.Range(2.0f, 4.0f);
                agent.angularSpeed += Random.Range(40, 61);
                break;
        }
    }
}
