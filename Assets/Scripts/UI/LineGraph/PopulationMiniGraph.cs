/*
 * FileName:            PopulationMiniGraph.cs
 * Author:              Marlow Greenan
 * CreationDate:        2/8/2025
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationMiniGraph : MonoBehaviour
{
    [SerializeField] private CivilizationStatsManager statsManager;
    [SerializeField] private UIGridRenderer gridRenderer;
    [SerializeField] private UILineRenderer lineRenderer;
    [SerializeField] private int _minimumPopulation = 0;
    [SerializeField] private int _maximumPopulation = 100;
    [SerializeField] private int _maxPoints;
    private void Awake()
    {
        if (statsManager == null)
            statsManager = GameObject.FindObjectOfType<CivilizationStatsManager>();
        StartCoroutine(TickAdvance());
    }
    IEnumerator TickAdvance()
    {
        int tickCount = 0;
        while (true)
        {
            yield return new WaitForSeconds(statsManager.TickTime);
            //
            if (lineRenderer.Points.Count >= _maxPoints)
            {
                List<Vector2> tempPoints = new List<Vector2>();
                for (int index = 0; index < lineRenderer.Points.Count; index++)
                {
                    if (index < lineRenderer.Points.Count - 1)
                        tempPoints.Add(new Vector2(lineRenderer.Points[index].x, lineRenderer.Points[index + 1].y));
                }
                lineRenderer.Points = tempPoints;
                lineRenderer.VertexHelper.Clear();
                tickCount--;
            }
            //
            lineRenderer.Points.Add(new Vector2(((lineRenderer.Points.Count * gridRenderer.gameObject.GetComponent<RectTransform>().sizeDelta.x) / _maxPoints), ((float)statsManager.Population / _maximumPopulation) * (float)gridRenderer.gameObject.GetComponent<RectTransform>().sizeDelta.y));
            if (lineRenderer.Points.Count > 1 && lineRenderer.Points.Count < _maxPoints + 2)
                gameObject.GetComponent<GraphAnimator>().AnimatePointLive(lineRenderer, tickCount, lineRenderer.Points[lineRenderer.Points.Count - 2] ,lineRenderer.Points[lineRenderer.Points.Count - 1]);
            tickCount++;
        }
    }
}
