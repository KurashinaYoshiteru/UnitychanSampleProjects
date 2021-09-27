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
        //Score.csからscoreを取得し、３桁でGUIに表示する
        int score = Score.instance.score;
        string scoreAddZero = score.ToString("000");
        mText.text = "Score:" + scoreAddZero;
    }
}
