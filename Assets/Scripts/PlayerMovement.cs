using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputActionAsset InputActions;

    private InputAction m_moveAction;
    private InputAction m_lookAction;

    private Vector2 m_moveAmt;
    private Vector2 m_lookAmt;

    private Animator m_animator;
    private Rigidbody m_rb;

    public float walkSpeed = 5;
    public float rotateSpeed = 5;
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
        m_lookAction = InputSystem.actions.FindAction("Look");

        m_animator = GetComponent<Animator>();
        m_rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        m_moveAmt = m_moveAction.ReadValue<Vector2>();
        m_lookAmt = m_lookAction.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Walking();
        Rotating();
    }

    private void Walking()
    {
        m_animator.SetFloat("ForwardSpeed", m_moveAmt.y);
        m_rb.MovePosition(m_rb.position + transform.forward * m_moveAmt.y * walkSpeed * Time.deltaTime);
    }

    private void Rotating()
    {
        float rotationAmt = m_lookAmt.x * rotateSpeed * Time.deltaTime;
        Quaternion deltaRot = Quaternion.Euler(0, rotationAmt, 0);
        m_rb.MoveRotation(m_rb.rotation * deltaRot);
        
    }
}
