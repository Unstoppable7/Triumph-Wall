using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent_Guarda : Agent
{

	private float costOfMan = 10;
    // Start is called before the first frame update
    private void Start()
    {
		SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


	public float GetCost()=> costOfMan;

	protected override void SetUp ( )
	{
		BlackBoard bb = this.GetComponent<BlackBoard>();
		blackBoard = new TreeBlackBoard();
		blackBoard.Value = bb;
		behaviourTree.SetVariable( "AgentBB", blackBoard );
	}

	protected override void Tick ( )
	{
	}
}
