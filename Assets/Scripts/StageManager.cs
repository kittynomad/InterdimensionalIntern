using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private CivStage[] stages;
    [SerializeField] private int _choicesUntilCivilizationShift = 3;
    private int curStage;
    private int choiceCount;

    private CivilizationStatsManager statsManager;

    void Start()
    {
        statsManager = FindAnyObjectByType<CivilizationStatsManager>();
        choiceCount = 0;
        curStage = 0;
        Debug.Log("Stages Length:" + (stages.Length - 1));
    }

    public void OnChoice()
    {
        choiceCount++;
        Debug.Log("Choice #:" + choiceCount);
        if (choiceCount >= _choicesUntilCivilizationShift)
        {
            Debug.Log("Population: " + statsManager.Population + "\nMax " + stages[curStage].MaxStats[0].Stat + ": " + stages[curStage].MaxStats[0].Value + "\nCurrent Stage: " + curStage);
            StageCheck();
            choiceCount = 0;
        }
    }

    private void StageCheck()
    {
        if (curStage > 0 && !stages[curStage].CivilizationAboveMinimumStats(statsManager))//statsManager.Population <= stages[curStage].MinPopulation)
        {
            Debug.Log("Not above minimum:" + !stages[curStage].CivilizationAboveMinimumStats(statsManager));
            curStage--;
        }
        else if (curStage < stages.Length - 1 && !stages[curStage].CivilizationBelowMaximumStats(statsManager))//statsManager.Population >= stages[curStage].MaxPopulation)
        {
            Debug.Log("Not below maximum: " + !stages[curStage].CivilizationBelowMaximumStats(statsManager));
            curStage++;
        }

        UpdateLiveGraphY();

        Debug.Log("CurrentStage:" + stages[curStage].Phase);
    }

    private void UpdateLiveGraphY()
    {
        float tempMinY = 0;
        float tempMaxY = 100;
        for (int index = 0; index < stages[curStage].MinStats.Count(); index++)
        {
            if (stages[curStage].MinStats[index].Stat != Enums.ModifyableStats.population)
                continue;
            tempMinY = stages[curStage].MinStats[index].Value;
        }
        for (int index = 0; index < stages[curStage].MaxStats.Count(); index++)
        {
            if (stages[curStage].MaxStats[index].Stat != Enums.ModifyableStats.population)
                continue;
            tempMaxY = stages[curStage].MaxStats[index].Value;
        }
        for (int index = 0; index < statsManager.LiveGraph.LiveStats.Count; index++)
        {
            if (statsManager.LiveGraph.LiveStats[index].Type != LiveStat.LiveStatType.POPULATION) //continues to next iteration if liveStat is not population
                continue;
            statsManager.LiveGraph.LiveStats[index].Min = new Vector2(statsManager.LiveGraph.LiveStats[index].Min.x, tempMinY);
            statsManager.LiveGraph.LiveStats[index].Max = new Vector2(statsManager.LiveGraph.LiveStats[index].Max.x, tempMaxY);
            break;
        }
    }
    void Update()
    {
        
    }
}
