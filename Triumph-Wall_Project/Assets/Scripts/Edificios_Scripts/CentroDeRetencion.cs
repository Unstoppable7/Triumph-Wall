using System.Collections.Generic;
using UnityEngine;

public class CentroDeRetencion : Edificio
{
	private List<Edificio> edificiosDelRecinto = new List<Edificio>();

	public int maxEmployeeNumP { set { maxEmployeeNumP = value; } get { return maxEmployeeNum; } }

	private UIDataTypes.Buildings.UICR_Data myUIData;

	public float salubridad = 0;
	public float control = 0;
	public float factorSuciedad = 1.0f;
	public int inmigrantesPorGuardia = 5;

	public override void SetUP ( )
	{
		myUIData = ScriptableObject.CreateInstance<UIDataTypes.Buildings.UICR_Data>();
		//TODO initialize buidlings
		//Oficina
		edificiosDelRecinto.Add( GetComponentInChildren<OficinaDeportacionBehaviour>() );
		//Dorms
		//Enfermeria
		//Cocina
		// SetUp all buidling
		for (int i = 0; i < edificiosDelRecinto.Count; i++)
		{
			edificiosDelRecinto[i].SetUP();
			edificiosDelRecinto[i].SetID(i);
		}
		// las funciones de los empleados se usaran desde el editor de rutas
		myUIData.showBuyEmployeeBtn = false;
		myUIData.showFireEmployeeBtn = false;
		myUIData.showEmployeeNum = true; 
		//inmigrantes salubridad y control
		myUIData.showInmigrantNum = true;
		
		//PARENT CONSTRUCT //look at the defaul data of Edificio to see whats available
		//getIT from Balance File
		currentEmployeeNum = 0;
		maxEmployeeNum = 10;
		currentInmigrantNum = GetCurrentIlegals();
		salubridad = 0;
		control = 0;
	}

	public override void Tick ( )
	{
		foreach ( Edificio building in edificiosDelRecinto)
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

	#region Salubrity

	private float CalculateSalubrity ( )
	{
		float inmigrantesMaxDormitorios = GetMaxOfIlegalsInDorms();
		float inmigrantesEnDormitorios = GetNumOfIlegalsInDorms();

		float aforo = inmigrantesEnDormitorios / inmigrantesMaxDormitorios;
		
		//MIN -1 MAX 1
		float result = 0;
		if (aforo > 1)
			result = -aforo * factorSuciedad * Time.deltaTime;
		else if(aforo < 1)
			result = (1+(1-aforo)) * factorSuciedad * Time.deltaTime;
		
		return result;
	}

	private float GetMaxOfIlegalsInDorms ( )
	{
		//TODO hacer dormitorios para poder completar esta funcion
		return 10;
	}
	private float GetNumOfIlegalsInDorms ( )
	{
		//TODO hacer dormitorios para poder completar esta funcion
		return 0;
	}

	#endregion

	#region Control
	private float CalculateControl ( )
	{
		currentInmigrantNum = GetCurrentIlegals();

		float result = 0;
		result = 1.0f - ((float)(currentInmigrantNum - (currentEmployeeNum * inmigrantesPorGuardia)) /
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
	public float TotalCostOfEmployeeInFacility ( )
	{
		float result = 0;
		foreach (Edificio building in edificiosDelRecinto)
		{
			result += building.GetTotalEmployeeCost();
		}
		return result;
	}
	public float GetBuildingEmployeeCost (int indx )
	{
		return edificiosDelRecinto[indx].GetTotalEmployeeCost();
	}
}
