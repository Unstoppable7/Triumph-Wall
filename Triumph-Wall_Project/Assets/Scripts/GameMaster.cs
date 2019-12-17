
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GameMaster : MonoBehaviour
{
	[SerializeField][Required]
	private GameState globalState = null;
    private CameraBehaviour cameraController = null;
	private InputController inputControl = null;
	private UIController uiController = null;
	private TimerController timerController = null;

	[SceneObjectsOnly][SerializeField]
	private CentroDeRetencion buildingsManager = null;

    void Start()
    {
		//GET
		inputControl = GetComponent<InputController>();
		uiController = GetComponent<UIController>();
        cameraController = Camera.main.GetComponent<CameraBehaviour>();
		timerController = GetComponentInChildren<TimerController>();
		//SET Controllers
		inputControl.SetUp(ref globalState);
		uiController.SetUP();
		timerController.SetUP();
		//Set Managers
		buildingsManager.SetUP();
    }

    // Update is called once per frame
    void Update()
    {
		inputControl.Tick();
		timerController.Tick();

		buildingsManager.Tick();
    }
}
