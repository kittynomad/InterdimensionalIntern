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

    [Header("StageSprites")]
    [SerializeField] private List<Sprite> _buildings;
    [SerializeField] private Sprite _foreground;
    [SerializeField] private Sprite _foregroundDetails;
    [SerializeField] private Sprite _background;
    [SerializeField] private Sprite _skyDetails;
    [SerializeField] private Sprite _sky;

    public Enums.CivilizationPhase Phase { get => _phase; set { _phase = value; } }

    public StatWithAssociatedValue[] MinStats { get => _minStats; set => _minStats = value; }
    public StatWithAssociatedValue[] MaxStats { get => _maxStats; set => _maxStats = value; }
    public ChoiceSet[] ChoiceSets { get => _choiceSets; set => _choiceSets = value; }
    public List<Sprite> Buildings { get => _buildings; set => _buildings = value; }
    public Sprite Foreground { get => _foreground; set => _foreground = value; }
    public Sprite Background { get => _background; set => _background = value; }
    public Sprite SkyDetails { get => _skyDetails; set => _skyDetails = value; }
    public Sprite Sky { get => _sky; set => _sky = value; }
    public Sprite ForegroundDetails { get => _foregroundDetails; set => _foregroundDetails = value; }

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
                if (!requireAllStats) Debug.Log( true);
            }
            else
            {
                if (requireAllStats) Debug.Log(false);
            }
        }

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
