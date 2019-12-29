using System.Collections.Generic;
using UnityEngine;

public class CentroDeRetencion : Edificio
{
	private CentroDeRetencion facility;
	private OficinaDeportacionBehaviour oficina;
	private Dormitorios dorms;
	private Cocina_Behaviour cocina;
	private Enfermeria_Behaviour enfermeria;

	private List<Edificio> edificiosDelRecinto = new List<Edificio>();

	//TODO change GameObject To Especific inmigrants Objects
	private List<GameObject> inmigrantsInFacility = new List<GameObject>();
	//TODO cambiar GameObject por la Clase guardia
	private List<GameObject> policeMen = new List<GameObject>();

	[SerializeField]
	private BuildingDataTypes.SO_CRData myData = null;

	[SerializeField]
	private UIDataTypes.Buildings.SO_UICR_Data myUIData = null;

	private float salubridad = 0;
	private float control = 0;

	public override void SetUP ( )
	{
		myUIData.name = "Facility";
		managerID = -1;
		myUIData.managerID = -1;
		//myUIData = ScriptableObject.CreateInstance<UIDataTypes.Buildings.UICR_Data>();
		TimerController.dailyEvent.AddListener( ResetDay );
		TimerController.monthlyEvent.AddListener( ResetMonth );

		//initialize buidlings
		//Oficina
		oficina = GetComponentInChildren<OficinaDeportacionBehaviour>();
		edificiosDelRecinto.Add( oficina );
        //Dorms
        dorms = GetComponentInChildren<Dormitorios>();
        edificiosDelRecinto.Add(dorms);
        //Enfermeria
        enfermeria = GetComponentInChildren<Enfermeria_Behaviour>();
        edificiosDelRecinto.Add(enfermeria);
        //Cocina
        cocina = GetComponentInChildren<Cocina_Behaviour>();
        edificiosDelRecinto.Add(cocina);

        // SetUp all buidling
        for (int i = 0; i < edificiosDelRecinto.Count; i++)
		{
			edificiosDelRecinto[i].SetID(i);
			edificiosDelRecinto[i].SetUP();
		}

		//PARENT CONSTRUCT //look at the defaul data of Edificio to see whats available
		//getIT from Balance File
		SetDataFromObject();
	}

	public override void Tick ( )
	{
		if(myData.debug)
			SetDataFromObject();

		foreach (Edificio building in edificiosDelRecinto)
		{
			building.Tick();
		}
		salubridad += CalculateSalubrity();
		salubridad = Mathf.Clamp( salubridad, -1, 1 );

		control = CalculateControl();
		control = Mathf.Clamp( control, 0, 1 );

		UpdateUIData();
	}  

	//after tick
	public override void UpdateUIData ( )
	{
		myUIData.maxEmployeeNum = maxEmployeeNum;
		myUIData.currentEmployeeNum = currentEmployeeNum;

		myUIData.maxInmigrantNum = maxInmigrantNum;
		myUIData.currentInmigrantNum = currentInmigrantNum;

		myUIData.salubrity = salubridad;
		myUIData.control = control;

		myUIData.updatedValuesEvent.Invoke();
	}

	public override void ShowUI ( )
	{
		UIController.Instance.ShowEdificioUI( myUIData );
	}

	public override void Repair ( )
	{
		throw new System.NotImplementedException();
	}

	public override void Upgrade ( )
	{
		throw new System.NotImplementedException();
	}

	public override void BuyEmployee ( )
	{
		base.BuyEmployee();
		maxInmigrantNum = (currentEmployeeNum * myData.inmigrantesPorGuardia);
	}

	public override void FireEmployee ( )
	{
		base.FireEmployee();
		maxInmigrantNum = (currentEmployeeNum * myData.inmigrantesPorGuardia);
	}

	protected override void ProcessInmigrant ( )
	{
		throw new System.NotImplementedException();
	}

	public override void ResetDay ( )
	{
		foreach (Edificio building in edificiosDelRecinto)
		{
			building.ResetDay();
		}
	}

	public override void ResetMonth ( )
	{
		foreach (Edificio building in edificiosDelRecinto)
		{
			building.ResetMonth();
		}
	}

	public override float GetUpgradePrice ( )
	{
		return 0.0f;
	}

	public override float GetRepairPrice ( )
	{
		return 0.0f;
	}

	public override int GetCurrentInmigrants ( )
	{
		int result = 0;
		foreach (Edificio building in edificiosDelRecinto)
		{
			result += building.GetCurrentInmigrants();
		}
		currentInmigrantNum = result;
		return base.GetCurrentInmigrants();
	}

	public override float GetTotalEmployeeCost ( )
	{
		float result = 0;
		foreach (Edificio building in edificiosDelRecinto)
		{
			result += building.GetTotalEmployeeCost();
		}
		return result;
	}

