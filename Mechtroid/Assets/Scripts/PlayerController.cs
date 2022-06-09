using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float horizontalInput;
    public float walkSpeed = 2;

    private const string STATE_IS_MOVING = "isMoving";

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool(STATE_IS_MOVING, false);
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool(STATE_IS_MOVING, IsMoving());
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput != 0)
        {
            this.rigidBody.velocity = new Vector2(horizontalInput * walkSpeed, 0);
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

    bool IsMoving() => this.rigidBody.velocity.x != 0;
}
