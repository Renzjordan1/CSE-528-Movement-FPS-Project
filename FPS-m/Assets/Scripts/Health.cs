using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int curHealth = 0;
    public int maxHealth = 150;
    public HealthBar healthBar;
    [SerializeField] private TMP_Text hp_txt;
    public GameObject image;
    [SerializeField] GameObject gameOverMenu = null;

    void Start()
    {
        //Start at full hp
        gameOverMenu.SetActive(false);
        curHealth = maxHealth;
    }
    void Update()
    {
        //update HP HUD
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
        //take damage
        curHealth -= damage;

        //died
        if(curHealth <= 0)
        {
            curHealth = 0; 

            Time.timeScale = 0;
		    gameOverMenu.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
        }
        // else{
        //     gameOverMenu.SetActive(false);
        // }

        healthBar.SetHealth( (float)curHealth);
        Debug.Log("HealthBar Value: " + curHealth);
    }
}