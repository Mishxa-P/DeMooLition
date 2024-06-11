using UnityEngine;

public class ArrowPointer : MonoBehaviour
{
    [SerializeField] private Transform owner;
    [SerializeField] private Transform goal;
    [SerializeField] private float rotationSpeed = 5.0f;
    [SerializeField] private float distanceFromOwner = 2.0f;
    [SerializeField] private float heightAboveHero = 0.0f;
    [SerializeField] private float destroyDistance = 10.0f;
    private void Update()
    {
        if (owner != null && goal != null)
        {
            Vector3 directionToTarget = goal.position - owner.position;
            if (directionToTarget.magnitude <= destroyDistance)
            {
                Destroy(gameObject);
            }
            directionToTarget.y = 0; 
            Vector3 direction = directionToTarget.normalized;
            float x = owner.position.x + direction.x * distanceFromOwner;
            float z = owner.position.z + direction.z * distanceFromOwner;
            float y = owner.position.y + heightAboveHero;
            transform.position = new Vector3(x, y, z);

            direction = owner.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            float newYRotation = Mathf.LerpAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, newYRotation, transform.eulerAngles.z);
    
        }
    }
}