	public override float GetCurrentEmployeeNum ( )
	{
		float result = 0;
		foreach (Edificio building in edificiosDelRecinto)
		{
			result += building.GetCurrentEmployeeNum();
		}
		return result;
	}

	protected override void SetDataFromObject ( )
	{
		//PARENT CONSTRUCT 
		//look at the defaul data of Edificio to see whats available
		//getIT from Balance File
		pricePerEmployee = myData.pricePerEmployee;
		maxEmployeeNum = myData.maxEmployeeNum;
		currentEmployeeNum = myData.currentEmployeeNum;

		maxInmigrantNum = (currentEmployeeNum * myData.inmigrantesPorGuardia);
		currentInmigrantNum = GetCurrentInmigrants();

		salubridad = myData.salubridad;
		control = myData.control;
	}

	protected override void UpdateDataObject ( )
	{
		myData.pricePerEmployee = pricePerEmployee;
		myData.maxEmployeeNum = maxEmployeeNum;
		myData.currentEmployeeNum = currentEmployeeNum;

		myData.maxInmigrantNum = (currentEmployeeNum * myData.inmigrantesPorGuardia);
		myData.currentInmigrantNum = GetCurrentInmigrants();

		myData.salubridad = salubridad;
		myData.control = control;
	}
	
	#region Salubrity

	private float CalculateSalubrity ( )
	{
		float inmigrantesMaxDormitorios = GetMaxOfIlegalsInDorms();
		float inmigrantesEnDormitorios = GetNumOfIlegalsInDorms();

		float aforo = inmigrantesEnDormitorios / inmigrantesMaxDormitorios;
		
		//MIN -1 MAX 1
		float result = 0;
		if (aforo > 1)
			result = -aforo * myData.factorSuciedad * Time.deltaTime;
		else if(aforo < 1)
			result = (1+(1-aforo)) * myData.factorSuciedad * Time.deltaTime;
		
		return result;
	}

	private float GetMaxOfIlegalsInDorms ( )
	{
		return dorms.GetMaxInmigrants();
	}
	private float GetNumOfIlegalsInDorms ( )
	{

		return dorms.GetCurrentInmigrants();
	}

	#endregion

	#region Control
	private float CalculateControl ( )
	{
		currentInmigrantNum = GetCurrentInmigrants();
		if (currentInmigrantNum <= 0)
			return 1;

		float result = 0;
		result = 1.0f - ((float)(currentInmigrantNum - (currentEmployeeNum * myData.inmigrantesPorGuardia)) /
			currentInmigrantNum);

		return result;
	}
	#endregion

	////////////////////////////////////////////MANAGER////////////
	#region OFICINA Especificos

	public int GetTotalDeported ( )
	{
		return oficina.GetTotalDeported();
	}
	public int GetNormalDeported ( )
	{
		return oficina.GetNormalDeported();
	}
	public int GetWoundedDeported ( )
	{
		return oficina.GetWoundedDeported();
	}
	public int GetGrevousDeported ( )
	{
		return oficina.GetGrevousDeported();
	}
	#endregion
	
	//Method called from buttons
	public void DoBuildingAction (B_Actions action, int bIndex)
	{
		float priceOfAction = 0;
		switch (action)
		{
		case B_Actions.UPGRADE:
			priceOfAction = edificiosDelRecinto[bIndex].GetUpgradePrice();
			if (ResourceController.CheckIfEnoughMoney( priceOfAction ))
			{
				edificiosDelRecinto[bIndex].Upgrade();
			}
			break;
		case B_Actions.BUY_EMPLOYEE:
			if (bIndex < 0)
			{
				BuyEmployee();
				break;
			}
			edificiosDelRecinto[bIndex].BuyEmployee();
			
			break;
		case B_Actions.FIRE_EMPLOYEE:
			if (bIndex < 0)
			{
				FireEmployee();
				break;
			}
			edificiosDelRecinto[bIndex].FireEmployee();
			break;
		case B_Actions.REPAIR:
			priceOfAction = edificiosDelRecinto[bIndex].GetPriceEmployee();
			if (ResourceController.CheckIfEnoughMoney( priceOfAction ))
			{
				edificiosDelRecinto[bIndex].Repair();
			}
			break;
		default:
			break;
		}
	}

	public float GetAverageHappiness ( )
	{
		if (inmigrantsInFacility.Count <= 0)
			return 1;

		float result = 0;
		//TODO change this to the inmigrant happiness getter
		foreach (GameObject inmigrant in inmigrantsInFacility)
		{
			result += inmigrant.GetHashCode();
		}

		result /= inmigrantsInFacility.Count;
		return result;
	}
}
