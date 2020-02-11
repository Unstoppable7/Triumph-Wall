using BehaviorDesigner.Runtime;
using UnityEngine;
[System.Serializable]
public class TreeBlackBoard : SharedVariable<BlackBoard>
{
    public static implicit operator TreeBlackBoard(BlackBoard value)
    {
        return new TreeBlackBoard { Value = value };
    }
}
