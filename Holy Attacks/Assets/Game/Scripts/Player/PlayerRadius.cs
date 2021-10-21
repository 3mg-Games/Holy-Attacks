using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRadius : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer SMR;
    public Material material;
    // Start is called before the first frame update
    void Start()
    {
        material = SMR.material;
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
            other.GetComponent<CivilianController>().SetTarget(transform.parent.transform, material);
        }

       
        
    }

    
}
