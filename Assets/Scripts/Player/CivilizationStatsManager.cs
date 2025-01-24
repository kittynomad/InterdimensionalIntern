using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilizationStatsManager : MonoBehaviour
{
    [Header("Civilization Stats")]
    [SerializeField] private int _population;
    [SerializeField] private float _resources;
    [SerializeField] private float _popGrowthPerTick;

    [Header("Game Settings")]
    [SerializeField] private float _tickTime;
    
    

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
