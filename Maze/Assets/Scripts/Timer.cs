using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float fTimeLimit = 1f;         //�e�v���n�u�̐�������
    private float m_fTimeLeft = 0f;       //�c�萶������

    private void Awake()
    {
        m_fTimeLeft = fTimeLimit;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //�b�����J�E���g���������Ԃ��o�߂�����Destroy
        m_fTimeLeft -= Time.deltaTime;
        if(m_fTimeLeft < 0)
        {
            Destroy(gameObject);
        }
    }
}
