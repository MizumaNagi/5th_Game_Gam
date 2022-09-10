using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;

    public List<StatusDataEachGroup> statusDataEachGroups = new List<StatusDataEachGroup>();

    public void Start()
    {
        GameStart();
        uiManager.Init(30);
    }


    public void GameStart()
    {
        for(int i = 0; i < 2; i++)
        {
            statusDataEachGroups.Add(new StatusDataEachGroup());
        }
    }

    public void GameEnd()
    {

    }

    // 
    private static GameManager instance;
    public static GameManager Instance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }
}
