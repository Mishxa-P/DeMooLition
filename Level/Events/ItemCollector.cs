
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    [SerializeField] private int goalItemCount = 10;
    [SerializeField] private string collectableItemName;
    private int currentItemCount = 0;
    private void Start()
    {
        LevelEventManager.SendGoalIncreased(goalItemCount, collectableItemName);
    }
    private void OnTriggerEnter(Collider other)
    {
        GameObject player = other.gameObject;
        if (player.tag == "Player")
        {
            int count = player.GetComponentInChildren<PlayerItems>().RemoveItemsByName(collectableItemName);
            if (count > 0)
            {
                currentItemCount += count;
                if (currentItemCount <= goalItemCount)
                {
                    LevelEventManager.SendGoalProgressed(count, collectableItemName);
                }
                Debug.Log("Player brought items! Current number of items brought = " + currentItemCount); 
            }
        } 
    }
}
