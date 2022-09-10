using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    [SerializeField] private int minCost;
    [SerializeField] private int maxCost;
    [SerializeField] private int minSize;
    [SerializeField] private int maxSize;
    [SerializeField] private int minCount;
    [SerializeField] private int maxCount;
    [SerializeField] private float[] multiCostEachType;

    public int MinCost => minCost;
    public int MaxCost => maxCost;
    public int MinSize => minSize;
    public int MaxSize => maxSize;
    public int MinCount => minCount;
    public int MaxCount => maxCount;
    public float[] MultiCostEachType => multiCostEachType;
}
