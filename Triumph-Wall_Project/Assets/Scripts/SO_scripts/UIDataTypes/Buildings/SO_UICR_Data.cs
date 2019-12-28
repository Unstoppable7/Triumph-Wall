using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UIDataTypes
{
	namespace Buildings
	{
		[CreateAssetMenu( menuName = "Edificios/UIData/Centro de Retencion" )]
		public class SO_UICR_Data : UIB_Data
		{
			[HideInInspector]
			public float salubrity = 0;
			[HideInInspector]
			public float control = 0;
		}
	}
}