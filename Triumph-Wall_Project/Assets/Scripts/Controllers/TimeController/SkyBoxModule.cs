using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Experimental.Rendering.HDPipeline;

public class SkyBoxModule : DNModuleBase
{
	[SerializeField]
	private Gradient skyColor = null;
	[SerializeField]
	private Gradient horizonColor = null;
	[SerializeField]
	private Volume sceneVolumeSettings = null;
	[SerializeField]
	private int indexOfSkybox = 2;

	public override void UpdateModule (float intensity, float timeOfDay)
	{
		//HDRP
		((ProceduralSky)sceneVolumeSettings.profile.components[indexOfSkybox]).skyTint.value = skyColor.Evaluate( intensity );
		((ProceduralSky)sceneVolumeSettings.profile.components[indexOfSkybox]).groundColor.value = horizonColor.Evaluate( intensity );
		//DRP
		//RenderSettings.skybox.SetColor( "_SkyTint", skyColor.Evaluate( intensity ) );
		//RenderSettings.skybox.SetColor( "_GroundColor", horizonColor.Evaluate( intensity ) );
	}
}
