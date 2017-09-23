using UnityEngine;
using System.Collections;

[System.Serializable]
[AddComponentMenu("Advanced Platformer Controller/ Platformer Controller")]
[RequireComponent(typeof(CharacterController))]
public class TwoDController : MonoBehaviour {

    #region Variables

    private Vector3 startRotation;

	public enum MoveType
	{
		Sprint,
		Crouch,
		Sprint_Crouch,
		None
	}

	public enum JumpType
	{
		Jump,
		Jump_DoubleJump,
		None
	}

    public enum JetpackType
    {
        Activated,
        Not_Activated
    }

    public enum LadderType
    {
        Activated,
        Not_Activated
    }

    public enum DashType
    {
        Activated,
        Not_Activated
    }
   
    /// <summary>
    /// Axis to use for Horizontal movement. 
    /// </summary>
	public string axis;

    /// <summary>
    /// Axis to use for ladder climbing.
    /// </summary>
    public string ladderAxis;

	public MoveType playerMoveType;
	public JumpType playerJumpType;
    public JetpackType jetpackType;
    public LadderType ladderType;
    public DashType dashType;

    /// <summary>
    /// Vector3 that will serve as velocity.
    /// </summary>
	public Vector3 moveDirection = Vector3.zero;

    /// <summary>
    /// Speed of the controller.
    /// </summary>
	public float speed;
    
    /// <summary>
    /// Speed when crouching.
    /// </summary>
    public float crouchSpeed;

    /// <summary>
    /// Speed when sprinting.
    /// </summary>
    public float sprintSpeed;

    /// <summary>
    /// Speed while climbing ladders.
    /// </summary>
    public float ladderSpeed;

    /// <summary>
    /// Graavity to be applied.
    /// </summary>
	public float gravity; 	

    /// <summary>
    /// Force of a jump.
    /// </summary>
	public float jumpForce;	 

    /// <summary>
    /// Force of a double jump.
    /// </summary>
	public float doubleJumpForce; 	

    /// <summary>
    /// Is the controller crouching?
    /// </summary>
	public bool isCrouching;

    /// <summary>
    /// Is the controller sprinting?
    /// </summary>
    public bool isSprinting;

    /// <summary>
    /// Is the controller double-jumping?
    /// </summary>
    public bool doubleJumping;

    /// <summary>
    // Set to true if the controller is facing left.
    /// </summary>
	public bool facingLeft = false; 	

    /// <summary>
    /// Set to true if the controller is fight right.
    /// </summary>
	public bool facingRight = true;		

    /// <summary>
    /// Set to true if the controller can double jump in this moment.
    /// </summary>
	private bool canDoubleJump; 		
 	

    /// <summary>
    /// Speed of the controller at Start.
    /// </summary>
	private float normalSpeed;   

    /// <summary>
    /// True if the controller is currently on a ladder.
    /// </summary>
    public bool isClimbing;

    /// <summary>
    /// True if the jetpack is being used.
    /// </summary>
    public bool jetPackEnabled;

    /// <summary>
    /// Duration of the jetpack.
    /// </summary>
    public float jetpackDuration;

    /// <summary>
    /// Time that it takes to fully recharge the jetpack (seconds).
    /// </summary>
    public float jetpackRecharge;

    /// <summary>
    /// Jetpack duration at Start.
    /// </summary>
    private float defaultJetDur;

    /// <summary>
    /// Force that the jetpack applies.
    /// </summary>
    public float jetForce;
            
    /// <summary>
    /// Controller attached to this gameobject.
    /// </summary>
    private CharacterController playerController;

    /// <summary>
    /// Jump button.
    /// </summary>
    public string jumpKey;

    /// <summary>
    /// Jetpack button.
    /// </summary>
    public string jetpackKey;

    /// <summary>
    /// Sprint button. 
    /// </summary>
    public string sprintKey;

    /// <summary>
    /// Crouch button.
    /// </summary>
    public string crouchKey;

