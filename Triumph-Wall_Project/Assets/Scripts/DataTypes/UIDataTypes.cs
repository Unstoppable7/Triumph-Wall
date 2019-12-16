
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace UIDataTypes
{
	namespace Buildings
	{
		public class UIB_Data : SerializedScriptableObject
		{
			///////////UI MODIFIERS///////////
			//variables que modifican la cantidad de elementos en el menu
			// EJ: if(showUpgradeBtn){
			//			if(canBeUpgraded){
			//				normal button
			//			}
			//			else{
			//				unselectable button
			//			}
			//	   }
			//	   else{
			//			hide button
			//	   }
			/////////////////////////////NUMS
			//Upgrading nums
			public bool showUpgradeNum = false;
			//durability nums
			public bool showDurabilityBar = false;
			//employee nums
			public bool showEmployeeNum = false;
			//processing nums
			public bool showProgress = false;
			//inmigrants nums
			public bool showInmigrantNum = false;

			/////////////////////////////BUTTONS
			public bool showUpgradeBtn = false;
			//employee buttons
			public bool showBuyEmployeeBtn = false;
			public bool showFireEmployeeBtn = false;

			///////////BUILDING UI DATA///////////
			//setted from Factory and from Manager
			public int managerID = -1;
			//Upgrading
			public bool canBeUpgraded = false;
			public int maxOfUpgrades = -1;
			public int currentUpgrade = -1;
			//Durability
			public float maxDurability = 100;
			public float currentDurability = 50;
			//Employees Flags
			public bool canBuyEmployee = false;
			public bool canFireEmployee = false;
			//Employees
			public float pricePerEmployee = -1;
			public int maxEmployeeNum = -1;
			public int currentEmployeeNum = -1;
			//processing
			public float currentProgress = -1;
			public float processSpeed = -1;
			//inmigrants
			public int maxInmigrantNum = -1;
			public int currentInmigrantNum = -1;

			//used in the buidlings to tell the ui to refresh the info
			public UnityEvent updatedValuesEvent = new UnityEvent();
		}

		public class UICR_Data : UIB_Data
		{
			public float salubrity = 0;
			public float control = 0;
		}
		public class UIDorm_Data : UIB_Data
		{
			public float hola = -1;
		}
        public class UIODI: UIB_Data //oficina deportaçao
        {
            public float deportTime = 0;
            public float numFuncs = 0;
        }
    }
}
