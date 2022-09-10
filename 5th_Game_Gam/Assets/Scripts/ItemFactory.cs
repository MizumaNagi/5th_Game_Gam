using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private float ballMakeDelaySec;
    [SerializeField] private Transform ballParent;

    public void StartBallMake(int makeCount, Vector3 makePos, float size)
    {
        StartCoroutine(DelayMake(makeCount, makePos, size));
    }

    private IEnumerator DelayMake(int remMakeCnt, Vector3 makePos, float size)
    {
        yield return new WaitForSeconds(ballMakeDelaySec);

        GameObject newBall = Instantiate(ballPrefab, makePos, Quaternion.identity);
        newBall.transform.SetParent(ballParent);
        newBall.transform.localScale = new Vector3(size, size, size);

        if(--remMakeCnt >= 0) StartCoroutine(DelayMake(remMakeCnt, makePos, size));
    }
}
