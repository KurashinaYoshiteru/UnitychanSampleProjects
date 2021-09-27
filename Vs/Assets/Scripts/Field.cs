using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public GameObject m_blockObject = null;                                  //ブロックオブジェクト
    public GameObject m_player1Object = null;                                //プレイヤー1オブジェクト
    public GameObject m_player2Object = null;                                //プレイヤー2オブジェクト

    public static readonly int FIELD_GRID_X = 9;                             //Xグリッド数
    public static readonly int FIELD_GRID_Y = 9;                             //Yグリッド数
    public static readonly float BLOCK_SCALE = 2.0f;                         //ブロックのスケール
    public static readonly Vector3 BLOCK_OFFSET = new Vector3(1, 1, 1);      //ブロック配置のオフセット

    //マップ上配置物の種類
    public enum ObjectKind
    {
        Empty,         //0
        Block,         //1
        Player1,       //2
        Player2        //3
    }

    //配置データ
    public static readonly int[] GRID_OBJECT_DATA = new int[]
    {
        1,  1,  1,  1,  1,  1,  1,  1,  1,
        1,  2,  0,  0,  0,  0,  0,  0,  1,
        1,  0,  1,  1,  1,  0,  1,  0,  1,
        1,  0,  0,  0,  0,  0,  0,  0,  1,
        1,  0,  1,  0,  1,  1,  1,  0,  1,
        1,  0,  1,  0,  1,  0,  0,  0,  1,
        1,  0,  1,  0,  0,  0,  1,  0,  1,
        1,  0,  0,  0,  1,  0,  0,  3,  1,
        1,  1,  1,  1,  1,  1,  1,  1,  1,
    };

    private GameObject m_blockParent = null;                   //生成したブロックの親

    private void Awake()
    {
        InitializeField();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //フィールドの初期化
    private void InitializeField()
    {
        //ブロックの親の初期化
        m_blockParent = new GameObject();
        m_blockParent.name = "BlockParent";
        m_blockParent.transform.parent = transform;

        GameObject originalObject = null;           //生成するブロックの元となるオブジェクト
        GameObject instanceObject;           //ブロックをとりあえず入れる変数
        Vector3 postion;                     //生成位置

        int gridX;
        int gridY;
        for(gridX = 0; gridX < FIELD_GRID_X; gridX++)
        {
            for (gridY = 0; gridY < FIELD_GRID_Y; gridY++)
            {
                //各位置に何を置くか、originalOnjectに何を入れる
                switch ((ObjectKind)GRID_OBJECT_DATA[gridX + gridY * FIELD_GRID_X])
                {
                    case ObjectKind.Block:
                        originalObject = m_blockObject;
                        break;
                    case ObjectKind.Player1:
                        originalObject = m_player1Object;
                        break;
                    case ObjectKind.Player2:
                        originalObject = m_player2Object;
                        break;
                    default:
                        originalObject = null;
                        break;
                }

                if (originalObject == null) continue;

                //ブロック生成
                postion = new Vector3(gridX * BLOCK_SCALE, 0, gridY * BLOCK_SCALE) + BLOCK_OFFSET;
                instanceObject = Instantiate(originalObject, postion, originalObject.transform.rotation) as GameObject;
                instanceObject.name = "" + originalObject.name + "(" + gridX + "," + gridY + ")";
                instanceObject.transform.localScale = Vector3.one * BLOCK_SCALE;
                instanceObject.transform.parent = m_blockParent.transform;
            }
        }
    }
}
