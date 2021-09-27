using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    private static Game mInstance;

    //�Q�[���̃C���X�^���X��Ԃ��֐�
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

    //�Q�[���̏�ԃ��X�g
    public enum STATE
    {
        NONE,            //�����Ȃ����
        START,           //�X�^�[�g���̏��
        MOVE,            //�Q�[�����̏��
        GAMEOVER         //�Q�[���I�[�o�[�̏��
    };

    //�Q�[���̏��
    public STATE state
    {
        get;
        set;
    }

    //��Ԃɂ���ĕ\�������e�L�X�g
    private Text mText;

    // Start is called before the first frame update
    void Start()
    {
        mText = GetComponent<Text>();

        //�Q�[���̏�Ԃ��X�^�[�g�ɂ���
        state = STATE.START;

        //�X�^�[�g���̏���
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
