using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRadius : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer renderer;
    [SerializeField] GameObject poofVfx;
    [SerializeField] float poofVfxduration = 1.5f;
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
            if (idx == -1)
            {
                var p = other.gameObject.transform.position;
                Vector3 pos = new Vector3(p.x, p.y + 1f, p.z);
                GameObject poof = Instantiate(poofVfx, pos, Quaternion.identity);
                Destroy(poof, poofVfxduration);
                other.GetComponent<CivilianController>().SetTarget(transform.parent.transform, material);
            }
        }

       
        
    }

    
}
