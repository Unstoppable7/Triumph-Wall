using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using Sirenix.OdinInspector;

public class BlackBoard : SerializedMonoBehaviour
{
    public Dictionary<string, object> variables = new Dictionary<string, object>();
}
