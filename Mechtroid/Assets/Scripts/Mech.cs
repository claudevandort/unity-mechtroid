using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mech : MonoBehaviour
{
    protected Rigidbody2D rigidBody;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    public Transform bulletPrefab;
    public Transform muzzlePrefab;
    public Transform groundExplosionPrefab;
    protected float horizontalInput = 0;
    protected bool runningInput = false;
    protected bool isGrounded = false;
    protected bool isRunning = false;
    public float walkSpeed = 500;
    public float runMultiplier = 2;
    public float jumpForce = 5;

    public LayerMask groundMask;

    protected const string STATE_IS_MOVING = "isMoving";
    protected const string STATE_IS_RUNNING = "isRunning";
    protected const string STATE_IS_GROUNDED = "isGrounded";
    protected const string STATE_IS_ALIVE = "isAlive";

    public GameObject light2d;

    protected int minHealth = 0;
    protected int maxHealth = 100;
    protected int _currentHealth;
    public HealthBar healthBar;
    [SerializeField] protected int currentHealth
    {
        get { return _currentHealth; }
        set { _currentHealth = Mathf.Clamp(value, minHealth, maxHealth); }
    }

    protected void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        animator.SetBool(STATE_IS_MOVING, false);
        animator.SetBool(STATE_IS_ALIVE, true);
    }

    protected void Update()
    {
        animator.SetBool(STATE_IS_MOVING, IsMoving());
        animator.SetBool(STATE_IS_GROUNDED, isGrounded);
        animator.SetBool(STATE_IS_ALIVE, IsAlive());
    }

    protected void Move(float horizontalInput, bool isRunning)
    {
        if (horizontalInput != 0 && isGrounded)
        {
            float horizontalVelocity = horizontalInput * walkSpeed * Time.deltaTime;
            if (isRunning)
                horizontalVelocity *= runMultiplier;
            rigidBody.velocity = new Vector2(horizontalVelocity, rigidBody.velocity.y);
            this.isRunning = isRunning;
            animator.SetBool(STATE_IS_RUNNING, isRunning);
            spriteRenderer.flipX = horizontalInput < 0;
            if (light2d != null)
            {
                float angle = horizontalInput < 0 ? 90 : -90;
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                light2d.transform.rotation = rotation;
            }
        }
    }

    public void Walk(float horizontalInput)
    {
        Move(horizontalInput, false);
    }

    public void Run(float horizontalInput)
    {
        Move(horizontalInput, true);
    }

    public void Jump()
    {
        if(isGrounded && !isRunning)
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public void Shoot()
    {
        float angle;
        Vector3 shootingOffset, shootingPosition;
        if (spriteRenderer.flipX)
        {
            angle = 180;
            shootingOffset = new Vector3(-spriteRenderer.size.x / 2, 0.2f, 0);
        }
        else
        {
            angle = 0;
            shootingOffset = new Vector3(spriteRenderer.size.x / 2, 0.2f, 0);
        }
        shootingPosition = transform.position + shootingOffset;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Instantiate(muzzlePrefab, shootingPosition, rotation);
        Instantiate(bulletPrefab, shootingPosition, rotation);
    }

    protected bool IsMoving() => rigidBody.velocity.x != 0;

    protected bool IsAlive() => currentHealth > 0;

    protected void OnTriggerStay2D(Collider2D collision)
    {
        isGrounded = collision != null && (((1 << collision.gameObject.layer) & groundMask) != 0);
    }

    protected  void OnTriggerExit2D(Collider2D collision)
    {
        isGrounded = false;
    }

    public void TakeDamage(int damageDealt)
    {
        currentHealth -= damageDealt;
        healthBar.SetHealth(currentHealth);
    }
}
