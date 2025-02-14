using System.Collections;
using System.Collections.Generic;
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
            Debug.Log("Population: " + statsManager.Population + "\nMax Population: " + stages[curStage].MaxPopulation + "\nCurrent Stage: " + curStage);
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
        Debug.Log("CurrentStage:" + stages[curStage].Phase);
    }

    void Update()
    {
        
    }
}
