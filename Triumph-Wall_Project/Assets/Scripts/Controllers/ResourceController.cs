using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ResourceController : MonoBehaviour
{
	//Stadisticas
	[Title("Stats")]

	[Title( "For calculations" ,"Todas estas variables son cogidas desde otros scripts y se usan para enseñar la UI")]
	//from inmigrant manager
	private int totalInmigrantsMonth = 0;
	private int woundedInmigrants = 0;
	private int deadInmigrants = 0;
	//from police Manager
	private int policeNum = 0;
	private float policeTotalCost = 0;
	//from CR
	private int arrestedInmigran = 0;//CR
	private float facilityHappiness = 0;//CR
	private float employeeTotalCost = 0; //CR
	private int totalDeported = 0;//CR->office
	private int normalDeported = 0; //CR->office
	private int woundedDeported = 0;//CR->Office
	private int greavousDeported = 0;//CR->Office
	//from here 
	private float frontierEfficiency = 0;//totalDeported(CR)/totalInmigrants(Inmigrant Manager)

	[Title("Opinion Publica")]	
	//opinion publica depende de
	// felicidad media (CR)
	// numero de deportados con heridas (CR)
	// numero de deportados con heridas graves(CR)
	// numero de heridos (inmigrantManager)
	// numero de muertos (inmigrantManager)
	private float publicOpinion = 0;
	//factores para calcular opinion publica
	private float happinesPoFactor = 0.4f;
	private float deportedWoundPoPenalty = 0.005f;
	private float deportedDeathPoPenalty = 0.01f;
	private float woundPoPenalty = 0.005f; //5 unidades por cada herido
	private float deathPoPenalty = 0.3f; //300 unidade por cada muerto 

	[Title("Dinero")]
	private float currentMoney = 0;
	//money with applied penalties and rewards
	// eficiencia
	// opinion publica
	// numero de heridos
	// numero de muertos
	private float finalRewardMoney = 0; //calculated here

	private float governmentMoney = 0;//from Balance File
	private float woundMoneyPenalty = 0.005f; //5 euros por cada herido
	private float deathMoneyPenalty = 0.01f; //10 euros por cada muerto  
	private float publicOpinionMoneyReward = 0.7f;
	private float efficiencyMoneyReward = 0.7f;

	[Title("Recursos Humanos", "variables usadas en el hud de listado de empleados")]
	//lista de policias ordenados por fecha de cotratacion
	//Lista de empleados ordenados por empleo
	//Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void SetUp ( )
	{

	}

	public void Tick ( )
	{

	}

	private float CalculatePublicOpinion ( )
	{

	}

	private CalculateFinalMoney ( )
	{

	}
}
