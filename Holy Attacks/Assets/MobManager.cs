using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobManager : MonoBehaviour
{
    int numEnemiesToBeAttacked, numFollowers;
    bool isMobAttacking, gotoNextEnemy;

    List<GameObject> followers = new List<GameObject>();
    List<GameObject> enemiesToBeAttacked = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (numEnemiesToBeAttacked > 0 && !isMobAttacking)  //remove numEnemeiestobeattacked
        {
            isMobAttacking = true;
            //StartCoroutine(MobAttack());
        }
    }
    /*
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
    }*/
}
