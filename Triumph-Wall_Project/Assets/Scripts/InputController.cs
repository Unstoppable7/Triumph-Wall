using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class InputController : MonoBehaviour
{
	Rewired.Player player = null;
	public GameObject testObject = null;
	private GameState currentState;

	private float borderThickness = 50.0f;

	//mose variables
	private Rewired.Mouse currentMouse = null;
	private Vector2 lmcClickPos;
	private Vector2 lmcCurrentPos;

	public void SetUp (ref GameState _currentState )
	{
		player = ReInput.players.GetPlayer( 0 );

		currentState = _currentState;
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
		//getMouse
		currentMouse = player.controllers.Mouse;

		if (player.GetButtonDown( Const.Input.Strings.selection ))
		{
			lmcClickPos = currentMouse.screenPosition;
			//select object if colided with something
				//show its UI

		}

		//if button still down call drag events
		if(player.GetButton( Const.Input.Strings.selection ))
		{
			lmcCurrentPos = currentMouse.screenPosition;

			if (currentMouse.screenPositionDelta.magnitude > Const.Input.Params.mouseThresHold)
			{
				//Dragging

			}
			else
			{
				//stoppped draggin
				lmcClickPos = lmcCurrentPos;
			}
		}

		/////////////////////////////ARROW INPUTS/////////////////////////
		if (player.GetButton( Const.Input.Strings.CamUP ) || (currentMouse.screenPosition.y >= Screen.height - borderThickness 
			&& currentMouse.screenPosition.y <= Screen.height + borderThickness))
		{

		}
		else if(player.GetButton( Const.Input.Strings.CamDOWN ) || (currentMouse.screenPosition.y <= 0 + borderThickness 
			&& currentMouse.screenPosition.y >= 0- borderThickness))
		{

		}
		if (player.GetButton( Const.Input.Strings.CamRIGHT) || (currentMouse.screenPosition.x >= Screen.width - borderThickness
			&& currentMouse.screenPosition.x <= Screen.width + borderThickness))
		{

		}
		else if (player.GetButton( Const.Input.Strings.CamLEFT) || (currentMouse.screenPosition.x <= 0 + borderThickness
			&& currentMouse.screenPosition.x >= 0 - borderThickness))
		{

		}
	}

	private void ConstructionState ( )
	{

	}

	private void EditPathState ( )
	{

	}
}