    /// <summary>
    /// Dash button.
    /// </summary>
    public string dashKey = "Dash";

    /// <summary>
    /// Duration of the dash in seconds.
    /// </summary>
    public float dashDuration = .1f;

    /// <summary>
    /// Force of the dash.
    /// </summary>
    public float dashForce = 5;

    /// <summary>
    /// Delay between dashes.
    /// </summary>
    public float dashCooldown = 1;

    // Used internally for dash control.
    bool isDashingLeft;
    bool isDashingRight;
    bool canDash = true;
    


#endregion Variables

    void Start ()
    {
        playerController = GetComponent<CharacterController>();
        facingRight = true;
		normalSpeed = speed;

        startRotation = transform.eulerAngles;        
        defaultJetDur = jetpackDuration;
	}
    
    void Update()
    {
        // Jetpack ===========       

        jetpackDuration = Mathf.Clamp(jetpackDuration, 0, defaultJetDur);

        if (jetPackEnabled && jetpackDuration > 0 && !isClimbing)
        {
            if (jetpackType == JetpackType.Activated)
            {
                if (Input.GetButton(jetpackKey))
                {
                    if (jetPackEnabled && !isClimbing)
                    {
                        ApplyJetForce();
                    }
                }
            }
        }

        // ============ Jetpack

        // Dashing ==========

        if (dashType == DashType.Activated)
        {
            if (Input.GetButtonDown(dashKey))
            {
                if (moveDirection.x == 1 && canDash)
                {
                    StartCoroutine(Dash(0));
                    StartCoroutine(DashCooldown());
                }
                else if (moveDirection.x == -1 && canDash)
                {
                    StartCoroutine(Dash(1));
                    StartCoroutine(DashCooldown());
                }
            }  
        }

        if (!isDashingRight && !isDashingLeft)
        {
            moveDirection.x = Input.GetAxisRaw(axis);
        }
        else if (isDashingRight)
        {
            moveDirection.x = dashForce;
        }
        else
        {
            moveDirection.x = -dashForce;
        }

        // ========= Dashing

        // Climbing ========

        if (isClimbing)
        {
            moveDirection.y = Input.GetAxisRaw(ladderAxis) * ladderSpeed;
        }

        // ========= Climbing

        // Reseting ========= 

        if (playerController.isGrounded)
        {
            doubleJumping = false;
            jetPackEnabled = false;
        }
        else if (transform.parent != null)
        {         
            transform.SetParent(null); //Detach from moving platform          
        }

        // ========= Resetting

        //Jumping ============

        if (playerJumpType == JumpType.Jump || playerJumpType == JumpType.Jump_DoubleJump)
        {
            if (Input.GetButtonDown(jumpKey))
            {
                JumpCheck();
            }
        }

        // =========== Jumping
            

        // Gravity =============

        if (moveDirection.y > -2 && !isClimbing)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // ============= Gravity
        

        
        

        if (!jetPackEnabled && jetpackDuration < defaultJetDur && playerController.isGrounded)
        {
            RechargeJetpack();
        }

        //Sprinting and Crouching
        if (playerController.isGrounded)
        {
            if (playerMoveType == MoveType.Sprint_Crouch || playerMoveType == MoveType.Crouch)
            {
                if (Input.GetButtonDown(crouchKey))
                {
                    Crouch();
                }
            }

            if (playerMoveType == MoveType.Sprint_Crouch || playerMoveType == MoveType.Sprint)
            {
                if (Input.GetButtonDown(sprintKey))
                {
                    Sprint();
                }
            }
        }

        if (playerMoveType == MoveType.Sprint_Crouch || playerMoveType == MoveType.Crouch)
        {
            if (Input.GetButtonUp(crouchKey) || Input.GetButtonUp(sprintKey))
            {
                ResetSpeed();
            }
        }
        

            if (moveDirection.x < 0 && !facingLeft)
            {
                FlipLeft();
            }
            else if (moveDirection.x > 0 && !facingRight)
            {
                FlipRight();
            }
            

            if ((playerController.collisionFlags & CollisionFlags.Above) != 0)
            {
                moveDirection.y = 0;
            }

            ApplyMovement();

    }

