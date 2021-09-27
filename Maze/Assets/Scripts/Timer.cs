using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float fTimeLimit = 1f;         //各プレハブの生存時間
    private float m_fTimeLeft = 0f;       //残り生存時間

    private void Awake()
    {
        m_fTimeLeft = fTimeLimit;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //秒数をカウントし生存時間が経過したらDestroy
        m_fTimeLeft -= Time.deltaTime;
        if(m_fTimeLeft < 0)
        {
            Destroy(gameObject);
        }
    }
}
