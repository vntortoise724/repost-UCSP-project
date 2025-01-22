using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyAction : StateMachineBehaviour
{
    [Header ("Adjust")]
    public float speed = 5f;
    public float attackRange = 3f;
    public float detectRange = 5f;

    Transform player;
    Rigidbody2D rb;
    Enemy zombie;

    // Start is called before the first frame update
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        zombie = animator.GetComponent<Enemy>();
    }

    // Update is called once per frame
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        zombie.LookAtPlayer();
        if (Vector2.Distance(player.position, rb.position) <= detectRange)
        {
            Vector2 target = new Vector2(player.position.x, rb.position.y);
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
        }
        
        if (Vector2.Distance(player.position, rb.position) <= attackRange) 
        {
            animator.SetTrigger("Attack");
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }
}
