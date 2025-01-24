using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilizationStatsManager : MonoBehaviour
{
    [Header("Civilization Stats")]
    [SerializeField] private int _population = 2;
    [SerializeField] private float _resources = 10.4f;
    [SerializeField] private float _popGrowthPerTick = 1f;
    [SerializeField] private float _happinessGrowthPerTick = 1f;
    [SerializeField] [Range(0.0f, 100.0f)] private float _happiness = 100f;

    [Header("Game Settings")]
    [SerializeField] private float _tickTime = 1f;
    
    

    public int Population { get => _population; set => _population = value; }
    public float Resources { get => _resources; set => _resources = value; }

    public void Start()
    {
        StartCoroutine(tickAdvance());
    }

    public void OnTick()
    {
        
        _population = Random.Range(0, 2) + 
            (int)((float)_population * _popGrowthPerTick);

        _happiness *= _happinessGrowthPerTick;
    }

    public IEnumerator tickAdvance()
    {
        while(true)
        {
            yield return new WaitForSeconds(_tickTime);
            OnTick();
        }
        
    }
}
