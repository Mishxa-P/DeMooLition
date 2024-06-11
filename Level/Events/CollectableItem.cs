using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    [SerializeField] private string itemName;
    public string ItemName
    {
        get { return itemName; }
        private set { itemName = value; }
    }
}
