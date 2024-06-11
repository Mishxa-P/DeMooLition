using TMPro;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private TMP_Text goalText;
    [SerializeField] private bool collectableItemGoal = false;
    [SerializeField] private string collectableItemName;
    private int currentValue = 0;
    private int goalValue = 0;
    private bool completed = false;
    private void OnEnable()
    {
        if (!collectableItemGoal)
        {
            LevelEventManager.OnGoalIncreased.AddListener(UpdateGoal);
            LevelEventManager.OnGoalProgressUpdated.AddListener(UpdateGoalProgress);
        }
        else
        {
            LevelEventManager.OnGoalCollectableItemIncreased.AddListener(UpdateCollectableItemGoal);
            LevelEventManager.OnGoalCollectableItemProgressUpdated.AddListener(UpdateCollectableItemGoalProgress);
        }
    }
    private void UpdateGoal(int value)
    {
        goalValue += value;
        goalText.text = currentValue.ToString() + " / " + goalValue.ToString();
    }
    private void UpdateCollectableItemGoal(int value, string name)
    {
        if (collectableItemName == name)
        {
            goalValue += value;
            goalText.text = currentValue.ToString() + " / " + goalValue.ToString();
        }
    }
    private void UpdateGoalProgress(int value)
    {
        currentValue += value;
        if (currentValue < goalValue)
        {
            goalText.text = currentValue.ToString() + " / " + goalValue.ToString();
           
        }
        else
        {
            goalText.text = goalValue.ToString() + " / " + goalValue.ToString();
            if (!completed)
            {
                LevelEventManager.SendOneOfGoalsCompleted();
                completed = true;
            }
        }
    }
    private void UpdateCollectableItemGoalProgress(int value, string name)
    {
        if (collectableItemName == name)
        {
            currentValue += value;
            if (currentValue < goalValue)
            {
                goalText.text = currentValue.ToString() + " / " + goalValue.ToString();
            }
            else
            {
                goalText.text = goalValue.ToString() + " / " + goalValue.ToString();
                if (!completed)
                {
                    LevelEventManager.SendOneOfGoalsCompleted();
                    completed = true;
                }
            }
        }
    }
}
