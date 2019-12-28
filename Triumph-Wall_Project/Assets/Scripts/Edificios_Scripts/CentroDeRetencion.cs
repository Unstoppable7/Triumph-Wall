using System.Collections.Generic;
using UnityEngine;

public class CentroDeRetencion : Edificio
{
	private OficinaDeportacionBehaviour oficina;
	private Dormitorios dorms;
	private Enfermeria_Behaviour enfermeria;

	private List<Edificio> edificiosDelRecinto = new List<Edificio>();

	//TODO change GameObject To Especific inmigrants Objects
	private List<GameObject> inmigrantsInFacility = new List<GameObject>();
	//TODO cambiar GameObject por la Clase guardia
	private List<GameObject> policeMen = new List<GameObject>();

	public int maxEmployeeNumP { set { maxEmployeeNumP = value; } get { return maxEmployeeNum; } }

	[SerializeField]
	private BuildingDataTypes.SO_CRData myData = null;

	[SerializeField]
	private UIDataTypes.Buildings.SO_UICR_Data myUIData = null;

	private float salubridad = 0;
	private float control = 0;

	public override void SetUP ( )
	{
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
        // SetUp all buidling
        for (int i = 0; i < edificiosDelRecinto.Count; i++)
		{
			edificiosDelRecinto[i].SetUP();
			edificiosDelRecinto[i].SetID(i);
		}

		//PARENT CONSTRUCT //look at the defaul data of Edificio to see whats available
		//getIT from Balance File
		SetDataFromObject();
		currentEmployeeNum = 0;
		maxEmployeeNum = 10;
		currentInmigrantNum = GetCurrentIlegals();
		salubridad = 0;
		control = 0;
	}

	public override void Tick ( )
	{
		if(myData.debug)
			SetDataFromObject();

		foreach ( Edificio building in edificiosDelRecinto)
		{
			building.Tick();
		}
		salubridad += CalculateSalubrity();
		salubridad = Mathf.Clamp( salubridad, -1, 1 );

		control = CalculateControl();
		control = Mathf.Clamp( control, 0, 1 );

		UpdateDataObject();
		UpdateUIData();
	}  

	//after tick
	public override void UpdateUIData ( )
	{
		myUIData.maxEmployeeNum = maxEmployeeNum;
		myUIData.currentEmployeeNum = currentInmigrantNum;

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

	protected override void StartProcessInmigrant ( )
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
		currentInmigrantNum = GetCurrentIlegals();
		if (currentInmigrantNum <= 0)
			return 1;

		float result = 0;
		result = 1.0f - ((float)(currentInmigrantNum - (currentEmployeeNum * myData.inmigrantesPorGuardia)) /
			currentInmigrantNum);

		return result;
	}

	private int GetCurrentIlegals ( )
	{
		int result = 0;
		foreach (Edificio building in edificiosDelRecinto)
		{
			result += building.GetCurrentInmigrants();
		}
		return result;
	}
	#endregion

	////////////////////////////////////////////MANAGER////////////
	public void AddBuilding(Edificio building)
	{
		edificiosDelRecinto.Add( building );
		building.SetID( edificiosDelRecinto.Count - 1 );
	}

	//Method called from buttons
	public void DoBuildingAction ( B_Actions action, int bIndex )
	{
		switch (action)
		{
		case B_Actions.UPGRADE:
			edificiosDelRecinto[bIndex].Upgrade();
			break;
		case B_Actions.BUY_EMPLOYEE:
			edificiosDelRecinto[bIndex].BuyEmployee();
			break;
		case B_Actions.FIRE_EMPLOYEE:
			edificiosDelRecinto[bIndex].FireEmployee();
			break;
		case B_Actions.REPAIR:
			edificiosDelRecinto[bIndex].Repair();
			break;
		default:
			break;
		}
	}

	//Method Called by Resource Manager

	public override int GetCurrentInmigrants ( )
	{
		currentInmigrantNum = GetCurrentIlegals();
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

	protected override void SetDataFromObject ( )
	{
		//PARENT CONSTRUCT 
		//look at the defaul data of Edificio to see whats available
		//getIT from Balance File
		pricePerEmployee = myData.pricePerEmployee;
		maxEmployeeNum = myData.maxEmployeeNum;
		currentEmployeeNum = myData.currentEmployeeNum;

		maxInmigrantNum = myData.maxInmigrantNum;
		currentInmigrantNum = GetCurrentIlegals();

		salubridad = myData.salubridad;
		control = myData.control;
	}

	protected override void UpdateDataObject ( )
	{
		myData.pricePerEmployee = pricePerEmployee;
		myData.maxEmployeeNum = maxEmployeeNum;
		myData.currentEmployeeNum = currentEmployeeNum;

		myData.maxInmigrantNum = maxInmigrantNum;
		myData.currentInmigrantNum = GetCurrentIlegals();

		myData.salubridad = salubridad;
		myData.control = control;
	}
}
