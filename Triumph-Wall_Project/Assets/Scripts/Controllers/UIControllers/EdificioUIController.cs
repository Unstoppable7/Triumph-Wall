using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class EdificioUIController : MonoBehaviour
{

	[SceneObjectsOnly]
	public GameObject canvas;
	[SceneObjectsOnly]
	public Slider progresSlider;
	[SceneObjectsOnly]
	public Slider durabilitySlider;
	[SceneObjectsOnly]
	public TextMeshProUGUI upgradesText;
	[SceneObjectsOnly]
	public TextMeshProUGUI speedText;
	[SceneObjectsOnly]
	public TextMeshProUGUI employeeText;
	[SceneObjectsOnly]
	public TextMeshProUGUI inmigrantsText;

	private UIDataTypes.Buildings.B_Data showingData;

	private UnityEvent hideEvent = new UnityEvent();

	public void ShowBaseUI ( UIDataTypes.Buildings.B_Data baseData )
	{
		durabilitySlider.value = baseData.durability;

		upgradesText.text = string.Format( "{0:00}/{1:00}", baseData.currentUpgrade, baseData.maxOfUpgrades );
		employeeText.text = string.Format( "{0:00}/{1:00}", baseData.currentEmployeeNum, baseData.maxEmployeeNum );
		inmigrantsText.text = string.Format( "{0:00}/{1:00}", baseData.currentInmigrantNum, baseData.maxInmigrantNum );
		speedText.text = string.Format( "{0:0}", baseData.processSpeed );
	}

	public void StartShowUI(UIDataTypes.Buildings.CR_Data data)
	{
		ShowBaseUI( data );
		//TODO enseñar ui especifica
		//eventos de actualizaciond e la UI
		UnityAction action = delegate { UpdateUI( data ); };
		data.updatedValuesEvent.AddListener( action );

		//evento  al esconderse esta UI
		UnityAction action2 = delegate { data.updatedValuesEvent.RemoveListener( action ); };
		hideEvent.AddListener( action2 );
		hideEvent.AddListener( delegate { hideEvent.RemoveListener( action2 ); } );
	}
	private void UpdateUI (UIDataTypes.Buildings.CR_Data data)
	{
		ShowBaseUI( data );
	}

	public void Hide ( )
	{
		hideEvent.Invoke();
	}
}
