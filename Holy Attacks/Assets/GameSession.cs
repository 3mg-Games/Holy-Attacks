using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField] float followerSpacingIncrement = 0.5f;
    [SerializeField] TextMeshProUGUI numOfFollowersText;
    [SerializeField] GameObject pause;
    [SerializeField] GameObject resume;

    int numFollowers;
    List<GameObject> followers = new List<GameObject>();

    int numEnemiesToBeAttacked;
    List<GameObject> enemiesToBeAttacked = new List<GameObject>();

    bool isMobAttacking = false;

    bool gotoNextEnemy = false;
    int i;

    PlayerController player;
    // Start is called before the first frame update
    void Awake()
    {
        numFollowers = 0;
        numEnemiesToBeAttacked = 0;
        i = 0;
        
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }


    // Update is called once per frame
    void Update()
    {
        if(numEnemiesToBeAttacked > 0 && !isMobAttacking)  //remove numEnemeiestobeattacked
        {
            isMobAttacking = true;
            StartCoroutine(MobAttack());
        }
    }

    public int GetNumFollowers()
    {
        return numFollowers;
    }

    public void AddFollower(GameObject gameObject)
    {
        followers.Add(gameObject);
        IncrementNumFollowers();
    }

    public void RemoveFollower(GameObject follower)
    {
        followers.Remove(follower);
        DecrementNumFollowers();
    }

    public float GetFollowerSpacingIncrement()
    {
        return followerSpacingIncrement;
    }

    private void IncrementNumFollowers()
    {
        numFollowers++;
        numOfFollowersText.text = numFollowers.ToString();
    }

    private void DecrementNumFollowers()
    {
        numFollowers--;
        numOfFollowersText.text = numFollowers.ToString();
    }

    public void AddEnemiesToBeAttacked(GameObject target)
    {
        enemiesToBeAttacked.Add(target);
        numEnemiesToBeAttacked++;
    }

    public void RemoveEnemyFromList(GameObject target)
    {
        enemiesToBeAttacked.Remove(target);
        numEnemiesToBeAttacked--;
        
    }

    public void KillEnemy(GameObject target)
    {
        enemiesToBeAttacked.Remove(target);
        numEnemiesToBeAttacked--;
        gotoNextEnemy = true;
        Destroy(target);
    }

    private IEnumerator MobAttack()
    {
        
            while (numEnemiesToBeAttacked > 0)
            {
                gotoNextEnemy = false;
            //GameObject target = enemiesToBeAttacked[0];
                GameObject target = GetNewTarget();
                for (int j = 0; j < numFollowers; j++)
                {
                    followers[j].GetComponent<CivilianController>().SetTargetAsEnemy(target.transform);
                }
                i++;
                yield return new WaitUntil(() => gotoNextEnemy == true);
            }

        isMobAttacking = false;
    }

    private GameObject GetNewTarget()
    {
        Vector3 playerPos = player.transform.position;

        float shortestDistance = Vector3.Distance(playerPos, enemiesToBeAttacked[0].transform.position);

        GameObject target = enemiesToBeAttacked[0];

        for(int itr = 1; itr < enemiesToBeAttacked.Count; itr++)
        {
            float distance = Vector3.Distance(playerPos, enemiesToBeAttacked[itr].transform.position);

            if(distance < shortestDistance)
            {
                shortestDistance = distance;
                target = enemiesToBeAttacked[itr];
            }
        }

        return target;
    }

    public void Pause()
    {
       player.PausePlayer(true);
        

        //put enemy in idle too
        Time.timeScale = 0;
        pause.SetActive(false);
        resume.SetActive(true);
    }

    public void Resume()
    {
        player.PausePlayer(false);
        Time.timeScale = 1;
        resume.SetActive(false);
        pause.SetActive(true);
    }
  
}
