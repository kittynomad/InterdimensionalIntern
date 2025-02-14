/*
 * FileName:            PopulationMiniGraph.cs
 * Author:              Marlow Greenan
 * CreationDate:        2/8/2025
 * 
 * Creates a line graph that updates every tick to show the live population.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationMiniGraph : MonoBehaviour
{
    [SerializeField] private CivilizationStatsManager statsManager;
    [SerializeField] private UIGridRenderer gridRenderer;
    [SerializeField] private UILineRenderer lineRenderer;
    [SerializeField] private int _maxX = 30;
    [SerializeField] private int _maxY = 1000;

    private void Awake()
    {
        if (statsManager == null)
            statsManager = GameObject.FindObjectOfType<CivilizationStatsManager>();
        StartCoroutine(TickAdvance());
    }
    /// <summary>
    /// Updates a population line graph every tick
    /// </summary>
    /// <returns></returns>
    IEnumerator TickAdvance()
    {
        int tickCount = 0;
        while (true)
        {
            yield return new WaitForSeconds(statsManager.TickTime);
            if (lineRenderer.Points.Count >= _maxX) //if the line has not reached the far right of the graph yet
            {
                List<Vector2> tempPoints = new List<Vector2>();
                for (int index = 0; index < lineRenderer.Points.Count; index++)
                {
                    if (index < lineRenderer.Points.Count - 1)
                        tempPoints.Add(new Vector2(lineRenderer.Points[index].x, lineRenderer.Points[index + 1].y));
                }
                lineRenderer.Points = tempPoints;
                lineRenderer.VertexHelper.Clear(); //clears lines
                tickCount--;
            }
            lineRenderer.Points.Add(new Vector2(((lineRenderer.Points.Count * gridRenderer.gameObject.GetComponent<RectTransform>().sizeDelta.x) / _maxX), ((float)statsManager.Population / _maxY) * (float)gridRenderer.gameObject.GetComponent<RectTransform>().sizeDelta.y));
            if (lineRenderer.Points.Count > 1 && lineRenderer.Points.Count < _maxX + 2) //has more than 1 point and is less than maxX draw all lines
                gameObject.GetComponent<GraphAnimator>().AnimatePointLive(lineRenderer, tickCount, lineRenderer.Points[lineRenderer.Points.Count - 2] ,lineRenderer.Points[lineRenderer.Points.Count - 1]);
            tickCount++;
        }
    }
}
