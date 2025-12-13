using UnityEngine;
using UnityEngine.InputSystem;

public class TwoDAnimationStateManager: MonoBehaviour
{
    public InputActionAsset InputActions;

    private InputAction m_moveAction;
    private InputAction m_sprintAction;
    private Vector2 m_moveAmt;

    Animator animator;
    float velocityZ = 0.0f;
    float velocityX = 0.0f;

    public float acceleration = 2.0f;
    public float decceleration = 0.5f;
    public float maximumWalkVel = 0.5f;
    public float maximumRunVel = 2.0f;

    private void Awake()
    {
        m_moveAction = InputSystem.actions.FindAction("Move");
        m_sprintAction = InputSystem.actions.FindAction("Sprint");

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        m_moveAmt = m_moveAction.ReadValue<Vector2>();

        bool forwardPressed = m_moveAmt.y > 0.0f;
        bool leftPressed = m_moveAmt.x < 0.0f;
        bool rightPressed = m_moveAmt.x > 0.0f;
        bool sprintPressed = m_sprintAction.IsPressed();

        float currentMaxVel = sprintPressed ? maximumRunVel : maximumWalkVel;

        if (forwardPressed && velocityZ < currentMaxVel)
        {
            velocityZ += Time.deltaTime * acceleration;
        }

        if (leftPressed && velocityX > -currentMaxVel)
        {
            velocityX -= Time.deltaTime * acceleration;
        }

        if (rightPressed && velocityX < currentMaxVel)
        {
            velocityX += Time.deltaTime * acceleration;
        }

        // Decrease z vel
        if (!forwardPressed && velocityZ > 0.0f)
        {
            velocityZ -= Time.deltaTime * decceleration;
        }

        // Reset z vel
        if (!forwardPressed && velocityZ < 0.0f)
        {
            velocityZ = 0.0f;
        }

        // Increase x vel if not pressed
        if (!leftPressed && velocityX < 0.0f)
        {
            velocityX += Time.deltaTime * decceleration;
        }

        // Decrease x vel if not pressed
        if (!rightPressed && velocityX > 0.0f)
        {
            velocityX -= Time.deltaTime * decceleration;
        }

        // Reset x vel
        if (!leftPressed && !rightPressed && velocityX != 0.0f && (velocityX > -0.05f && velocityX < 0.05f))
        {
            velocityX = 0.0f;
        }

        // Cap forward
        if(forwardPressed && sprintPressed && velocityZ > currentMaxVel)
        {
            velocityZ = currentMaxVel;
        }
        // Deccel to max walk vel
        else if (forwardPressed && velocityZ > currentMaxVel)
        {
            velocityZ -= Time.deltaTime * decceleration;
            // Round to max vel 
            if (velocityZ > currentMaxVel && velocityZ < (currentMaxVel + 0.05f))
            {
                velocityZ = currentMaxVel;
            }
        }
        // round to max vel
        else if (forwardPressed && velocityZ < currentMaxVel && velocityZ > (currentMaxVel - 0.05f))
        {
            velocityZ = currentMaxVel;
        }

        animator.SetFloat("VelocityZ", velocityZ);
        animator.SetFloat("VelocityX", velocityX);
    }
}
