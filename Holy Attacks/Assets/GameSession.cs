using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    [SerializeField] float followerSpacingIncrement = 0.5f;

    int numFollowers;
    List<GameObject> followers = new List<GameObject>();

    int numEnemiesToBeAttacked;
    List<GameObject> enemiesToBeAttacked = new List<GameObject>();

    bool isMobAttacking = false;

    bool gotoNextEnemy = false;
    int i;
    // Start is called before the first frame update
    void Awake()
    {
        numFollowers = 0;
        numEnemiesToBeAttacked = 0;
        i = 0;
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
    }

    private void DecrementNumFollowers()
    {
        numFollowers--;
    }

    public void AddEnemiesToBeAttacked(GameObject target)
    {
        enemiesToBeAttacked.Add(target);
        numEnemiesToBeAttacked++;
    }

    public void RemoveEnemyFromList(GameObject target)
    {
        enemiesToBeAttacked.Remove(target);
        Destroy(target);
        numEnemiesToBeAttacked--;
        gotoNextEnemy = true;
    }

    private IEnumerator MobAttack()
    {
        
            while (numEnemiesToBeAttacked > 0)
            {
                gotoNextEnemy = false;
                GameObject target = enemiesToBeAttacked[0];
                for (int j = 0; j < numFollowers; j++)
                {
                    followers[j].GetComponent<CivilianController>().SetTargetAsEnemy(target.transform);
                }
                i++;
                yield return new WaitUntil(() => gotoNextEnemy == true);
            }

        isMobAttacking = false;
    }

  
}
