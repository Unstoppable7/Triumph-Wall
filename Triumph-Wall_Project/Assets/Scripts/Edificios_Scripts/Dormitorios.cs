using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dormitorios : Edificio
{

    public Queue<GameObject> sleepingPlaces;

    public int structureCost, maintenanceCost;

    [SerializeField]
    private UIDataTypes.Buildings.SO_UIDorm_Data myUIData = null;

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
        immigrantNum = sleepingPlaces.Count;
    }

    void RemoveImmigrant()
    {
        sleepingPlaces.Dequeue();
        immigrantNum = sleepingPlaces.Count;
    }

    public override void SetUP()
    {
        //TODO hacer que cada dia se gaste un poco la durabilidad
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
        currentDurability = maxDurability;
    }

    protected override void StartProcessInmigrant()
    {

    }
       
    public override void Upgrade()
    {
        maxImmigrants += 2;
    }

    public override void ResetDay()
    {
        throw new System.NotImplementedException();
    }

    public override void ResetMonth()
    {
        throw new System.NotImplementedException();
    }

    protected override void SetDataFromObject()
    {
        throw new System.NotImplementedException();
    }

    protected override void UpdateDataObject()
    {
        throw new System.NotImplementedException();
    }
}
