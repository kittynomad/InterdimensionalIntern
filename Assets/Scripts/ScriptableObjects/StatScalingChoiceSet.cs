using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StatScalingChoiceSet : ChoiceSet
{
    [SerializeField] private StatWithAssociatedValue[] _minimumValuestoScale;
    [SerializeField] private StatWithAssociatedValue[] _maximumValuestoScale;

    [SerializeField] private float _maximumValuesWeightBonus;
    public override float Weight 
    { get
        {
            CivilizationStatsManager cm = FindObjectOfType<CivilizationStatsManager>();

            float weightBonus = 0f;

            foreach (StatWithAssociatedValue s in _minimumValuestoScale)
            {
                if (cm.GetStatFromModifyableStatsEnum(s.Stat) < s.Value)
                    return base.Weight; //return regular weight if any of min stats are not met

                weightBonus = weightBonus + CalculateSingleStatWeightBonusPercent(s.Stat, cm.GetStatFromModifyableStatsEnum(s.Stat));
            }

            
                
                return weightBonus / _minimumValuestoScale.Length;
        }
        set => base.Weight = value; }

    private float CalculateSingleStatWeightBonusPercent(Enums.ModifyableStats stat, float currentStatValue)
    {
        StatWithAssociatedValue min = new StatWithAssociatedValue();
        StatWithAssociatedValue max = new StatWithAssociatedValue();

        foreach(StatWithAssociatedValue s in _minimumValuestoScale)
        {
            if (s.Stat == stat)
                min = s;
        }

        foreach (StatWithAssociatedValue s in _maximumValuestoScale)
        {
            if (s.Stat == stat)
                max = s;
        }

        float statDifference = max.Value - min.Value;
        float currentStatAboveMin = currentStatValue - min.Value;

        return currentStatAboveMin / statDifference >= 1f ? _maximumValuesWeightBonus : (currentStatAboveMin / statDifference) * _maximumValuesWeightBonus;
    }
}
