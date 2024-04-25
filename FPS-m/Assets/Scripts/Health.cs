using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    private int curHealth = 0;
    public int maxHealth = 150;
    public HealthBar healthBar;
    [SerializeField] private TMP_Text hp_txt;
    public GameObject image;

    void Start()
    {
        curHealth = maxHealth;

    }
    void Update()
    {
        hp_txt.text = "HP: " + curHealth;
        if(curHealth >= 100){
            image.GetComponent<Image>().color = Color.green;
        }
        else if(curHealth >= 50){
            image.GetComponent<Image>().color = Color.yellow;
        }
        else{
            image.GetComponent<Image>().color = Color.red;
        }

    }
    public void DamagePlayer( int damage )
    {
        curHealth -= damage;
        if(curHealth <= 0)
            curHealth = 0; 
        healthBar.SetHealth( curHealth );
        Debug.Log("HealthBar Value: " + curHealth);
    }
}