using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayFallMode : MonoBehaviour
{
    [SerializeField] private ItemFactory itemFactory;
    [SerializeField] private GameObject fallAreaBack;
    [SerializeField] private GameObject drawLineAreaBack;
    [SerializeField] private SelectForcus groupForcus;
    [SerializeField] private GameObject fallMarkerPrefab;
    [SerializeField] private UIManager uiManager;

    private GameObject[] fallMarkers = new GameObject[2];
    private int targetLayer = 1 << 7;
    private bool isSelecting = false;
    private Camera mainCamera;
    private bool[] isPlaceEachGroup = new bool[2];

    public bool[] IsPlaceEachGroup => isPlaceEachGroup;

    private void Start()
    {
        mainCamera = Camera.main;
        fallAreaBack.SetActive(false);
    }

    private void Update()
    {
        if (isSelecting == false) return;
        if (Input.GetMouseButtonDown(0) == false) return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, 100f, targetLayer) == true)
        {
            int targetIndex = groupForcus.CurrentSelectIndex;
            GameManager.Instance().statusDataEachGroups[targetIndex].fallStartPos = hit.point;

            if(fallMarkers[targetIndex] != null)
            {
                Destroy(fallMarkers[targetIndex]);
                fallMarkers[targetIndex] = null;
            }
            EndFallMode();

            if (uiManager.IsEnoughCost() == false) return;

            fallMarkers[targetIndex] = Instantiate(fallMarkerPrefab, hit.point, Quaternion.identity);
            fallMarkers[targetIndex].GetComponentInChildren<TMPro.TMP_Text>().text = $"Å´{targetIndex + 1}";
            fallMarkers[targetIndex].transform.SetParent(fallAreaBack.transform.parent);
            isPlaceEachGroup[targetIndex] = true;
            itemFactory.StartBallMake(uiManager.NextMakeBallCnt, new Vector3(hit.point.x, hit.point.y, 0f), uiManager.NextMakeSize);
        }
    }

    public void StartFallMode(int buttonIndex)
    {
        isSelecting = true;

        fallAreaBack.SetActive(true);
        drawLineAreaBack.SetActive(false);
    }

    private void EndFallMode()
    {
        isSelecting = false;

        fallAreaBack.SetActive(false);
        drawLineAreaBack.SetActive(true);
    }
}
