using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class InputController : MonoBehaviour
{
	Rewired.Player player = null;
	public GameObject testObject = null;
	public void SetUp ( )
	{
		player = Rewired.ReInput.players.GetPlayer( 0 );
	}
	public void Tick ( )
	{
		if (player.controllers.Mouse.screenPositionDelta.magnitude > 0.5f 
			&& player.GetButton( Const.InputStrings.selection ))
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay( player.controllers.Mouse.screenPosition );

			if (Physics.Raycast( ray, out hit ))
			{
				testObject.transform.position = new Vector3( hit.point.x, 2, hit.point.z);
			}
		}
	}

}
