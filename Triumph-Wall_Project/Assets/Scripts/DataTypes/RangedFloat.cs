//Código de uso público: https://bitbucket.org/richardfine/scriptableobjectdemo/commits/03a730f1b0581c0d424268bc03e33dac21f34248?w=0#chg-Assets/ScriptableObject/Audio/MinMaxRangeAttribute.cs
//Creado por: Richard Fine https://bitbucket.org/richardfine/
//Definición de la variable RangedFloat
using System;

[Serializable]
public struct RangedFloat
{
    public RangedFloat(float min, float max)
    {
        minValue = min;
        maxValue = max;
    }
    public float minValue;
	public float maxValue;
}
