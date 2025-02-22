/*
 * PopUpManager.cs
 * Marlow Greenan
 * 2/22/2025
 * 
 * A random assortment of buttons.
 */
using System.Collections.Generic;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    [SerializeField] private CivilizationStatsManager _statsManager;

    [SerializeField] private GameObject _popUpCanvas;
    [SerializeField] private List<GameObject> _popUpPrefabs;
    [SerializeField] private int _minTicksAfterChoice = 5;
    [SerializeField] private int _minTicksBeforeNextChoice = 5;
    private int maxTicksAfterChoice;
    private int popUpCount = 0;
    private int popUpTimer = 10;
    private bool hasPlayedPopUp = false;

    public bool HasPlayedPopUp { get => hasPlayedPopUp; set => hasPlayedPopUp = value; }

    private void Start()
    {
        maxTicksAfterChoice = _statsManager.TicksBetweenChoices - _minTicksBeforeNextChoice;
    }
    /// <summary>
    /// Creates a new popUp countdown
    /// </summary>
    public void NewPopUp()
    {
        hasPlayedPopUp = false;
        popUpTimer = Random.Range(_minTicksAfterChoice, maxTicksAfterChoice); //Determines the time after choice that a pop-up appears
        popUpCount = 0;
    }
    /// <summary>
    /// Updates the popUp countdown
    /// </summary>
    public void UpdatePopUp()
    {
        if (!hasPlayedPopUp)
        {
            popUpCount++;
            if (popUpCount > popUpTimer)
            {
                SpawnPopUp();
                popUpCount = 0;
            }
        }
    }
    /// <summary>
    /// Spawns a new popUp and resents the popUp countdown
    /// </summary>
    public void SpawnPopUp()
    {
        int popUpIndex = Random.Range(0, _popUpPrefabs.Count);
        float borderX = (_popUpCanvas.gameObject.GetComponent<RectTransform>().rect.width / 2) - (_popUpPrefabs[popUpIndex].gameObject.GetComponent<RectTransform>().rect.width / 2);
        float borderY = (_popUpCanvas.gameObject.GetComponent<RectTransform>().rect.height / 2) - (_popUpPrefabs[popUpIndex].gameObject.GetComponent<RectTransform>().rect.height / 2);
        Vector2 position = new Vector2(Random.Range(-borderX, borderX), Random.Range(-borderY, borderY));
        Instantiate(_popUpPrefabs[popUpIndex], new Vector3(position.x, position.y, 0), Quaternion.identity, _popUpCanvas.transform);
        hasPlayedPopUp = true;
    }
}
