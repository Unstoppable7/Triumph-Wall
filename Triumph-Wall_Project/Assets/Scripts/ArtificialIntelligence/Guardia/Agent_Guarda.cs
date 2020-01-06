using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent_Guarda : Agent
{

	private float costOfMan = 10;

    private void Awake()
    {
        SetUp();
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }


	public float GetCost()=> costOfMan;

	protected override void SetUp ( )
	{
		blackBoard = this.GetComponent<BlackBoard>();
        blackBoard.variables.Add("enemiesInSight", new List<Agent>());
        blackBoard.variables.Add("alliesInSight", new List<Agent>());
        blackBoard.variables.Add("Target", new GameObject());
	}

	protected override void Tick ( )
	{
	}
}
