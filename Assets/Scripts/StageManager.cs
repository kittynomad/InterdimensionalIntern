using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private CivStage[] stages;
    [SerializeField] private Transform _buildingsParent;
    //[SerializeField] private Transform _buildingsSprite1Parent;
    //[SerializeField] private Transform _buildingsSprite2Parent;
    [SerializeField] private int _choicesUntilCivilizationShift = 3;
    [SerializeField] private Animator _backgroundAnimator;
    private int curStage;
    private int choiceCount;

    private CivilizationStatsManager statsManager;

    public int CurStage { get => curStage; set => curStage = value; }
    public CivStage[] Stages { get => stages; set => stages = value; }

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
        statsManager.PopUpManager.PopUpCanvas.SetActive(false);
        statsManager.NextPersonCanvas.SetActive(true);
        StopAllCoroutines();
    }
    public void NextPersonResume()
    {
        statsManager.PopUpManager.PopUpCanvas.SetActive(true);
        statsManager.NextPersonCanvas.SetActive(false);
        statsManager.StartCoroutine(statsManager.tickAdvance());
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
            CurStage++;
            _backgroundAnimator.SetInteger("curStage", CurStage);
        }

        UpdateLiveGraphY();
        UpdateBuildingSprites();

        Debug.Log("CurrentStage:" + Stages[CurStage].Phase);
    }
    private void UpdateBuildingSprites()
    {
        if (stages[curStage].Buildings.Count > 0)
        {
            for (int index = 0; index < _buildingsParent.childCount; index++)
            {
                int randomSprite = Random.Range(0, stages[curStage].Buildings.Count);
                _buildingsParent.GetChild(index).GetComponent<SpriteRenderer>().sprite = stages[curStage].Buildings[randomSprite];
            }
        }
        //if (stages[curStage].BuildingSprite1 != null)
        //{
        //    for (int index = 0; index < _buildingsSprite1Parent.childCount; index++)
        //    {
        //        _buildingsSprite1Parent.GetChild(index).GetComponent<SpriteRenderer>().sprite = stages[curStage].BuildingSprite1;
        //    }
        //    for (int index = 0; index < _buildingsSprite2Parent.childCount; index++)
        //    {
        //        if (stages[curStage].BuildingSprite2 != null)
        //            _buildingsSprite2Parent.GetChild(index).GetComponent<SpriteRenderer>().sprite = stages[curStage].BuildingSprite2;
        //        else
        //            _buildingsSprite2Parent.GetChild(index).GetComponent<SpriteRenderer>().sprite = stages[curStage].BuildingSprite1;
        //    }
        //}
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
