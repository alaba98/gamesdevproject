using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float walkSpeed = 1f;
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float backwardWalkSpeed = 0.5f; 
    [SerializeField] float rotationSpeed = 400f;
    [SerializeField] float jumpForce = 2f;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] Vector3 groundCheckOffset;
    [SerializeField] LayerMask groundCheckLayerMask;

    CameraController cameraController;
    Quaternion targetRotation;
    Animator animator;
    CharacterController characterController;
    AimStateManager aim;

    bool isGrounded;
    bool isJumping;
    float ySpeed;

    private void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        aim = GetComponent<AimStateManager>();
    }

    private void Update()
    {
        // Player Input
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Adjust movement input if aiming
        if (aim.IsAiming())
        {
            h = 0; // Disable horizontal movement
            v = Input.GetKey(KeyCode.F) ? v : Mathf.Clamp01(v); // Allow backward movement when aiming and holding F
            if(movementSpeed == runSpeed)//set check to correct speed if already running
            {
                if (v < 0)
                {
                    movementSpeed = backwardWalkSpeed;
                    WalkBackwards(); 
                }
                else
                {
                    Walk();
                }
            }
        }

        float movementValue = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));
        var movementInput = (new Vector3(h, 0, v)).normalized;
        var movementDirection = cameraController.HorizontalRotation * movementInput;

        GroundCheck();

        if (isGrounded)
        {
            ySpeed = -0.5f;  // Reset vertical speed to a small negative value when grounded
            isJumping = false; // Reset the jump flag when grounded

            if (movementDirection != Vector3.zero && !Input.GetKey(KeyCode.Space))
            {
                if (v < 0 && aim.IsAiming())
                {
                    movementSpeed = backwardWalkSpeed;
                    WalkBackwards();
                }
                else
                {
                    movementSpeed = walkSpeed;
                    Walk();
                }
            }
            else if (movementDirection != Vector3.zero && !aim.IsAiming() && Input.GetKey(KeyCode.Space)) // Prevent running if moving backwards
            {
                Run();
            }
            else if (movementDirection != Vector3.zero && aim.IsAiming() && Input.GetKey(KeyCode.Space) && v>0) // Prevent running if moving backwards
            {
                Walk();
            }
            else if (movementDirection != Vector3.zero && aim.IsAiming() && Input.GetKey(KeyCode.Space) && v < 0) // Prevent running if moving backwards
            {
                WalkBackwards();    
            }
            else if (movementDirection == Vector3.zero)
            {
                Idle();
            }

            if (Input.GetKeyDown(KeyCode.Y))
            {
                Jump();
            }
        }
        else
        {
            ySpeed += Physics.gravity.y * Time.deltaTime;  // Apply gravity when not grounded
        }

        var velocity = movementDirection * movementSpeed;
        velocity.y = ySpeed;
        characterController.Move(velocity * Time.deltaTime);

        // Rotate the player towards the aim direction or movement direction
        if (!isJumping)
        {
            if (aim.IsAiming())
            {
                Vector3 aimDirection = aim.GetAimDirection();
                aimDirection.y = 0;  // Zero out the y-component to keep the rotation on the vertical plane
                if (aimDirection != Vector3.zero)
                {
                    targetRotation = Quaternion.LookRotation(aimDirection);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
            }
            else if (movementValue > 0)
            {
                targetRotation = Quaternion.LookRotation(movementDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }

        // Update the animator
        float animationSpeed = v < 0 && aim.IsAiming() && Input.GetKey(KeyCode.F) ? -Mathf.Abs(v) : movementValue;
        animator.SetFloat("Speed", isJumping ? 0 : animationSpeed, 0.15f, Time.deltaTime);
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundCheckLayerMask);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);
    }

    private void Idle()
    {
        animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
    }

    private void Walk()
    {
        movementSpeed = walkSpeed;
        animator.SetFloat("Speed", 0.25f, 0.1f, Time.deltaTime);
    }

    private void WalkBackwards()
    {
        movementSpeed = backwardWalkSpeed; // Use the backward walk speed
        animator.SetFloat("Speed", -0.25f, 0.1f, Time.deltaTime); // Use a negative speed value for backward walk
    }

    private void Run()
    {
        movementSpeed = runSpeed;
        animator.SetFloat("Speed", 1, 0.1f, Time.deltaTime);
    }

    private void Jump()
    {
        isJumping = true;
        ySpeed = Mathf.Sqrt(jumpForce * -2 * Physics.gravity.y);
        animator.SetTrigger("Jump");
    }
}











