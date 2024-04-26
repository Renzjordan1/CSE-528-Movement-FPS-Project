using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    public Health playerHealth;
    private void Start()
    {
        playerHealth = GameObject.Find("Player").GetComponent<Health>();
        healthBar = GetComponent<Slider>();
        healthBar.maxValue = playerHealth.maxHealth;
        healthBar.minValue = 0;
        healthBar.value = playerHealth.maxHealth;
        Debug.Log("HealthBar Value: " + healthBar.value);
    }
    public void SetHealth(float hp)
    {
        healthBar.value = hp;
    }
}