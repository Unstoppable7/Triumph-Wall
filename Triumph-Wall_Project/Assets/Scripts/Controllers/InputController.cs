using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour
{
	Rewired.Player player = null;

	private SO_GameState currentState;

	private CameraBehaviour cameraController = null;

	[SerializeField]
	private float borderThickness = 50.0f;

	//mose variables
	private Rewired.Mouse currentMouse = null;
	private Vector2 lmcClickPos;
	private Vector2 lmcCurrentPos;

	private Vector2 tClickPos;
	private Vector2 tCurrentPos;

	public void SetUp (ref SO_GameState _currentState )
	{
		player = ReInput.players.GetPlayer( 0 );

		currentState = _currentState;

		cameraController = Camera.main.GetComponent<CameraBehaviour>();
	}

	public void Tick ( )
	{
		if (EventSystem.current.IsPointerOverGameObject( -1 ))    // is the touch on the GUI
		{
			return;
		}
		switch (currentState.gameState)
		{
		case SO_GameState.GameStates.MANAGMENT:
			ManagmentState();
			break;
		case SO_GameState.GameStates.EDIT_PATH:
			EditPathState();
			break;
		case SO_GameState.GameStates.CONSTRUCTION:
			ConstructionState();
			break;
		default:
			break;
		}
	}

	private void ManagmentState ( )
	{
		//getMouse
		currentMouse = player.controllers.Mouse;

		/////////////////////LEFT CLICK////////////////////////////////
		if (player.GetButtonDown( Const.Input.Strings.LEFT_CLICK ))
		{
			lmcClickPos = currentMouse.screenPosition;
			//cameraController.StartDrag(lmcClickPos);

			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay( player.controllers.Mouse.screenPosition );
			if (Physics.Raycast( ray, out hit ))
			{
				//select object if colided with something
				if (hit.collider.gameObject.GetComponent<Edificio>())
				{
					//show its UI
					UIController.Instance.HideUI();
					hit.collider.gameObject.GetComponent<Edificio>().ShowUI();
				}				
			}
        }
		//if button still down call drag events
		if (player.GetButton( Const.Input.Strings.LEFT_CLICK ))
		{
			lmcCurrentPos = currentMouse.screenPosition;

			if (currentMouse.screenPositionDelta.magnitude > Const.Input.Params.mouseThresHold)
			{
				//Dragging
				//cameraController.MoveDrag(lmcCurrentPos);
			}
			else
			{
				//stoppped draggin
				lmcClickPos = lmcCurrentPos;
			}
		}

		/////////////////////RIGHT CLICK////////////////////////////////
		if (player.GetButtonDown( Const.Input.Strings.RIGHT_CLICK ))
		{
			UIController.Instance.HideUI();
		}


		/////////////////////////////ARROW INPUTS/////////////////////////
		if (player.GetButton( Const.Input.Strings.CAM_UP ) || (currentMouse.screenPosition.y >= Screen.height - borderThickness 
			&& currentMouse.screenPosition.y <= Screen.height + borderThickness))
		{
			cameraController.MoveZ(1);
        }
		else if(player.GetButton( Const.Input.Strings.CAM_DOWN ) || (currentMouse.screenPosition.y <= 0 + borderThickness 
			&& currentMouse.screenPosition.y >= 0- borderThickness))
		{
			cameraController.MoveZ(-1);
        }
		if (player.GetButton( Const.Input.Strings.CAM_RIGHT) || (currentMouse.screenPosition.x >= Screen.width - borderThickness
			&& currentMouse.screenPosition.x <= Screen.width + borderThickness))
		{
			cameraController.MoveX(1);
        }
		else if (player.GetButton( Const.Input.Strings.CAM_LEFT) || (currentMouse.screenPosition.x <= 0 + borderThickness
			&& currentMouse.screenPosition.x >= 0 - borderThickness))
		{
			cameraController.MoveX(-1);
        }

		///////////////////////////////////MOUSE WHEEL////////////////////
		if (player.GetAxis(Const.Input.Strings.WHEEL ) > 0)
		{
			cameraController.ZoomInOut(player.GetAxis(Const.Input.Strings.WHEEL));
		}
		else if(player.GetAxis( Const.Input.Strings.WHEEL ) < 0)
		{
			cameraController.ZoomInOut(player.GetAxis(Const.Input.Strings.WHEEL));
        }
	}

	private void ConstructionState ( )
	{

	}

	private void EditPathState ( )
	{

	}
}
