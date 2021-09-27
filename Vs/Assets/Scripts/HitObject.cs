using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitObject : MonoBehaviour
{
    //�����蔻��O���[�v
    public enum HItGroup
    {
        Player1,
        Player2,
        Other
    }

    public HItGroup m_hitGroup = HItGroup.Player1;        //�����̃O���[�v

    //�ՓˑΏۂ̃I�u�W�F�N�g�Ŏ����̃O���[�v�łȂ����true��Ԃ�
    protected bool IsHitOK(GameObject hittedObject)
    {
        HitObject hit = hittedObject.GetComponent<HitObject>();
        if (hit == null) return false;
        if (hit.m_hitGroup == this.m_hitGroup) return false;

        return true;
    }
}
