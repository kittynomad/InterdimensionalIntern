/*
 * FileName:            UILineRenderer.cs
 * Author:              Marlow Greenan
 * CreationDate:        2/7/2025
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILineRenderer : Graphic
{
    [SerializeField] private UIGridRenderer _grid;
    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private List<Vector2> _points;
    [SerializeField] private float _thickness = 1;
    private VertexHelper vertexHelper;

    private float width;
    private float height;
    private float unitWidth;
    private float unitHeight;

    public List<Vector2> Points { get => _points; set => _points = value; }
    public VertexHelper VertexHelper { get => vertexHelper; set => vertexHelper = value; }

    /*
    private void Update()
    {
        if (_grid != null)
        {
            if (_gridSize != _grid.GridSize)
            {
                _gridSize = _grid.GridSize;
                SetVerticesDirty();
            }
        }
    }
    */
    /// <summary>
    /// Creates lines between all the points
    /// </summary>
    /// <param name="vertexHelper"></param>
    protected override void OnPopulateMesh(VertexHelper vertexHelper)
    {
        this.vertexHelper = vertexHelper;
        vertexHelper.Clear();
        width = rectTransform.rect.width;
        height = rectTransform.rect.height;

        unitWidth = width / (float)_gridSize.x;
        unitHeight = height / (float)_gridSize.y;

        if (_points.Count < 2)
            return;
        float angle = 0;
        for (int index = 0; index < _points.Count; index++)
        {
            Vector2 point = _points[index];
            if (index < _points.Count - 1)
                angle = GetAngle(_points[index], _points[index + 1]) + 45f;
            DrawVerticiesForPoint(point, vertexHelper, angle);
        }
        for (int i = 0; i < _points.Count - 1; i++)
        {
            int index = i * 2;
            vertexHelper.AddTriangle(index + 0, index + 1, index + 3);
            vertexHelper.AddTriangle(index + 3, index + 2, index + 0);
        }
    }
    /// <summary>
    /// Gets the angle between pointA and pointB to render the line correctly
    /// </summary>
    /// <param name="pointA"></param>
    /// <param name="pointB"></param>
    /// <returns></returns>
    private float GetAngle(Vector2 pointA, Vector2 pointB)
    {
        return (float)(Mathf.Atan2(pointB.y - pointA.y, pointB.x - pointA.x) * (180 / Mathf.PI));
    }
    /// <summary>
    /// Draws lines between points
    /// </summary>
    /// <param name="point"></param>
    /// <param name="vertexHelper"></param>
    /// <param name="angle"></param>
    private void DrawVerticiesForPoint(Vector2 point, VertexHelper vertexHelper, float angle)
    {
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        vertex.position = Quaternion.Euler(0, 0, angle) * new Vector3(-_thickness / 2, 0);
        vertex.position += new Vector3(unitWidth * point.x, unitHeight * point.y);
        vertexHelper.AddVert(vertex);

        vertex.position = Quaternion.Euler(0, 0, angle) * new Vector3(_thickness / 2, 0);
        vertex.position += new Vector3(unitWidth * point.x, unitHeight * point.y);
        vertexHelper.AddVert(vertex);
    }
}
