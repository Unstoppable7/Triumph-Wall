using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cocina_Behaviour : Edificio
{

    [SerializeField]
    private UIDataTypes.Buildings.SO_UICocina_Data myUIData = null;

    public int totalImmigrantsInFrontier;
    public override void Repair()
    {
        throw new System.NotImplementedException();
    }

    public override void ResetDay()
    {
        throw new System.NotImplementedException();
    }

    public override void ResetMonth()
    {
        throw new System.NotImplementedException();
    }

    public override void SetUP()
    {
        totalImmigrantsInFrontier = 0;

        maxInmigrantNum = 10;
        currentInmigrantNum = totalImmigrantsInFrontier;
        Tick();
    }

    public override void ShowUI()
    {
        UIController.Instance.ShowEdificioUI(myUIData);
    }

    public override void Tick()
    {

        UpdateUIData();
    }

    public override void UpdateUIData()
    {
        myUIData.currentInmigrantNum = currentInmigrantNum; 
        myUIData.maxInmigrantNum = maxInmigrantNum;
        myUIData.updatedValuesEvent.Invoke();
    }

    public override void Upgrade()
    {
        throw new System.NotImplementedException();
    }

    protected override void SetDataFromObject()
    {
        throw new System.NotImplementedException();
    }

    protected override void StartProcessInmigrant()
    {
        //usare esta funcion para saber cuantos immigrantes hay actualmente
        currentInmigrantNum++; //si current immigrant num es mas grande que max immigrant num se deberan contratar a mas cocineros
    }

    protected override void UpdateDataObject()
    {
        throw new System.NotImplementedException();
    }
}
