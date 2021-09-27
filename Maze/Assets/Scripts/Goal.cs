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
        GameObject hitObject = hitCollider.gameObject;            //衝突相手のオブジェクト

        //衝突相手のオブジェクトがPlayerコンポーネントを持っていない(=プレイヤーじゃない)ときは処理終了
        if (hitObject.GetComponent<Player>() == null) return;

        Game.SetStageClear();
    }
}
