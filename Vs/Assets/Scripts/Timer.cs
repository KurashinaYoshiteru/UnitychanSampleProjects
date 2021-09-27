using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float fTimeLimit = 1f;     //各プレハブの生存時間
    private float m_fTimeLimit = 0f;   //残り生存時間

    private void Awake()
    {
        m_fTimeLimit = fTimeLimit;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //指定された時間が経過したらオブジェクト削除
        m_fTimeLimit -= Time.deltaTime;
        if (m_fTimeLimit < 0f) Destroy(gameObject);
    }
}
