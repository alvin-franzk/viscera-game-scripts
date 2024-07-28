using System.Collections;
using UnityEngine;

public class NewPlayerController : MonoBehaviour
{
    private Animator animator;

    [Header ("Movement Settings")]
    private static float normalSpeed = 5f;
    [SerializeField] private float speed = 5f;
    // [SerializeField] private float sprintSpeed = 8.5f;
    [SerializeField] private LayerMask aimLayerMask;
    [SerializeField] private bool canDoAction = true; // prevents moving during attack & attacking during move
    // NOTE (FUTURE FEATURE): use this for rooted debuffs [SerializeField] private bool canMove = true;

    [Header("Dodge Settings")]
    [SerializeField] private float dodgeCooldown = 2f;
    [SerializeField] private bool canDodge = true;

    [Header("Attack Settings")]
    // NOTE (FUTURE FEATURE): use this for disarm debuffs [SerializeField] private bool canAttack = true;
    [SerializeField] private float attackCooldown;
    private float[] slashCooldown = new float[3];

    [Header("Block Settings")]
    [SerializeField] private float blockCooldown = 2f;
    [SerializeField] private float blockWalkSpeed = 1f;
    [SerializeField] private bool canBlock = true;

    void Awake() => animator = GetComponent<Animator>();

    void Start()
    {
        UpdateAnimClipTimes();
    }
    // Update is called once per frame
    void Update()
    {
        AimTowardMouse();
        HandleMovement();
        HandleAttack();
        HandleDodge();
        HandleBlock();
    }

    void AimTowardMouse()
    {
        if (canDoAction)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
            {
                var direction = hitInfo.point - transform.position;
                direction.y = 0f;
                direction.Normalize();
                transform.forward = direction;
            }
        }        
    }

    void HandleMovement()
    {
        if (canDoAction)
        {
            // read input
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 movement = new(horizontal, 0f, vertical);

            /*// sprint state
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = sprintSpeed;
                animator.SetBool("isSprinting", true);
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed = normalSpeed;
                animator.SetBool("isSprinting", false);
            }*/

            // moving
            if (movement.magnitude > 0)
            {
                movement.Normalize();
                movement *= speed * Time.deltaTime;
                transform.Translate(movement, Space.World);
            }

            // animating
            float velocityZ = Vector3.Dot(movement.normalized, transform.forward);
            float velocityX = Vector3.Dot(movement.normalized, transform.right);

            animator.SetFloat("VelocityZ", velocityZ, 0.1f, Time.deltaTime);
            animator.SetFloat("VelocityX", velocityX, 0.1f, Time.deltaTime);
        }
        
    }

    void HandleAttack() // IN-PROGRESS: Combo System
    {
        // melee attack
        if (Input.GetMouseButtonDown(0) && canDoAction)
        {
            //TODO: create simple 1-2-3 combo
            canDoAction = false;
            canDodge = false;
            animator.SetTrigger("TriggerMelee");
            attackCooldown = slashCooldown[0];
            StartCoroutine(ResetAttackCooldown(attackCooldown));
        }
    }

    void HandleDodge()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canDodge)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 movement = new(horizontal, 0f, vertical);

            // moving
            if (movement.magnitude > 0)
            {
                canDodge = false;
                animator.SetTrigger("TriggerDodge");
                // TODO (FUTURE ME PROBLEM): dodge action cancels out attack animation
                // TODO (FUTURE ME PROBLEM): add more force / movement to roll

                // TODO: apply invulnerability during dodge animation



                StartCoroutine(ResetDodgeCooldown());
            }                
        }
    }

    void HandleBlock()
    {
        // read input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new(horizontal, 0f, vertical);

        // moving
        if (canBlock && movement.magnitude == 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                canBlock = false;

                if (Input.GetKey(KeyCode.Space))
                {
                    canDodge = false;
                    speed = blockWalkSpeed;
                    animator.SetBool("isBlocking", true);
                }
            }
        }
        else if (!canBlock)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                speed = normalSpeed;
                animator.SetBool("isBlocking", false);
                StartCoroutine(ResetBlockCooldown());
                StartCoroutine(ResetDodgeCooldown());
            }
        }
    }

    private IEnumerator ResetDodgeCooldown()
    {
        while (!canDodge)
        {
            // TODO: Dynamic cooldown base on attack animation length (dodge action is disabled during attack)
            yield return new WaitForSeconds(dodgeCooldown);
            canDodge = true;
        }

    }

    private IEnumerator ResetAttackCooldown(float attackCooldown)
    {
        while (!canDoAction)
        {
            yield return new WaitForSeconds(attackCooldown);
            canDoAction = true;
            canDodge = true;
        }
    }

    private IEnumerator ResetBlockCooldown()
    {
        while (!canBlock)
        {
            yield return new WaitForSeconds(blockCooldown);
            canBlock = true;
        }
    }
    void UpdateAnimClipTimes() // NOTE: This method is used to find length of animation clip; put in bottom and IGNORE
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
}
