using System.Collections;
using System.Collections.Generic;

using UnityEngine;
#if UNITY_EDITOR
namespace SimpleTools
{
	public class DevelopmentTracker : ScriptableObject {
		public string version = "1.0.0";
		public int build;
		public string description;
		public List<string> history = new List<string>();
	}
}
#endif