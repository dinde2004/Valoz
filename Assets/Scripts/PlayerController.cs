using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static void DrawRectange(Vector2 top_right_corner, Vector2 bottom_left_corner)
    {
        Vector2 center_offset = (top_right_corner + bottom_left_corner) * 0.5f; 
        Vector2 displacement_vector = top_right_corner - bottom_left_corner;
        float x_projection = Vector2.Dot(displacement_vector, Vector2.right);
        float y_projection = Vector2.Dot(displacement_vector, Vector2.up);

        Vector2 top_left_corner = new Vector2(-x_projection * 0.5f, y_projection * 0.5f) + center_offset;
        Vector2 bottom_right_corner = new Vector2(x_projection * 0.5f, -y_projection * 0.5f) + center_offset;

        Gizmos.DrawLine(top_right_corner, top_left_corner);
        Gizmos.DrawLine(top_left_corner, bottom_left_corner);
        Gizmos.DrawLine(bottom_left_corner, bottom_right_corner);
        Gizmos.DrawLine(bottom_right_corner, top_right_corner); 

    }

    public Animator animator;

    public Text healthText;

    public float playerFeetRadius = 0.2f;
    private float directionX = 0f;
    private float directionY = 0f;

    public Transform playerFeet;
    private Rigidbody2D playerRb;

    public Transform attackPoint;
    public float attackRange = 0.12f;
    public LayerMask enemyLayers;
    public float fireRange = 0.65f;
    public float fireWidth = 0f;
    private bool check = true;

    private int damage = 1;
    public float attackRate = 1f;
    float nextAttackTime = 0f;
    float nextTime = 0f;
    float nextWinTime = 0f;

    Vector2 movementInput;
    public ContactFilter2D movementFilter;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    public float collisionOffset = 0.001f;
    bool canMove = true;
    
    int killed = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Get reference to rigidbody component for left right movement and jumping
        playerRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        PlayerStats.refresh();
    }

    // Update is called once per frame
    void Update()
    {   
        if (killed == 14 && nextWinTime == 0f) nextWinTime = Time.time + 2f;
        if (nextWinTime != 0 && Time.time > nextWinTime) SceneManager.LoadScene(2);
        if (nextTime != 0 && Time.time > nextTime) {
            Destroy(this.gameObject);
            SceneManager.LoadScene(3);
        }

        healthText.text = "Health: " + PlayerStats.health + " / " + PlayerStats.maxHealth;
        //Get direction keypress from user
        directionX = Input.GetAxis("Horizontal");
        directionY = Input.GetAxis("Vertical");

        animator.SetFloat("Speed", Mathf.Abs(directionX) + Mathf.Abs(directionY));
        if (canMove) {
            if(movementInput != Vector2.zero){
                    
                bool success = TryMove(movementInput);

                if(!success && movementInput.x != 0) {
                    success = TryMove(new Vector2(movementInput.x, 0));
                }

                if(!success && movementInput.y != 0) {
                    success = TryMove(new Vector2(0, movementInput.y));
                }
                    
            }
        }


        if (directionX > 0) {
            if (!check)
            transform.localScale = new Vector3(-transform.localScale.x,
                transform.localScale.y, transform.localScale.z);
            check = true;
        }

        if (directionX < 0) {
            if (check)
            transform.localScale = new Vector3(-transform.localScale.x,
                transform.localScale.y, transform.localScale.z);
            check = false;
        }
        
        if (Input.GetMouseButtonDown(0)) {
            if (Time.time > nextAttackTime) {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (Time.time > nextAttackTime) {
                Fire(check ? 1 : -1);
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }

        if (Time.time > nextAttackTime) {
            canMove = true;
        }

    }

    private bool TryMove(Vector2 direction) {
        if(direction != Vector2.zero) {
            // Check for potential collisions
            int count = playerRb.Cast(
                direction, // X and Y values between -1 and 1 that represent the direction from the body to look for collisions
                movementFilter, // The settings that determine where a collision can occur on such as layers to collide with
                castCollisions, // List of collisions to store the found collisions into after the Cast is finished
                PlayerStats.speed * Time.fixedDeltaTime + collisionOffset); // The amount to cast equal to the movement plus an offset

            if(count == 0){
                playerRb.MovePosition(playerRb.position + direction * PlayerStats.speed * Time.fixedDeltaTime);
                return true;
            } else {
                return false;
            }
        } else {
            // Can't move if there's no direction to move in
            return false;
        }
        
    }
    void OnMove(InputValue movementValue) {
        movementInput = movementValue.Get<Vector2>();
    }

    void Attack() 
    {
        canMove = false;
        animator.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach(Collider2D enemy in hitEnemies) 
        {
            if (enemy.GetComponent<Enemy>().takeDamage(damage)) ++killed;
            Debug.Log("We attack " + enemy.name);
        }
    }

    void Fire(int direction) {
        canMove = false;
        animator.SetTrigger("Fire");
        Vector2 vt = new Vector2(attackPoint.position.x + direction * fireRange, attackPoint.position.y + fireWidth);
        Collider2D[] hitEnemies = Physics2D.OverlapAreaAll(new Vector2(attackPoint.position.x, attackPoint.position.y - fireWidth), vt, enemyLayers);
        foreach(Collider2D enemy in hitEnemies) 
        {
            if (enemy.GetComponent<Enemy>().takeDamage(damage)) ++killed;
            Debug.Log("We fire " + enemy.name);
        }
    }

    public void decreaseHealth() {
        animator.SetTrigger("Damaged");
        PlayerStats.decreaseHealth();
        if (PlayerStats.health <= 0) {
            animator.SetTrigger("Dead");
            nextTime = Time.time + 1f;
        }
    }

    void OnDrawGizmosSelected() {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Vector2 vt = new Vector2(attackPoint.position.x + fireRange, attackPoint.position.y + fireWidth);
        DrawRectange(new Vector2(attackPoint.position.x, attackPoint.position.y - fireWidth), vt);
    }
}
