using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour
{
    PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    public void StaffAttck()
    {
        player.ShockWave();
    }

    public void DeactivateStaff()
    {
        player.DeactivateGlow(true);
    }
}
