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

    public int CurStage { get => curStage; set => curStage = value; }
    public CivStage[] Stages { get => stages; set => stages = value; }
    public Transform[] BuildingsParents { get => _buildingsParents; set => _buildingsParents = value; }

    void Start()
    {
        statsManager = FindAnyObjectByType<CivilizationStatsManager>();
        choiceCount = 0;
        CurStage = 0;
        Debug.Log("Stages Length:" + (Stages.Length - 1));
    }

    public void OnChoice()
    {
        choiceCount++;
        Debug.Log("Choice #:" + choiceCount);
        if (choiceCount >= _choicesUntilCivilizationShift)
        {
            Debug.Log("Population: " + statsManager.Population + "\nMax " + Stages[CurStage].MaxStats[0].Stat + ": " + Stages[CurStage].MaxStats[0].Value + "\nCurrent Stage: " + CurStage);
            StageCheck();
            choiceCount = 0;
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
        }
        else if (CurStage < Stages.Length - 1 && !Stages[CurStage].CivilizationBelowMaximumStats(statsManager))//statsManager.Population >= stages[curStage].MaxPopulation)
        {
            Debug.Log("Not below maximum: " + !Stages[CurStage].CivilizationBelowMaximumStats(statsManager));
            if (curStage >= Stages.Length - 1) //if on last stage
                ResetCivilization();
            else
                CurStage++;
            _backgroundAnimator.SetInteger("curStage", CurStage);
        }
        UpdateLiveGraphY();
        UpdateStageSprites();

        Debug.Log("CurrentStage:" + Stages[CurStage].Phase);
    }
    private void ResetCivilization()
    {
        Debug.Log("Civ reset");
        curStage = 0;
        _backgroundAnimator.SetInteger("curStage", CurStage);
        StartCoroutine(statsManager.CivRestartManager.ShowCivRestartScreen());
    }
    private void UpdateStageSprites()
    {
        foreach (ParallaxBackground parallaxBackground in GameObject.FindObjectsOfType<ParallaxBackground>())
        {
            switch (parallaxBackground.SpriteType)
            {
                case ParallaxBackground.ParallaxType.Foreground:
                    parallaxBackground.GetComponent<SpriteRenderer>().sprite = Stages[curStage].Foreground; break;
                case ParallaxBackground.ParallaxType.ForegroundDetails:
                    parallaxBackground.GetComponent<SpriteRenderer>().sprite = Stages[curStage].ForegroundDetails; break;
                case ParallaxBackground.ParallaxType.Background:
                    parallaxBackground.GetComponent<SpriteRenderer>().sprite = Stages[curStage].Background; break;
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
                int randomSprite = Random.Range(0, stages[curStage].Buildings.Count);
                foreach (SpriteRenderer spriteRenderer in _buildingsParents[index].GetComponentsInChildren<SpriteRenderer>())
                {
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
