using System.Collections;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    [SerializeField] private int fruitSpawnAmount = 1;
    [SerializeField] private GameObject fruit;
    [SerializeField] Transform spawnPoint;

    [Header("Tree Shake Animation")]
    [SerializeField] private float duration = 2.5f;
    [SerializeField] private float shakeMagnitude = 5.0f;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Start()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }
    private void OnTriggerEnter(Collider other)
    {
        GameObject enemy = other.gameObject;
        if (enemy.tag == "Bull" && enemy.GetComponentInParent<EnemyController>().CanDestroyObjects)
        {
            SpawnApple();
        }
    }

    [ContextMenu("Spawn Appple")]
    public void SpawnApple()
    {
        for (int i = 0; i < fruitSpawnAmount; i++)
        {
            Instantiate(fruit, spawnPoint.position + new Vector3(0.0f, 0.0f, Random.Range(-1.5f, 1.5f)), Quaternion.identity);
        }
        StartCoroutine(ShakeAnimation());
    }
    private IEnumerator ShakeAnimation()
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float percentComplete = elapsed / duration;
            float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

            float y = Mathf.PerlinNoise(Time.time * 2f, 0f) * 2f - 1f;
            float z = Mathf.PerlinNoise(0f, Time.time * 2f) * 2f - 1f;

            y *= shakeMagnitude * damper;
            z *= shakeMagnitude * damper;

            transform.localRotation = initialRotation * Quaternion.Euler(0, y, z);

            yield return null;
        }

        transform.localPosition = initialPosition;
        transform.localRotation = initialRotation;
    }
}
