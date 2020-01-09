using UnityEngine;
namespace BuildingDataTypes
{
	[CreateAssetMenu( menuName = "Edificios/BalanceData/Centro de Retencion" )]
	public class SO_CRData : BEdificio_Data
	{
		public float salubridad = 0;
		public float control = 0;
		public float factorSuciedad = 1.0f;
		public int inmigrantesPorGuardia = 5;
	}
}