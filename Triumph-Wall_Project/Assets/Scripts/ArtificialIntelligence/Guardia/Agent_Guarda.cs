using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent_Guarda : Agent
{

	private float costOfMan = 10;
	private SensorySystem sensorySystem = null;
	[SerializeField]
	private Ruta defaultRuta = null;

    private void Awake()
    {
        SetUp();
    }

    // Start is called before the first frame update
    private void Start()
    {
		sensorySystem.SetTagToFilter( "CR_Inmigrant" );
    }

	public float GetCost()=> costOfMan;

	protected override void SetUp ( )
	{
		sensorySystem = this.GetComponent<SensorySystem>();
		blackBoard = this.GetComponent<BlackBoard>();
        blackBoard.variables.Add("enemiesInSight", new List<Agent>());
        blackBoard.variables.Add("alliesInSight", new List<Agent>());
        blackBoard.variables.Add("Target", new GameObject());

		if(defaultRuta != null)
			blackBoard.variables.Add("Ruta", defaultRuta.path);
		else
			blackBoard.variables.Add("Ruta", new List<Transform>());
	}

	protected override void Tick ( )
	{
	}
}
