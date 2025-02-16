/*
 * FileName:            UIGridRenderer.cs
 * Author:              Marlow Greenan
 * CreationDate:        2/7/2025
 */
using UnityEngine;
using UnityEngine.UI;

public class UIGridRenderer : Graphic
{
    [SerializeField] private Vector2Int _gridSize = new Vector2Int(10, 10);
    [SerializeField] private float _thickness = 1f;

    private float width;
    private float height;
    private float cellWidth;
    private float cellHeight;

    public Vector2Int GridSize { get => _gridSize; set => _gridSize = value; }

    /// <summary>
    ///  Creates a graph
    /// </summary>
    /// <param name="vertexHelper"></param>
    protected override void OnPopulateMesh(VertexHelper vertexHelper)
    {
        vertexHelper.Clear();
        width = rectTransform.rect.width;
        height = rectTransform.rect.height;
        cellWidth = width / (float)_gridSize.x;
        cellHeight = height / (float)_gridSize.y;

        int count = 0;
        for (int y = 0; y < _gridSize.y; y++)
        {
            for (int x = 0; x < _gridSize.x; x++)
            {
                DrawCell(x, y, count, vertexHelper);
                count++;
            }
        }
    }
    /// <summary>
    /// Creates a cell of a graph
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="index"></param>
    /// <param name="vertexHelper"></param>
    private void DrawCell(int x, int y, int index, VertexHelper vertexHelper)
    {
        float xPosition = cellWidth * x;
        float yPosition = cellHeight * y;

        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        vertex.position = new Vector3(xPosition, yPosition);
        vertexHelper.AddVert(vertex);

        vertex.position = new Vector3(xPosition, yPosition + cellHeight);
        vertexHelper.AddVert(vertex);

        vertex.position = new Vector3(xPosition + cellWidth, yPosition + cellHeight);
        vertexHelper.AddVert(vertex);

        vertex.position = new Vector3(xPosition + cellWidth, yPosition);
        vertexHelper.AddVert(vertex);

        float widthSquare = _thickness * _thickness;
        float distanceSquare = widthSquare / 2f;
        float distance = Mathf.Sqrt(distanceSquare);

        vertex.position = new Vector3(xPosition + distance, yPosition + distance);
        vertexHelper.AddVert(vertex);

        vertex.position = new Vector3(xPosition + distance, yPosition + (cellHeight - distance));
        vertexHelper.AddVert(vertex);

        vertex.position = new Vector3(xPosition + (cellWidth - distance), yPosition + (cellHeight - distance));
        vertexHelper.AddVert(vertex);

        vertex.position = new Vector3(xPosition + (cellWidth - distance), yPosition + distance);
        vertexHelper.AddVert(vertex);

        int offset = index * 8;
        //Left Edge
        vertexHelper.AddTriangle(offset + 0, offset + 1, offset + 5);
        vertexHelper.AddTriangle(offset + 5, offset + 4, offset + 0);
        //Top Edge
        vertexHelper.AddTriangle(offset + 1, offset + 2, offset + 6);
        vertexHelper.AddTriangle(offset + 6, offset + 5, offset + 1);
        //Right Edge
        vertexHelper.AddTriangle(offset + 2, offset + 3, offset + 7);
        vertexHelper.AddTriangle(offset + 7, offset + 6, offset + 2);
        //Bottom Edge
        vertexHelper.AddTriangle(offset + 3, offset + 0, offset + 4);
        vertexHelper.AddTriangle(offset + 4, offset + 7, offset + 3);
    }
}
