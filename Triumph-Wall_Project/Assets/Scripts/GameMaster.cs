
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
	private UIController uiController;

	[SceneObjectsOnly][SerializeField]
	private CentroDeRetencion buildingsManager;

    void Start()
    {
		//GET
		inputControl = GetComponent<InputController>();
		uiController = GetComponent<UIController>();
        cameraController = Camera.main.GetComponent<CameraBehaviour>();
		//SET
		inputControl.SetUp(ref globalState);
		uiController.SetUP();
		buildingsManager.SetUP();
    }

    // Update is called once per frame
    void Update()
    {
		inputControl.Tick();
		buildingsManager.Tick();
    }
}
