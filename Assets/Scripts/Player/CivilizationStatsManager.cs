using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class CivilizationStatsManager : MonoBehaviour
{
    [Header("Civilization Stats")]
    [SerializeField] private int _population = 2;
    [SerializeField] private float _resources = 10.4f;
    [SerializeField] private float _popGrowthPercentPerTick = 100f;
    [SerializeField] private float _happinessGrowthPercentPerTick = 100f;
    [SerializeField] [Range(0.0f, 100.0f)] private float _happiness = 100f;

    [Header("Game Settings")]
    [SerializeField] private float _tickTime = 1f;
    [SerializeField] private LiveGraph _liveGraph;
    private bool advancingTick = true;
    private int tickCount = 0;
    [SerializeField] private PopUpManager _popUpManager;

    [Header("Choice Settings")]
    [SerializeField] private ChoiceUIManager _choiceUIManager;
    [SerializeField] private int _ticksBetweenChoices = 30;
    [SerializeField] private int _ticksToChoose = 6;
    [SerializeField] private bool _autoChoose = false;

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
    public LiveGraph LiveGraph { get => _liveGraph; set => _liveGraph = value; }
    public int[] StageChangers { get => stageChangers; set => stageChangers = value; }





    public int TicksBetweenChoices { get => _ticksBetweenChoices; set => _ticksBetweenChoices = value; }

    public void Start()
    {
        if (_choiceUIManager == null)
            _choiceUIManager = GameObject.FindObjectOfType<ChoiceUIManager>();
        _choiceUIManager.gameObject.SetActive(false);
        if (_liveGraph == null)
            _liveGraph = GameObject.FindObjectOfType<LiveGraph>();
        _popUpManager.NewPopUp();
        StartCoroutine(tickAdvance());
    }

    public void OnTick()
    {
        _population = Random.Range(0, 2) + 
            (int)((float)_population * (_popGrowthPercentPerTick / 100f));

        //changes the stage number and animation if population hits a certain number
        //if (_population > stageChangers[stage])
        //{
            //stage++;
            //anim.SetInteger("curStage", stage);
        //}

        Happiness *= (_happinessGrowthPercentPerTick / 100f);
        if (_happiness >= 100f) _happiness = 100f;
        _liveGraph.UpdateLiveGraph();
        _popUpManager.UpdatePopUp();
        ChoiceDelay();
    }
    /// <summary>
    /// Makes the choices appear after tickCount is greater than or equal to _ticksBetweenChoices.
    /// </summary>
    public void ChoiceDelay()
    {
        if (tickCount >= _ticksBetweenChoices)
        {
            StopAllCoroutines();
            tickCount = 0;
            _choiceUIManager.gameObject.SetActive(true);
            _choiceUIManager.DisplayNewChoices();

            //StartCoroutine(ChoicePauseTimer());
            return;
        }
        tickCount++;
    }
    /// <summary>
    /// Pauses the tick advance for _ticksToChoose ticks then continues it after time runs out or the player has made a choice.
    /// </summary>
    /// <returns></returns>
    IEnumerator ChoicePauseTimer()
    {
        
        yield return new WaitForSeconds(_ticksToChoose);
        if (_autoChoose && _choiceUIManager.gameObject.activeSelf) //If the player hasn't chosen yet
        {
            int autoChoice = Random.Range(0, _choiceUIManager.Choices.Choices.Count());
            ApplyChoice(_choiceUIManager.Choices.Choices[autoChoice]);
        }
        //ContinueTickAdvance();
    }
    /// <summary>
    /// Resumes the advance of ticks.
    /// </summary>
    public void ContinueTickAdvance()
    {
        _choiceUIManager.gameObject.SetActive(false);
        StartCoroutine(tickAdvance());
    }

    public void ApplyChoice(BasicChoice choice)
    {
        foreach (StatModifier s in choice.StatModifiers)
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
                    _popGrowthPercentPerTick = m(_popGrowthPercentPerTick, s.ModificationValue);
                    break;
                case (Enums.ModifyableStats.happiness):
                    _happiness = m(_happiness, s.ModificationValue);
                    break;
                case (Enums.ModifyableStats.happinessGrowth):
                    _happinessGrowthPercentPerTick = m(_happinessGrowthPercentPerTick, s.ModificationValue);
                    break;
                case (Enums.ModifyableStats.resources):
                    _resources = m(_resources, s.ModificationValue);
                    break;
            }
        }
        _popUpManager.NewPopUp();
        ContinueTickAdvance();
    }

    public float GetStatFromModifyableStatsEnum(Enums.ModifyableStats s)
    {
        switch(s)
        {
            case (Enums.ModifyableStats.happiness):
                return _happiness;
            case (Enums.ModifyableStats.happinessGrowth):
                return _happinessGrowthPercentPerTick;
            case (Enums.ModifyableStats.population):
                return _population;
            case (Enums.ModifyableStats.populationGrowth):
                return _popGrowthPercentPerTick;
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
