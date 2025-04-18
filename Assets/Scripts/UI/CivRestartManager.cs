using System.Collections;
using UnityEngine;

public class CivRestartManager : MonoBehaviour
{
    [SerializeField] private CivilizationStatsManager _statsManager;
    private void Start()
    {
        StartCoroutine(ShowCivRestartScreen());
    }
    public void Button_NewGame()
    {
        foreach (Transform transform in _statsManager.GetComponent<StageManager>().BuildingsParents)
        {
            transform.gameObject.SetActive(true);
        }
        _statsManager.ResumeGame();
        transform.GetChild(0).gameObject.SetActive(false);
        FindObjectOfType<StageManager>().UpdateStageSprites();
    }
    public IEnumerator ShowCivRestartScreen()
    {
        yield return new WaitForSeconds(0.1f);
        _statsManager.PauseGame();
        ParallaxBackground.ResumeAllParallax();
        foreach (Transform transform in _statsManager.GetComponent<StageManager>().BuildingsParents)
        {
            transform.gameObject.SetActive(false);
        }
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
