using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CivStage : ScriptableObject
{
    [SerializeField] private Enums.CivilizationPhase _phase;
    [SerializeField] private int minPop;
    [SerializeField] private int maxPop;

    public Enums.CivilizationPhase Phase { get => _phase; set { _phase = value; } }
    public int MinPopulation { get => minPop; set { minPop = value; } }
    public int MaxPopulation { get => maxPop; set { maxPop = value; } }
}
