using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    private static Score mInstance;

    //Scoreのインスタンスを返す関数
    public static Score instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = FindObjectOfType<Score>();
            }
            return mInstance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(instance != this)
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    //スコア変数、setはここからしか呼ばれないようにする
    public int score
    {
        get;
        private set;
    }

    //スコアを加算する
    public void Add()
    {
        score++;
    }

    //スコアをリセットする
    public void Reset()
    {
        score = 0;
    }
}
