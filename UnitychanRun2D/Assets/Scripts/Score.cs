using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    private static Score mInstance;

    //Score�̃C���X�^���X��Ԃ��֐�
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

    //�X�R�A�ϐ��Aset�͂������炵���Ă΂�Ȃ��悤�ɂ���
    public int score
    {
        get;
        private set;
    }

    //�X�R�A�����Z����
    public void Add()
    {
        score++;
    }

    //�X�R�A�����Z�b�g����
    public void Reset()
    {
        score = 0;
    }
}
