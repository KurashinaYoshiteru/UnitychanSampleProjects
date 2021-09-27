using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : HitObject
{
    private static readonly float bulletMoveSpeed = 10.0f;         //弾速
    public GameObject hitEffectPrefab = null;                      //ヒットエフェクトプレハブ

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //移動
        {
            Vector3 vecAddPos = Vector3.forward * bulletMoveSpeed;
            transform.position += transform.rotation * vecAddPos * Time.deltaTime;
        }
    }

    //弾が何かに当たった時の処理
    private void OnTriggerEnter(Collider hitCollider)
    {
        //当たってOKなものでなければ処理終了
        if (IsHitOK(hitCollider.gameObject) == false) return;
        
        //ヒットエフェクトがあれば生成
        if(hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }

}
