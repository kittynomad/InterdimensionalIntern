using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CivStage : ScriptableObject
{
    [SerializeField] private Enums.CivilizationPhase _phase;
    [SerializeField] private StatWithAssociatedValue[] _minStats;
    [SerializeField] private StatWithAssociatedValue[] _maxStats;
    [SerializeField] private int minPop;
    [SerializeField] private int maxPop;

    public Enums.CivilizationPhase Phase { get => _phase; set { _phase = value; } }
    public int MinPopulation { get => minPop; set { minPop = value; } }
    public int MaxPopulation { get => maxPop; set { maxPop = value; } }

    public bool CivilizationAboveMinimumStats(CivilizationStatsManager civ, bool requireAllStats = false)
    {
        foreach(StatWithAssociatedValue s in _minStats)
        {
            if(civ.GetStatFromModifyableStatsEnum(s.Stat) >= s.Value)
            {
                if (!requireAllStats) return true;
            }
            else
            {
                if (requireAllStats) return false;
            }
        }

        return requireAllStats;
    }

    public bool CivilizationBelowMaximumStats(CivilizationStatsManager civ, bool requireAllStats = true)
    {
        foreach (StatWithAssociatedValue s in _maxStats)
        {
            if (civ.GetStatFromModifyableStatsEnum(s.Stat) <= s.Value)
            {
                if (!requireAllStats) return true;
            }
            else
            {
                if (requireAllStats) return false;
            }
        }

        return requireAllStats;
    }
}
