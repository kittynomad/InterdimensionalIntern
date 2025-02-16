/*
 * FileName:            GraphAnimator.cs
 * Author:              Marlow Greenan
 * CreationDate:        2/7/2025
 */
using System.Collections.Generic;
using UnityEngine;

public class GraphAnimator : MonoBehaviour
{
    [SerializeField] private UILineRenderer[] _lines;
    [SerializeField] private float _time = 0.5f;

    /// <summary>
    /// When gameobject gets enabled
    /// </summary>
    private void OnEnable()
    {
        AnimateLines();
    }
    /// <summary>
    /// Animates all lines getting drawn
    /// </summary>
    private void AnimateLines()
    {
        foreach(UILineRenderer line in _lines)
        {
            AnimateLine(line);
        }
    }
    /// <summary>
    /// Gets points from lineRender to animate a line
    /// </summary>
    /// <param name="lineRenderer"></param>
    private void AnimateLine(UILineRenderer lineRenderer)
    {
        List<Vector2> points = new List<Vector2>(lineRenderer.Points);
        Animate(lineRenderer, points);
    }
    /// <summary>
    /// Animates lines from points
    /// </summary>
    /// <param name="lineRenderer"></param>
    /// <param name="points"></param>
    private void Animate(UILineRenderer lineRenderer, List<Vector2> points)
    {
        lineRenderer.Points = new List<Vector2>();
        for (int index = 0; index < points.Count; index++)
        {
            AnimatePoint(lineRenderer, index, new Vector2(0, 4), points[index]);
        }
    }
    /// <summary>
    /// Animates a line from pointA to pointB
    /// </summary>
    /// <param name="lineRenderer"></param>
    /// <param name="index"></param>
    /// <param name="pointA"></param>
    /// <param name="pointB"></param>
    public void AnimatePoint(UILineRenderer lineRenderer, int index, Vector2 pointA, Vector2 pointB)
    {
        LeanTween.delayedCall(_time * index, () =>
        {
            if (index > 0)
            {
                pointA = lineRenderer.Points[index - 1];
                lineRenderer.Points.Add(pointA);
            }
            else
                lineRenderer.Points.Add(pointA);
            LeanTween.value(gameObject, (value) =>
            {
                lineRenderer.Points[index] = value;
                lineRenderer.SetVerticesDirty();
            }, pointA, pointB, _time);
        });
    }
    /// <summary>
    /// Animates a line in an live graph from pointA to pointB
    /// </summary>
    /// <param name="lineRenderer"></param>
    /// <param name="index"></param>
    /// <param name="pointA"></param>
    /// <param name="pointB"></param>
    public void AnimatePointLive(UILineRenderer lineRenderer, int index, Vector2 pointA, Vector2 pointB)
    {
        LeanTween.delayedCall(_time * index, () =>
        {
            LeanTween.value(gameObject, (value) =>
            {
                lineRenderer.Points[index] = value;
                lineRenderer.SetVerticesDirty();
            }, pointA, pointB, _time);
        });
    }
}
