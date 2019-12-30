using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(SensorySystem), typeof(BlackBoard))]
public class Agent : MonoBehaviour
{
    BlackBoard blackBoard;
    BlackBoard BlackBoard
    {
        get { return blackBoard; }
        set { value = blackBoard; }
    }

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CheckSystems()
    {
        if (this.GetComponent<SensorySystem>() == null)
        {
            Debug.LogError("No Sensory System atached: " + this.name);
        }
        if (this.GetComponent<BlackBoard>() == null)
        {
            Debug.LogError("No BlackBoardatached: " + this.name);
        }
    }
}
