using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationStateController : MonoBehaviour
{
    public InputActionAsset InputActions;

    private InputAction m_moveAction;
    private Vector2 m_moveAmt;

    Animator animator;
    float velocity = 0.0f;

    public float acceleration = 1.0f;
    public float deacceleration = 0.5f;

    private void OnEnable()
    {
        // Enable action map
        InputActions.FindActionMap("Player").Enable();

    }

    private void OnDisable()
    {
        // Disable action map
        InputActions.FindActionMap("Player").Disable();
    }

    private void Awake()
    {
        m_moveAction = InputSystem.actions.FindAction("Move");

        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        m_moveAmt = m_moveAction.ReadValue<Vector2>();

        bool forwardPressed = m_moveAmt.y > 0.0f;

        Debug.Log(forwardPressed);

        if(forwardPressed && velocity < 1.0f)
        {
            velocity += Time.deltaTime * acceleration;
        }

        if (!forwardPressed && velocity > 0.0f)
        {
            velocity -= Time.deltaTime * deacceleration;
        }

        if (!forwardPressed && velocity < 0.0f)
        {
            velocity = 0.0f;
        }

        animator.SetFloat("Velocity", velocity);
    }
}
