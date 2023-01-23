using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Game")]
    public Player player;
    public GameObject enemyContainer;

    [Header("UI")]
    public Text healthText;
    public Text ammoText;
    public Text enemytext;
    public Text infoText;

    private void Start()
    {
        infoText.gameObject.SetActive(false); 
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "Health: " + player.health;
        ammoText.text = "Ammo: " + player.Ammo;

        int aliveEnemies = 0;
        foreach(Enemy enemy in enemyContainer.GetComponentsInChildren<Enemy>()) { 
            
            if(enemy.Killed == false)
            {
                aliveEnemies++;
            }
        }
        enemytext.text = "Enemis: " + aliveEnemies;

        if(aliveEnemies == 0)
        {
            infoText.gameObject.SetActive (true);
            infoText.text = "Winner Winner\nChicken Dinner";
        }
        if(player.Killed == true)
        {
            infoText.gameObject.SetActive(true);
            infoText.text = "Try Again\nNext Time";
        }
    }

}
