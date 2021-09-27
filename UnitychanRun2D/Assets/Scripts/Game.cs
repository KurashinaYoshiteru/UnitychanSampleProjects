using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    private static Game mInstance;

    //ゲームのインスタンスを返す関数
    public static Game instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = FindObjectOfType<Game>();
            }
            return mInstance;
        }
    }

    //ゲームの状態リスト
    public enum STATE
    {
        NONE,            //何もない状態
        START,           //スタート時の状態
        MOVE,            //ゲーム中の状態
        GAMEOVER         //ゲームオーバーの状態
    };

    //ゲームの状態
    public STATE state
    {
        get;
        set;
    }

    //状態によって表示されるテキスト
    private Text mText;

    // Start is called before the first frame update
    void Start()
    {
        mText = GetComponent<Text>();

        //ゲームの状態をスタートにする
        state = STATE.START;

        //スタート時の処理
        StartCoroutine("StartCountDown");
    }

    // Update is called once per frame
    void Update()
    {

        switch (state)
        {
            case STATE.START:
                break;
            case STATE.MOVE:
                break;
            case STATE.GAMEOVER:
                mText.text = "Game Over";
                if (Input.GetButtonDown("Jump"))
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                break;
        }
    }

    IEnumerator StartCountDown()
    {
        mText.text = "3";
        yield return new WaitForSeconds(1.0f);
        mText.text = "2";
        yield return new WaitForSeconds(1.0f);
        mText.text = "1";
        yield return new WaitForSeconds(1.0f);
        mText.text = "";
        state = STATE.MOVE;
    }
}
