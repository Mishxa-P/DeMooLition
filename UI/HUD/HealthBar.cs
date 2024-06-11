using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    [SerializeField] private PlayerState playerState;
    [SerializeField] private List<Image> health;
    [SerializeField] private Sprite fullHealth;
    [SerializeField] private Sprite emptyHealth; 

    private void OnEnable()
    {
        LevelEventManager.OnPlayerDamaged.AddListener(UpdateHealth);
    }
    private void Start()
    {
        foreach (var h in health) 
        { 
            h.sprite = fullHealth;
        }
    }
    private void UpdateHealth(int currentHealth)
    {
        foreach (var h in health)
        {
            h.sprite = fullHealth;
        }
        for (int i = health.Count - 1; i >= currentHealth; i--)
        {
            health[i].sprite = emptyHealth;
        }
        if (currentHealth == 0)
        {
            LevelEventManager.SendLevelFailed();
        }
    }
}
