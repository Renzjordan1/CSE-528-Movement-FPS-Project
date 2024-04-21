using UnityEngine;
using System.Collections;
using TMPro;

public class PlayerCharacter : MonoBehaviour 
{
    [SerializeField] private TMP_Text hp_txt;
    [SerializeField] GameObject gameOverMenu = null;
    [SerializeField] private TMP_Text kills_txt;



	private int _health;
    CharacterController cc;

    private int kills = 0;
    private bool isOver = false;
    

    void Start() 
    {
        gameOverMenu.SetActive(false);

		_health = 150;
        cc = (CharacterController)GetComponent<CharacterController>();
        //if (cc.isTrigger)
        //    Debug.Log("Palyer's Character controller is a trigger");
        //else
         //   Debug.Log("Palyer's Character controller is not a trigger");

    }

	public void Hurt(int damage) 
    {
		_health -= damage;

        if(_health <= 0){
            _health = 0;
            Time.timeScale = 0;
		    gameOverMenu.SetActive(true);

            isOver = true;

            Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
        }
        else{
            gameOverMenu.SetActive(false);
        }


		Debug.Log("Health: " + _health);
        hp_txt.text = "HP: " + _health;
        if(_health >= 100){
            hp_txt.color = Color.green;
        }
        else if(_health >= 50){
            hp_txt.color = Color.yellow;
        }
        else{
            hp_txt.color = Color.red;
        }

        //if (cc.isTrigger)
        //    Debug.Log("Palyer's Character controller is a trigger");
        //else
        //    Debug.Log("Palyer's Character controller is not a trigger");

    }

    public bool GetIsOver() { return isOver; }

    public void updateKills()
    {
        kills += 1;
        kills_txt.text = "Kills: " + kills;
    }
}
