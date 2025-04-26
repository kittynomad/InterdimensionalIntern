using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using UnityEditor.SceneManagement;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private CivStage[] stages;
    [SerializeField] private Transform[] _buildingsParents;
    [SerializeField] private int _choicesUntilCivilizationShift = 3;
    [SerializeField] private Animator _backgroundAnimator;
    private int curStage;
    private int choiceCount;

    private CivilizationStatsManager statsManager;

    public static ChoiceSet[] ChoicesThisTime;

    public int CurStage { get => curStage; set => curStage = value; }
    public CivStage[] Stages { get => stages; set => stages = value; }
    public Transform[] BuildingsParents { get => _buildingsParents; set => _buildingsParents = value; }

    void Start()
    {
        statsManager = FindAnyObjectByType<CivilizationStatsManager>();
        choiceCount = 0;
        CurStage = 0;
        Debug.Log("Stages Length:" + (Stages.Length - 1));
        ChoicesThisTime = new ChoiceSet[_choicesUntilCivilizationShift];
    }

    public void OnChoice(ChoiceSet c = null)
    {
        ChoicesThisTime[choiceCount] = c;
        choiceCount++;
        Debug.Log("Choice #:" + choiceCount);
        if (choiceCount >= _choicesUntilCivilizationShift)
        {
            Debug.Log("Population: " + statsManager.Population + "\nMax " + Stages[CurStage].MaxStats[0].Stat + ": " + Stages[CurStage].MaxStats[0].Value + "\nCurrent Stage: " + CurStage);
            StageCheck();
            choiceCount = 0;
            ChoicesThisTime = new ChoiceSet[_choicesUntilCivilizationShift];
            StartCoroutine(NextPersonPause(2));
        }
    }
    IEnumerator NextPersonPause(int delay)
    {
        yield return new WaitForSeconds(delay);
        statsManager.NextPersonCanvas.SetActive(true);
        statsManager.PauseGame();
    }
    public void NextPersonResume()
    {
        statsManager.NextPersonCanvas.SetActive(false);
        statsManager.ResumeGame();
    }
    private void StageCheck()
    {
        if (CurStage > 0 && !Stages[CurStage].CivilizationAboveMinimumStats(statsManager))//statsManager.Population <= stages[curStage].MinPopulation)
        {
            Debug.Log("Not above minimum:" + !Stages[CurStage].CivilizationAboveMinimumStats(statsManager));
            CurStage--;
            _backgroundAnimator.SetInteger("curStage", CurStage);
            UpdateStageSprites();
        }
        //else if (CurStage < Stages.Length - 1 && !Stages[CurStage].CivilizationBelowMaximumStats(statsManager))//statsManager.Population >= stages[curStage].MaxPopulation)
        else if (!Stages[CurStage].CivilizationBelowMaximumStats(statsManager))//statsManager.Population >= stages[curStage].MaxPopulation)
        {
            Debug.Log("Not below maximum: " + !Stages[CurStage].CivilizationBelowMaximumStats(statsManager));
            if (curStage >= stages.Length - 1) //if on last stage
                ResetCivilization();
            else
                CurStage++;
            _backgroundAnimator.SetInteger("curStage", CurStage);
            UpdateStageSprites();
        }
        UpdateLiveGraphY();

        Debug.Log("CurrentStage:" + Stages[CurStage].Phase);
    }
    private void ResetCivilization()
    {
        Debug.Log("Civ reset");
        curStage = 0;
        statsManager.Population = 2;
        statsManager.PopGrowthPercentPerTick = 100;
        statsManager.Resources = 10.4f;
        statsManager.Happiness = 20;
        statsManager.HappinessGrowthPercentPerTick = 99.8f;
        statsManager.Temperature = 70;
        statsManager.TempGrowthPercentPerTick = 100;
        _backgroundAnimator.SetInteger("curStage", CurStage);
        statsManager.ParticlePeopleHandler.Ps.Clear();
        StartCoroutine(statsManager.CivRestartManager.ShowCivRestartScreen());
    }
    public void UpdateStageSprites()
    {
        foreach (ParallaxBackground parallaxBackground in GameObject.FindObjectsOfType<ParallaxBackground>())
        {
            switch (parallaxBackground.SpriteType)
            {
                case ParallaxBackground.ParallaxType.Foreground:
                    parallaxBackground.GetComponent<SpriteRenderer>().sprite = Stages[curStage].Foreground; break;
                case ParallaxBackground.ParallaxType.ForegroundDetails:
                    parallaxBackground.GetComponent<SpriteRenderer>().sprite = Stages[curStage].ForegroundDetails; break;
                case ParallaxBackground.ParallaxType.Background1:
                    parallaxBackground.GetComponent<SpriteRenderer>().sprite = Stages[curStage].Background1; break;
                case ParallaxBackground.ParallaxType.Background2:
                    parallaxBackground.GetComponent<SpriteRenderer>().sprite = Stages[curStage].Background2; break;
                case ParallaxBackground.ParallaxType.Background3:
                    parallaxBackground.GetComponent<SpriteRenderer>().sprite = Stages[curStage].Background3; break;
                case ParallaxBackground.ParallaxType.Background4:
                    parallaxBackground.GetComponent<SpriteRenderer>().sprite = Stages[curStage].Background4; break;
                case ParallaxBackground.ParallaxType.Background5:
                    parallaxBackground.GetComponent<SpriteRenderer>().sprite = Stages[curStage].Background5; break;
                case ParallaxBackground.ParallaxType.SkyDetails:
                    parallaxBackground.GetComponentInChildren<SpriteRenderer>().sprite = Stages[curStage].SkyDetails; break;
                case ParallaxBackground.ParallaxType.Sky:
                    parallaxBackground.GetComponent<SpriteRenderer>().sprite = Stages[curStage].Sky; break;
            }
        }
        UpdateBuildingSprites();
    }
    private void UpdateBuildingSprites()
    {
        if (stages[curStage].Buildings.Count > 0)
        {
            for (int index = 0; index < _buildingsParents.Length; index++)
            {
                foreach (SpriteRenderer spriteRenderer in _buildingsParents[index].GetComponentsInChildren<SpriteRenderer>())
                {
                    int randomSprite = Random.Range(0, stages[curStage].Buildings.Count);
                    spriteRenderer.sprite = stages[curStage].Buildings[randomSprite];
                }
            }
        }
    }

    private void UpdateLiveGraphY()
    {
        for (int index = 0; index < statsManager.LiveGraph.LiveStats.Count; index++)
        {
            for (int index2 = 0; index2 < stages[curStage].MinStats.Count(); index2++)
            {
                if (statsManager.LiveGraph.LiveStats[index].Type == stages[curStage].MinStats[index2].Stat)
                    statsManager.LiveGraph.LiveStats[index].Min = new Vector2(statsManager.LiveGraph.LiveStats[index].Min.x, stages[curStage].MinStats[index2].Value);
            }
            for (int index2 = 0; index2 < stages[curStage].MaxStats.Count(); index2++)
            {
                if (statsManager.LiveGraph.LiveStats[index].Type == stages[curStage].MaxStats[index2].Stat)
                    statsManager.LiveGraph.LiveStats[index].Max = new Vector2(statsManager.LiveGraph.LiveStats[index].Max.x, stages[curStage].MaxStats[index2].Value);
            }
        }
    }
}
