using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    bool isReadyToBeAttacked = false;
    bool isEnemyAttacked = false;

    PlayerMovement player;
    // Start is called before the first frame update
    void Start()
    {
       player = FindObjectOfType<PlayerMovement>().GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isReadyToBeAttacked && player.GetIsPlayerMoving() && !isEnemyAttacked)
        {
            isReadyToBeAttacked = false;
            isEnemyAttacked = true;
            player.AttackEnemy(transform);
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
        }
    }


}
