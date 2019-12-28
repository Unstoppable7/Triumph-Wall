using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using UIDataTypes.Buildings;

public class EdificioUIController : MonoBehaviour
{

	[SceneObjectsOnly]
	public GameObject canvas;
	[Title( "Durability" )]
	[SceneObjectsOnly]
	public Slider durabilitySlider;

	[Title("Upgrades")]
	[SceneObjectsOnly]
	public GameObject upgradesObject;
	[SceneObjectsOnly]
	public TextMeshProUGUI upgradesText;

	[Title("Process")]
	[SceneObjectsOnly]
	public Slider progresSlider;
	[SceneObjectsOnly]
	public GameObject speedObject;
	[SceneObjectsOnly]
	public TextMeshProUGUI speedText;

	[Title( "Emplyees" )]
	[SceneObjectsOnly]
	public GameObject emplyeeObject;
	[SceneObjectsOnly]
	public TextMeshProUGUI employeeText;

	[Title( "Inmigrants" )]
	[SceneObjectsOnly]
	public GameObject inmigrantsObject;
	[SceneObjectsOnly]
	public TextMeshProUGUI inmigrantsText;

	[Title( "Parcela" )]
	[SceneObjectsOnly]
	public Slider salubritySlider;
	[SceneObjectsOnly]
	public Slider controlSlider;



    private UnityEvent hideEvent = new UnityEvent();

	//Ui Basica de TODOS los edificios
	//tiene en cuenta las booleanas de UIB_Data para enseñar diferentes elementos
	public void ShowBaseUI (UIB_Data baseData)
	{
		if (baseData.showDurabilityBar)
		{
			durabilitySlider.value = baseData.currentDurability / baseData.maxDurability;
		}
		else
		{
			durabilitySlider.gameObject.SetActive( false );
		}

		if (baseData.showUpgradeNum)
		{
			upgradesText.text = string.Format( "{0:00}/{1:00}", baseData.currentUpgrade, baseData.maxOfUpgrades );

		}
		else
		{
			upgradesObject.SetActive( false );
		}

		if (baseData.showEmployeeNum)
		{
			employeeText.text = string.Format( "{0:00}/{1:00}", baseData.currentEmployeeNum, baseData.maxEmployeeNum );
		}
		else
		{
			emplyeeObject.SetActive( false );
		}

		if (baseData.showInmigrantNum)
		{

			inmigrantsText.text = string.Format( "{0:00}/{1:00}", baseData.currentInmigrantNum, baseData.maxInmigrantNum );
		}
		else
		{
			inmigrantsObject.SetActive( false );
		}

		if (baseData.showProgress)
		{
			progresSlider.value = baseData.currentProgress;
			speedText.text = string.Format( "{0:00}", baseData.processSpeed );
		}
		else
		{
			speedObject.SetActive( false );
			progresSlider.gameObject.SetActive( false );
		}
	}

	//SetUp de la UI de edificio concreto
	public void StartShowUI(SO_UICR_Data data)
	{
		ShowBaseUI( data );
		//enseñar Ui especifica
		salubritySlider.gameObject.SetActive( true );
		salubritySlider.value = data.salubrity;

		controlSlider.gameObject.SetActive( true );
		controlSlider.value = data.control;

		data.updatedValuesEvent.RemoveAllListeners();
		//eventos de actualizaciond e la UI
		UnityAction action = delegate { UpdateUI( data ); };
		data.updatedValuesEvent.AddListener( action );
		//evento  al esconderse esta UI
		UnityAction action2 = delegate 
		{
			data.updatedValuesEvent.RemoveListener( action );
			salubritySlider.gameObject.SetActive( false );
			controlSlider.gameObject.SetActive( false );
		};
		hideEvent.AddListener( action2 );
		hideEvent.AddListener( delegate { hideEvent.RemoveListener( action2 ); } );
	}

    public void StartShowUI(SO_UIODI_Data data)
    {
        ShowBaseUI(data);
		//enseñar Ui especifica

		//eventos de actualizaciond e la UI
		data.updatedValuesEvent.RemoveAllListeners();
		UnityAction action = delegate { UpdateUI(data); };
        data.updatedValuesEvent.AddListener(action);
        //evento  al esconderse esta UI
        UnityAction action2 = delegate
        {
            data.updatedValuesEvent.RemoveListener(action);
            
        };
        hideEvent.AddListener(action2);
        hideEvent.AddListener(delegate { hideEvent.RemoveListener(action2); });
    }

    public void StartShowUI(SO_UIDorm_Data data)
    {
        ShowBaseUI(data);
        //enseñar Ui especifica

        //eventos de actualizaciond e la UI
        UnityAction action = delegate { UpdateUI(data); };
        data.updatedValuesEvent.AddListener(action);
        //evento  al esconderse esta UI
        UnityAction action2 = delegate
        {
            data.updatedValuesEvent.RemoveListener(action);

        };
        hideEvent.AddListener(action2);
        hideEvent.AddListener(delegate { hideEvent.RemoveListener(action2); });
    }

    //updatea la UI cuando el edificio updatea los valores
    private void UpdateUI (SO_UICR_Data data)
	{
		ShowBaseUI( data );
		//enseñar Ui especifica
		salubritySlider.value = data.salubrity;
		controlSlider.value = data.control;
	}

    private void UpdateUI(SO_UIDorm_Data data)
    {
        ShowBaseUI(data);
        //enseñar Ui especifica
        durabilitySlider.value = data.currentDurability;
    }
    private void UpdateUI(SO_UIODI_Data data)
    {
        ShowBaseUI(data);
        //enseñar Ui especifica
        durabilitySlider.value = data.currentDurability;

    }

    //se llama desde el input controller
    public void Hide ( )
	{
		canvas.SetActive( false );
		hideEvent.Invoke();
	}
}
