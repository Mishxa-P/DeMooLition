
using UnityEngine.Events;

public class LevelEventManager
{
    public static UnityEvent<float> OnPlayerDashed = new UnityEvent<float>();

    public static UnityEvent<int> OnPlayerDamaged = new UnityEvent<int>();

    public static UnityEvent<int> OnGoalIncreased = new UnityEvent<int>();

    public static UnityEvent<int, string> OnGoalCollectableItemIncreased = new UnityEvent<int, string>();

    public static UnityEvent<int> OnGoalProgressUpdated = new UnityEvent<int>();

    public static UnityEvent<int, string> OnGoalCollectableItemProgressUpdated = new UnityEvent<int, string>();

    public static UnityEvent OnLevelFailed = new UnityEvent();

    public static UnityEvent OnLevelCompleted = new UnityEvent();

    public static UnityEvent OnOneOfGoalsCompleted = new UnityEvent();
    public static void SendPlayerDashed(float cooldownTime)
    {
        OnPlayerDashed?.Invoke(cooldownTime);
    }
    public static void SendPlayerDamaged(int currentHealth)
    {
        OnPlayerDamaged?.Invoke(currentHealth);
    }
    public static void SendGoalIncreased(int value)
    {
        OnGoalIncreased?.Invoke(value);
    }
    public static void SendGoalIncreased(int value, string itemName)
    {
        OnGoalCollectableItemIncreased?.Invoke(value, itemName);
    }
    public static void SendGoalProgressed(int value)
    {
        OnGoalProgressUpdated?.Invoke(value);
    }
    public static void SendGoalProgressed(int value, string itemName)
    {
        OnGoalCollectableItemProgressUpdated?.Invoke(value, itemName);
    }
    public static void SendLevelFailed()
    {
        OnLevelFailed?.Invoke();
    }
    public static void SendLevelCompleted()
    {
        OnLevelCompleted?.Invoke();
    }
    public static void SendOneOfGoalsCompleted()
    {
        OnOneOfGoalsCompleted?.Invoke();
    }
}