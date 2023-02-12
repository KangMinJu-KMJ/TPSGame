using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField]
    private Text txtscore;
    public int killCount;

    void Start()
    {
        txtscore = GameObject.Find("Canvas_HP").transform.GetChild(1).transform.GetChild(0)
            .GetComponent<Text>();
        DisplayScore(0);
    }

    public void DisplayScore(int _score)
    {
        killCount += _score;
        txtscore.text = "Kill : <color=#ff0000>" + killCount.ToString()+"</color>";
        //이러면 숫자만 빨간색이 됨

    }
}
