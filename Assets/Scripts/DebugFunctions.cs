using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugFunctions : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _debugInfoDisplay;

    private CivilizationStatsManager stats;

    public void Start()
    {
        stats = FindObjectOfType<CivilizationStatsManager>();
    }

    public void Update()
    {
        _debugInfoDisplay.text = "pop: " + stats.Population +
            "\nResources: " + stats.Resources + 
            "\nHappiness: " + stats.Happiness + "%" +
            "\nTemperature: " + stats.Temperature;
    }
}
