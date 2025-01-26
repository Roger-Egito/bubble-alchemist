using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovementStats : MonoBehaviour
{

}

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance { get; private set; }

    [Header("References")]
    [SerializeField] private Collider2D feetCollision;
    [SerializeField] private Collider2D bodyCollision;
    [SerializeField] private Rigidbody2D rigidBody;

    [Header("Walk")]
    [Range(001f, 100f)][SerializeField] private float maxWalkSpeed = 12.5f;
    [Range(0.25f, 50f)][SerializeField] private float groundAcceleration = 12.5f;
    [Range(0.25f, 50f)][SerializeField] private float groundDeceleration = 12.5f;
    [Range(0.25f, 50f)][SerializeField] private float airAcceleration = 12.5f;
    [Range(0.25f, 50f)][SerializeField] private float airDeceleration = 12.5f;

    [Header("Run")]
    [Range(1f, 100f)][SerializeField] private float maxRunSpeed = 20f;

    [Header("Grounded/Collision Checks")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask ingredientLayer;
    [SerializeField] private float groundDetectionRayLength = 0.02f;
    [SerializeField] private float headDetectionRayLength = 0.02f;
    [SerializeField] private bool showIsGroundedBox = true;
    [SerializeField] private bool showHeadBumpBox = true;
    [Range(0f, 1f)][SerializeField] private float headWidth = 0.75f;

    [Header("Jump")]
    [SerializeField] private float jumpHeight = 6.5f;
    [Range(1f, 1.1f)][SerializeField] private float jumpHeighCompensationFactor = 1.054f;
    [SerializeField] private float timeTillJumpApex = 0.35f;
    [Range(0.01f, 5f)][SerializeField] private float gravityOnReleaseMultiplier = 2f;
    [SerializeField] private float maxFallSpeed = 26f;
    [Range(1, 5)][SerializeField] private int numberOfJumpsAllowed = 2;

    [Header("Jump Cut")]
    [Range(0.02f, 0.3f)][SerializeField] private float timeForUpwardsCancel = 0.027f;

    [Header("Jump Apex")]
    [Range(0.5f, 1f)]
    [SerializeField] private float apexThreshHold = 0.97f;
    [Range(0.01f, 1f)]
    [SerializeField] private float apexHangTime = 0.075f;

    [Header("Jump Buffer")]
    [Range(0f, 1f)][SerializeField] private float jumpBufferTime = 0.125f;

    [Header("Jump Coyote Time")]
    [Range(0f, 1f)][SerializeField] private float jumpCoyoteTime = 0.1f;

    // Debug
    [Header("Jump Visualization Tool")]
    [SerializeField] private bool showWalkJumpArc = false;
    [SerializeField] private bool showRunJumpArc = false;
    [SerializeField] private bool stopOnCollision = true;
    [SerializeField] private bool drawRight = true;
    [Range(5, 100)][SerializeField] private int arcResolution = 20;
    [Range(5, 500)][SerializeField] private int visualizationSteps = 90;

    #region Move Left / Right

    private Vector2 moveVelocity;
    public bool isFacingRight;

    private RaycastHit2D groundHit;
    private RaycastHit2D headHit;
    [SerializeField] private bool isGrounded;
    private bool bumpedHead;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(this); else instance = this;
        isFacingRight = true;
    }

    private void Update()
    {
        CountTimers();
        JumpChecks();
    }

    private void FixedUpdate()
    {
        CollisionChecks();
        Jump();

        if (isGrounded)
        {
            Move(groundAcceleration, groundDeceleration, InputManager.instance.GetMovement());
        } else
        {
            Move(airAcceleration, airDeceleration, InputManager.instance.GetMovement());
        }
    }

    private void Move(float acceleration, float deceleration, Vector2 moveInput)
    {
        if (moveInput != Vector2.zero)
        {
            TurnCheck(moveInput);

            Vector2 targetVelocity = Vector2.zero;
            if (InputManager.instance.IsRunHeld())
            {
                targetVelocity = new Vector2(moveInput.x, 0f) * maxRunSpeed;
            }
            else { targetVelocity = new Vector2(moveInput.x, 0f) * maxWalkSpeed; }

            moveVelocity = Vector2.Lerp(moveVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
            rigidBody.linearVelocity = new Vector2(moveVelocity.x, rigidBody.linearVelocity.y);
        }

        else if (moveInput == Vector2.zero)
        {
            moveVelocity = Vector2.Lerp(moveVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
            rigidBody.linearVelocity = new Vector2(moveVelocity.x, rigidBody.linearVelocity.y);
        }
    }

    private void TurnCheck(Vector2 moveInput)
    {
        if (isFacingRight && moveInput.x < 0) Turn(false);
        else if (!isFacingRight && moveInput.x > 0) Turn(true);
    }

    private void Turn(bool turnRight)
    {
        if (turnRight)
        {
            isFacingRight = true;
            transform.Rotate(0f, 180f, 0f);
        }
        else
        {
            isFacingRight = false;
            transform.Rotate(0f, -180f, 0f);
        }
    }

    #endregion

    #region Collision Checks
    private void IsGrounded()
    {
        Vector2 boxCastOrigin = new Vector2(feetCollision.bounds.center.x, feetCollision.bounds.min.y);
        Vector2 boxCastSize = new Vector2(feetCollision.bounds.size.x, groundDetectionRayLength);

        groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, groundDetectionRayLength, (groundLayer + ingredientLayer));

        if (groundHit.collider != null)
        {
            isGrounded = true;
        } else
        {
            isGrounded = false;
        }
        
        // Debug Visualization
        
        if (showIsGroundedBox)
        {
            Color rayColor;
            if (isGrounded) rayColor = Color.green ; else rayColor = Color.red;

            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * groundDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x + boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * groundDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y - groundDetectionRayLength), Vector2.right * boxCastSize.x, rayColor);
        }
    }

    private void BumpedHead()
    {
        Vector2 boxCastOrigin = new Vector2(feetCollision.bounds.center.x, bodyCollision.bounds.max.y);
        Vector2 boxCastSize = new Vector2(feetCollision.bounds.size.x * headWidth, headDetectionRayLength);

        headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, headDetectionRayLength, groundLayer);
        if (headHit.collider != null)
        {
            bumpedHead = true;
        } else
        {
            bumpedHead = false;
        }

        // Debug visualization

        if (showHeadBumpBox)
        {
            Color rayColor;
            if (bumpedHead) rayColor = Color.green ; else rayColor = Color.red;
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2 * headWidth, boxCastOrigin.y), Vector2.up * headDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x + boxCastSize.x / 2 * headWidth, boxCastOrigin.y), Vector2.up * headDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2 * headWidth, boxCastOrigin.y + headDetectionRayLength), Vector2.right * boxCastSize.x * headWidth, rayColor);
        }
    }

    private void CollisionChecks()
    {
        IsGrounded();
        BumpedHead();
    }

    #endregion

    #region Jump

    public float gravity { get; private set; }
    public float initialJumpVelocity { get; private set; }
    public float adjustedJumpHeight { get; private set; }

    // Jump Variables
    [SerializeField] private float verticalVelocity;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isFastFalling;
    [SerializeField] private bool isFalling;
    [SerializeField] private float fastFallTime;
    [SerializeField] private float fastFallReleaseSpeed;
    [SerializeField] private int numberOfJumpsUsed;

    // Apex Variables
    [SerializeField] private float apexPoint;
    [SerializeField] private float timePastApexThreshold;
    [SerializeField] private bool isPastApexThreshold;

    // Jump Buffer Variables
    [SerializeField] private float jumpBufferTimer;
    [SerializeField] private float jumpBufferTimerPressed;
    [SerializeField] private bool jumpReleasedDuringBuffer;

    // Coyote Time Variables
    [SerializeField] private float coyoteTimer;


    private void JumpChecks()
    {
        if (InputManager.instance.WasJumpPressed())
        {
            jumpBufferTimer = jumpBufferTime;
            jumpReleasedDuringBuffer = false;
        }

        if (InputManager.instance.WasJumpReleased())
        {
            if (jumpBufferTimer > 0f) jumpReleasedDuringBuffer = true;
            
            if (isJumping && verticalVelocity > 0f)
                if (isPastApexThreshold)
                {
                    isPastApexThreshold = false;
                    isFastFalling = true;
                    fastFallTime = timeForUpwardsCancel;
                    verticalVelocity = 0f;
                }
                else
                {
                    isFastFalling = true;
                    fastFallReleaseSpeed = verticalVelocity;
                }
        }

        // ACTUAL JUMP WITH COYOTE TIME AND BUFFER
        if (jumpBufferTimer > 0f && !isJumping && (isGrounded || coyoteTimer > 0f))
        {
            InitiateJump(1);

            // Allows for shorter jumps
            if (jumpReleasedDuringBuffer)
            {
                isFastFalling = true;
                fastFallReleaseSpeed = verticalVelocity;
            }
        }

        // ACTUAL JUMP WITH DOUBLE JUMP
        else if (jumpBufferTimer > 0f && isJumping && numberOfJumpsUsed < numberOfJumpsAllowed)
        {
            isFastFalling = false;
            InitiateJump(1);
        }

        // Handle air jump AFTER the coyote time has lapsed (takes off an extra jump so we don't get a bonus jump)
        else if (jumpBufferTimer > 0f && isFalling && numberOfJumpsUsed < numberOfJumpsAllowed - 1)
        {
            InitiateJump(2);
            isFastFalling = false;
        }

        // LANDED
        if ((isJumping || isFalling) && isGrounded && verticalVelocity <= 0f)
        {
            isJumping = false;
            isFalling = false;
            isFastFalling = false;
            fastFallTime = 0f;
            isPastApexThreshold = false;
            numberOfJumpsUsed = 0;

            verticalVelocity = Physics2D.gravity.y;
        }
    }

    private void InitiateJump(int jumpCost)
    {
        if (!isJumping)
        {
            isJumping = true;
        }

        jumpBufferTimer = 0f;
        numberOfJumpsUsed += jumpCost;
        verticalVelocity = initialJumpVelocity;
    }

    private void Jump()
    {
        if (isJumping)
        {
            if (bumpedHead)
            {
                isFastFalling = true;
            }

            if (verticalVelocity >= 0f)
            {
                apexPoint = Mathf.InverseLerp(initialJumpVelocity, 0f, verticalVelocity);

                if (apexPoint > apexThreshHold)
                {
                    if (!isPastApexThreshold)
                    {
                        isPastApexThreshold = true;
                        timePastApexThreshold = 0f;
                    }

                    if (isPastApexThreshold)
                    {
                        timePastApexThreshold += Time.fixedDeltaTime;
                        if (timePastApexThreshold < apexHangTime)
                        {
                            verticalVelocity = 0f;
                        }
                        else
                        {
                            verticalVelocity = -0.01f;
                        }
                    }
                }

                // GRAVITY ON ASCENDING BUT NOT PAST THRESHOLD
                else
                {
                    verticalVelocity += gravity * Time.fixedDeltaTime;
                    if (isPastApexThreshold)
                    {
                        isPastApexThreshold = false;
                    }
                }
            }

            // GRAVITY ON DESCENDING
            else if (!isFastFalling)
            {
                verticalVelocity += gravity * gravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }

            else if (verticalVelocity < 0f)
            {
                if (!isFalling)
                {
                    isFalling = true;
                }
            }
        }

        // JUMP CUT
        if (isFastFalling)
        {
            if (fastFallTime >= timeForUpwardsCancel)
            {
                verticalVelocity += gravity * gravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }
            else if (fastFallTime < timeForUpwardsCancel)
            {
                verticalVelocity = Mathf.Lerp(fastFallReleaseSpeed, 0f, (fastFallTime / timeForUpwardsCancel));
            }

            fastFallTime += Time.fixedDeltaTime;
        }

        // NORMAL GRAVITY WHILE FALLING
        if (!isGrounded && !isJumping)
        {
            if (!isFalling) 
            {
                isFalling = true;
            }

            verticalVelocity += gravity * Time.fixedDeltaTime;
        }

        // CLAMP FALL SPEED
        verticalVelocity = Mathf.Clamp(verticalVelocity, -maxFallSpeed, 50f);

        rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, verticalVelocity);
        
    }

    private void OnValidate()
    {
        CalculateValues();
    }

    private void OnEnable()
    {
        CalculateValues();
    }

    private void CalculateValues()
    {
        adjustedJumpHeight = jumpHeight * jumpHeighCompensationFactor;
        gravity = -(2f * adjustedJumpHeight) / Mathf.Pow(timeTillJumpApex, 2f);
        initialJumpVelocity = Mathf.Abs(gravity) * timeTillJumpApex;
    }

    #endregion

    #region Timers

    private void CountTimers()
    {
        jumpBufferTimer -= Time.deltaTime;

        if (!isGrounded) coyoteTimer -= Time.deltaTime; else coyoteTimer = jumpCoyoteTime;
    }

    #endregion
}