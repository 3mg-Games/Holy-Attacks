using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    bool isReadyToBeAttacked = false;
    bool hasEnemyBeenAttacked =  false; //false
    GameSession gameSession;

    PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
       player = FindObjectOfType<PlayerController>().GetComponent<PlayerController>();
        gameSession = FindObjectOfType<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
       /* if(isReadyToBeAttacked && !player.GetIsPlayerMoving() && !hadEnemyBeenAttacked)
        {
            isReadyToBeAttacked = false;
            hadEnemyBeenAttacked = true;
            player.AttackEnemy(transform);
        }*/

        if(isReadyToBeAttacked && !player.GetIsPlayerMoving() && !hasEnemyBeenAttacked)
        {
            isReadyToBeAttacked = false;
            hasEnemyBeenAttacked = true;
            gameSession.AddEnemiesToBeAttacked(gameObject);
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isReadyToBeAttacked = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            isReadyToBeAttacked = false;
            hasEnemyBeenAttacked = false;
            gameSession.RemoveEnemyFromList(gameObject);
        }
    }


}
