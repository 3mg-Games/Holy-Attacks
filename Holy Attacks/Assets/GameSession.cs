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

    public int numFollowers;
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
        numOfFollowersText.text = numFollowers.ToString();

        if (numEnemiesToBeAttacked > 0 && !isMobAttacking && !player.GetIsPlayerMoving())  //remove numEnemeiestobeattacked
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
        Destroy(follower);
    }

    public float GetFollowerSpacingIncrement()
    {
        return followerSpacingIncrement;
    }

    private void IncrementNumFollowers()
    {
        numFollowers++;
        //numOfFollowersText.text = numFollowers.ToString();
    }

    private void DecrementNumFollowers()
    {
        numFollowers--;
        //numOfFollowersText.text = numFollowers.ToString();
    }

    public void AddEnemiesToBeAttacked(GameObject target)
    {
        enemiesToBeAttacked.Add(target);
        numEnemiesToBeAttacked++;
    }

    public void RemoveEnemyFromList(GameObject target, bool exit)
    {
        enemiesToBeAttacked.Remove(target);
        numEnemiesToBeAttacked--;

        if (exit)
        {
            if (numEnemiesToBeAttacked <= 0)
            {
                foreach (GameObject follower in followers)
                {
                    follower.GetComponent<CivilianController>().SetTargetASPlayer();
                }
            }

            else
            {
                StartCoroutine(MobAttack());
            }
        }

        
    }

    public void KillEnemy(GameObject target)
    {
        //enemiesToBeAttacked.Remove(target);
        //numEnemiesToBeAttacked--;
        RemoveEnemyFromList(target, false);

        gotoNextEnemy = true;
        

        GameObject closestFollower = GetNewTarget(followers, target);
        RemoveFollower(closestFollower);

        Destroy(target);
    }

    private IEnumerator MobAttack()
    {
        
           // while (numEnemiesToBeAttacked > 0)
           // {
             //   gotoNextEnemy = false;
            //GameObject target = enemiesToBeAttacked[0];
                GameObject target = GetNewTarget(enemiesToBeAttacked, player.gameObject);
                if(target == null)
                 {
                     yield return null;
                 }        

        
                 
                for (int j = 0; j < numFollowers; j++)
                {
            GameObject follower = followers[j];
            if(follower != null)
            {
                follower.GetComponent<CivilianController>().SetTargetAsEnemy(target.transform);
            }
                    
                }
        //    i++;
        // yield return new WaitUntil(() => gotoNextEnemy == true);
                isMobAttacking = false;
                yield return null;
          //  }

       // isMobAttacking = false;
    }

    

    public GameObject GetNewEnemy()
    {
        GameObject target = GetNewTarget(enemiesToBeAttacked, player.gameObject);
        return target;
    }
    

    private GameObject GetNewTarget(List<GameObject> targetList, GameObject target2)
    {
        

        if (targetList.Count == 0)
            return null;

        else
        {

            GameObject target = targetList[0];
            Vector3 target2Pos = target2.transform.position;

        float shortestDistance = Vector3.Distance(target2Pos, targetList[0].transform.position);

        

       

            for (int itr = 1; itr < targetList.Count; itr++)
            {
                float distance = Vector3.Distance(target2Pos, targetList[itr].transform.position);

                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    target = targetList[itr];
                }
            }

            return target;
        }
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

    public int GetFollowerNumber(GameObject follower)
    {
        int idx = followers.IndexOf(follower);
        return idx + 1;
    }
  
}
