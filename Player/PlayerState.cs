using UnityEngine;
using System.Collections;
public class PlayerState : MonoBehaviour
{
    [SerializeField] private Vector3 stunMaxAddPosition;
    [SerializeField] private float stunDuration;
    [SerializeField] private float invTime = 3.0f;
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int healAmount = 1;
    [SerializeField] private float healTime = 10.0f;
    public bool IsStunned { get; private set; } = false;
    private bool invulnerable = false;
    private float currentInvTime = 0.0f;

    private bool shouldBeHealed = false;
    private float currentHealTime = 0.0f;
    private float stunCurrentTime;
    private Vector3 stunPositionChange; // temp

    private CharacterController characterController;
    public Health Health { get; private set; }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        Health = new Health(maxHealth);
    }
    private void Start()
    {
        currentHealTime = healTime;
        currentInvTime = invTime;
    }
    private void Update()
    {
        if (IsStunned) // temp
        {
            PlayStunAnimation();
        }
        if (shouldBeHealed)
        {
            currentHealTime -= Time.deltaTime;
            if (currentHealTime <= 0)
            {
                Health.Damage(-healAmount);
                shouldBeHealed = false;
                currentHealTime = healTime;
                LevelEventManager.SendPlayerDamaged(Health.CurrentHealth);
            }
        }
        if (invulnerable)
        {
            currentInvTime -= Time.deltaTime;
            if (currentInvTime <= 0)
            {
                invulnerable = false;
                currentInvTime = invTime;
                GetComponentInChildren<Renderer>().material.color = Color.green; // temp
            }
        }
    }
    public void Stun(int damage)
    {
        if (IsStunned || invulnerable)
        {
            return;
        }
        Health.Damage(damage);
        shouldBeHealed = true;
        invulnerable = true;
 
        LevelEventManager.SendPlayerDamaged(Health.CurrentHealth);
        //play animation
        transform.rotation = Quaternion.Euler(-90.0f, transform.rotation.y, transform.rotation.z);  // temp
        GetComponentInChildren<Renderer>().material.color = Color.red; // temp
        //Debug.Log("Player is stunned");
        StartCoroutine(StunCorotine());
    }

    private void PlayStunAnimation() // temp
    {
        stunCurrentTime += Time.deltaTime;
        if (stunCurrentTime < stunDuration / 2.0f)
        {
            transform.position += stunPositionChange * Time.deltaTime;
        }
        else
        {
            transform.position -= stunPositionChange * Time.deltaTime;
        }
    }


    private IEnumerator StunCorotine()
    {
        IsStunned = true;
        characterController.enabled = false;
        stunCurrentTime = 0.0f;
        stunPositionChange = (stunMaxAddPosition / stunDuration) * 2.0f;
        yield return new WaitForSeconds(stunDuration);
        transform.rotation = Quaternion.Euler(0.0f, transform.rotation.y, transform.rotation.z);  // temp
        IsStunned = false;
        characterController.enabled = true;
    }
}
