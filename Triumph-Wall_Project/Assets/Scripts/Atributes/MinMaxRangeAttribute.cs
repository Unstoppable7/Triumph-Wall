//Código de uso público: https://bitbucket.org/richardfine/scriptableobjectdemo/commits/03a730f1b0581c0d424268bc03e33dac21f34248?w=0#chg-Assets/ScriptableObject/Audio/MinMaxRangeAttribute.cs
//Creado por: Richard Fine https://bitbucket.org/richardfine/
//Definición del atributi MinMaxAttribute
using System;

public class MinMaxRangeAttribute : Attribute {
    public MinMaxRangeAttribute(float min, float max)
	{
		Min = min;
		Max = max;
	}

	public float Min { get; private set; }
	public float Max { get; private set; }

}

/*Ejemplo de Implementación:
  [MinMaxRange(0, 2)]
	public RangedFloat pitch;
 */
