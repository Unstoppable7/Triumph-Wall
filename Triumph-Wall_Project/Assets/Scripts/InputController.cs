using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class InputController : MonoBehaviour
{
	Rewired.Player player = null;
	public GameObject testObject = null;
	private GameState currentState = null;

	private float borderThickness = 50.0f;
	private Rewired.Mouse currentMouse = null;

	public void SetUp ( )
	{
		player = ReInput.players.GetPlayer( 0 );
	}
	public void Tick ( )
	{
		switch (currentState.gameState)
		{
		case GameState.GameStates.MANAGMENT:
			ManagmentState();
			break;
		case GameState.GameStates.EDIT_PATH:
			break;
		case GameState.GameStates.CONSTRUCTION:
			break;
		default:
			break;
		}
	}

	private void ManagmentState ( )
	{
		currentMouse = player.controllers.Mouse;
		if (currentMouse.screenPositionDelta.magnitude > Const.Input.Params.mouseThresHold
			&& player.GetButtonDown( Const.Input.Strings.selection ))
		{
			//select object if colided with something
			//RaycastHit hit;
			//Ray ray = Camera.main.ScreenPointToRay( player.controllers.Mouse.screenPosition );

			//if (Physics.Raycast( ray, out hit ))
			//{
			//	testObject.transform.position = new Vector3( hit.point.x, 2, hit.point.z );
			//}
		}

		//if button still down call drag events
		if(player.GetButton( Const.Input.Strings.selection ))
		{

		}

		/////////////////////////////ARROW INPUTS/////////////////////////
		if(player.GetButton( Const.Input.Strings.CamUP ) || currentMouse.screenPosition.y >= Screen.height - borderThickness)
		{

		}
		else if(player.GetButton( Const.Input.Strings.CamDOWN ) || currentMouse.screenPosition.y <= 0 + borderThickness)
		{

		}
		if (player.GetButton( Const.Input.Strings.CamRIGHT) || currentMouse.screenPosition.x >= Screen.width - borderThickness)
		{

		}
		else if (player.GetButton( Const.Input.Strings.CamLEFT) || currentMouse.screenPosition.x <= 0 + borderThickness)
		{

		}



	}
}
