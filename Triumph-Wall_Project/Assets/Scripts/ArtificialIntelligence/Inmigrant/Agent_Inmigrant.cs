﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent_Inmigrant : Agent
{
	[Range(0,1)]
	private float happiness = 1;
	private float comida = 1;
	[Range(0,1)]
	public float normalPortion = 0.1f;

	public bool hurt = false;

	public void AddComida(float food)
	{
		comida += food;
	}

	public float GetNormalPortion ( ) => normalPortion;
	public float Gethappiness ( ) => happiness;

	protected override void SetUp ( )
	{
		throw new System.NotImplementedException();
	}

	protected override void Tick ( )
	{
		throw new System.NotImplementedException();
	}
}
