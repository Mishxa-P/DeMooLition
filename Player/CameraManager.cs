using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineBrain))]
public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform bull;
    [SerializeField] private Transform player;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float smoothTime = 0.25f;

    public static CameraManager Singleton;

    private CinemachineFramingTransposer transposer;
    private float initialCameraDistance;
    private float vel;
    private void Awake()
    {
        if (Singleton != null)
        {
            Debug.LogError("Camera manager is already exist!");
        }
        else
        {
            Singleton = this;
            transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
    }
    private void Start()
    {
        initialCameraDistance = transposer.m_CameraDistance;
    }
    private void Update()
    {
        float newCameraDistance = initialCameraDistance + Vector3.Distance(player.position, bull.position) / 2.5f;
        transposer.m_CameraDistance = Mathf.SmoothDamp(transposer.m_CameraDistance, newCameraDistance, ref vel, smoothTime);
    }
}

