using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour 
{
    private static readonly float  bulletMoveSpeed = 10.0f;        //弾丸速度
    public GameObject hitEffectPrefab = null;                      //衝突時のエフェクト

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //移動
        {
            Vector3 vecAddPos = (Vector3.forward * bulletMoveSpeed);
            transform.position += transform.rotation * vecAddPos * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //衝突エフェクト生成
        if(hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }
}
