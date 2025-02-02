using UnityEngine;
using UnityEngine.UI; 

public class Health : MonoBehaviour
{
    public float health; 
    public float maxHealth;
    public Image healthBar; 

    private void Start()
    {
        maxHealth = health; 
    }

    void Update()
    {
        healthBar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1); 
    }
}