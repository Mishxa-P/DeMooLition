using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float dashSpeed = 25.0f;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private float dashCooldown = 1.0f;

    [Header("Grounds")]
    [SerializeField] private LayerMask whatIsGround;
    [Header("Development")]
    [SerializeField] private bool useKeyboardInput = false;

    private JoystickController joystickController;
    private CharacterController controller;
    private PlayerState playerState;
    private float turnSmoothVelocity;
    private bool canDash = true;

    private float quarterOfCapsuleSizeY;

 
    private bool isDisabled = false;   //temp 

    private void OnEnable()
    {
        LevelEventManager.OnLevelCompleted.AddListener(DisableMovement);   //temp 
    }
    private void Start()
    {
        joystickController = GameObject.Find("Joystick").GetComponent<JoystickController>();
        controller = GetComponent<CharacterController>();
        playerState = GetComponent<PlayerState>();


        quarterOfCapsuleSizeY = GetComponent<CapsuleCollider>().height / 4.0f;
    }

    private void Update()
    {
        if (isDisabled)
        {
            return;
        }
        if (!playerState.IsStunned)
        {
            float horizontal = joystickController.GetInputDirection().x;
            float vertical = joystickController.GetInputDirection().y;

            if (useKeyboardInput)
            {
                horizontal = Input.GetAxisRaw("Horizontal");
                vertical = Input.GetAxisRaw("Vertical");
            }

            Move(horizontal, vertical);

            if ((canDash && useKeyboardInput && Input.GetButtonDown("Dash")) || (canDash && joystickController.IsKnobInOuterCircle()))
            {
                StartCoroutine(Dash(horizontal, vertical));
            }

       
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 5.0f, whatIsGround))
            {
                transform.position = new Vector3(transform.position.x, hit.point.y + quarterOfCapsuleSizeY, transform.position.z);
            }
        }
    }

    private void Move(float x, float z)
    {
        Vector3 direction = new Vector3(x, 0.0f, z).normalized;

        if (direction.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
            controller.Move(direction * speed * Time.deltaTime);
        }
    }
    private IEnumerator Dash(float x, float z)
    {
        canDash = false;
        LevelEventManager.SendPlayerDashed(dashCooldown + dashTime);
        float startTime = Time.time;
        while (Time.time < startTime + dashTime)
        {
            Vector3 direction = new Vector3(x, 0.0f, z);
            controller.Move(direction * dashSpeed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void DisableMovement()
    {
        isDisabled = true;
    }
}
