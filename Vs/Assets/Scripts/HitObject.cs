using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitObject : MonoBehaviour
{
    //当たり判定グループ
    public enum HItGroup
    {
        Player1,
        Player2,
        Other
    }

    public HItGroup m_hitGroup = HItGroup.Player1;        //自分のグループ

    //衝突対象のオブジェクトで自部のグループでなければtrueを返す
    protected bool IsHitOK(GameObject hittedObject)
    {
        HitObject hit = hittedObject.GetComponent<HitObject>();
        if (hit == null) return false;
        if (hit.m_hitGroup == this.m_hitGroup) return false;

        return true;
    }
}
