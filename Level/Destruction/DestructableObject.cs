using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class DestructableObject : MonoBehaviour
{
    [SerializeField] private float explosionForce = 500.0f;
    [SerializeField] private float explosionRadius = 5.0f;
    [SerializeField] private float upwardsModifier = 5.0f;
    [SerializeField] private float destroyTime = 3.75f;
    [SerializeField] private bool goalDestroyObject = false;
    [SerializeField] private GameObject floatingTextPrefab;
    private void Start()
    {
        if (goalDestroyObject)
        {
            LevelEventManager.SendGoalIncreased(1);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject enemy = collision.gameObject;
        if (enemy.tag == "Bull" && enemy.GetComponentInParent<EnemyController>().CanDestroyObjects || enemy.tag == "Projectile")
        {
            GetComponent<BoxCollider>().enabled = false;
            if (GetComponent<NavMeshObstacle>() != null)
            {
                GetComponent<NavMeshObstacle>().enabled = false;
            }
            Explosion();
            StartCoroutine(DestroyCoroutine());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        GameObject enemy = other.gameObject;
        if (enemy.tag == "Bull" && enemy.GetComponentInParent<EnemyController>().CanDestroyObjects)
        {
            GetComponent<BoxCollider>().enabled = false;
            Explosion();
            StartCoroutine(DestroyCoroutine());
        }
    }
    private void Explosion()
    {
        Rigidbody[] rbs = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rbs)
        {
            rb.constraints = RigidbodyConstraints.None;
            rb.AddExplosionForce(explosionForce, rb.transform.position + new Vector3(Random.Range(1.0f, 5.0f), 0.0f, Random.Range(-5.0f, 5.0f)), explosionRadius, upwardsModifier);
        }
    }
    private IEnumerator DestroyCoroutine()
    {
        if (goalDestroyObject)
        {
            LevelEventManager.SendGoalProgressed(1);
            if (GetComponent<Outline>() != null)
            {
                GetComponent<Outline>().enabled = false;
            }
        }
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }
}
