/*
 * FileName:            LiveGraph.cs
 * Author:              Marlow Greenan
 * CreationDate:        2/8/2025
 * Updated:             2/14/2025
 * 
 * Creates a line graph that updates every tick to show the live population.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LiveStat
{
    public enum LiveStatType
    {
        POPULATION,
        RESOURCES,
        HAPPINESS,
    }
    [SerializeField] private UILineRenderer _line;
    [SerializeField] private LiveStatType _type;
    [SerializeField] private Vector2 _min = new Vector2(0, 0);
    [SerializeField] private Vector2 _max = new Vector2(20, 1000);
    private int tickCount = 0;

    public UILineRenderer Line { get => _line; set => _line = value; }
    public LiveStatType Type { get => _type; set => _type = value; }
    public Vector2 Min { get => _min; set => _min = value; }
    public Vector2 Max { get => _max; set => _max = value; }
    public int TickCount { get => tickCount; set => tickCount = value; }
}
public class LiveGraph : MonoBehaviour
{
    [SerializeField] private CivilizationStatsManager statsManager;
    [SerializeField] private UIGridRenderer gridRenderer;
    [SerializeField] private List<LiveStat> _liveStats = new List<LiveStat>();

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
        while (true)
        {
            yield return new WaitForSeconds(statsManager.TickTime);
            foreach (LiveStat liveStat in _liveStats)
            {
                float stat = 0;
                switch(liveStat.Type)
                {
                    default:
                    case LiveStat.LiveStatType.POPULATION: stat = (float)statsManager.Population; break;
                    case LiveStat.LiveStatType.RESOURCES: stat = (float)statsManager.Resources; break;
                    case LiveStat.LiveStatType.HAPPINESS: stat = (float)statsManager.Happiness; break;
                }
                if (liveStat.Line.Points.Count >= liveStat.Max.x) //if the line has not reached the far right of the graph yet
                {
                    List<Vector2> tempPoints = new List<Vector2>();
                    for (int index = 0; index < liveStat.Line.Points.Count; index++)
                    {
                        if (index < liveStat.Line.Points.Count - 1)
                            tempPoints.Add(new Vector2(liveStat.Line.Points[index].x, liveStat.Line.Points[index + 1].y));
                    }
                    liveStat.Line.Points = tempPoints;
                    liveStat.Line.VertexHelper.Clear(); //clears lines
                    liveStat.TickCount--;
                }
                liveStat.Line.Points.Add(new Vector2(((liveStat.Line.Points.Count * gridRenderer.gameObject.GetComponent<RectTransform>().sizeDelta.x) / liveStat.Max.x), (stat / liveStat.Max.y) * (float)gridRenderer.gameObject.GetComponent<RectTransform>().sizeDelta.y));
                if (liveStat.Line.Points[liveStat.Line.Points.Count - 1].y > (float)gridRenderer.gameObject.GetComponent<RectTransform>().sizeDelta.y) //if y is greater than the size of the grid,change it to be the top of the grid
                    liveStat.Line.Points[liveStat.Line.Points.Count - 1] = new Vector2(liveStat.Line.Points[liveStat.Line.Points.Count - 1].x, (float)gridRenderer.gameObject.GetComponent<RectTransform>().sizeDelta.y);
                if (liveStat.Line.Points.Count > 1 && liveStat.Line.Points.Count < liveStat.Max.x + 2) //has more than 1 point and is less than maxX draw all lines
                    gameObject.GetComponent<GraphAnimator>().AnimatePointLive(liveStat.Line, liveStat.TickCount, liveStat.Line.Points[liveStat.Line.Points.Count - 2], liveStat.Line.Points[liveStat.Line.Points.Count - 1]);
                liveStat.TickCount++;
            }
        }
    }
}