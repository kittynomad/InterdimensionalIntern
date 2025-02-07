using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour
{
    public enum ModifyableStats
    {
        population,
        populationGrowth,
        resources,
        happiness,
        happinessGrowth
    }

    public enum CivilizationPhase
    {
        huts,
        cabins,
        castles,
        brick,
        modern,
        future,
        superFuture
    }
}
