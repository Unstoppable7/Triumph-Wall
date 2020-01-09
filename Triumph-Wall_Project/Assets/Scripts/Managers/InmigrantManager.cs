using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class InmigrantManager: MonoBehaviour
{
	private bool dayWithCasualty = false;
	private int inmigrantsThisMonth = 0;
	private int woundCasualties = 0;
	private int deathCasualties = 0;

	public float timeOfSpawning = 10.0f;

	[SerializeField][AssetsOnly]
	private GameObject inmigrantPrefab;

	[SerializeField]
	private List<Transform> spawnPositions;

	private List<Agent_Inmigrant> inmigrantsRunning = new List<Agent_Inmigrant>();

	public bool GetDayCasualty ( ) => dayWithCasualty;

	public void SetUp ( )
	{
		TimerController.dailyEvent.AddListener( ResetDay );
		TimerController.monthlyEvent.AddListener( ResetMonth );
		StartCoroutine( SpawnInmigrant() );
	}

	public void Tick ( )
	{

	}

	private IEnumerator SpawnInmigrant ( )
	{
		while (true)
		{
			GameObject newInmigrant = Instantiate( inmigrantPrefab, transform );

			switch (Random.Range( 0, 2 ))
			{
			case 0:
				newInmigrant.GetComponent<Agent_Inmigrant>().hurt = false;
				newInmigrant.GetComponent<Agent_Inmigrant>().wounded = false;
				break;
			case 1:
				newInmigrant.GetComponent<Agent_Inmigrant>().hurt = true;
				newInmigrant.GetComponent<Agent_Inmigrant>().wounded = true;
				break;
			}
			newInmigrant.transform.position = spawnPositions[Random.Range(0, spawnPositions.Count-1)].position;
			inmigrantsRunning.Add( newInmigrant.GetComponent<Agent_Inmigrant>() );
			yield return new WaitForSeconds( timeOfSpawning );
		}
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
