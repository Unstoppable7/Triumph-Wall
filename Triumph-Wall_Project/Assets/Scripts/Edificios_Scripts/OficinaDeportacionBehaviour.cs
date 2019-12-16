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

    public Queue<GameObject> immigrantsToDeport;

    private UIDataTypes.Buildings.UIODI myUIData;

    void Start()
    {
        
    }

    public override void SetUP()
    {
        numFuncs = 1;
        deportTime = 10.2f; //empieza siendo 10 segundos, restando 0'2 segundos por funcionario, hasta un maximo de 10 - (n * 0.2), donde n es el num de funcionarios
        resetDeportTime = deportTime;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Tick()
    {

    }

    public override void UpdateUIData()
    {
        myUIData.numFuncs = numFuncs;
        myUIData.deportTime = deportTime;

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
        if(deportTime <=0.0f)
        {
            immigrantsToDeport.Dequeue();
            deportTime = resetDeportTime;
            print("deported");
        }
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
}
