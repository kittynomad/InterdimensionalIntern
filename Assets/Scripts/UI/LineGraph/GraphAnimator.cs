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
    [SerializeField] private float _time = 1f;

    private void OnEnable()
    {
        AnimateLines();
    }
    private void AnimateLines()
    {
        foreach(UILineRenderer line in _lines)
        {
            AnimateLine(line);
        }
    }
    public void AnimateLine(UILineRenderer lineRenderer)
    {
        List<Vector2> points = new List<Vector2>(lineRenderer.Points);
        Animate(lineRenderer, points);
    }
    private void Animate(UILineRenderer lineRenderer, List<Vector2> points)
    {
        lineRenderer.Points = new List<Vector2>();
        for (int index = 0; index < points.Count; index++)
        {
            AnimatePoint(lineRenderer, index, new Vector2(0, 4), points[index]);
        }
    }
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
}
