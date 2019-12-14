using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.EventSystems;

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
		if (EventSystem.current.IsPointerOverGameObject( -1 ))    // is the touch on the GUI
		{
			return;
		}
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

		/////////////////////LEFT CLICK////////////////////////////////
		if (player.GetButtonDown( Const.Input.Strings.LEFT_CLICK ))
		{
			//lmcClickPos = currentMouse.screenPosition;
			//select object if colided with something
			//show its UI

			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay( player.controllers.Mouse.screenPosition );

			if (Physics.Raycast( ray, out hit ))
			{
				//CameraScript.StartDragMove()
				lmcClickPos = new Vector2( hit.point.x, hit.point.z );
				if (hit.collider.gameObject.GetComponent<Edificio>())
				{
					hit.collider.gameObject.GetComponent<Edificio>().ShowUI();
				}
				else
				{
					Debug.Log( hit.collider.gameObject.name );
				}
			}

		}
		//if button still down call drag events
		if (player.GetButton( Const.Input.Strings.LEFT_CLICK ))
		{
			lmcCurrentPos = currentMouse.screenPosition;

			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay( player.controllers.Mouse.screenPosition);

			if (Physics.Raycast( ray, out hit ))
			{
				lmcCurrentPos = new Vector2( hit.point.x, hit.point.z );
			}

			if (currentMouse.screenPositionDelta.magnitude > Const.Input.Params.mouseThresHold)
			{
				//Dragging
				//CameraScript.OnDrag( PositionDelta );
				Vector3 posD = lmcCurrentPos - lmcClickPos;
				Vector3 move = new Vector3( posD.x * 1, 0, posD.y * 1 );
				testObject.transform.Translate( -move, Space.World );

			}
			else
			{
				//stoppped draggin
				//CameraScript.StopDraggin();
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

		}
		else if(player.GetButton( Const.Input.Strings.CAM_DOWN ) || (currentMouse.screenPosition.y <= 0 + borderThickness 
			&& currentMouse.screenPosition.y >= 0- borderThickness))
		{

		}
		if (player.GetButton( Const.Input.Strings.CAM_RIGHT) || (currentMouse.screenPosition.x >= Screen.width - borderThickness
			&& currentMouse.screenPosition.x <= Screen.width + borderThickness))
		{

		}
		else if (player.GetButton( Const.Input.Strings.CAM_LEFT) || (currentMouse.screenPosition.x <= 0 + borderThickness
			&& currentMouse.screenPosition.x >= 0 - borderThickness))
		{

		}

		///////////////////////////////////MOUSE WHEEL////////////////////
		if (player.GetAxis(Const.Input.Strings.WHEEL ) > 0)
		{
		}
		else if(player.GetAxis( Const.Input.Strings.WHEEL ) < 0)
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
