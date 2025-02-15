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
    [SerializeField] private ChoiceUIManager _choiceUIManager;
    [SerializeField] private float _ticksBetweenChoices = 30f;
    [SerializeField] private float _ticksToChoose = 6f;
    private bool advancingTick = true;
    private int tickCount = 0;

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
        if (_choiceUIManager == null)
            _choiceUIManager = GameObject.FindObjectOfType<ChoiceUIManager>();
        _choiceUIManager.gameObject.SetActive(false);
        StartCoroutine(tickAdvance());
    }

    public void OnTick()
    {
        _population = Random.Range(0, 2) + 
            (int)((float)_population * _popGrowthPerTick);

        //changes the stage number and animation if population hits a certain number
        //if (_population > stageChangers[stage])
        //{
            //stage++;
            //anim.SetInteger("curStage", stage);
        //}

        Happiness *= _happinessGrowthPerTick;
        ChoiceDelay();
        Debug.Log(tickCount);
    }
    public void ChoiceDelay()
    {
        if (tickCount >= _ticksBetweenChoices)
        {
            advancingTick = false;
            tickCount = 0;
            StartCoroutine(ChoicePauseTimer());
            return;
        }
        tickCount++;
    }
    IEnumerator ChoicePauseTimer()
    {
        _choiceUIManager.gameObject.SetActive(true);
        yield return new WaitForSeconds(_ticksToChoose);
        _choiceUIManager.gameObject.SetActive(false);
        advancingTick = true;
        StartCoroutine(tickAdvance());

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

    public float GetStatFromModifyableStatsEnum(Enums.ModifyableStats s)
    {
        switch(s)
        {
            case (Enums.ModifyableStats.happiness):
                return _happiness;
            case (Enums.ModifyableStats.happinessGrowth):
                return _happinessGrowthPerTick;
            case (Enums.ModifyableStats.population):
                return _population;
            case (Enums.ModifyableStats.populationGrowth):
                return _popGrowthPerTick;
            case (Enums.ModifyableStats.resources):
                return _resources;
        }

        return -1f;
    }

    public IEnumerator tickAdvance()
    {
        while(advancingTick)
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
