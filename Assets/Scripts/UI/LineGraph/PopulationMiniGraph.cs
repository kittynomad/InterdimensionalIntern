/*
 * FileName:            PopulationMiniGraph.cs
 * Author:              Marlow Greenan
 * CreationDate:        2/8/2025
 */
using System.Collections;
using UnityEngine;

public class PopulationMiniGraph : MonoBehaviour
{
    [SerializeField] private CivilizationStatsManager statsManager;
    [SerializeField] private UIGridRenderer gridRenderer;
    [SerializeField] private UILineRenderer lineRenderer;
    [SerializeField] private int _minimumPopulation = 0;
    [SerializeField] private int _maximumPopulation = 100;
    [SerializeField] private int _maxPoints = 20;
    private void Awake()
    {
        if (statsManager == null)
            statsManager = GameObject.FindObjectOfType<CivilizationStatsManager>();
        StartCoroutine(TickAdvance());
    }
    IEnumerator TickAdvance()
    {
        while (true)
        {
            yield return new WaitForSeconds(statsManager.TickTime);
            if (lineRenderer.Points.Count > _maxPoints)
                lineRenderer.Points.RemoveAt(0);
            lineRenderer.Points.Add(new Vector2((lineRenderer.Points.Count * gridRenderer.gameObject.GetComponent<RectTransform>().sizeDelta.x) / _maxPoints,
                (statsManager.Population / _maximumPopulation) * gridRenderer.gameObject.GetComponent<RectTransform>().sizeDelta.y));
        }
    }
}
