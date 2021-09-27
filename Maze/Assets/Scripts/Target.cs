using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject hitEffectPrefab = null;             //衝突時のエフェクト
    private static int m_allTargetNum = 0;                //ステージに配置されるターゲット数

    private void Awake()
    {
        m_allTargetNum++;          //ターゲットの総数を数える
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
        GameObject hitObject = hitCollider.gameObject;         //衝突相手オブジェクト

        //衝突相手のオブジェクトがBulletコンポーネントを持っていない(=弾じゃない)ときは処理終了
        if (hitObject.GetComponent<Bullet>() == null)
        {
            return;
        }

        //衝突エフェクト生成
        if(hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, transform.position, transform.rotation);
        }

        //ステージクリアのチェック
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
