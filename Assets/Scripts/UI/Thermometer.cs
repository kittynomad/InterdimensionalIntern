using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thermometer : MonoBehaviour
{
    [SerializeField] private CivilizationStatsManager _statsManager;

    [SerializeField] private Gradient _fillGradient;
    public void UpdateThermometer()
    {
        gameObject.GetComponent<Image>().fillAmount = _statsManager.Temperature / _statsManager.ThermometerMax;
        gameObject.GetComponent<Image>().color = _fillGradient.Evaluate(gameObject.GetComponent<Image>().fillAmount);
    }
}
