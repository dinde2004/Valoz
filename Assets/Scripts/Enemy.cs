using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    public AIPath aiPath;
    Vector2 direction;
    public int health = 2;
    public int damage = 1;
    public float speed = 1f;

    public Animator animator;

    public Transform attackPoint;
    public float attackRange = 0.12f;

    public float attackRate = 0.15f;
    float nextAttackTime = 0f;
    public LayerMask playerLayers;
    float nextTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        speed = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        direction = aiPath.desiredVelocity;
        if (direction.x > 0f) {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        } else if (direction.x < 0f) {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        animator.SetFloat("Speed", Mathf.Abs(direction.x) + Mathf.Abs(direction.y));

        if (nextTime != 0f && Time.time > nextTime) {
            Destroy(this.gameObject);
        }
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);
        if (hitPlayer.Length == 0) {

        } else {
            if (Time.time > nextAttackTime && PlayerStats.health != 0) Attack(hitPlayer[0]);
        }
    }
    
    void Attack(Collider2D collision) {
        animator.SetTrigger("Attack");
        collision.GetComponent<PlayerController>().decreaseHealth(damage);
        Debug.Log("Being damaged!!");
        nextAttackTime = Time.time + 2.2f;
    }

    public bool takeDamage(int damage) {
        animator.SetTrigger("Damaged");
        health -= damage;
        if (health <= 0) {
            die();
            return true;
        }
        return false;
    }
    public void die() {
        animator.SetTrigger("Dead");
        nextTime = Time.time + 1f;
    }

    void OnDrawGizmosSelected() {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
