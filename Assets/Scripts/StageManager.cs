using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private CivStage[] stages;
    private int curStage;
    private int choiceCount;

    private CivilizationStatsManager statsManager;

    void Start()
    {
        statsManager = FindAnyObjectByType<CivilizationStatsManager>();
        choiceCount = 0;
        Debug.Log("Stages Length:" + (stages.Length - 1));
    }

    public void OnChoice()
    {
        Debug.Log("Choice #:" + choiceCount);
        choiceCount++;
        if (choiceCount == 3)
        {
            StageCheck();
            choiceCount = 0;
        }
    }

    private void StageCheck()
    {
        if (curStage > 0 && statsManager.Population <= stages[curStage].MinPopulation)
        {
            curStage--;
        }
        else if (curStage < stages.Length - 1 && statsManager.Population >= stages[curStage].MaxPopulation)
        {
            curStage++;
        }
    }

    void Update()
    {
        
    }
}
