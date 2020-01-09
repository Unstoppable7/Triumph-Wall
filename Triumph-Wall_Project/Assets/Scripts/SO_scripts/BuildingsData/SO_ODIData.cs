using UnityEngine;

namespace BuildingDataTypes
{
	[CreateAssetMenu( menuName = "Edificios/BalanceData/Oficina de Deportacion" )]
	public class SO_ODIData : BEdificio_Data
	{
		public float priceOfUpgrade = 100;
		public float priceOfRepair = 100;
		public int totalDeported = 0;
		public int normalDeported = 0;
		public int woundedDeported = 0;
		public int greavousDeported = 0;
	}
}
