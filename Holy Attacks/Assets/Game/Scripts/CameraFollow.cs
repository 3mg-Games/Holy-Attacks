using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform player;

    float camZ;
    Vector3 delta;
    // Start is called before the first frame update
    void Start()
    {
        delta.x = player.position.x - transform.position.x;
        delta.y = player.position.y - transform.position.y;
        delta.z = player.position.z - transform.position.z;
        
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(player.position.x, transform.position.y, player.position.z - delta.z);
    }

    // Update is called once per frame
   
      
}
