using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToGetWorldPos : MonoBehaviour
{
    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private float pointLength;
    [SerializeField] private LineRenderer lineRenderer;


    private int targetLayer = 1 << 6;
    private float remPointLength = 0f;
    private float renderLineWidth = 0.2f;
    private Vector3 beforeFrameCursorPos = Vector3.positiveInfinity;
    private Camera mainCamera;
    private List<Vector3> clickPosList = new List<Vector3>();

    public List<Vector3> ClickPosList => clickPosList;

    private void Start()
    {
        mainCamera = Camera.main;
        lineRenderer.startWidth = renderLineWidth;
        lineRenderer.endWidth = renderLineWidth;
    }

    void Update()
    {
        if (Input.GetMouseButton(0) == false) return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, 100f, targetLayer) == true)
        {
            // 1�_�ڂ̂�
            if(beforeFrameCursorPos == Vector3.positiveInfinity)
            {
                clickPosList.Add(hit.point);
                remPointLength = pointLength;
            }
            else
            {
                remPointLength -= Vector3.Distance(beforeFrameCursorPos, hit.point);
                beforeFrameCursorPos = hit.point;
            }

            // ���_�ǉ�
            if (remPointLength < 0f)
            {
                clickPosList.Add(hit.point);
                //Instantiate(pointPrefab, hit.point, Quaternion.identity);
                remPointLength = pointLength;

                // �K�C�h���C���X�V
                lineRenderer.positionCount = clickPosList.Count;
                lineRenderer.SetPositions(clickPosList.ToArray());
            }
        }
    }

    public void ResetClickPos()
    {
        clickPosList.Clear();
        beforeFrameCursorPos = Vector3.positiveInfinity;
        lineRenderer.positionCount = 0;
    }
}
