using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character, IDamageable
{
    public Transform MeleeAttackOrigin;
    public Transform player;
    public float MeleeAttackRadius = 0.4f;
    public float MeleeDamage = 1f;

    public LayerMask playerLayer = 3;
    public bool isFlipped = false;

    public virtual void ApplyDamage(float amount)
    {
        getCurrentHealth -= amount;
        getAnimator.SetTrigger("Injured");
        if (getCurrentHealth <= 0)
        {
            Die();
        }
    }

    public void LookAtPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (transform.position.x < player.position.x && transform.rotation.y == 0) 
        {
            transform.rotation = Quaternion.Euler(0, 180f, 0);
        } 
        else if (transform.position.x > player.position.x && transform.rotation.y != 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Debug.DrawRay(transform.position, -Vector2.up * groundedLeeWay, Color.green);
        if (MeleeAttackOrigin != null)
        {
            Gizmos.DrawWireSphere(MeleeAttackOrigin.position, MeleeAttackRadius);
        }
    }

    public void HandleMeleeAttack()
    {
        Debug.Log("Zombie: Attempting Melee Attack");
        Collider2D[] overlappedColliders = Physics2D.OverlapCircleAll(MeleeAttackOrigin.position, MeleeAttackRadius, playerLayer);
        for (int i = 0; i < overlappedColliders.Length; i++)
        {
            IDamageable enemyAttributes = overlappedColliders[i].GetComponent<IDamageable>();
            if (enemyAttributes != null)
            {
                enemyAttributes.ApplyDamage(MeleeDamage);
            }
        }
    }
}
