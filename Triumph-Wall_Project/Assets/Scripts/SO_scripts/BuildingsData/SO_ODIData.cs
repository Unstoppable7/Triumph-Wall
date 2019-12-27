using UnityEngine;

namespace BuildingDataTypes
{
	[CreateAssetMenu( menuName = "Edificios/BalanceData/Oficina de Deportacion" )]
	public class SO_ODIData : BEdificio_Data
	{
		public int totalDeported = 0;

		public int normalDeported = 0;
		public int woundedDeported = 0;
		public int greavousDeported = 0;
	}
}
