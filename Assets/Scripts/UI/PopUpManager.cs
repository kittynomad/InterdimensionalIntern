using System.Collections;
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

    private void Start()
    {
        maxTicksAfterChoice = _statsManager.TicksBetweenChoices - _minTicksBeforeNextChoice;
    }
    public void NewPopUp()
    {
        popUpTimer = Random.Range(_minTicksAfterChoice, maxTicksAfterChoice);
        popUpCount = 0;
    }
    public void UpdatePopUp()
    {
        popUpCount++;
        if (popUpCount > popUpTimer)
        {
            SpawnPopUp();
            popUpCount = 0;
        }
    }
    public void SpawnPopUp()
    {
        int popUpIndex = Random.Range(0, _popUpPrefabs.Count);
        float borderX = (_popUpCanvas.transform.localScale.x / 2) - (_popUpPrefabs[popUpIndex].transform.localScale.x / 2);
        float borderY = (_popUpCanvas.transform.localScale.y / 2) - (_popUpPrefabs[popUpIndex].transform.localScale.y / 2);
        Vector2 position = new Vector2(Random.Range(-borderX, borderX), Random.Range(-borderY, borderY));
        Instantiate(_popUpPrefabs[popUpIndex], new Vector3(position.x, position.y, 0), Quaternion.identity, _popUpCanvas.transform);
    }
    public void Button_Close(GameObject popUp)
    {
        Destroy(popUp);
    }
}