    private void ApplyMovement ()
    {

        playerController.Move(moveDirection * speed * Time.deltaTime);
    }

    private void JumpCheck ()
    {
        if (playerController.isGrounded && !isCrouching )
        {
            Jump();
        }
        else if (canDoubleJump && playerJumpType == JumpType.Jump_DoubleJump && !playerController.isGrounded)
        {
            DoubleJump();
        }        
              
    }

	private void Jump ()		
	{
		moveDirection.y = (jumpForce);	 
        
		canDoubleJump = true;

        if (jetpackType == JetpackType.Activated && playerJumpType != JumpType.Jump_DoubleJump)
        {
            StartCoroutine(ActivateJetpack());
        }
    }    

	private void DoubleJump ()	
	{
		moveDirection.y = doubleJumpForce;	 
		doubleJumping = true;
		canDoubleJump = false; 
        	
        if (jetpackType == JetpackType.Activated)
        {
            StartCoroutine(ActivateJetpack());
        }
	}   
	
    
	public void FlipLeft ()	 
	{
        if (!facingLeft)
        {
            facingLeft = true;
            facingRight = false;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
	}

	public void FlipRight ()	
	{
        if (!facingRight)
        {
            facingRight = true;
            facingLeft = false;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
	}

    

    private IEnumerator ActivateJetpack ()
    {
        yield return new WaitForSeconds(.3f);

        if (jetpackDuration > 0)
            jetPackEnabled = true;        
    }      

    

    private void ApplyJetForce ()
    {
        moveDirection.y += jetForce;
        jetpackDuration -= Time.deltaTime;
    }

    private void RechargeJetpack ()
    {
        jetpackDuration += (defaultJetDur / jetpackRecharge) * Time.deltaTime;
    }


    private void ResetSpeed()
    {
        speed = normalSpeed;

        isSprinting = false;
        isCrouching = false;        
    }

    private void Sprint ()
    {
        speed = sprintSpeed;

        isSprinting = true;
        isCrouching = false;
    }
            
    private void Crouch ()
    {
        speed = crouchSpeed;

        isSprinting = false;
        isCrouching = true;
    }



    private void CheckMovingPlatform (ControllerColliderHit other)
    {
        // Moving Platforms
        if (other.gameObject.GetComponent<MovingPlatform>() != null && (playerController.collisionFlags & CollisionFlags.Below) != 0)
        {
            gameObject.transform.SetParent(other.transform);
        }
        else
        {
            gameObject.transform.SetParent(null);
        }
    }

    private void CheckSlopes (ControllerColliderHit other)
    {
        if (other.gameObject.GetComponent<Slope>() != null && (playerController.collisionFlags & CollisionFlags.Below) != 0)
        {
            Slope slope = other.gameObject.GetComponent<Slope>();

            if (slope.invertRotation)
            {
                transform.localEulerAngles = new Vector3(startRotation.x, startRotation.y, slope.gameObject.transform.localEulerAngles.z);
            } else
            {
                transform.localEulerAngles = new Vector3(startRotation.x, startRotation.y, -slope.gameObject.transform.localEulerAngles.z);
            }
            
        }
        else
        {
            transform.localEulerAngles = startRotation;
        }
    }
        
    IEnumerator Dash (int direction)
    {
        if (direction == 0)
        {
            isDashingRight = true;
        } else
        {
            isDashingLeft = true;
        }
                
        yield return new WaitForSeconds(dashDuration);

        isDashingRight = false;
        isDashingLeft = false;
    }        
    
    IEnumerator DashCooldown ()
    {
        canDash = false;

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;       
    }

    void OnControllerColliderHit(ControllerColliderHit other)
    {
        CheckMovingPlatform(other);

        CheckSlopes(other);
    }
}
