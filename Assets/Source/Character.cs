using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Attributes")]
    public float healthPool;

    [Header("Movement")]
    //public float speed = 3f;
    public float jumpForce = 3f;
    public float groundedLeeWay = 0.1f;
    public LayerMask groundLayer;

    private Rigidbody2D rb2D = null;
    private Animator animator = null;
    private float currentHealth = 1f;

    public Rigidbody2D getrb2D
    {
        get { return rb2D; }
        protected set { rb2D = value; }
    }

    public float getCurrentHealth
    {
        get { return currentHealth; }
        protected set { currentHealth = value; }   
    }

    public Animator getAnimator
    {
        get { return animator; }
        protected set { animator = value; }
    }

    // Start (Awake) is called before the first frame update
    void Awake()
    {
        if (GetComponent<Rigidbody2D>())
        {
            rb2D = GetComponent<Rigidbody2D>();
        }
        
        if (GetComponent<Animator>())
        {
            animator = GetComponent<Animator>();
        }

        currentHealth = healthPool;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*protected bool isGrounded()
    {
        return Physics2D.OverlapArea(new Vector2(transform.position.x - 0.5f, transform.position.y - 0.5f), new Vector2(transform.position.x + 0.5f, transform.position.y - 0.51f), groundLayer);
    }*/

    protected bool isGrounded()
    {
        return Physics2D.Raycast(transform.position, -Vector2.up, groundedLeeWay);
    }

    protected virtual void Die()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
