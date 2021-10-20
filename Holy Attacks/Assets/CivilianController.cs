using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CivilianController : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;

    Transform target;
    bool followPlayer = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(followPlayer)
        {
            agent.SetDestination(target.position);
        }
    }

   

    public void SetTarget(Transform transform)
    {
        target = transform;
        followPlayer = true;
    }

   
}
