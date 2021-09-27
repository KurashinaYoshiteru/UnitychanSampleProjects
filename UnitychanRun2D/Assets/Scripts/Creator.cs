using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creator : MonoBehaviour
{
    public GameObject prefab;                        //生成するプレハブ
    public Vector3 randomPosRange = Vector3.up;      //生成する位置から離れたランダムな位置
    public Vector3 velocity = Vector3.left;          //生成時の早さ
    public float offsetTime = 0f;                    //最初に生成するまでの時間
    public float intervalTime = 3f;                  //生成間隔
    public float leftTime = 5f;                      //生成したオブジェクトが消えるまでの時間

    private float mTime = 0f;                        //このクラスで管理する時間

    // Start is called before the first frame update
    void Start()
    {
        mTime = -offsetTime;
    }

    // Update is called once per frame
    void Update()
    {
        //ゲーム中の状態でなければ生成しない
        if (Game.instance.state != Game.STATE.MOVE) return;

        mTime += Time.deltaTime;

        //時間が0以下なら処理終了
        if (mTime < 0f) return;

        //生成間隔を超えたら対象オブジェクトを生成
        if (mTime >= intervalTime)
        {
            //生成位置をランダムに指定
            Vector3 randomPos = Vector3.one;
            randomPos.x = Random.Range(-randomPosRange.x, randomPosRange.x);
            randomPos.y = Random.Range(-randomPos.y, randomPos.y);
            Vector3 pos = transform.position + randomPos;

            //生成、指定時間後消滅
            GameObject obj = Instantiate(prefab, pos, transform.rotation) as GameObject;

            obj.GetComponent<Rigidbody2D>().velocity = velocity;

            Destroy(obj, leftTime);

            //時間リセット
            mTime = 0f;
        }
    }
}
