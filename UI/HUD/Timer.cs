using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
   [SerializeField] private float timeInSeconds = 60;
   [SerializeField] private TextMeshProUGUI timerTMP;

   [Space(5)]
   [Header("Development")]
   [SerializeField] private bool stopTimer = false;

    private bool timerIsRunning = false;

    private void Start()
    {
        // Запуск таймера
        timerIsRunning = true;
    }

    private void Update()
    {
        if (timerIsRunning && !stopTimer)
        {
            if (timeInSeconds > 0)
            {
                timeInSeconds -= Time.deltaTime;
                DisplayTime(timeInSeconds);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeInSeconds = 0;
                timerIsRunning = false;
                LevelEventManager.SendLevelFailed();
            }
        }
    }

    private void DisplayTime(float timeToDisplay)
    {
        int seconds = Mathf.RoundToInt(timeToDisplay);
        timerTMP.text = seconds.ToString();
    }
}
