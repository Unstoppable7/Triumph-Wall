﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GameMaster : MonoBehaviour
{
	[SerializeField][Required]
	private GameState globalState = null;

	private InputController inputControl = null;
    // Start is called before the first frame update
    void Start()
    {
		inputControl = GetComponent<InputController>();
		inputControl.SetUp(ref globalState);
    }

    // Update is called once per frame
    void Update()
    {
		inputControl.Tick();

    }
}
