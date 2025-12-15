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

    int VelocityZHash, VelocityXHash;

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

    private void Start()
    {
        VelocityXHash = Animator.StringToHash("VelocityX");
        VelocityZHash = Animator.StringToHash("VelocityZ");
    }

    void ChangeVelocity(bool forwardPressed, bool leftPressed, bool rightPressed, float currentMaxVelocity)
    {
        // increase z vel
        if (forwardPressed && velocityZ < currentMaxVelocity)
        {
            velocityZ += Time.deltaTime * acceleration;
        }

        // increase left x vel
        if (leftPressed && velocityX > -currentMaxVelocity)
        {
            velocityX -= Time.deltaTime * acceleration;
        }

        // increase right x vel
        if (rightPressed && velocityX < currentMaxVelocity)
        {
            velocityX += Time.deltaTime * acceleration;
        }

        // Decrease z vel
        if (!forwardPressed && velocityZ > 0.0f)
        {
            velocityZ -= Time.deltaTime * decceleration;
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
    }

    void ResetOrLockVelocity(bool forwardPressed, bool leftPressed, bool rightPressed, bool sprintPressed, float currentMaxVelocity)
    {
        // Reset z vel
        if (!forwardPressed && velocityZ < 0.0f)
        {
            velocityZ = 0.0f;
        }

        // Reset x vel
        if (!leftPressed && !rightPressed && velocityX != 0.0f && (velocityX > -0.05f && velocityX < 0.05f))
        {
            velocityX = 0.0f;
        }

        // Cap forward------------------------------------------------------------------------------------------------------
        if (forwardPressed && sprintPressed && velocityZ > currentMaxVelocity)
        {
            velocityZ = currentMaxVelocity;
        }

        // Deccel to max walk vel
        else if (forwardPressed && velocityZ > currentMaxVelocity)
        {
            velocityZ -= Time.deltaTime * decceleration;
            // Round to max vel 
            if (velocityZ > currentMaxVelocity && velocityZ < (currentMaxVelocity + 0.05f))
            {
                velocityZ = currentMaxVelocity;
            }
        }
        // round to max vel
        else if (forwardPressed && velocityZ < currentMaxVelocity && velocityZ > (currentMaxVelocity - 0.05f))
        {
            velocityZ = currentMaxVelocity;
        }

        // Cap Left -----------------------------------------------------------------------------------------------------------------------
        if (leftPressed && sprintPressed && velocityX < -currentMaxVelocity)
        {
            velocityX = -currentMaxVelocity;
        }
        // Deccel to max walk vel
        else if (leftPressed && velocityX < -currentMaxVelocity)
        {
            velocityX += Time.deltaTime * decceleration;
            // Round to max vel 
            if (velocityX < -currentMaxVelocity && velocityX > (-currentMaxVelocity - 0.05f))
            {
                velocityX = -currentMaxVelocity;
            }
        }
        // round to max vel
        else if (leftPressed && velocityX > -currentMaxVelocity && velocityX < (-currentMaxVelocity + 0.05f))
        {
            velocityX = -currentMaxVelocity;
        }

        // Cap Right----------------------------------------------------------------------------------------------------------------------------------------
        if (rightPressed && sprintPressed && velocityX > currentMaxVelocity)
        {
            velocityX = currentMaxVelocity;
        }
        // Deccel to max walk vel
        else if (rightPressed && velocityX > currentMaxVelocity)
        {
            velocityX -= Time.deltaTime * decceleration;
            // Round to max vel 
            if (velocityX > currentMaxVelocity && velocityX < (currentMaxVelocity + 0.05f))
            {
                velocityX = currentMaxVelocity;
            }
        }
        // round to max vel
        else if (rightPressed && velocityX < currentMaxVelocity && velocityX > (currentMaxVelocity - 0.05f))
        {
            velocityX = currentMaxVelocity;
        }
    }

    void Update()
    {
        m_moveAmt = m_moveAction.ReadValue<Vector2>();

        bool forwardPressed = m_moveAmt.y > 0.0f;
        bool leftPressed = m_moveAmt.x < 0.0f;
        bool rightPressed = m_moveAmt.x > 0.0f;
        bool sprintPressed = m_sprintAction.IsPressed();

        float currentMaxVel = sprintPressed ? maximumRunVel : maximumWalkVel;

        ChangeVelocity(forwardPressed, leftPressed, rightPressed, currentMaxVel);
        ResetOrLockVelocity(forwardPressed, leftPressed, rightPressed, sprintPressed, currentMaxVel);



        // Set Animator floats
        animator.SetFloat(VelocityZHash, velocityZ);
        animator.SetFloat(VelocityXHash, velocityX);
    }
}
