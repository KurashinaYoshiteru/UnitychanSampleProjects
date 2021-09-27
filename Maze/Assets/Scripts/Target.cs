using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject hitEffectPrefab = null;             //�Փˎ��̃G�t�F�N�g
    private static int m_allTargetNum = 0;                //�X�e�[�W�ɔz�u�����^�[�Q�b�g��

    private void Awake()
    {
        m_allTargetNum++;          //�^�[�Q�b�g�̑����𐔂���
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider hitCollider)
    {
        GameObject hitObject = hitCollider.gameObject;         //�Փˑ���I�u�W�F�N�g

        //�Փˑ���̃I�u�W�F�N�g��Bullet�R���|�[�l���g�������Ă��Ȃ�(=�e����Ȃ�)�Ƃ��͏����I��
        if (hitObject.GetComponent<Bullet>() == null)
        {
            return;
        }

        //�Փ˃G�t�F�N�g����
        if(hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, transform.position, transform.rotation);
        }

        //�X�e�[�W�N���A�̃`�F�b�N
        {
            m_allTargetNum--;

            if (m_allTargetNum <= 0)
            {
                Game.SetStageClear();
            }
        }

        Destroy(gameObject);
    }
}
