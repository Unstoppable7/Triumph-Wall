using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DNModuleBase : MonoBehaviour
{
	public abstract void SetUp ();
	public abstract void UpdateModule (float intensity, float timeOfDay);
}
