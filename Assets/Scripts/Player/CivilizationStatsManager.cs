using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
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

    [Header("Stage Settings")]
    [SerializeField] private int stage = 0;
    [SerializeField] private int[] stageChangers;
    [SerializeField] private Animator anim;

    delegate float modifierDelegate(float modifiedVar, float modValue);


    public int Population { get => _population; set => _population = value; }
    public float Resources { get => _resources; set => _resources = value; }
    public float Happiness { get => _happiness; set => _happiness = value; }
    public int Stage { get => stage; set => stage = value; }
    public float TickTime { get => _tickTime; set => _tickTime = value; }

    public void Start()
    {
        StartCoroutine(tickAdvance());
    }

    public void OnTick()
    {
        
        _population = Random.Range(0, 2) + 
            (int)((float)_population * _popGrowthPerTick);

        //changes the stage number and animation if population hits a certain number
        if (_population > stageChangers[stage])
        {
            stage++;
            anim.SetInteger("curStage", stage);
        }

        Happiness *= _happinessGrowthPerTick;
    }

    public void ApplyChoice(BasicChoice choice)
    {
        foreach(StatModifier s in choice.StatModifiers)
        {
            modifierDelegate m = null;

            switch(s.ModificationToPerform)
            {
                case (Enums.StatOperators.add):
                    m = Add;
                    break;
                case (Enums.StatOperators.subtract):
                    m = Subtract;
                    break;
                case (Enums.StatOperators.multiply):
                    m = Multiply;
                    break;
                case (Enums.StatOperators.divide):
                    m = Divide;
                    break;
            }

            switch(s.StatToModify)
            {
                case (Enums.ModifyableStats.population):
                    Population = (int)m(Population, s.ModificationValue);
                    break;
                case (Enums.ModifyableStats.populationGrowth):
                    _popGrowthPerTick = m(_popGrowthPerTick, s.ModificationValue);
                    break;
                case (Enums.ModifyableStats.happiness):
                    _happiness = m(_happiness, s.ModificationValue);
                    break;
                case (Enums.ModifyableStats.happinessGrowth):
                    _happinessGrowthPerTick = m(_happinessGrowthPerTick, s.ModificationValue);
                    break;
                case (Enums.ModifyableStats.resources):
                    _resources = m(_resources, s.ModificationValue);
                    break;
            }
        }
    }

    public IEnumerator tickAdvance()
    {
        while(true)
        {
            yield return new WaitForSeconds(_tickTime);
            OnTick();
        }
        
    }

    //basic math for delegate
    private float Add(float fOne, float fTwo) { return fOne + fTwo; }
    private float Subtract(float fOne, float fTwo) { return fOne - fTwo; }
    private float Multiply(float fOne, float fTwo) { return fOne * fTwo; }
    private float Divide(float fOne, float fTwo) { return fOne / fTwo; }

}
