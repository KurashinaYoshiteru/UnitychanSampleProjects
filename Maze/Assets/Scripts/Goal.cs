using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
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
        GameObject hitObject = hitCollider.gameObject;            //�Փˑ���̃I�u�W�F�N�g

        //�Փˑ���̃I�u�W�F�N�g��Player�R���|�[�l���g�������Ă��Ȃ�(=�v���C���[����Ȃ�)�Ƃ��͏����I��
        if (hitObject.GetComponent<Player>() == null) return;

        Game.SetStageClear();
    }
}
