using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu( menuName = "GameFile/GameState" )]
public class GameState : ScriptableObject
{
	public enum GameStates { MANAGMENT, EDIT_PATH, CONSTRUCTION }

	[EnumPaging]
	public GameStates gameState = GameStates.MANAGMENT;

}
