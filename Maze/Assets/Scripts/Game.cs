using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static bool m_stageClearFlag = false;         //�X�e�[�W�N���A���Ă�����true

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�X�e�[�W�N���A����ƌĂ΂��
    public static void SetStageClear()
    {
        m_stageClearFlag = true;
    }

    //�X�e�[�W�N���A���Ă��邩�ǂ����̃`�F�b�N
    public static bool IsStageCleared()
    {
        return m_stageClearFlag;
    }

}
