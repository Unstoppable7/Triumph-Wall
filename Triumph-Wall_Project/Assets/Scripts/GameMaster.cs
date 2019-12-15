﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GameMaster : MonoBehaviour
{
	[SerializeField][Required]
	private GameState globalState = null;
    private CameraBehaviour cameraController = null;

	public Edificio debug;

	private InputController inputControl = null;
	private UIController uiController;
    // Start is called before the first frame update
    void Start()
    {
		inputControl = GetComponent<InputController>();
		uiController = GetComponent<UIController>();
		inputControl.SetUp(ref globalState);
        cameraController = Camera.main.GetComponent<CameraBehaviour>();
		uiController.SetUP();
		debug.SetUP();
    }

    // Update is called once per frame
    void Update()
    {
		inputControl.Tick();
        cameraController.Tick();
		debug.Tick();
    }
}
