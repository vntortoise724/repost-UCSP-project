using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Character, IDamageable
{
    [Header("Adjust Speed")]
    public float speed = 3f;

    [Header("Scene")]
    public GameObject URDone;

    [Header("Input")]
    public KeyCode MeleeAttackKey = KeyCode.F;
    public KeyCode rangedAttackKey = KeyCode.Mouse0;
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode runKey = KeyCode.LeftShift;
    public string xMoveAxis = "Horizontal";

    [Header("Combat")]
    public Transform MeleeAttackOrigin = null;
    public Transform rangedAttackOrigin = null;
    public GameObject projectile = null;
    public float MeleeAttackRadius = 0.6f;
    public float MeleeDamage = 2.5f;
    public float MeleeAttackDelay = 1.1f;
    public float RangedAttackDelay = 0;
    public float freezeDelay = 0;

    public LayerMask enemyLayer = 8;
    private float moveIntentionX = 0;
    private bool attempJump = false;
    private bool attempRun = false;
    private bool AttempMeleeAttack = false;
    private bool AttempRangedAttack = false;
    private float timeUntilMeleeReadied = 0;
    private float timeUntilRangedReadied = 0;
    private bool isMeleeAttacking = false;
    private bool isRangedAttacking = false;
    private bool isFrozen = false;

    private enum MovementStatus { idle, walk, run, jump, land}
    // Update is called once per frame
    void Update()
    {
        GetInput();
        HandleJump();
        HandleMeleeAttack();
        HandleRangedAttack();
        HandleAnimation();
    }

    void FixedUpdate()
    {
        HandleRun();
    }

    private void OnDrawGizmosSelected()
    {
        Debug.DrawRay(transform.position, -Vector2.up * groundedLeeWay, Color.green);
        if (MeleeAttackOrigin != null )
        {
            Gizmos.DrawWireSphere(MeleeAttackOrigin.position, MeleeAttackRadius);
        }
    }

    private void GetInput()
    {
        moveIntentionX = Input.GetAxis(xMoveAxis);
        AttempMeleeAttack = Input.GetKeyDown(MeleeAttackKey);
        AttempRangedAttack = Input.GetKeyDown(rangedAttackKey);
        attempJump = Input.GetKeyDown(jumpKey);
        attempRun = Input.GetKey(runKey);
    }

    private void HandleJump()
    {
        if (attempJump && isGrounded())
        {
            getrb2D.velocity = new Vector2(getrb2D.velocity.x, jumpForce);
        }
    }

    private void HandleRun()
    {
        if (moveIntentionX < 0 && transform.rotation.y == 0 && !isMeleeAttacking && !isRangedAttacking)
        {
            transform.rotation = Quaternion.Euler(0 ,180f ,0);
        }
        else if (moveIntentionX > 0 && transform.rotation.y != 0 && !isMeleeAttacking && !isRangedAttacking)
        {
            transform.rotation = Quaternion.Euler(0 ,0 ,0);
        }

        if (!isFrozen || !isGrounded()) 
        {
            getrb2D.velocity = new Vector2(moveIntentionX * speed, getrb2D.velocity.y); 
        }
        
        else if ( isGrounded() )
        {
            getrb2D.velocity = new Vector2(0, getrb2D.velocity.y);
        }
    }

    private void HandleMeleeAttack()
    {
        if(AttempMeleeAttack && timeUntilMeleeReadied <= 0)
        {
            Debug.Log("PLayer: Attempting Melee Attack");
            Collider2D[] overlappedColliders = Physics2D.OverlapCircleAll(MeleeAttackOrigin.position, MeleeAttackRadius, enemyLayer);
            for (int i = 0; i < overlappedColliders.Length; i++)
            {
                IDamageable enemyAttributes = overlappedColliders[i].GetComponent<IDamageable>();
                if (enemyAttributes != null)
                {
                    enemyAttributes.ApplyDamage(MeleeDamage);
                }

                timeUntilMeleeReadied = MeleeAttackDelay;
            }
        }
        else
        {
            timeUntilMeleeReadied -= Time.deltaTime;
        }
    }

    private void HandleRangedAttack()
    {
        if (AttempRangedAttack && timeUntilRangedReadied <= 0)
        {
            Debug.Log("PLayer: Attempting Ranged Attack");
            Instantiate(projectile, rangedAttackOrigin.position, rangedAttackOrigin.rotation);

            timeUntilMeleeReadied = RangedAttackDelay;
        }
        else
        {
            timeUntilRangedReadied -= Time.deltaTime;
        }
    }

    private void HandleAnimation()
    {
        MovementStatus Status;
        //getAnimator.SetBool("IsGrounded", isGrounded());

        if (Mathf.Abs(moveIntentionX) > 0.1f && isGrounded())
        {
            Status = MovementStatus.walk;
            speed = 3f;
            if (attempRun)
            {
                Status = MovementStatus.run;
                speed = 8f;
            }
        }
        else
        {
            Status = MovementStatus.idle;
        }

        if (attempJump && isGrounded() || getrb2D.velocity.y > .5f)
        {
            if (!isMeleeAttacking || !isRangedAttacking)
            {
                Status = MovementStatus.jump;
            }
        } 
        else if (!attempJump && !isGrounded() || getrb2D.velocity.y <= -.1f)
        {
            Status = MovementStatus.land;
        }

        getAnimator.SetInteger("Status", (int)Status);

        if (AttempMeleeAttack)
        {
            if (!isMeleeAttacking || !isRangedAttacking) 
            {
                StartCoroutine(MeleeAttackAnimationDelay());
                StartCoroutine(ActionFreezeDelay()); 
            }
            
        }

        if (AttempRangedAttack)
        {
            if(!isMeleeAttacking || !isRangedAttacking)
            {
                StartCoroutine(RangedAttackAnimationDelay());
            }
        }
    }

    private IEnumerator RangedAttackAnimationDelay()
    {
        getAnimator.SetTrigger("Shoot");
        isRangedAttacking = true;
        yield return new WaitForSeconds(RangedAttackDelay);
        isRangedAttacking = false;
    }

    private IEnumerator MeleeAttackAnimationDelay()
    {
        getAnimator.SetTrigger("Melee");
        isMeleeAttacking = true;
        yield return new WaitForSeconds(MeleeAttackDelay);
        isMeleeAttacking = false;
    }

    private IEnumerator ActionFreezeDelay()
    {
        isFrozen = true;
        yield return new WaitForSeconds(freezeDelay);
        isFrozen = false;
    }

    public virtual void ApplyDamage(float amount)
    {
        URDone.SetActive(false);
        getCurrentHealth -= amount;
        getAnimator.SetTrigger("Injured");
        if (getCurrentHealth <= 0)
        {
            Die();
            URDone.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
