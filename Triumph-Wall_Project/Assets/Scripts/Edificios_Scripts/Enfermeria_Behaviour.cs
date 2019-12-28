using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enfermeria_Behaviour : Edificio
{

    public Queue<GameObject> immigrantsToHeal = new Queue<GameObject>();


    [SerializeField]
    private UIDataTypes.Buildings.SO_UIENF_Data myUIData = null;
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
        processSpeed = 10.2f;

        currentProgress = processSpeed;
        maxEmployeeNum = 10;
        maxInmigrantNum = maxEmployeeNum;
        currentEmployeeNum = 0;
        currentInmigrantNum = 0;
    }

    public override void ShowUI()
    {
        UIController.Instance.ShowEdificioUI(myUIData);
    }

    public override void Tick()
    {
        currentInmigrantNum = immigrantsToHeal.Count;
        RemoveImmigrant();


        UpdateUIData();
    }

    public override void UpdateUIData()
    {
        myUIData.processSpeed = processSpeed;
        myUIData.maxEmployeeNum = maxEmployeeNum;
        myUIData.maxInmigrantNum = maxInmigrantNum;
        myUIData.currentProgress = currentProgress / processSpeed;
        myUIData.currentEmployeeNum = currentEmployeeNum;
        myUIData.currentInmigrantNum = currentInmigrantNum;
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
        throw new System.NotImplementedException();
    }

    protected override void UpdateDataObject()
    {

    }

    private void RemoveImmigrant()
    {
        if (currentProgress <= 0.0f && immigrantsToHeal.Count > 0)
        {
         

            base.DecrementInmigrants();
            immigrantsToHeal.Dequeue();
            currentProgress = processSpeed;
        }
        else if (immigrantsToHeal.Count > 0)    //TODO cambiar el tiempo de procesamiento segun la gravedad de las heridas del immigrante
            currentProgress -= Time.deltaTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
