using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OficinaDeportacionBehaviour : Edificio
{
    public int numFuncs;
    public float deportTime;
    private float resetDeportTime;

    public int structureCost, maintenanceCost;
    public int durabilityDays, resetDurabilityDays;

    public Queue<GameObject> immigrantsToDeport = new Queue<GameObject>();
    public GameObject test;

    private UIDataTypes.Buildings.UIODI_Data myUIData;

    void Start()
    {
        immigrantsToDeport.Enqueue(test);
        SetUP();
    }

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

    // Update is called once per frame
    void Update()
    {
        currentInmigrantNum = immigrantsToDeport.Count;
        RemoveImmigrant();
        Tick();
    }

    public override void Tick()
    {
        //RemoveImmigrant();
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

    public void AddOfficial()
    {
        currentEmployeeNum++;
    }

}
