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
    protected GameObject shooter;
    protected float horizontalInput = 0;
    protected bool runningInput = false;
    protected bool isGrounded = false;
    protected bool isRunning = false;
    public float walkSpeed = 500;
    public float runMultiplier = 2;
    public float jumpForce = 5;
    public float groundSensorLength = 1.09f;

    public LayerMask groundMask;

    protected const string STATE_IS_MOVING = "isMoving";
    protected const string STATE_IS_RUNNING = "isRunning";
    protected const string STATE_IS_GROUNDED = "isGrounded";

    protected void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        shooter = GameObject.Find("Shooter");
    }

    protected void Start()
    {
        animator.SetBool(STATE_IS_MOVING, false);
    }

    protected void Update()
    {
        animator.SetBool(STATE_IS_MOVING, IsMoving());
        animator.SetBool(STATE_IS_GROUNDED, isGrounded);
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
        float angle = this.spriteRenderer.flipX ? 180 : 0;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Instantiate(muzzlePrefab, shooter.transform.position, rotation);
        Instantiate(bulletPrefab, shooter.transform.position, rotation);
    }

    protected bool IsMoving() => this.rigidBody.velocity.x != 0;

    private void OnTriggerStay2D(Collider2D collision)
    {
        isGrounded = collision != null && (((1 << collision.gameObject.layer) & groundMask) != 0);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isGrounded = false;
    }
}
