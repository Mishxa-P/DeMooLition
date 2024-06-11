using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 30.0f;
    [SerializeField] private bool rightDirection = true;

    private Vector3 rotationVel; 
    private void Start()
    {
        if (rightDirection)
        {
            rotationVel = new Vector3(0.0f, rotateSpeed, 0.0f);
        }
        else
        {
            rotationVel = new Vector3(0.0f, -rotateSpeed, 0.0f);
        }
      
    }
    private void Update()
    {
        transform.Rotate(rotationVel * Time.deltaTime);
    }
}
