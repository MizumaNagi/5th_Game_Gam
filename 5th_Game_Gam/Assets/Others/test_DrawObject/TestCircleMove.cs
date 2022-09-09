using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCircleMove : MonoBehaviour
{
    private float radius = 3f;

    [Range(0, 360)] public float r;
    public Transform t;

    private void FixedUpdate()
    {
        t.position = Quaternion.Euler(0, 0, 360 - r) * new Vector3(0, radius, 0);
    }
}
