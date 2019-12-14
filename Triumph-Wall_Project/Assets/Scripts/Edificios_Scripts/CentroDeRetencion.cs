using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UIDataTypes.Buildings;
using UnityEngine;

public class CentroDeRetencion : Edificio
{

	private CR_Data myData;
	private float salubridad;

	private float factorSuciedad = 1.0f;

	private int inmigrantesPorGuardia = 5;

	private List<Edificio> edificiosDelRecinto = new List<Edificio>();

	public override void Repair ( )
	{
		throw new System.NotImplementedException();
	}

	public override void SetUP ( )
	{
		myData = ScriptableObject.CreateInstance<CR_Data>();

		useUpgrades = false;
		usedurability = false;
		useEmployee = false; // llas funciones de los empleados se usaran desde el editor de rutas
		useProcess = false ;
		useInmigrants = true;//inmigrantes salubridad y control
		//TODO initialize buidlings

		maxInmigrantNum = CalculateMaxIlegalsInFacility();
		currentInmigrantNum = GetCurrentTotalOfIlegals();
	}

	//after tick
	public override void UpdateUIData ( )
	{
		myData.durability = currentDurability / maxDurability;
		myData.progress = currentProgress;
		myData.updatedValuesEvent.Invoke();
	}

	public override void ShowUI ( )
	{
		UIController.Instance.ShowEdificioUI(myData );
	}

	public override void Tick ( )
	{
		foreach ( Edificio building in edificiosDelRecinto)
		{
			building.Tick();
		}
		salubridad = CalculateSalubrity();
		UpdateUIData();
	}

	public override void Upgrade ( )
	{
		throw new System.NotImplementedException();
	}

	protected override void StartProcessInmigrant ( )
	{
		throw new System.NotImplementedException();
	}
	/// <summary>
	/// En vez de calcular la salubridad desde aqui podriamos hacerlo desde los dormitorios
	/// ya que solo afectan a la salubridad los inmigrantes de los dormitorios
	/// </summary>
	/// <returns></returns>
	#region Salubrity

	private float CalculateSalubrity ( )
	{
		float inmigrantesEnDormitorios = GetNumOfInmigrantsInDorms();
		float inmigrantesMaxDormitorios = GetMaxOfInmigrantsInDorms();

		float aforo = inmigrantesEnDormitorios / inmigrantesMaxDormitorios;
		
		//MIN -1 MAX 1
		float result = 0;
		if (aforo > 1)
			result -= aforo * factorSuciedad * Time.deltaTime;
		else
			result += (1 + (1 - aforo * factorSuciedad * Time.deltaTime));

		return result;
	}

	private float GetNumOfInmigrantsInDorms ( )
	{
		//TODO hacer dormitorios para poder completar esta funcion
		return 10;
	}
	private float GetMaxOfInmigrantsInDorms ( )
	{
		//TODO hacer dormitorios para poder completar esta funcion
		return 10;
	}
#endregion

	private float CalculateControl ( )
	{
		float result = 0;
		result = 1 - ((currentInmigrantNum - (currentEmployeeNum * inmigrantesPorGuardia)) / maxInmigrantNum);

		return result;
	}
	private int CalculateMaxIlegalsInFacility ( )
	{
		int result = 0;
		foreach (Edificio building in edificiosDelRecinto)
		{
			result += building.GetMaxInmigrants();
		}
		return result;
	}
	private int GetCurrentTotalOfIlegals ( )
	{
		int result = 0;
		foreach (Edificio building in edificiosDelRecinto)
		{
			result += building.GetCurrentInmigrants();
		}
		return result;
	}
}
