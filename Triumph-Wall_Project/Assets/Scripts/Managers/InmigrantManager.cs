using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InmigrantManager: MonoBehaviour
{
	private bool dayWithCasualty = false;
	private int inmigrantsThisMonth = 0;
	private int woundCasualties = 0;
	private int deathCasualties = 0;

	//TODO change GameObject To Especific inmigrants Objects
	private List<GameObject> inmigrantsRunning = new List<GameObject>();

	public bool GetDayCasualty ( ) => dayWithCasualty;

	public void SetUp ( )
	{
		TimerController.dailyEvent.AddListener( ResetDay );
		TimerController.monthlyEvent.AddListener( ResetMonth );
	}

	public void Tick ( )
	{

	}

	private void ResetDay ( )
	{
		dayWithCasualty = false;
	}

	private void ResetMonth ( )
	{
		inmigrantsThisMonth = 0;
	}

	public int GetAllInmigrantsThisMonth ( )
	{
		return inmigrantsThisMonth;
	}
	public int GetWoundedCasualties ( )
	{
		return woundCasualties;
	}
	public int GetDeathCasualties ( )
	{
		return deathCasualties;
	}
}
