using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputActionAsset InputActions;

    private InputAction m_moveAction;
    private InputAction m_sprintAction;

    private Vector2 m_moveAmt;
    private Vector3 movement;

    private Animator m_animator;
    private Rigidbody m_rb;

    public float walkSpeed = 5;
    public float strafeSpeed = 4;
    public float rotateSpeed = 5;
    public float walkSpeedOffset = 0.5f;
    bool isGrounded;
    public LayerMask groundLayer;
    //public float jumpSpeed = 5;

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
        m_sprintAction = InputSystem.actions.FindAction("Sprint");

        m_animator = GetComponent<Animator>();
        m_rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        m_moveAmt = m_moveAction.ReadValue<Vector2>();

        isGrounded = CheckGround();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private bool CheckGround()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f, groundLayer))
        {
            return true;
        }

        return false;
    }

    private void HandleMovement()
    {
        if (m_sprintAction.IsPressed())
        {
            Sprinting();
        }
        else
        {
            Walking();
        }
        
    }

    private void Walking()
    {
        float h = m_moveAmt.x;
        float v = Mathf.Clamp(m_moveAmt.y, -0.5f, 1f);

        Vector3.Normalize(movement);

        movement = (transform.forward * v * walkSpeed * walkSpeedOffset) + (transform.right * h * strafeSpeed * walkSpeedOffset);
        m_rb.MovePosition(transform.position + movement * Time.deltaTime);
    }

    private void Sprinting()
    {
        float h = m_moveAmt.x;

        // Clamp vertical input to reduce movement speed when walking backwards
        float v = Mathf.Clamp(m_moveAmt.y, -0.5f, 1f);

        Vector3.Normalize(movement);

        movement = (transform.forward * v * walkSpeed) + (transform.right * h * strafeSpeed);
        m_rb.MovePosition(transform.position + movement * Time.deltaTime);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BottomLadder")
        {
            transform.position = GameManager.instance.ladderScript.topTransform.position;
        }

        if (other.gameObject.tag == "TopLadder")
        {
            transform.position = GameManager.instance.ladderScript.bottomTransform.position;
        }
    }

}
