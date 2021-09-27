using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreGUI : MonoBehaviour
{
    private Text mText;

    // Start is called before the first frame update
    void Start()
    {
        mText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //Score.cs����score���擾���A�R����GUI�ɕ\������
        int score = Score.instance.score;
        string scoreAddZero = score.ToString("000");
        mText.text = "Score:" + scoreAddZero;
    }
}
