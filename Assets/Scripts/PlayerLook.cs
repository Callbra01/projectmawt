using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    public InputActionAsset InputActions;

    private InputAction m_lookAction;

    private Vector2 m_lookAmt;

    public float sensitivity = 5f;

    public float smoothing = 1.5f;

    private Vector2 mouseLook;
    private Vector2 smoothMovement;

    private GameObject player;

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
        m_lookAction = InputSystem.actions.FindAction("Look");

    }

    void Start()
    {
        player = transform.parent.gameObject;
    }

    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        m_lookAmt = m_lookAction.ReadValue<Vector2>();

        Vector2 mDir = new Vector2(m_lookAmt.x, m_lookAmt.y);

        mDir.x *= sensitivity * smoothing;
        mDir.y *= sensitivity * smoothing;

        smoothMovement.x = Mathf.Lerp(smoothMovement.x, mDir.x, 1f / smoothing);
        smoothMovement.y = Mathf.Lerp(smoothMovement.y, mDir.y, 1f / smoothing);

        mouseLook += smoothMovement;

        mouseLook.y = Mathf.Clamp(mouseLook.y, -80, 90f);

        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        player.transform.rotation = Quaternion.AngleAxis(mouseLook.x, player.transform.up);
    }
}
