using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent_Inmigrant : MonoBehaviour
{
	[Range(0,1)]
	private float happiness = 1;
	private float comida = 1;
	[Range(0,1)]
	public float normalPortion = 0.1f;

	public void AddComida(float food)
	{
		comida += food;
	}

	public float GetNormalPortion ( ) => normalPortion;
	public float Gethappiness ( ) => happiness;
}
