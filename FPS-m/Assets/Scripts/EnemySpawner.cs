using UnityEngine;
using System.Collections;
using TMPro;

public class EnemyManager2 : MonoBehaviour
{

    [SerializeField] private GameObject enemyPrefab1;
    [SerializeField] private GameObject enemyPrefab2;
    [SerializeField] private GameObject enemyPrefab3;

    [SerializeField] private TMP_Text wave_txt;

    private int wave = 0;
    public int numEnemies = 0;
    private GameObject[] _enemy;

    void Start()
    {
        // if (numEnemies < 1) numEnemies = 1;

        _enemy = new GameObject[numEnemies];
    }

    void Update()
    {   
        if(numEnemies == 0){
            wave += 1;
            numEnemies = wave;
            _enemy = new GameObject[numEnemies];
            wave_txt.text = "Wave: " + wave;

            for (int i = 0; i < numEnemies; i++)
            {
                if (_enemy[i] == null)
                {
                    float value = Random.value;
                    if (value < 1.0f / 3.0f)
                        _enemy[i] = Instantiate(enemyPrefab1) as GameObject;
                    else if (value < 2.0f / 3.0f)
                        _enemy[i] = Instantiate(enemyPrefab2) as GameObject;
                    else
                        _enemy[i] = Instantiate(enemyPrefab3) as GameObject;


                    Transform spawnpoint =  gameObject.transform;

                    int x = Random.Range(0, 2);
                    switch(x){
                    case 0:
                        _enemy[i].transform.position = new Vector3(0, 1, 0);
                        break;
                    
                    case 1:
                        _enemy[i].transform.position = spawnpoint.position;
                        break;
                    }


                    float angle = Random.Range(0, 360);
                    _enemy[i].transform.Rotate(0, angle, 0);
                }
            }
        }


    }
}