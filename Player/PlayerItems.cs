
using System.Collections.Generic;
using UnityEngine;

public class PlayerItems : MonoBehaviour
{
    [SerializeField] private int maxItemCount = 3;
    [SerializeField] private GameObject floatingTextPrefab;
    
    private List<string> items = new List<string>();
    public int CollectedItems { get; private set; } = 0;
    private void OnTriggerEnter(Collider other)
    {
        GameObject item = other.gameObject;

        if (item.GetComponent<CollectableItem>() != null)
        {
            if (items.Count < maxItemCount)
            {
                Debug.Log("Player collected " + item.ToString());
                items.Add(item.GetComponent<CollectableItem>().ItemName);
                Destroy(item);

                //temp
                if (floatingTextPrefab != null)
                {
                    GameObject floatingText = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
                    floatingText.GetComponentInChildren<TextMesh>().text = "+1 " + item.GetComponent<CollectableItem>().ItemName;
                    Destroy(floatingText, 2.5f);
                }
            }  
            else
            {
                //temp
                if (floatingTextPrefab != null)
                {
                    GameObject floatingText = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
                    floatingText.GetComponentInChildren<TextMesh>().text = maxItemCount.ToString();
                    floatingText.GetComponentInChildren<TextMesh>().color = Color.red;
                    Destroy(floatingText, 2.5f);
                }
            }
        }
    }

    public int RemoveItemsByName(string name)
    {
        int count = items.Count;
        items.RemoveAll(word => word == name);
        count -= items.Count;

        //temp
        if (floatingTextPrefab != null)
        {
            if (count > 0)
            {
                GameObject floatingText = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
                if (count == 1)
                {
                    floatingText.GetComponentInChildren<TextMesh>().text = "- " + count + " " + name;
                }
                else
                {
                    floatingText.GetComponentInChildren<TextMesh>().text = "- " + count + " " + name + "s"; //temp
                }
                Destroy(floatingText, 2.5f);
            }    
        }

        return count;
    }
}
