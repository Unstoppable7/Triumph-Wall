using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class ResourceController : SerializedMonoBehaviour
{
	[SerializeField][Required][AssetsOnly]
	private SO_ResourceController myBalanceFile = null;

	private InmigrantManager inmigrantManager = null;
	private PoliceManager policeManager = null;
	private CentroDeRetencion crFacility = null;

	[Title( "For calculations" ,
		"Todas estas variables son cogidas desde otros scripts y se usan para enseñar la UI")]
	//from inmigrant manager
	private int totalInmigrantsMonth = 0;
	private int woundedInmigrants = 0;
	private int deadInmigrants = 0;
	//from police Manager
	private float policeTotalCost = 0;
	//from CR
	private int arrestedInmigrants = 0;//CR
	private float facilityHappiness = 0;//CR
	private float employeeTotalCost = 0; //CR
	private int totalDeported = 0;//CR->office
	private int normalDeported = 0; //CR->office
	private int woundedDeported = 0;//CR->Office
	private int greavousDeported = 0;//CR->Office
	//from here 
	private float frontierEfficiency = 0;//totalDeported(CR)/totalInmigrants(Inmigrant Manager)
	private int daysWithoutCasualties = 0;

	[Title( "Opinion Publica" )]
	//opinion publica depende de
	// felicidad media (CR)
	// numero de deportados con heridas (CR)
	// numero de deportados con heridas graves(CR)
	// numero de heridos (inmigrantManager)
	// numero de muertos (inmigrantManager)
	private float publicOpinion = 0;
	//factores positivos para calcular opinion publica

	[Title("Dinero")]
	private float currentMoney = 0;
	//money with applied penalties and rewards
	// eficiencia
	// opinion publica
	// numero de heridos
	// numero de muertos
	// sueldo de los policias
	// sueldo de los empleados
	private float finalRewardMoney = 0; //calculated here

	//TODO localizar los empleados por ID y por edificio
	//desde estas listas se debe poder despedir e ir hasta la lozalizacion
	//del empleado
	[Title( "Recursos Humanos", "variables usadas en el hud de listado de empleados" )]
	//lista de policias ordenados por fecha de cotratacion
	List<UIEmployee> uiPolicias = new List<UIEmployee>();
	//Lista de empleados ordenados por empleo
	List<UIEmployee> facilityEmployees = new List<UIEmployee>();

	public void SetUp ( )
	{
		inmigrantManager = FindObjectOfType<InmigrantManager>();
		policeManager = FindObjectOfType<PoliceManager>();
		crFacility = FindObjectOfType<CentroDeRetencion>();

		if(!inmigrantManager || !policeManager || !crFacility)
		{
			throw new System.Exception( "Asegurate de que los managers estan en escena" );
		}

		TimerController.dailyEvent.AddListener( DailyActions );
		TimerController.monthlyEvent.AddListener( MonthlyActions );
	}

	public void Tick ( )
	{
		if (inmigrantManager || policeManager || crFacility)
		{
			GetAllData();
			CalculatePublicOpinion();
			CalculateFinalMoney();
			UpdateUI();
		}
	}

#region MONTHLY

	private void MonthlyActions ( )
	{
		GiveMoneyToPlayer();
	}

	private void GiveMoneyToPlayer ( )
	{
		Tick();
		currentMoney += finalRewardMoney;
	}
#endregion

#region DAILY

	private void DailyActions ( )
	{
		CountDayWithoutCasualty();
	}

	private void CountDayWithoutCasualty ( )
	{
		if (inmigrantManager.GetDayCasualty())
		{
			daysWithoutCasualties = 0;
		}
		else
		{
			daysWithoutCasualties++;
		}
	}

	#endregion

	private void UpdateUI ( )
	{

	}

	private void GetAllData ( )
	{
		//from inmigrant manager
		totalInmigrantsMonth = inmigrantManager.GetAllInmigrantsThisMonth();
		woundedInmigrants = inmigrantManager.GetWoundedCasualties();
		deadInmigrants = inmigrantManager.GetDeathCasualties();
		//from police Manager
		policeTotalCost = policeManager.GetTotalPoliceMenCost();
		//from CR
		totalDeported = crFacility.GetTotalDeported();//CR->office
		normalDeported = crFacility.GetNormalDeported();//CR->office
		woundedDeported = crFacility.GetWoundedDeported();//CR->Office
		greavousDeported = crFacility.GetGrevousDeported();//CR->Office
		facilityHappiness = crFacility.GetAverageHappiness();//CR
		employeeTotalCost = crFacility.GetTotalEmployeeCost();//CR
		arrestedInmigrants = crFacility.GetCurrentInmigrants();//CR
		
		//from here 
		if (totalInmigrantsMonth <= 0)
		{
			frontierEfficiency = 0;
			return;
		}
		frontierEfficiency = totalDeported / totalInmigrantsMonth;
	}

	private void CalculatePublicOpinion ( )
	{
		// 0 to 1
		publicOpinion = 0;
		// felicidad media (CR)
		publicOpinion += facilityHappiness * myBalanceFile.happinesPoFactor;
		//eficiencia
		publicOpinion += frontierEfficiency * myBalanceFile.efficiencyPoFactor;
		//days witoutCasualty
		publicOpinion += daysWithoutCasualties * myBalanceFile.daysWithoutCasualtiesPoFactor;
		// numero de deportados con heridas (CR)
		publicOpinion -= woundedDeported * myBalanceFile.woundedDeportedPoPenalty;
		// numero de deportados con heridas graves(CR)
		publicOpinion -= greavousDeported * myBalanceFile.greavousDeportedPoPenalty;
		// numero de heridos (inmigrantManager)
		publicOpinion -= woundedInmigrants * myBalanceFile.woundPoPenalty;
		// numero de muertos (inmigrantManager)
		publicOpinion -= deadInmigrants * myBalanceFile.deathPoPenalty;
		//clamp it baby
		publicOpinion = Mathf.Clamp( publicOpinion , -1.0f, 1.0f);
	}

	private void CalculateFinalMoney ( )
	{
		//money with applied penalties and rewards
		// eficiencia
		// opinion publica
		// numero de heridos
		// numero de muertos
		// sueldo de los policias
		// sueldo de los empleados
		
		finalRewardMoney = (frontierEfficiency * myBalanceFile.efficiencyMoneyFactor + 
			publicOpinion * myBalanceFile.publicOpinionMoneyFactor) * 
			myBalanceFile.governmentMoney;

		finalRewardMoney -= woundedInmigrants * myBalanceFile.woundedMoneyPenalty;
		finalRewardMoney -= deadInmigrants * myBalanceFile.deathMoneyPenalty;

		finalRewardMoney -= policeTotalCost;
		finalRewardMoney -= employeeTotalCost;
	}

	public bool CheckIfEnoughMoney(float actionCost)
	{
		return currentMoney - actionCost >= 0.00f;
	}
}
