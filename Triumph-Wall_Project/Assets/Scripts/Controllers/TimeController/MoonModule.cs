using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.HDPipeline;

public class MoonModule : DNModuleBase
{
	[SerializeField]
	private Light moon = null;
	private HDAdditionalLightData moonData = null;
	[SerializeField]
	private Gradient moonColor = null;
	[SerializeField]
	private float baseIntensity = 0;
	private void Start ( )
	{
		moonData = moon.gameObject.GetComponent<HDAdditionalLightData>();

	}
	public override void UpdateModule (float intensity, float timeOfDay)
	{
		moon.color = moonColor.Evaluate( 1 - intensity );
		moonData.intensity = (1 - intensity) * baseIntensity + 0.05f;

		if (timeOfDay < TimerController.startDay || timeOfDay > TimerController.endDay)
		{
			if (moon.shadows == LightShadows.None)
			{
				moon.shadows = LightShadows.Hard;
			}
		}
		else
		{
			if (moon.shadows == LightShadows.Hard)
			{
				moon.shadows = LightShadows.None;
			}
		}
	}
}
