using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float fTimeLimit = 1f;     //�e�v���n�u�̐�������
    private float m_fTimeLimit = 0f;   //�c�萶������

    private void Awake()
    {
        m_fTimeLimit = fTimeLimit;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //�w�肳�ꂽ���Ԃ��o�߂�����I�u�W�F�N�g�폜
        m_fTimeLimit -= Time.deltaTime;
        if (m_fTimeLimit < 0f) Destroy(gameObject);
    }
}
