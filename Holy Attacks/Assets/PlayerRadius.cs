using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRadius : MonoBehaviour
{
   
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger happened");
        

        if(other.tag == "Civilian")
        {
            other.GetComponent<CivilianController>().SetTarget(transform.parent.transform);
        }

       
        
    }

    
}
