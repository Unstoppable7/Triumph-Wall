using UnityEngine;
using MyUtils.CustomEvents;

namespace UIDataTypes
{
	namespace Buildings
	{
        [CreateAssetMenu(menuName = "Edificios/UIData/Cocina")]
        public class SO_UICocina_Data : UIB_Data
		{
			public bool alertFood = false;
			public float foodStorage = 0;
			public int currentPortion = 0;

			public IntEvent notifyDorpdownChange = new IntEvent();
		}
	}
}

