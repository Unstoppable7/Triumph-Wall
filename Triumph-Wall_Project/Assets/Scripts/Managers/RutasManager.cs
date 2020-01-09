using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class RutasManager : SerializedMonoBehaviour
{
	[SerializeField]
	private List<Ruta> rutas = new List<Ruta>();
	[SerializeField]
	private SO_GameState currentState;

	public void StartEditing ( )
	{
		currentState.gameState = SO_GameState.GameStates.EDIT_PATH;
	}

}
