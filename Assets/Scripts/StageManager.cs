using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        //UpdateLiveGraphY(stages[curStage].)

        Debug.Log("CurrentStage:" + stages[curStage].Phase);
    }
    /// <summary>
    /// Updates live graph population boundaries to coordinate for the minimum and maximum population for the stage
    /// </summary>
    /// <param name="minY"></param>
    /// <param name="maxY"></param>
    private void UpdateLiveGraphY(float minY, float maxY)
    {
        for (int index = 0; index < statsManager.LiveGraph.LiveStats.Count; index++)
        {
            if (statsManager.LiveGraph.LiveStats[index].Type != LiveStat.LiveStatType.POPULATION) //continues to next iteration if liveStat is not population
                continue;
            statsManager.LiveGraph.LiveStats[index].Min = new Vector2(statsManager.LiveGraph.LiveStats[index].Min.x, minY);
            statsManager.LiveGraph.LiveStats[index].Max = new Vector2(statsManager.LiveGraph.LiveStats[index].Max.x, maxY);
            break;
        }
    }
    void Update()
    {
        
    }
}
