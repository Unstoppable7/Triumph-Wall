using System.Collections.Generic;
using UnityEngine;
using MyUtils.CustomEvents;

public class Enfermeria_Behaviour : Edificio
{

    private Queue<Agent_Inmigrant> immigrantsToHeal = new Queue<Agent_Inmigrant>();
	public InmigrantEvent inmigrantHealed = new InmigrantEvent();

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

		pricePerEmployee = 10;
        currentEmployeeNum = 1;
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

    protected override void UpdateUIData()
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
	public override void IncrementInmigrants (Agent_Inmigrant inmigrant = null)
	{
		base.IncrementInmigrants();
		immigrantsToHeal.Enqueue( inmigrant );
	}
	public override void DecrementInmigrants (Agent_Inmigrant inmigrant = null)
	{
		for(int i = 0; i < currentEmployeeNum; i++)
		{
			if (currentInmigrantNum - 1 < 0) break;
			base.DecrementInmigrants();

			if(immigrantsToHeal.Count > 0)
			{
				inmigrantHealed.Invoke( immigrantsToHeal.Dequeue() );
			}
			else
			{
				inmigrantHealed.Invoke( null);
			}
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
	}

	public override void ResetMonth ( )
	{
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
	public override float GetUpgradePrice ( )
	{
		//TODO from blanacefile SO
		return 10;
	}

	public override float GetRepairPrice ( )
	{
		//TODO from blanacefile SO
		return 10;
	}
}
