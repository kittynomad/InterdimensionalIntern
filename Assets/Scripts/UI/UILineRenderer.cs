using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UILineRenderer : Graphic
{
    [SerializeField] private UIGridRenderer _grid;
    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private List<Vector2> _points;
    [SerializeField] private float _thickness = 1;

    private float width;
    private float height;
    private float unitWidth;
    private float unitHeight;

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
    protected override void OnPopulateMesh(VertexHelper vertexHelper)
    {
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
    private float GetAngle(Vector2 pointA, Vector2 pointB)
    {
        return (float)(Mathf.Atan2(pointB.y - pointA.y, pointB.x - pointA.x) * (180 / Mathf.PI));
    }
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
