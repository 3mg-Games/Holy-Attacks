using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float maxDistance;
    Vector3 initialPos;
    bool hasProjectileFired = false;
    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasProjectileFired)
        {
            if (Vector3.Distance(initialPos, transform.position) >= maxDistance)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetMaxDistance(float distance)
    {
        maxDistance = distance;
        hasProjectileFired = true;
    }
}
