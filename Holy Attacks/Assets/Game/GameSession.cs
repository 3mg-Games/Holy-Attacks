using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class GameSession : MonoBehaviour
{
    [SerializeField] float followerSpacingIncrement = 0.5f;
    [SerializeField] TextMeshProUGUI numOfFollowersText;
    [SerializeField] GameObject pause;
    [SerializeField] GameObject resume;
    [SerializeField] CinemachineVirtualCamera originalCam;
    [SerializeField] float durationOfEnemyConfusion = 3f;
    //[SerializeField] float dist

    bool isZoomedOut = false;

     int numFollowers;
    List<GameObject> followers = new List<GameObject>();

     int numEnemiesToBeAttacked;
    List<GameObject> enemiesToBeAttacked = new List<GameObject>();

    bool isMobAttacking = false;

    bool gotoNextEnemy = false;
    int i;

    PlayerController player;

    List<GameObject> onStayFollowers = new List<GameObject>();
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

    public void AddOnStayFollower(GameObject follower)
    {
        onStayFollowers.Add(follower);
    }

    public void RemoveOnStayFollower(GameObject follower)
    {
        onStayFollowers.Remove(follower);
    }

    // Update is called once per frame
    void Update()
    {
        numOfFollowersText.text = numFollowers.ToString();

        if(numFollowers > 5 && !isZoomedOut)
        {
            isZoomedOut = true;
            originalCam.Priority = 1;
        }

        if (numEnemiesToBeAttacked > 0 && !isMobAttacking && !player.GetIsPlayerMoving())  //remove numEnemeiestobeattacked
        {
            isMobAttacking = true;
            StartCoroutine(MobAttack());
           // Debug.Log("mob attacking");
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

            else if(!player.GetIsPlayerMoving())
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

    public GameObject GetNewFollower(GameObject enemy)
    {
        GameObject target = GetNewTarget(followers, enemy);
        return target;
    }
    

    private GameObject GetNewTarget(List<GameObject> targetList, GameObject target2)
    {
        

        if (targetList.Count == 0 || target2 == null)
            return null;

        else
        {

            
            Vector3 target2Pos = target2.transform.position;
            int itr = 0;
            
            while (itr < targetList.Count && targetList[itr] == null)
            {
                itr++;

            }

            if (itr >= targetList.Count)
                return null;


            GameObject target = targetList[itr];
            float shortestDistance = Vector3.Distance(target2Pos, targetList[itr].transform.position);

            itr++;

       

            for (; itr < targetList.Count; itr++)
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

    public float GetDurationOfEnemyConfusion()
    {
        return durationOfEnemyConfusion;
    }
  
}
