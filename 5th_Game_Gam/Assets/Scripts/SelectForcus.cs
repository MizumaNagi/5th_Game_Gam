using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectForcus : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    [SerializeField] private Transform forcus;

    private int currentSelectIndex = 0;
    public int CurrentSelectIndex => currentSelectIndex;

    private void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int tmp = i;
            buttons[tmp].onClick.AddListener(() => Moveforcus(tmp));
        }
    }

    private void Moveforcus(int index)
    {
        if (buttons.Length <= index) return;

        currentSelectIndex = index;
        forcus.transform.position = buttons[index].transform.position;
    }
}
