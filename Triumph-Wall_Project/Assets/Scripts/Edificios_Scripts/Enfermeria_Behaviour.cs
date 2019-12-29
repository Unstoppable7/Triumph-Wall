using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enfermeria_Behaviour : Edificio
{

    public Queue<GameObject> immigrantsToHeal = new Queue<GameObject>();

	private int processSpeedEmployeeCap = 10;

    [SerializeField]
    private UIDataTypes.Buildings.SO_UIENF_Data myUIData = null;

    public override void SetUP()
	{
		myUIData.name = "Nursing";
		myUIData.managerID = managerID;
		processSpeed = 10.2f;

		maxOfUpgrades = 10;
		currentUpgrade = 0;
        currentProgress = processSpeed;
        maxEmployeeNum = 1;
        maxInmigrantNum = maxEmployeeNum;
        currentEmployeeNum = 0;
        currentInmigrantNum = 0;
	}
	public override void Tick ( )
	{
		currentInmigrantNum = immigrantsToHeal.Count;
		ProcessInmigrant();
		UpdateUIData();
	}

	public override void ShowUI()
    {
        UIController.Instance.ShowEdificioUI(myUIData);
    }

    public override void UpdateUIData()
    {
		myUIData.maxOfUpgrades = maxOfUpgrades;
		myUIData.currentUpgrade = currentUpgrade;
        myUIData.processSpeed = processSpeed;
        myUIData.maxEmployeeNum = maxEmployeeNum;
        myUIData.maxInmigrantNum = maxInmigrantNum;
        myUIData.currentProgress = 1 - (currentProgress / processSpeed); // para que la slider suba y no baje
        myUIData.currentEmployeeNum = currentEmployeeNum;
        myUIData.currentInmigrantNum = currentInmigrantNum;
        myUIData.updatedValuesEvent.Invoke();
    }
	public override void IncrementInmigrants (GameObject inmigrant = null)
	{
		if(currentInmigrantNum + 1 <= maxInmigrantNum)
		{
			base.IncrementInmigrants();
			immigrantsToHeal.Enqueue( inmigrant );
		}
	}
	public override void DecrementInmigrants (GameObject inmigrant = null)
	{
		for(int i = 0; i < currentEmployeeNum; i++)
		{
			if (currentInmigrantNum - 1 < 0) break;

			base.DecrementInmigrants();
			immigrantsToHeal.Dequeue();
		}
	}

	public override void BuyEmployee ( )
	{
		base.BuyEmployee();
		//empieza siendo 10 segundos, restando 0'2 segundos por funcionario,
		//hasta un maximo de 10 - (n * 0.2), donde n es el num de funcionarios
		//hasta un maximo de 10 - (n * 0.2), donde n es el num de funcionarios
		if(currentEmployeeNum + 1 <= processSpeedEmployeeCap)
			processSpeed = 10.0f - (currentEmployeeNum * 0.2f);

		maxInmigrantNum = currentEmployeeNum;
	}
	public override void FireEmployee ( )
	{
		base.FireEmployee();
		//empieza siendo 10 segundos, restando 0'2 segundos por funcionario,
		//hasta un maximo de 10 - (n * 0.2), donde n es el num de funcionarios
		//hasta un maximo de 10 - (n * 0.2), donde n es el num de funcionarios
		if (currentEmployeeNum - 1 <= processSpeedEmployeeCap)
			processSpeed = 10.0f - (currentEmployeeNum * 0.2f);

		maxInmigrantNum = currentEmployeeNum;
	}

	public override void Upgrade()
    {
        if(currentUpgrade +1 <= maxOfUpgrades)
		{
			currentUpgrade++;
			maxEmployeeNum++;
		}
    }
	public override void Repair ( )
	{
		throw new System.NotImplementedException();
	}

	public override void ResetDay ( )
	{
		throw new System.NotImplementedException();
	}

	public override void ResetMonth ( )
	{
		throw new System.NotImplementedException();
	}

    protected override void ProcessInmigrant()
	{
		if (currentProgress <= 0.0f && immigrantsToHeal.Count > 0)
		{
			DecrementInmigrants();
			currentProgress = processSpeed;
		}
		else if (immigrantsToHeal.Count > 0)    //TODO cambiar el tiempo de procesamiento segun la gravedad de las heridas del immigrante
			currentProgress -= Time.deltaTime;
	}

	protected override void SetDataFromObject ( )
	{
		throw new System.NotImplementedException();
	}

	protected override void UpdateDataObject()
    {
		throw new System.NotImplementedException();
	}
}
