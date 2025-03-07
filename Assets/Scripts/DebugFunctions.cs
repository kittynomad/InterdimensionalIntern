using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugFunctions : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _debugInfoDisplay;

    private CivilizationStatsManager stats;
    private StageManager sm;

    public void Start()
    {
        stats = FindObjectOfType<CivilizationStatsManager>();
        sm = FindObjectOfType<StageManager>();
    }

    public void Update()
    {
        _debugInfoDisplay.text = "pop: " + stats.Population +
            "\nResources: " + stats.Resources + 
            "\nHappiness: " + stats.Happiness + "%" +
            "\nTemperature: " + stats.Temperature + 
            "\nStage: " + sm.Stages[sm.CurStage];
    }

    public void IncreasePopulation(int popIncreaseAmount)
    {
        stats.Population += popIncreaseAmount;
    }

    public void IncreaseResources(float resourceIncreaseAmount)
    {
        stats.Resources += resourceIncreaseAmount;
    }

    public void IncreaseHappiness(float happinessIncreaseAmount)
    {
        stats.Happiness += happinessIncreaseAmount;
    }

    public void SetTickSpeed(float tickSpeed)
    {
        stats.TickTime = tickSpeed;
    }
}
