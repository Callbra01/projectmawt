using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputActionAsset InputActions;

    private InputAction m_moveAction;

    private Vector2 m_moveAmt;
    private Vector3 movement;

    private Animator m_animator;
    private Rigidbody m_rb;

    public float walkSpeed = 5;
    public float strafeSpeed = 4;
    public float rotateSpeed = 5;
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
        Sprinting();
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

    private void Sprinting()
    {
        float h = m_moveAmt.x;
        float v = m_moveAmt.y;

        Vector3.Normalize(movement);

        movement = (transform.forward * v * walkSpeed) + (transform.right * h * strafeSpeed);
        m_rb.MovePosition(transform.position + movement * Time.deltaTime);

        m_animator.SetFloat("ForwardSpeed", m_moveAmt.y);
        //m_rb.MovePosition(m_rb.position + transform.forward * m_moveAmt.y * walkSpeed * Time.deltaTime);
    }

}
