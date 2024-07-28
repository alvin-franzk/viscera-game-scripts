using System.Collections;
using UnityEngine;

public class PlayerMovementStateMachine : PlayerStateMachine
{
    [HideInInspector] public Animator animator;
    [HideInInspector] public PlayerLookDirection playerLookDirection;
    [HideInInspector] public PlayerStatus playerStatus;
    [HideInInspector] public GameObject enemy;
    [HideInInspector] public EnemyMovementStateMachine _emsm;

    [HideInInspector] public float[] slashCooldown = new float[3];

    public bool canDoAction = true;

    [Header("Settings")]
    public float speed;
    public float sprintSpeed;
    public float sprintCost;

    [Header("Block Settings")]
    public float blockReduction;
    public bool isBlocking = false;
    
    [Header("Attack Settings")]
    public float knockbackStrength;
    public float maxAttackDamage;
    public float minAttackDamage;
    public float attackCost;
    public bool isAttacking = false;
    public float critModifier;
    [Range(0f, 1f)] public float critChance;

    [Header("Dodge Settings")]
    public float dodgeCooldown;
    public float dodgeInvDuration;
    public float dodgeCost;
    public bool canDodge = true;

    [Header("Audio Clips")]
    public AudioClip deathSound;
    public AudioClip[] gruntSound;
    public AudioClip[] hurtSound;
    public AudioClip[] runSound;
    public AudioClip[] sprintSound;
    public AudioClip[] attackSound;
    public AudioClip[] dodgeSound;
    public AudioClip[] landSound;

    [HideInInspector] public Idle idleState;
    [HideInInspector] public Moving movingState;
    [HideInInspector] public Dodging dodgingState;
    [HideInInspector] public Blocking blockingState;
    [HideInInspector] public Sprinting sprintingState;
    [HideInInspector] public Dying dyingState;
    [HideInInspector] public Hurt hurtState;

    [HideInInspector] public Attacking attackingState; // Parent class for combo system
    /*
    [HideInInspector] public Attack01 attack01State;
    [HideInInspector] public Attack02 attack02State;
    [HideInInspector] public Attack03 attack03State;
    */

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerLookDirection = GetComponent<PlayerLookDirection>();
        playerStatus = GetComponent<PlayerStatus>();
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        _emsm = enemy.GetComponent<EnemyMovementStateMachine>();
       
        #region Movement States
        idleState = new Idle(this);
        movingState = new Moving(this);
        dodgingState = new Dodging(this);
        blockingState = new Blocking(this);
        sprintingState = new Sprinting(this);
        dyingState = new Dying(this);
        hurtState = new Hurt(this);
        #endregion

        #region Attack States
        attackingState = new Attacking(this);
        /*
        attack01State = new Attack01(this);
        attack02State = new Attack02(this);
        attack03State = new Attack03(this);
        */
        #endregion

        #region Initializing Variables
        speed = 2f;
        sprintSpeed = 2.5f;
        sprintCost = -10f;
        blockReduction = 0.5f;
        knockbackStrength = 20f;
        maxAttackDamage = -7f;
        minAttackDamage = -3f; 
        attackCost = -3f;
        dodgeCooldown = 1.25f;
        dodgeInvDuration = 0.75f;
        dodgeCost = -10f;

        critModifier = 1.5f;
        critChance = 0.1f;
        #endregion

        UpdateAnimClipTimes();
    }

    protected override PlayerBaseState GetInitialState()
    {
        return idleState;
    }

    public IEnumerator ResetDodgeCooldown()
    {
        while (!canDodge)
        {
            // Debug.Log("Dodge Cooldown Initiated");
            yield return new WaitForSeconds(dodgeCooldown);
            canDodge = true;
        }
    }
    public IEnumerator ResetDodgeInvulnerability()
    {
        while (playerStatus.isInvulnerable)
        {
            // Debug.Log("Dodge Cooldown Initiated");
            yield return new WaitForSeconds(dodgeInvDuration);
            playerStatus.isInvulnerable = false;
        }
    }

    public IEnumerator ResetCanDoAction()
    {
        while (!canDoAction)
        {
            // Debug.Log("Dodge Cooldown Initiated");
            yield return new WaitForSeconds(0.7f);
            canDoAction = true;
            playerLookDirection.canLook = true;
            ChangeState(idleState);
        }
    }

    public IEnumerator ResetStaminaRegen()
    {
        while (playerStatus.currentStamina == 0)
        {
            yield return new WaitForSeconds(2f);
            playerStatus.ResetStaminaRegen();
        }
    }

    public void UpdateAnimClipTimes() // NOTE: This method is used to find length of animation clip; put in bottom and IGNORE
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                case "Slash 1":
                    slashCooldown[0] = clip.length;
                    break;
                case "Slash 2":
                    slashCooldown[1] = clip.length;
                    break;
                case "Slash 3":
                    slashCooldown[2] = clip.length;
                    break;
            }
        }
    }

    public bool CritRoll()
    {
        var roll = Random.Range(1f, 101f);
        roll = Mathf.RoundToInt(roll);

        for (int i = 0; i < (critChance * 100f); i++)
        {
            if (roll == i)
            {
                return true;
            }
        }
        return false;
    }

    public float attackRoll()
    {
        var attack = Random.Range((int)minAttackDamage, (int)maxAttackDamage + 1);
        return attack;
    }
}
