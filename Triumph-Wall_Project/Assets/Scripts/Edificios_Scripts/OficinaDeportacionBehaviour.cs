using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OficinaDeportacionBehaviour : Edificio
{
	
	private int totalDeported = 0;
	//TODO Consultar inmigrante a la hora de deportarlo para saber si esta:
	//- Normal
	//- Herido
	//- Gravemente Herido
	private int normalDeported = 0;
	private int woundedDeported = 0;
	private int greavousDeported = 0;

	public int numFuncs;
    public float deportTime;
    private float resetDeportTime;

    public int structureCost, maintenanceCost;
    public int durabilityDays, resetDurabilityDays;

    public Queue<GameObject> immigrantsToDeport = new Queue<GameObject>();

    private UIDataTypes.Buildings.UIODI_Data myUIData;

	public override void SetUP()
    {
        myUIData = ScriptableObject.CreateInstance<UIDataTypes.Buildings.UIODI_Data>();

        currentEmployeeNum = 1;
        maxEmployeeNum = 10;

        currentProgress = 10.2f; //empieza siendo 10 segundos, restando 0'2 segundos por funcionario, hasta un maximo de 10 - (n * 0.2), donde n es el num de funcionarios
        resetDeportTime = currentProgress;

        myUIData.showEmployeeNum = true;
        myUIData.showProgress = true;
        myUIData.showInmigrantNum = true;

    }

    public override void Tick()
	{
		currentInmigrantNum = immigrantsToDeport.Count;
		RemoveImmigrant();
        UpdateUIData();
    }

    public override void UpdateUIData()
    {
        myUIData.processSpeed = resetDeportTime;
        myUIData.maxEmployeeNum = maxEmployeeNum;
        myUIData.maxInmigrantNum = maxEmployeeNum;
        myUIData.currentProgress = currentProgress/ resetDeportTime;
        myUIData.currentEmployeeNum = currentEmployeeNum;
        myUIData.currentInmigrantNum = currentInmigrantNum;
        myUIData.updatedValuesEvent.Invoke();
    }

    public override void ShowUI()
    {
        UIController.Instance.ShowEdificioUI(myUIData);
    }

    void AddImmigrant(GameObject immigrant)
    {
        immigrantsToDeport.Enqueue(immigrant);
    }

    void RemoveImmigrant()
    {
        if (currentProgress <= 0.0f && immigrantsToDeport.Count > 0)
        {
            immigrantsToDeport.Dequeue();
            deportTime = resetDeportTime;
        }
        else
            currentProgress -= Time.deltaTime;
    }

    public override void Repair()
    {
        int money = 0;//he pensado que le podriamos pasar como parametro a la funcion el dinero que tiene el player

        if(money >= maintenanceCost)
        {
            durabilityDays = resetDurabilityDays;
        }
    }

    protected override void StartProcessInmigrant()
    {
        RemoveImmigrant();
    }

    public override void Upgrade()
    {
        throw new System.NotImplementedException();
    }

	public override void ResetDay ( )
	{
		throw new System.NotImplementedException();
	}

	public override void ResetMonth ( )
	{
		totalDeported = 0;
		normalDeported = 0;
		woundedDeported = 0;
		greavousDeported = 0;
	}

	public int GetTotalDeported ( )
	{
		return totalDeported;
	}

	public int GetNormalDeported ( )
	{
		return normalDeported;
	}
	public int GetWoundedDeported ( )
	{
		return woundedDeported;
	}
	public int GetGrevousDeported ( )
	{
		return greavousDeported;
	}
}
