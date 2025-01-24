using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilizationStatsManager : MonoBehaviour
{
    [SerializeField] private int _population;
    [SerializeField] private float _resources;

    public int Population { get => _population; set => _population = value; }
    public float Resources { get => _resources; set => _resources = value; }
}
