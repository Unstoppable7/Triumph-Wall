using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerController : MonoBehaviour
{
	[SerializeField]
	private bool use24Clock = true;
	[SerializeField]
	private TMPro.TextMeshProUGUI clockText;

	[Header( "Time" )]
	[Tooltip( "Day Length in Minutes" )]
	[SerializeField][Range(0f, 1440.0f)]
	private float _targetDayLength = 0.5f; //length of day in minutes

	[SerializeField]
	private float _elapsedTime;

	[SerializeField][Range( 0f, 1f )]
	private float _timeOfDay;

	[SerializeField]
	private int _dayNumber = 0; //tracks the days passed

	[SerializeField]
	private int _yearNumber = 0;

	[SerializeField]
	private int _yearLength = 100;

	private float _timeScale = 100f;
	public bool pause = false;

	[SerializeField]
	private AnimationCurve timeCurve;
	private float timeCurveNormalization;

	[Header( "Sun Light" )]
	[SerializeField]
	private Transform dailyRotation;
	private Light sun;
	private float intensity;
	[SerializeField]
	private float sunBaseIntensity = 1f;
	[SerializeField]
	private float sunVariation = 1.5f;
	[SerializeField]
	private Gradient sunColor;

	[Header( "Seasonal Variables" )]
	[SerializeField]
	private Transform sunSeasonalRotation;
	[SerializeField]
	[Range( -45f, 45f )]
	private float maxSeasonalTilt;

	private void Start ( )
	{
		sun = dailyRotation.GetComponentInChildren<Light>();
		NormalTimeCurve();
	}

	private void Update ( )
	{
		if (!pause)
		{
			UpdateTimeScale();
			UpdateTime();
			//UpdateClock();
		}
		AdjustSunRotation();
		SunIntensity();
		AdjustSunColor();
		//UpdateModules(); //will update modules each frame
	}

	private void UpdateTimeScale ( )
	{
		_timeScale = 24 / (_targetDayLength / 60);
		_timeScale *= timeCurve.Evaluate( _elapsedTime / (_targetDayLength * 60) ); //changes timescale based on time curve
		_timeScale /= timeCurveNormalization; //keeps day length at target value
	}

	private void NormalTimeCurve ( )
	{
		float stepSize = 0.01f;
		int numberSteps = Mathf.FloorToInt( 1f / stepSize );
		float curveTotal = 0;

		for (int i = 0; i < numberSteps; i++)
		{
			curveTotal += timeCurve.Evaluate( i * stepSize );
		}

		timeCurveNormalization = curveTotal / numberSteps; //keeps day length at target value
	}

	private void UpdateTime ( )
	{
		_timeOfDay += Time.deltaTime * _timeScale / 86400;
		_elapsedTime += Time.deltaTime;

		if (_timeOfDay > 1) //new day!!
		{
			_elapsedTime = 0;
			_dayNumber++;
			_timeOfDay -= 1;

			if (_dayNumber > _yearLength) //new year!
			{
				_yearNumber++;
				_dayNumber = 0;
			}
		}
	}

	//rotates the sun daily (and seasonally soon too);
	private void AdjustSunRotation ( )
	{
		float sunAngle = _timeOfDay * 360f;
		dailyRotation.transform.localRotation = Quaternion.Euler( new Vector3( 0f, 0f, sunAngle ) );

		float seasonalAngle = -maxSeasonalTilt * Mathf.Cos( _dayNumber / _yearLength * 2f * Mathf.PI );
		sunSeasonalRotation.localRotation = Quaternion.Euler( new Vector3( seasonalAngle, 0f, 0f ) );
	}
	private void SunIntensity ( )
	{
		intensity = Vector3.Dot( sun.transform.forward, Vector3.down );
		intensity = Mathf.Clamp01( intensity );

		sun.intensity = intensity * sunVariation + sunBaseIntensity;
	}

	private void AdjustSunColor ( )
	{
		sun.color = sunColor.Evaluate( intensity );
	}
}
