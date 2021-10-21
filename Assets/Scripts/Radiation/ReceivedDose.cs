using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Radiation;

public class ReceivedDose : MonoBehaviour
{
    [HideInInspector]
    public float RecieveRoentgen;
    private float RecievedDoseRad;
    
 
    void Start()
    {
        
    }

    
    void Update()
    {
       
    }
    void FixedUpdate()
    {
       
     Debug.Log( (RecievedDoseRad += (RecieveRoentgen / 3600)) * 0.02); // roentgen/h in  rad/c , 0.02 fixedupdate - rad/s in rad
        
    }
}
