using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFactory : MonoBehaviour
{
   
    void Start()
    {
        
    }


    void Update()
    {
        
    }

    Agent CreateAgent()
    {
        return new Agent();
    }

    /*Vehicle CreateVehicle()
    {
        return new Vehicle();
    }*/
    
    /*Tower CreateTower()
    {
        return new Tower();
    }*/
}
