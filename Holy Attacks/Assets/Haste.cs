using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Haste : MonoBehaviour
{
    [SerializeField] float speedIncPercentage = 20f;
    [SerializeField] float hasteTime = 5f;

    GameSession gameSession;

    PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            List<GameObject> followers = gameSession.GetFollowers();
            player.PlayerHaste(speedIncPercentage, hasteTime);

            foreach(GameObject follower in followers)
            {
                if(follower != null)
                {
                    follower.GetComponent<CivilianController>().CivilianHaste(speedIncPercentage, hasteTime);
                }
            }

            Destroy(gameObject);
        }
    }
}
