using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRadius : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer renderer;
    Material material;

    GameSession gameSession;
    // Start is called before the first frame update
    void Start()
    {
        material = renderer.material;
        gameSession = FindObjectOfType<GameSession>();
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
            int idx = gameSession.GetFollowerNumber(other.gameObject) - 1;
            if(idx == -1)
            other.GetComponent<CivilianController>().SetTarget(transform.parent.transform, material);
        }

       
        
    }

    
}
