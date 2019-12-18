using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dormitorios : Edificio
{

    public Queue<GameObject> sleepingPlaces;

    public int structureCost, maintenanceCost;
    private UIDataTypes.Buildings.UIDORM_Data myUIData;

    public int immigrantNum = 5, maxImmigrants;

    void Start()
    {
        sleepingPlaces = new Queue<GameObject>();
        SetUP();
    }

    // Update is called once per frame
    void Update()
    {
        maxImmigrants = 10;
        currentInmigrantNum = 5;
        Tick();
    }

    public override void Tick()
    {
        UpdateUIData();
    }

    void AddImmigrant(GameObject immigrant)
    {
        sleepingPlaces.Enqueue(immigrant);        
    }

    void RemoveImmigrant()
    {
        sleepingPlaces.Dequeue();
    }

    public override void SetUP()
    {
        myUIData = ScriptableObject.CreateInstance<UIDataTypes.Buildings.UIDORM_Data>();

        myUIData.showInmigrantNum = true;
    }

    public override void UpdateUIData()
    {
        myUIData.currentInmigrantNum = immigrantNum;
        myUIData.maxInmigrantNum = maxImmigrants;
        myUIData.updatedValuesEvent.Invoke();
    }

    public override void ShowUI()
    {
        UIController.Instance.ShowEdificioUI(myUIData);
    }

    public override void Repair()
    {

    }

    protected override void StartProcessInmigrant()
    {

    }

    public override void Upgrade()
    {
        maxImmigrants += 2;
    }
}
