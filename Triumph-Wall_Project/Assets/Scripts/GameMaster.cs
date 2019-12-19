
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GameMaster : MonoBehaviour
{
	[SerializeField][Required]
	private SO_GameState globalState = null;
    private CameraBehaviour cameraController = null;
	private InputController inputControl = null;
	private UIController uiController = null;
	private TimerController timerController = null;
	private ResourceController resourceController = null;

	[SerializeField][ReadOnly]
	private CentroDeRetencion buildingsManager = null;
	[ShowInInspector][ReadOnly]
	private InmigrantManager inmigrantManager = null;
	[ShowInInspector][ReadOnly]
	private PoliceManager policeManager = null;

	void Start()
    {
		//GET
		inputControl = GetComponent<InputController>();
		uiController = GetComponent<UIController>();
        cameraController = Camera.main.GetComponent<CameraBehaviour>();
		timerController = GetComponentInChildren<TimerController>();
		resourceController = GetComponentInChildren<ResourceController>();
		//GET Managers
		buildingsManager = FindObjectOfType<CentroDeRetencion>();
		inmigrantManager = FindObjectOfType<InmigrantManager>();
		policeManager = FindObjectOfType<PoliceManager>();

		//SET Controllers
		inputControl.SetUp(ref globalState);
		uiController.SetUP();
		timerController.SetUP();
		resourceController.SetUp();
		//Set Managers
		buildingsManager.SetUP();
		inmigrantManager.SetUp();
		policeManager.SetUp();
    }

    // Update is called once per frame
    void Update()
    {
		//Update Controllers
		inputControl.Tick();
		timerController.Tick();
		resourceController.Tick();
		//Update Managers
		inmigrantManager.Tick();
		policeManager.Tick();
		buildingsManager.Tick();
    }
}
