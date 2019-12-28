using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.HDPipeline;
using UnityEngine.Events;

public class TimerController : SerializedMonoBehaviour
{
	[FoldoutGroup( "Clock" )]
	[SerializeField]
	private bool use24Clock = true;
	[SerializeField]
	[FoldoutGroup( "Clock" )]
	private TMPro.TextMeshProUGUI clockText = null;
	[ShowInInspector][DisplayAsString]
	[FoldoutGroup( "Clock" )]
	private float _elapsedTime;

	[FoldoutGroup( "Time" )]
	[ShowInInspector]
	public static float startDay = 0.25f;
	[ShowInInspector]
	[FoldoutGroup( "Time" )]
	public static float endDay = 0.75f;
	[Tooltip( "Day Length in Minutes" )]
	[SerializeField][Range(0f, 1440.0f)]
	[FoldoutGroup( "Time" )]
	private float _targetDayLength = 0.5f; //length of day in minutes

	[SerializeField][Range( 0f, 1f )]
	[FoldoutGroup( "Time" )]
	private float _timeOfDay = 0;

	private float _timeScale = 100f;
	[FoldoutGroup( "Time" )]
	public bool pause = false;

	[SerializeField]
	[FoldoutGroup( "Time" )]
	private AnimationCurve timeCurve = null;
	private float timeCurveNormalization = 0;

	[FoldoutGroup( "Tracking" )]
	[ShowInInspector][DisplayAsString]
	private int _dayNumber = 1; //tracks the days passed
	[ShowInInspector][DisplayAsString]
	[FoldoutGroup( "Tracking" )]
	private int _monthNumber = 1; //tracks the days passed
	[ShowInInspector][DisplayAsString]
	[FoldoutGroup( "Tracking" )]
	private int _yearNumber = 1;

	[SerializeField][Tooltip("In Days")]
	[FoldoutGroup( "Tracking" )]
	private int _monthLenght = 30; //tracks the days passed
	[SerializeField]
	[Tooltip( "In Months" )]
	[FoldoutGroup( "Tracking" )]
	private int _yearLength = 12; 

	[FoldoutGroup( "Sun Light" )]
	[SerializeField]
	private Transform dailyRotation = null;
	[FoldoutGroup( "Sun Light" )]
	[SerializeField]
	private Light sun = null;
	private HDAdditionalLightData sunData = null;
	private float intensity;
	[SerializeField]
	[FoldoutGroup( "Sun Light" )]
	private float sunBaseIntensity = 1f;
	[SerializeField]
	[FoldoutGroup( "Sun Light" )]
	private float sunVariation = 1.5f;
	[SerializeField]
	[FoldoutGroup( "Sun Light" )]
	private Gradient sunColor = null;

	[FoldoutGroup( "Seasonal Variables" )]
	[SerializeField]
	private Transform sunSeasonalRotation = null;
	[SerializeField]
	[Range( -45f, 45f )]
	[FoldoutGroup( "Seasonal Variables" )]
	private float maxSeasonalTilt = 30;
	
	[ShowInInspector][ReadOnly]
	private List<DNModuleBase> moduleList = new List<DNModuleBase>();

	[ShowInInspector]
	public static UnityEvent dailyEvent = new UnityEvent();
	[ShowInInspector]
	public static UnityEvent monthlyEvent = new UnityEvent();
	[ShowInInspector]
	public static UnityEvent anualEvent = new UnityEvent();

	public void SetUP ( )
	{
		sunData = sun.gameObject.GetComponent<HDAdditionalLightData>();
		moduleList.Clear();
		moduleList.AddRange( GetComponents<DNModuleBase>() );

		foreach(DNModuleBase module in moduleList)
		{
			module.SetUp();
		}

		NormalTimeCurve();
	}

