using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class ResourceController : SerializedMonoBehaviour
{
	private InmigrantManager inmigrantManager = null;
	private PoliceManager policeManager = null;
	private CentroDeRetencion crFacility = null;
	//Stadisticas
	[Title("Stats")]

	[Title( "For calculations" ,"Todas estas variables son cogidas desde otros scripts y se usan para enseñar la UI")]
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

	[Title("Opinion Publica")]	
	//opinion publica depende de
	// felicidad media (CR)
	// numero de deportados con heridas (CR)
	// numero de deportados con heridas graves(CR)
	// numero de heridos (inmigrantManager)
	// numero de muertos (inmigrantManager)
	private float publicOpinion = 0;
	//factores positivos para calcular opinion publica
	private float happinesPoFactor = 0.4f;
	private float efficiencyPoFactor = 0.4f;
	private float daysWithoutCasualtiesPoFactor = 0.4f;
	//facotores negativos
	private float woundedDeportedPoPenalty = 0.005f;
	private float greavousDeportedPoPenalty = 0.01f;
	private float woundPoPenalty = 0.005f; //5 unidades por cada herido
	private float deathPoPenalty = 0.3f; //300 unidade por cada muerto 

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

	private float governmentMoney = 0;//from Balance File
	private float woundedMoneyPenalty = 0.005f; //5 euros por cada herido
	private float deathMoneyPenalty = 0.01f; //10 euros por cada muerto  

	[SerializeField][HideInInspector]
	private float publicOpinionMoneyFactor = 0.3f;
	[SerializeField][HideInInspector]
	private float efficiencyMoneyFactor = 0.7f;

	[PropertyRange( 0, 1 )][ShowInInspector]
	private float PublicOpinionMoneyFactor
	{
		get { return publicOpinionMoneyFactor; }
		set
		{
			publicOpinionMoneyFactor = value;
			efficiencyMoneyFactor = 1 - publicOpinionMoneyFactor;
		}
	}

	[PropertyRange( 0, 1 )][ShowInInspector]
	private float EfficiencyMoneyFactor
	{
		get { return efficiencyMoneyFactor; }
		set
		{
			efficiencyMoneyFactor = value;
			publicOpinionMoneyFactor = 1 - efficiencyMoneyFactor;
		}
	}

	//TODO localizar los empleados por ID y por edificio
	//desde estas listas se debe poder despedir e ir hasta la lozalizacion
	//del empleado
	[Title( "Recursos Humanos", "variables usadas en el hud de listado de empleados" )]
	//lista de policias ordenados por fecha de cotratacion
	List<UIEmployee> uiPolicias = new List<UIEmployee>();
	//Lista de empleados ordenados por empleo
	List<UIEmployee> facilityEmployees = new List<UIEmployee>();

	void Start()
    {
		SetUp();
    }

    void Update()
    {
		Tick();
    }

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
		}
	}

	private void MonthlyActions ( )
	{
		GiveMoneyToPlayer();
	}

	private void GiveMoneyToPlayer ( )
	{
		Tick();
		currentMoney += finalRewardMoney;
	}

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
		frontierEfficiency = totalDeported / totalInmigrantsMonth;
	}

	private void CalculatePublicOpinion ( )
	{
		// 0 to 1
		publicOpinion = 0;
		// felicidad media (CR)
		publicOpinion += facilityHappiness * happinesPoFactor;
		//eficiencia
		publicOpinion += frontierEfficiency * efficiencyPoFactor;
		//days witoutCasualty
		publicOpinion += daysWithoutCasualties * daysWithoutCasualtiesPoFactor;
		// numero de deportados con heridas (CR)
		publicOpinion -= woundedDeported * woundedDeportedPoPenalty;
		// numero de deportados con heridas graves(CR)
		publicOpinion -= greavousDeported * greavousDeportedPoPenalty;
		// numero de heridos (inmigrantManager)
		publicOpinion -= woundedInmigrants * woundPoPenalty;
		// numero de muertos (inmigrantManager)
		publicOpinion -= deadInmigrants * deathPoPenalty;
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
		finalRewardMoney = (frontierEfficiency * efficiencyMoneyFactor + publicOpinion * publicOpinionMoneyFactor) * governmentMoney; //calculated here

		finalRewardMoney -= woundedInmigrants * woundedMoneyPenalty;
		finalRewardMoney -= deadInmigrants * deathMoneyPenalty;

		finalRewardMoney -= policeTotalCost;
		finalRewardMoney -= employeeTotalCost;
	}

	public bool CheckIfEnoughMoney(float actionCost)
	{
		return currentMoney - actionCost >= 0.00f;
	}
}
