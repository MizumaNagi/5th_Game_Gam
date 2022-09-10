using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] private PlayFallMode playFallMode;
    [SerializeField] private GameData gameData;
    [SerializeField] private SelectForcus groupForcus;
    [SerializeField] private SelectForcus typeForcus;

    //
    [SerializeField] private Image[] groupImgs;
    [SerializeField] private TMP_Text[] groupTxts;

    // 
    [SerializeField] private TMP_InputField sizeCost;
    [SerializeField] private Slider sizeSlider;
    [SerializeField] private TMP_InputField countCost;
    [SerializeField] private Slider countSlider;
    [SerializeField] private TMP_InputField typeCost;
    [SerializeField] private TMP_InputField totalCost;
    [SerializeField] private TMP_Text haveCost;

    private int haveCostNum;
    private int sizeCostNum;
    private int countCostNum;
    private int typeCostNum;
    private int totalCostNum;

    private int nextMakeBallCnt;
    private float nextMakeSize;

    public int NextMakeBallCnt => nextMakeBallCnt;
    public float NextMakeSize => nextMakeSize;

    public void Init(int initHaveCost)
    {
        sizeSlider.onValueChanged.AddListener(delegate { UpdateCostValue(sizeSlider, sizeCost, gameData.MinSize, gameData.MaxSize, 0); });
        countSlider.onValueChanged.AddListener(delegate { UpdateCostValue(countSlider, countCost, gameData.MinCount, gameData.MaxCount, 1); });
        haveCostNum = initHaveCost;
        UpdateHaveCost();
    }

    private void Update()
    {
        float typeCostMulti = gameData.MultiCostEachType[typeForcus.CurrentSelectIndex];
        typeCost.text = typeCostMulti.ToString();
        totalCostNum = Mathf.RoundToInt((sizeCostNum + countCostNum) * typeCostMulti);
        totalCost.text = totalCostNum.ToString();
    }

    private void UpdateCostValue(Slider targetSlider, TMP_InputField targetField, int minStatusValue, int maxStatusValue, int statusType)
    {
        float sliderVal = targetSlider.value;
        int currentCost = Mathf.RoundToInt(Mathf.Lerp(gameData.MinCost, gameData.MaxCost, sliderVal));
        targetField.text = currentCost.ToString();
        int statusVal = Mathf.RoundToInt(Mathf.Lerp(minStatusValue, maxStatusValue, sliderVal));
        if (statusType == 0)
        {
            groupImgs[groupForcus.CurrentSelectIndex].rectTransform.sizeDelta = new Vector2(statusVal, statusVal);
            sizeCostNum = currentCost;
            nextMakeSize = Mathf.Lerp(0.2f, 0.9f, sliderVal);
        }
        else if (statusType == 1)
        {
            groupTxts[groupForcus.CurrentSelectIndex].text = statusVal.ToString();
            countCostNum = currentCost;
            nextMakeBallCnt = statusVal;
        }
        UpdateHaveCost();
    }

    private void UpdateHaveCost()
    {
        haveCost.text = haveCostNum.ToString();
    }

    public bool IsEnoughCost()
    {
        return haveCostNum >= totalCostNum;
    }
}
