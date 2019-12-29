using Sirenix.OdinInspector;
using UnityEngine;
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
            public bool showRepairBtn = false;
            //employee buttons
            public bool showBuyEmployeeBtn = false;
            public bool showFireEmployeeBtn = false;

            ///////////BUILDING UI DATA///////////
            //setted from Factory and from Manager
            [ShowInInspector][DisplayAsString]
            public int managerID = -1;
            //Upgrading
            [HideInInspector]
            public bool canBeUpgraded = false;
            [HideInInspector]
            public int maxOfUpgrades = -1;
            [HideInInspector]
            public int currentUpgrade = -1;
            //Durability
            [HideInInspector]
            public float maxDurability = 100;
            [HideInInspector]
            public float currentDurability = 50;
            //Employees Flags
            [HideInInspector]
            public bool canBuyEmployee = false;
            [HideInInspector]
            public bool canFireEmployee = false;
            //Employees
            [HideInInspector]
            public float pricePerEmployee = -1;
            [HideInInspector]
            public int maxEmployeeNum = -1;
            [HideInInspector]
            public int currentEmployeeNum = -1;
            //processing
            [HideInInspector]
            public float currentProgress = -1;
            [HideInInspector]
            public float processSpeed = -1;
            //inmigrants
            [HideInInspector]
            public int maxInmigrantNum = -1;
            [HideInInspector]
            public int currentInmigrantNum = -1;

            //used in the buidlings to tell the ui to refresh the info
            [HideInInspector]
            public UnityEvent updatedValuesEvent = new UnityEvent();
        }
    }
}