	public void Tick ( )
	{
		if (!pause)
		{
			UpdateTimeScale();
			UpdateTime();
			UpdateClock();
		}
		AdjustSunRotation();
		SunIntensity();
		AdjustSunColor();
		UpdateModules(); //will update modules each frame
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
			_timeOfDay -= 1;

			_dayNumber++;

			if(_dayNumber > _monthLenght)
			{
				_monthNumber++;
				_dayNumber = 1;
				monthlyEvent.Invoke();
			}

			if (_monthNumber > _yearLength) //new year!
			{
				_yearNumber++;
				_monthNumber = 1;
				anualEvent.Invoke();
			}

			dailyEvent.Invoke();
		}
	}

	private void UpdateClock ( )
	{
		float time = _elapsedTime / (_targetDayLength * 60);
		float hour = Mathf.FloorToInt( time * 24 );
		float minute = Mathf.FloorToInt( ((time * 24) - hour) * 60 );

		string calendarString = string.Format("{0:00}/{1:00}/{2:00}", _dayNumber, _monthNumber, _yearNumber);

		if (!use24Clock && hour > 12)
			hour -= 12;

		if(!use24Clock)
		{
			if (time > 0.5f)
				clockText.text = string.Format( "{0:00}:{1:00} pm\n"+ calendarString, hour, minute );
			else
				clockText.text = string.Format( "{0:00}:{1:00} am\n"+ calendarString, hour, minute );
		}
		else
		{
			clockText.text = string.Format( "{0:00}:{1:00}\n"+ calendarString, hour, minute );
		}
	}

	//rotates the sun daily (and seasonally soon too);
	private void AdjustSunRotation ( )
	{
		float sunAngle = _timeOfDay * 360f;
		dailyRotation.transform.localRotation = Quaternion.Euler( new Vector3( 0f, 0f, sunAngle ) );
		float totaldays = ((_monthNumber - 1) * (_monthLenght) + _dayNumber-1);
		float maxYearDay = (_monthLenght * _yearLength);

		float cos = Mathf.Cos((totaldays / maxYearDay) * 2f * Mathf.PI );
		float seasonalAngle = -maxSeasonalTilt * cos;

		sunSeasonalRotation.localRotation = Quaternion.Euler( new Vector3( seasonalAngle, 0f, 0f ) );
	}
	private void SunIntensity ( )
	{
		intensity = Vector3.Dot( sun.transform.forward, Vector3.down );
		intensity = Mathf.Clamp01( intensity );

		//sun.intensity = intensity * sunVariation + sunBaseIntensity;
		sunData.intensity = intensity * sunVariation + sunBaseIntensity;

		if (_timeOfDay < startDay || _timeOfDay > endDay)
		{
			if (sun.shadows == LightShadows.Hard)
			{
				sun.shadows = LightShadows.None;
			}
		}
		else
		{
			if (sun.shadows == LightShadows.None)
			{
				sun.shadows = LightShadows.Hard;
			}
		}
	}
	private void AdjustSunColor ( )
	{
		sun.color = sunColor.Evaluate( intensity );
	}

	public void AddModule (DNModuleBase module)
	{
		moduleList.Add( module );
	}
	public void RemoveModule (DNModuleBase module)
	{
		moduleList.Remove( module );
	}
	//update each module based on current sun intensity
	private void UpdateModules ( )
	{
		foreach (DNModuleBase module in moduleList)
		{
			module.UpdateModule( intensity, _timeOfDay );
		}
	}

	[SerializeField][HideInInspector]
	private float where = 0;

	[FoldoutGroup( "Set Day and Night Scene", -1 )]
	[ShowInInspector][PropertyRange( 0.0f, 1.0f )]
	private float Where { get { return where; } set { where = value; MakeDay(); } }

	//[FoldoutGroup( "Set Day and Night Scene" ,-1)]
	//[Button( ButtonSizes.Medium)]
	private void MakeDay ()
	{
		_timeOfDay = where;
		SetUP();
		Tick();
	}
}
