using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Experimental.Rendering.HDPipeline;

public class SkyBoxModule : DNModuleBase
{
	[SerializeField]
	private Gradient skyColor;
	[SerializeField]
	private Gradient horizonColor;
	[SerializeField]
	private Volume sceneVolumeSettings;
	[SerializeField]
	private int indexOfSkybox = 2;

	public override void UpdateModule (float intensity, float timeOfDay)
	{
		//HDRP
		((ProceduralSky)sceneVolumeSettings.profile.components[indexOfSkybox]).skyTint.value = skyColor.Evaluate( intensity );
		((ProceduralSky)sceneVolumeSettings.profile.components[indexOfSkybox]).groundColor.value = skyColor.Evaluate( intensity );
		//DRP
		//RenderSettings.skybox.SetColor( "_SkyTint", skyColor.Evaluate( intensity ) );
		//RenderSettings.skybox.SetColor( "_GroundColor", horizonColor.Evaluate( intensity ) );
	}
}
