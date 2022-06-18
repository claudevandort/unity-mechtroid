using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private BoxCollider2D boxCollider;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public Transform bulletPrefab;
    public Transform muzzlePrefab;
    private GameObject shooter;
    private float horizontalInput = 0;
    private bool isGrounded = false;
    private bool isRunning = false;
    public float walkSpeed = 500;
    public float runMultiplier= 2;
    public float jumpForce = 5;
    public float groundSensorLength = 1.09f;

    public LayerMask groundMask;

    private const string STATE_IS_MOVING = "isMoving";
    private const string STATE_IS_RUNNING = "isRunning";
    private const string STATE_IS_GROUNDED = "isGrounded";

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        shooter = GameObject.Find("Shooter");
    }

    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool(STATE_IS_MOVING, false);
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        Move();
        Shoot();
        animator.SetBool(STATE_IS_MOVING, IsMoving());
        animator.SetBool(STATE_IS_GROUNDED, isGrounded);
    }

    private void FixedUpdate()
    {

    }

    private void Move()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput != 0 && isGrounded)
        {
            if (Input.GetButton("Fire3"))
            {
                this.rigidBody.velocity = new Vector2(horizontalInput * Time.deltaTime * walkSpeed * runMultiplier, this.rigidBody.velocity.y);
                isRunning = true;
                animator.SetBool(STATE_IS_RUNNING, true);
            }
            else
            {
                this.rigidBody.velocity = new Vector2(horizontalInput * Time.deltaTime * walkSpeed, this.rigidBody.velocity.y);
                isRunning = false;
                animator.SetBool(STATE_IS_RUNNING, false);
            }
            if (horizontalInput < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (horizontalInput > 0)
            {
                spriteRenderer.flipX = false;
            }
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded && !isRunning)
        {
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void Shoot()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            float angle = this.spriteRenderer.flipX ? 180 : 0;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            Instantiate(muzzlePrefab, shooter.transform.position, rotation);
            Instantiate(bulletPrefab, shooter.transform.position, rotation);
        }
    }

    bool IsMoving() => this.rigidBody.velocity.x != 0;

    private void OnTriggerStay2D(Collider2D collision)
    {
        isGrounded = collision != null && (((1 << collision.gameObject.layer) & groundMask) != 0);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isGrounded = false;
    }
}
