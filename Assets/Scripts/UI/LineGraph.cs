/*
 * FileName:            LineGraph.cs
 * Author:              Marlow Greenan
 * CreationDate:        2/7/2025
 */
using CodeMonkey.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineGraph : MonoBehaviour
{
    [SerializeField] private RectTransform _graphBox;
    [SerializeField] private GameObject _pointPrefab;
    [SerializeField] private GameObject _linePrefab;
    private void Awake()
    {
        CreatePoint(new Vector2(200, 200));
        List<int> valueList = new List<int>() { 5, 10, 11, 2, 18, 42, 29 };
        ShowGraph(valueList);
    }
    /// <summary>
    /// Creates a point on the graph at anchoredPosition
    /// </summary>
    /// <param name="anchoredPosition"></param>
    /// <returns></returns>
    private GameObject CreatePoint(Vector2 anchoredPosition)
    {
        GameObject point = Instantiate(_pointPrefab);
        point.transform.parent = _graphBox.gameObject.transform;
        point.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
        point.GetComponent<RectTransform>().sizeDelta = new Vector2(11, 11);
        point.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        point.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
        return point;
    }
    /// <summary>
    /// Creates a line between pointA and pointB
    /// </summary>
    /// <param name="pointA"></param>
    /// <param name="pointB"></param>
    private void CreateLine(Vector2 pointA, Vector2 pointB)
    {
        GameObject line = Instantiate(_linePrefab);
        line.transform.parent = _graphBox.gameObject.transform;
        line.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        Vector2 direction = (pointA - pointB).normalized;
        float distance = Vector2.Distance(pointA, pointB);

        line.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        line.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
        line.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 3f);
        line.GetComponent<RectTransform>().anchoredPosition = pointA + direction * distance * 0.5f;
        line.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(direction));
    }
    /// <summary>
    /// Displys the points in valueList as a line graph
    /// </summary>
    /// <param name="valueList"></param>
    private void ShowGraph(List<int> valueList)
    {
        float graphHeight = _graphBox.sizeDelta.y;
        float yMaximum = 100f;
        float xSize = 50f;
        GameObject lastPoint = null;
        for (int index = 0; index < valueList.Count; index++)
        {
            GameObject point = CreatePoint(new Vector2(xSize + index * xSize, (valueList[index] / yMaximum) * graphHeight));
            if (lastPoint != null)
                CreateLine(lastPoint.GetComponent<RectTransform>().anchoredPosition, 
                    point.GetComponent<RectTransform>().anchoredPosition);
            lastPoint = point;
        }
    }    
}
