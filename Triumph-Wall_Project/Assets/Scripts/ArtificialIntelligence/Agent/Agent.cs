using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(SensorySystem), typeof(BlackBoard), typeof(BehaviorTree))]
public abstract class Agent : MonoBehaviour
{
    [SerializeField][FoldoutGroup("Base Agent")]
	protected BlackBoard blackBoard = null;

	// Start is called before the first frame update
	protected abstract void SetUp ( );

	// Update is called once per frame
	protected abstract void Tick ( );

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
