using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRadius : MonoBehaviour
{
    Animator animator;

    EnemyController enemy;

    bool civiliansAreNear = false;

    GameSession gameSession;
    // Start is called before the first frame update
    void Start()
    {
        animator = transform.parent.GetComponent<Animator>();
        enemy = transform.parent.GetComponent<EnemyController>();
        gameSession = FindObjectOfType<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!civiliansAreNear && other.tag == "Civilian")
        {
            civiliansAreNear = true;
            //GameObject target = gameSession.GetNewFollower(transform.parent.gameObject);
           enemy.SetTarget(other.gameObject);

            
            //Debug.Log("fasfsdjf");
          //  animator.SetBool("Punch", true);
        }
    }
}
