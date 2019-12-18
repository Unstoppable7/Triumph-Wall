using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceManager : MonoBehaviour
{
	//TODO cambiar GameObject por la Clase guardia
	private List<GameObject> policeMen = new List<GameObject>();

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
		//TODO reccorrer cada poli cogiendo lo que cuestan
		return 0;
	}
}
