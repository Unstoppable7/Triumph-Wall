
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace UIDataTypes
{
	namespace Buildings
	{
		public class B_Data : SerializedScriptableObject
		{
			public int managerID =-1 ;

			public bool canBeUpgraded = false;
			public int maxOfUpgrades = -1;
			public int currentUpgrade = -1;

			public float durability = -1;

			public bool canBuyEmployee = false;
			public bool canFireEmployee = false;

			public float pricePerEmployee = -1;
			public int maxEmployeeNum = -1;
			public int currentEmployeeNum = -1;

			public float progress = -1;
			public float processSpeed = -1;

			public int maxInmigrantNum = -1;
			public int currentInmigrantNum = -1;

			//used in the buidlings to tell the ui to refresh the info
			public UnityEvent updatedValuesEvent = new UnityEvent();
		}

		public class CR_Data : B_Data
		{

		}

	}
}

namespace CustomUnityEvent
{
	public class UFloatEvent : UnityEvent<float>
	{

	}
	public class UIntEvent : UnityEvent<int>
	{

	}
}