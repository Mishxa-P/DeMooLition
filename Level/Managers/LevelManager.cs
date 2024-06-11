using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int totalGoalsCount = 1;
    [SerializeField] private GameObject congratulationsUI; //temp

    private int completedGoalsCount = 0;
    private void OnEnable()
    {
        LevelEventManager.OnLevelFailed.AddListener(RestartLevel);
        LevelEventManager.OnOneOfGoalsCompleted.AddListener(CompleteGoal);
    }
    private void RestartLevel()
    {
        Debug.Log("LEVEL FAILED!");

        // temp
        if (completedGoalsCount == totalGoalsCount)
        {
            return;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void CompleteGoal()
    {
        completedGoalsCount++;
        Debug.Log("TotalGoals = " + totalGoalsCount + " CompletedGoals = " + completedGoalsCount);
        if (completedGoalsCount >= totalGoalsCount)
        {
            Debug.Log("LEVEL COMPLETED!");
            StartCoroutine(CompleteLevel());
            LevelEventManager.SendLevelCompleted();
        }
    }

    private IEnumerator CompleteLevel()
    {
        congratulationsUI.SetActive(true);
        congratulationsUI.GetComponent<Animator>().SetTrigger("Play");
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene("MainMenu");
    }
}
