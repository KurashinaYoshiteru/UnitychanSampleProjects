using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static bool m_stageClearFlag = false;         //ステージクリアしていたらtrue

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ステージクリアすると呼ばれる
    public static void SetStageClear()
    {
        m_stageClearFlag = true;
    }

    //ステージクリアしているかどうかのチェック
    public static bool IsStageCleared()
    {
        return m_stageClearFlag;
    }

}
