using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class EdificioUIController : MonoBehaviour
{
	private Edificio currentShowing;

	public GameObject canvas;
	public Slider progresSlider;
	public Slider durabilitySlider;
	public TextMeshProUGUI upgradesText;
	public TextMeshProUGUI speedText;
	public TextMeshProUGUI employeeText;
	public TextMeshProUGUI inmigrantsText;

	public void StartShowing (ref Edificio toShow)
	{
		currentShowing = toShow;
	}

	public void Hide ( )
	{
		canvas.SetActive( false );
	}

	public void ShowUI ( )
	{
		progresSlider.value = currentShowing.GetProgress();
		durabilitySlider.value = currentShowing.GetCurrentDurability();

		upgradesText.text = string.Format( "{0:00}/{1:00}", currentShowing.GetCurrentUpgrade(), currentShowing.GetMaxUpgrades() );
		employeeText.text = string.Format( "{0:00}/{1:00}", currentShowing.GetCurrentEmployee(), currentShowing.GetMaxEmployee() );
		inmigrantsText.text = string.Format( "{0:00}/{1:00}", currentShowing.GetCurrentInmigrants(), currentShowing.GetMaxInmigrants() );
		speedText.text = string.Format( "{0:0}", currentShowing.GetProcesSpeed());

		canvas.SetActive( true );

	}
}
