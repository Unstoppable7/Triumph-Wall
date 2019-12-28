using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;

public class ResourceController : SerializedMonoBehaviour
{
	#region UI
	[SerializeField][FoldoutGroup( "UI" )]
	private GameObject economyMenu = null;
	[SerializeField][FoldoutGroup("UI/Medidores")]
	private Slider efficiencySlider = null;
	[SerializeField][FoldoutGroup("UI/Medidores")]
	private TextMeshProUGUI efficiencyText = null;
	[SerializeField][FoldoutGroup("UI/Medidores")]
	private Slider publicOpinionSlider = null;
	[SerializeField][FoldoutGroup("UI/Medidores")]
	private TextMeshProUGUI publicOpinionText = null;

	[SerializeField][FoldoutGroup("UI/Stats")]
	private TextMeshProUGUI moneyText = null;
	[SerializeField][FoldoutGroup("UI/Stats")]
	private TextMeshProUGUI daysText = null;
	[SerializeField][FoldoutGroup("UI/Stats")]
	private TextMeshProUGUI policeMenText = null;
	[SerializeField][FoldoutGroup("UI/Stats")]
	private TextMeshProUGUI employeesText = null;
	[SerializeField][FoldoutGroup("UI/Stats")]
	private TextMeshProUGUI inmigrantsText = null;

	[SerializeField][FoldoutGroup("UI/Details")]
	private TextMeshProUGUI efficiencyMoneyFactorText = null;
	[SerializeField][FoldoutGroup("UI/Details")]
	private TextMeshProUGUI publicOpinionMoneyFactorText = null;
	[SerializeField][FoldoutGroup("UI/Details")]
	private TextMeshProUGUI policeMenMoneyFactorText = null;
	[SerializeField][FoldoutGroup("UI/Details")]
	private TextMeshProUGUI employeeMoneyFactorText = null;
	[SerializeField][FoldoutGroup("UI/Details")]
	private TextMeshProUGUI woundedMoneyFactorText = null;
	[SerializeField][FoldoutGroup("UI/Details")]
	private TextMeshProUGUI deadMoneyFactorText = null;
	[SerializeField][FoldoutGroup("UI/Details")]
	private TextMeshProUGUI rewardMoneyText = null;

#endregion
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
	private int totalPoliceMen = 0;
	private float policeTotalCost = 0;
	//from CR
	private int arrestedInmigrants = 0;//CR
	private float facilityHappiness = 0;//CR
	private int totalEmployees = 0;
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
	private float efficiencyRewardMoney = 0; //calculated here
	private float publicOpinionRewardMoney = 0; //calculated here

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
		efficiencySlider.value = frontierEfficiency;
		efficiencyText.text = string.Format( "{0:0}%", frontierEfficiency*100 );
		publicOpinionSlider.value = publicOpinion;
		publicOpinionText.text = string.Format( "{0:0}%", publicOpinion*100 );

		moneyText.text = string.Format( "{0:0}", currentMoney );
		moneyText.text = string.Format( "{0:0}", currentMoney );
		daysText.text = daysWithoutCasualties.ToString();
		policeMenText.text = totalPoliceMen.ToString();
		employeesText.text = totalEmployees.ToString();
		inmigrantsText.text = arrestedInmigrants.ToString();

		//positive Factors
		efficiencyMoneyFactorText.text = string.Format( "+{0:0}", frontierEfficiency * myBalanceFile.efficiencyMoneyFactor * myBalanceFile.governmentMoney );
		publicOpinionMoneyFactorText.text = string.Format( "+{0:0}", publicOpinion * myBalanceFile.publicOpinionMoneyFactor * myBalanceFile.governmentMoney );

		//negative Factors
		policeMenMoneyFactorText.text = string.Format( "-{0:0}", policeTotalCost );
		employeeMoneyFactorText.text = string.Format( "-{0:0}", employeeTotalCost );

		woundedMoneyFactorText.text = string.Format( "-{0:0}", woundedInmigrants * myBalanceFile.woundedMoneyPenalty );
		deadMoneyFactorText.text = string.Format( "-{0:0}", deadInmigrants * myBalanceFile.deathMoneyPenalty );

		//TODO public Opinion detail VIEW 
		//si quieres acordarte de lo que tenias que enseñar mira la funcion de calcular public opinion

		rewardMoneyText.text = string.Format( "{0:0}/{1:0}", finalRewardMoney, myBalanceFile.governmentMoney );

	}

	private void GetAllData ( )
	{
		//from inmigrant manager
		totalInmigrantsMonth = inmigrantManager.GetAllInmigrantsThisMonth();
		woundedInmigrants = inmigrantManager.GetWoundedCasualties();
		deadInmigrants = inmigrantManager.GetDeathCasualties();
		//from police Manager
		totalPoliceMen = policeManager.GetTotalPoliceMen();
		policeTotalCost = policeManager.GetTotalPoliceMenCost();
		//from CR
		totalDeported = crFacility.GetTotalDeported();//CR->office
		normalDeported = crFacility.GetNormalDeported();//CR->office
		woundedDeported = crFacility.GetWoundedDeported();//CR->Office
		greavousDeported = crFacility.GetGrevousDeported();//CR->Office
		facilityHappiness = crFacility.GetAverageHappiness();//CR
		totalEmployees = (int)crFacility.GetCurrentEmployeeNum();//CR
		employeeTotalCost = crFacility.GetTotalEmployeeCost();//CR
		arrestedInmigrants = crFacility.GetCurrentInmigrants();//CR
		
		//from here 
		if (totalInmigrantsMonth <= 0)
		{
			frontierEfficiency = 1;
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
		publicOpinion = Mathf.Clamp( publicOpinion , 0.0f, 1.0f);
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

		efficiencyRewardMoney = (frontierEfficiency * myBalanceFile.efficiencyMoneyFactor) * myBalanceFile.governmentMoney;
		publicOpinionRewardMoney = (publicOpinion * myBalanceFile.publicOpinionMoneyFactor) * myBalanceFile.governmentMoney;
		finalRewardMoney = efficiencyRewardMoney + publicOpinionRewardMoney;


		finalRewardMoney -= woundedInmigrants * myBalanceFile.woundedMoneyPenalty;
		finalRewardMoney -= deadInmigrants * myBalanceFile.deathMoneyPenalty;

		finalRewardMoney -= policeTotalCost;
		finalRewardMoney -= employeeTotalCost;
	}

	public bool CheckIfEnoughMoney(float actionCost)
	{
		return currentMoney - actionCost >= 0.00f;
	}

	public void SwitchEconomyMenu ( )
	{
		economyMenu.SetActive( !economyMenu.activeInHierarchy );
	}

	public void HideEconomyMenu ( )
	{
		economyMenu.SetActive( false );
	}
}
