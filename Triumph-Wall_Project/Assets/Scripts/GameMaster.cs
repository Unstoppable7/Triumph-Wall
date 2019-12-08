using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
	public enum GameStates { MANAGMENT, EDIT_PATH, CONSTRUCTION}
	InputController inputControl = null;
    // Start is called before the first frame update
    void Start()
    {
		inputControl = GetComponent<InputController>();
		inputControl.SetUp();
    }

    // Update is called once per frame
    void Update()
    {
		inputControl.Tick();

    }
}
