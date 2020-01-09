using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceManager : MonoBehaviour
{
	private List<Agent_Guarda> policeMen = new List<Agent_Guarda>();

	public void SetUp ( )
	{

	}
	public void Tick ( )
	{

	}

	public int GetTotalPoliceMen ( )
	{
		return policeMen.Count;
	}

	public float GetTotalPoliceMenCost ( )
	{
		float totalCost = 0;
		foreach(Agent_Guarda man in policeMen)
		{
			totalCost += man.GetCost();
		}
		return totalCost;
	}
}
