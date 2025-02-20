using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CivStage : ScriptableObject
{
    [SerializeField] private Enums.CivilizationPhase _phase;
    [SerializeField] private StatWithAssociatedValue[] _minStats;
    [SerializeField] private StatWithAssociatedValue[] _maxStats;
    [SerializeField] private ChoiceSet[] _choiceSets;

    public Enums.CivilizationPhase Phase { get => _phase; set { _phase = value; } }

    public StatWithAssociatedValue[] MinStats { get => _minStats; set => _minStats = value; }
    public StatWithAssociatedValue[] MaxStats { get => _maxStats; set => _maxStats = value; }
    public ChoiceSet[] ChoiceSets { get => _choiceSets; set => _choiceSets = value; }

    public bool CivilizationAboveMinimumStats(CivilizationStatsManager civ, bool requireAllStats = false)
    {
        foreach(StatWithAssociatedValue s in MinStats)
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
        foreach (StatWithAssociatedValue s in MaxStats)
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
