using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    /*
     * Modified and based off the series of tutorials by bendux on YouTube!
     */
    private float _horizontal;
    [SerializeField] private float Speed = 8f;
    [SerializeField] private float JumpingPower = 16f;
    private bool _facingRight = true;

    private bool _wallSliding;
    [SerializeField] private float WallSlidingSpeed = 2f;

    private bool _wallJumping;
    private float _wallJumpDirection;
    [SerializeField] private float WallJumpingTime = 0.2f;
    private float _wallJumpCounter;
    [SerializeField] private float WallJumpDuration = 0.4f;
    [SerializeField] private Vector2 WallJumpPower = new Vector2(8f, 16f);
    
    private bool _jumping = false;
    [SerializeField] private float JumpCooldown = 0.4f;
    
    [SerializeField] private float CoyoteTime = 0.2f;
    [SerializeField] private float CoyoteTimeCounter;

    [SerializeField] private float JumpBufferTime = 0.2f;
    [SerializeField] private float JumpBufferCounter;
    
    private bool _dash = true;
    private bool _dashing;
    [SerializeField] private float DashPower = 24f;
    [SerializeField] private float DashTime = 0.2f;
    [SerializeField] private float DashCooldown = 1f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private TrailRenderer tr;


    private void Update()
    {
        if (_dashing)
        {
            return;
        }
        
        
        _horizontal = Input.GetAxisRaw("Horizontal");
        
        if (IsGrounded())
        {
            CoyoteTimeCounter = CoyoteTime;
        }
        else
        {
            CoyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            JumpBufferCounter = JumpBufferTime;
        }
        else
        {
            JumpBufferCounter -= Time.deltaTime;
        }
        
        if (CoyoteTimeCounter > 0f && JumpBufferCounter > 0f && !_jumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpingPower);

            JumpBufferCounter = 0f;
            StartCoroutine(Cooldown(JumpCooldown));

        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && _dash)
        {
            StartCoroutine(Dash());
        }

        WallSlide();
        WallJump();

        if (!_wallJumping)
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        
        if (_dashing)
        {
            return;
        }
        
        if (!_wallJumping)
        {
            rb.velocity = new Vector2(_horizontal * Speed, rb.velocity.y);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && _horizontal != 0f)
        {
            _wallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -WallSlidingSpeed, float.MaxValue));
        }
        else
        {
            _wallSliding = false;
        }
    }

    private void WallJump()
    {
        if (_wallSliding)
        {
            _wallJumping = false;
            _wallJumpDirection = -transform.localScale.x;
            _wallJumpCounter = WallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            _wallJumpCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && _wallJumpCounter > 0f)
        {
            _wallJumping = true;
            rb.velocity = new Vector2(_wallJumpDirection * WallJumpPower.x, WallJumpPower.y);
            _wallJumpCounter = 0f;

            if (transform.localScale.x != _wallJumpDirection)
            {
                _facingRight = !_facingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), WallJumpDuration);
        }
    }

    private void StopWallJumping()
    {
        _wallJumping = false;
    }
    
    private IEnumerator Dash()
    {
        _dash = false;
        _dashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * DashPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(DashTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        _dashing = false;
        yield return new WaitForSeconds(DashCooldown);
        _dash = true;
    }

    private void Flip()
    {
        if (_facingRight && _horizontal < 0f || !_facingRight && _horizontal > 0f)
        {
            _facingRight = !_facingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    
    private IEnumerator Cooldown(float jumpCooldown)
    {
        _jumping = true;
        yield return new WaitForSeconds(jumpCooldown);
        _jumping = false;
    }
    
}