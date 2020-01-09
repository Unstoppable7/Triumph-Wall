using Sirenix.OdinInspector;
using UnityEngine;

namespace BuildingDataTypes
{
	//[CreateAssetMenu( menuName = "SOs/Edificios/BalanceData/base" )]
	public class BEdificio_Data : SerializedScriptableObject
	{
		[Tooltip("True, te deja editar los valores en ejecucion del edificio")]
		public bool debug = false;
		//Upgrading
		public bool canBeUpgraded = false;
		public int maxOfUpgrades = 0;
		public int currentUpgrade = 0;
		//Durability
		public float maxDurability = 100;
		public float currentDurability = 50;
		//Employees Flags
		public bool canBuyEmployee = false;
		public bool canFireEmployee = false;
		//Employees
		public float pricePerEmployee = 0;
		public int maxEmployeeNum = 0;
		public int currentEmployeeNum = 0;
		//processing
		public bool canProcess = false;
		public float currentProgress = 0;
		public float processSpeed = 0;
		//inmigrants
		public int maxInmigrantNum = 0;
		public int currentInmigrantNum = 0;
	}

}
