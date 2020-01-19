using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .1f;

    public Rigidbody2D rb;
    public new Collider2D collider2D;
    public float speed;
    public float jumpForce;

        
    // Initially facing the right
    private bool facingRight = true;
    private bool isGrounded = false;

    private readonly float extra = .01f;


    private float movement;

    public int extraJumps;
    private int currentJumps = 0;

    private Vector3 m_Velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start() {
    
    }

    void FixedUpdate() {

        Vector3 targetVelocity = new Vector2(movement * speed, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing * Time.deltaTime);
        
    }

    // Update is called once per frame
    void Update() {

        // Checks if player is grounded
        isGrounded = CheckIfGrounded();

        // If grounded, reset jumps
        if (isGrounded) {
            currentJumps = extraJumps + 1;
        }
       
        movement = Input.GetAxisRaw("Horizontal");

        if ((movement < 0.0f && facingRight) || (movement > 0.0f && !facingRight)) {
            FlipPlayer();
        }

        if (Input.GetButtonDown("Jump")) {
            if (currentJumps > 0) {
                Jump();
                currentJumps --;
            }
        }
    }

    bool CheckIfGrounded() {

        RaycastHit2D raycastHit2D = Physics2D.Raycast(collider2D.bounds.center, Vector2.down, collider2D.bounds.extents.y + extra,
            LayerMask.GetMask("Ground"));
        
        bool isHit = raycastHit2D.collider != null;

        //TODO: For debugging
        if (isHit)
        {
            Debug.Log(raycastHit2D.collider.name);
        }


        //TODO: For debugging
        Color rayColor = Color.red;
        if (isHit) {
            rayColor = Color.green;
        }
        Debug.DrawRay(collider2D.bounds.center, Vector2.down * (collider2D.bounds.extents.y + extra), rayColor);

        return isHit;
    }

    void Jump() {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void FlipPlayer() {
        facingRight = !facingRight;
        transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1f, 1f, 1f));
    }
}
