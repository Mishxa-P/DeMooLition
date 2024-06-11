using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Range(0, 50)]
    [SerializeField] private int damage = 25;

    [Header("Default State")]
    [Range(0.1f, 20.0f)]
    [SerializeField] private float defaultStateSpeed = 10.0f;
    [Range(0.0f, 10.0f)]
    [SerializeField] private float defaultStateTurnSmoothTime = 0.0f;

    [Header("Accelerating State")]
    [Range(1.0f, 10.0f)]
    [SerializeField] private float acceleratingStateMaxSpeedMultiplier = 1.5f;
    [Range(1.0f, 10.0f)]
    [SerializeField] private float acceleratingStateSpeedChangePerSecond = 3.0f;
    [Range(1.0f, 25.0f)]
    [SerializeField] private float acceleratingStateRadius = 15.0f;

    [Header("Destruction State")]
    [Range(1.0f, 25.0f)]
    [SerializeField] private float destructionStateRadius = 2.5f;
    [Range(0.0f, 10.0f)]
    [SerializeField] private float destructionStateTime = 1.0f;
    [SerializeField] private GameObject windVFX_1;
    [SerializeField] private GameObject windVFX_2;
    [SerializeField] private GameObject dustVFX;
    [SerializeField] private GameObject directionArrow;
    [SerializeField] private BoxCollider bullCollision;


   [Header("After Destruction State")]
    [SerializeField] private bool stunnedInAfterDestructionState = true;
    [Range(0.0f, 10.0f)]
    [SerializeField] private float afterDestructionStateTurnSmoothTime = 0.8f;
    [Range(0.0f, 5.0f)]
    [SerializeField] private float afterDestructionStateTime = 1.25f;

    [Header("Stunned State")]
    [Range(0.0f, 10.0f)]
    [SerializeField] private float stunnedStateTime = 1.0f;
    [SerializeField] private string stunnedLayer = "Unbreakable";

    public enum EnemyState { Default, Accelerating, Destruction, AfterDestruction, Stunned};

    [Header("Development")]
    public EnemyState state;
    [SerializeField] private bool showAcceleratingStateRadius = false;
    [SerializeField] private bool showDestructionStateRadius = false;
    [SerializeField] private Mesh showRadiusMesh;

    private Transform playerTransform;
    private NavMeshAgent agent;
    private Rigidbody rb;
    private Animator animator;

    private float currentStateTime = 0.0f;
    private float currentStateSpeed;
    private float currentStateTurnSmoothTime;
    //--destruction state--
    public bool CanDestroyObjects { get; private set; }
    //--after destruction state--
    private float turnSmoothVelocity;

    // temp
    private bool isDisabled = false;

    private void OnEnable()
    {
        LevelEventManager.OnLevelCompleted.AddListener(Disable);   //temp 
    }

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if (playerTransform == null)
        {
            Debug.LogError("Player is not found");
        }
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        CanDestroyObjects = false;
        state = EnemyState.Default;
        animator = GetComponentInChildren<Animator>();
        ActivateDestructionVFX(false);
        bullCollision.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;
        if (obj.tag == "Player")
        {
            if (!isDisabled)
            {
                //Debug.Log("Collided with player!");
                PlayerState player = obj.GetComponentInParent<PlayerState>();
                player.Stun(damage);
            }
        }
        if (obj.layer == LayerMask.NameToLayer(stunnedLayer))
        {
            StunnedByObstacle();
            ActivateDestructionVFX(false);
        }
    }
    private void Update()
    {
        switch (state)
        {
            case EnemyState.Default:
                DefaultStateMovement();
                animator.SetFloat("Speed_f", 0.92f);
                if ((transform.position - playerTransform.position).magnitude <= acceleratingStateRadius)
                {
                    SwitchState(EnemyState.Accelerating);
                }
                break;
            case EnemyState.Accelerating:
                AcceleratingStateMovement();
                if (((transform.position - playerTransform.position).magnitude <= destructionStateRadius && Vector3.Angle(transform.forward, playerTransform.position - transform.position) < 18.0f) ||
                    (transform.position - playerTransform.position).magnitude <= 2.0f) 
                {
                    animator.SetFloat("Speed_f", 0.95f);
                    currentStateSpeed = agent.speed;
                    currentStateTime = destructionStateTime;
                    agent.enabled = false;
                    CanDestroyObjects = true;
                    SwitchState(EnemyState.Destruction);
                    ActivateDestructionVFX(true);
                    bullCollision.enabled = true;
                }
                break;
            case EnemyState.Destruction:
                animator.SetFloat("Speed_f", 1.0f);
                DestructionStateMovement();
                currentStateTime -= Time.deltaTime;
                if (currentStateTime <= 0)
                {
                    currentStateTurnSmoothTime = afterDestructionStateTurnSmoothTime;
                    currentStateSpeed = agent.speed;
                    currentStateTime = afterDestructionStateTime;
                    agent.enabled = true;
                    CanDestroyObjects = true;
                    if (stunnedInAfterDestructionState)
                    {
                        SwitchState(EnemyState.Stunned);
                        bullCollision.enabled = false;
                    }
                    else
                    {
                        SwitchState(EnemyState.AfterDestruction);
                    }
                    ActivateDestructionVFX(false);
                }
                break;
            case EnemyState.AfterDestruction:
                animator.SetFloat("Speed_f", 0.9f);
                AfterDestructionStateMovement();
                currentStateTime -= Time.deltaTime;
                if (currentStateTime <= 0)
                {
                    currentStateTurnSmoothTime = defaultStateTurnSmoothTime;
                    //agent.speed = defaultStateSpeed;
                    currentStateSpeed = agent.speed;
                    agent.enabled = true;
                    CanDestroyObjects = false;
                    SwitchState(EnemyState.Default);
                    bullCollision.enabled = false;
                }
                break;
            case EnemyState.Stunned:
                animator.SetFloat("Speed_f", 0.0f);
                currentStateTime -= Time.deltaTime;
                if (currentStateTime <= 0)
                {
                    currentStateTurnSmoothTime = defaultStateTurnSmoothTime;
                    agent.speed = defaultStateSpeed;
                    currentStateSpeed = agent.speed;
                    agent.enabled = true;
                    CanDestroyObjects = false;
                    SwitchState(EnemyState.Default);
                    bullCollision.enabled = false;
                }
                break;
        }
    }
    public void StunnedByObstacle()
    {
        // play stunned animation
        currentStateTime = stunnedStateTime;
        SwitchState(EnemyState.Stunned);
    }
    private void DefaultStateMovement()
    {
        agent.destination = playerTransform.position;
    }
    private void AcceleratingStateMovement()
    {
        if (agent.speed < defaultStateSpeed * acceleratingStateMaxSpeedMultiplier)
        {
            agent.speed += acceleratingStateSpeedChangePerSecond * Time.deltaTime;
        }
        agent.destination = playerTransform.position;
    }
    private void DestructionStateMovement()
    {
        transform.position += transform.forward * currentStateSpeed * Time.deltaTime;
    }
    private void AfterDestructionStateMovement()
    {
        if (agent.speed > defaultStateSpeed)
        {
            agent.speed -= acceleratingStateSpeedChangePerSecond * 2.0f * Time.deltaTime;
        }
        LookToward(playerTransform.position);
        Vector3 movement = transform.forward * currentStateSpeed * Time.deltaTime;
        agent.Move(movement);
    }
    private void LookToward(Vector3 destination)
    {
        float targetAngle = Vector3.Angle(transform.forward, destination - transform.position);
        if (Vector3.Angle(transform.right, destination - transform.position) <= 90.0f)
        {
            targetAngle = -targetAngle;
        }
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, transform.eulerAngles.y - targetAngle, ref turnSmoothVelocity, currentStateTurnSmoothTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }
    private void SwitchState(EnemyState nextState)
    {
        state = nextState;
        //Debug.Log("Enemy in " + nextState.ToString() + " state");
    }

    private void OnDrawGizmosSelected()
    {
        if (showAcceleratingStateRadius)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawMesh(showRadiusMesh, 0, transform.position, Quaternion.identity, new Vector3 (acceleratingStateRadius * 2.0f, 0.1f, acceleratingStateRadius * 2.0f));
        }
        if (showDestructionStateRadius)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawMesh(showRadiusMesh, 0, transform.position, Quaternion.identity, new Vector3(destructionStateRadius * 2.0f, 0.1f, destructionStateRadius * 2.0f));
        }
    }

    private void ActivateDestructionVFX(bool activate = true)
    {
        if (activate)
        {
            directionArrow.SetActive(true);
            directionArrow.GetComponent<ParticleSystem>().Play(true);
            windVFX_1.SetActive(true);
            windVFX_2.SetActive(true);
            dustVFX.SetActive(true);
        }
        else 
        {
            windVFX_1.SetActive(false);
            windVFX_2.SetActive(false);
            dustVFX.SetActive(false);
            directionArrow.SetActive(false);
        }
    }

    private void Disable()
    {
        isDisabled = true;
    }
}
