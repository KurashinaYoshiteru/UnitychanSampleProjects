using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AI : Player_Base
{
    //チェック方向
    private enum CheckDir
    {
        Left,
        Up,
        Right,
        Down,
        EnumMax
    }

    //チェック情報
    private enum CheckData
    {
        X,
        Y,
        EnumMax
    }

    //チェック方向
    private static readonly int[][] CHECK_DIR_LIST = new int[(int)CheckDir.EnumMax][]
    {
        new int[(int)CheckData.EnumMax]{-1,  0},
        new int[(int)CheckData.EnumMax]{ 0,  1},
        new int[(int)CheckData.EnumMax]{ 1,  0},
        new int[(int)CheckData.EnumMax]{ 0, -1}
    };

    private static readonly int AI_PRIO_MIN = 99;                 //AI優先度で最も低い値
    private static readonly float AI_INTERVAL_MIN = 0.5f;         //AI思考間隔の最短
    private static readonly float AI_INTERVAL_MAX = 0.8f;         //AI思考間隔の最長
    private static readonly float AI_IGNORE_DISTANCE = 2.0f;      //プレイヤーへの接近限界
    private static readonly float SHOOT_INTERVAL = 1.0f;          //射撃間隔

    private float m_aiInterval = 0f;                              //現在のAIの思考間隔
    private float m_shootInterval = 0f;                           //現在の射撃の間隔
    private PlayerInput m_pressInput = PlayerInput.Move_Left;     //現在のAIの入力

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void GetInput()
    {
        //ユーザーが動かしているプレイヤーを取得
        GameObject mainObject = Player_Key.m_mainPlayer;
        if (mainObject == null) return;

        //思考更新時間
        m_aiInterval -= Time.deltaTime;
        m_shootInterval -= Time.deltaTime;

        //プレイヤーとAIの距離を算出
        Vector3 aiSubPosition = transform.position - mainObject.transform.position;
        aiSubPosition.y = 0f;

        //距離があったら動く
        if(aiSubPosition.magnitude > AI_IGNORE_DISTANCE)
        {
            if (m_aiInterval < 0f)
            {
                m_aiInterval = Random.Range(AI_INTERVAL_MIN, AI_INTERVAL_MAX);
                int[] prioTable = GetMovePrioTable();

                int highest = AI_PRIO_MIN;
                int i;
                for (i = 0; i < (int)CheckDir.EnumMax; i++)
                {
                    if (highest > prioTable[i])
                    {
                        highest = prioTable[i];
                    }
                }

                PlayerInput pressInput = PlayerInput.Move_Left;
                if (highest == prioTable[(int)CheckDir.Left]) pressInput = PlayerInput.Move_Left;
                else if (highest == prioTable[(int)CheckDir.Right]) pressInput = PlayerInput.Move_Right;
                else if (highest == prioTable[(int)CheckDir.Up]) pressInput = PlayerInput.Move_Up;
                else if (highest == prioTable[(int)CheckDir.Down]) pressInput = PlayerInput.Move_Down;

                m_pressInput = pressInput;
            }
            m_playerInput[(int)m_pressInput] = true;
        }

        if(m_shootInterval < 0f)
        {
            if((Mathf.Abs(aiSubPosition.x) < 1f) || (Mathf.Abs(aiSubPosition.z) < 1f))
            {
                m_playerInput[(int)PlayerInput.Shoot] = true;
                m_shootInterval = SHOOT_INTERVAL;
            }
        }

    }

    //X座標の位置をグリッドに変換(float→int)
    private int GetGridX(float posX)
    {
        return Mathf.Clamp((int)(posX / Field.BLOCK_SCALE), 0, Field.FIELD_GRID_X - 1);
    }

    //Y座標の位置をグリッドに変換(float→int)
    private int GetGridY(float posZ)
    {
        return Mathf.Clamp((int)(posZ / Field.BLOCK_SCALE), 0, Field.FIELD_GRID_Y - 1);
    }

    //AIが移動するときの優先度算出
    private int[] GetMovePrioTable()
    {
        int i, j;

        //AIの位置算出
        Vector3 aiPosition = transform.position;
        int aiX = GetGridX(aiPosition.x);
        int aiY = GetGridY(aiPosition.z);

        //プレイヤーの位置算出
        GameObject mainObject = Player_Key.m_mainPlayer;
        Vector3 playerPosition = mainObject.transform.position;
        int playerX = GetGridX(playerPosition.x);
        int playerY = GetGridY(playerPosition.z);
        int playerGrid = playerX + playerY * Field.FIELD_GRID_X;

        //グリッドの各位置の優先度を格納する配列
        int[] calcGrid = new int[(Field.FIELD_GRID_X * Field.FIELD_GRID_Y)];
        for(i = 0; i < Field.FIELD_GRID_X * Field.FIELD_GRID_Y; i++)
        {
            calcGrid[i] = AI_PRIO_MIN;
        }

        //プレイヤーが現在いる場所に1を入れる
        calcGrid[playerGrid] = 1;

        int checkPrio = 1;     //チェックする優先度は1から
        int checkX;
        int checkY;
        int tempX;
        int tempY;
        int tempGrid;

        //チェックしたらtrue
        bool update;
        do
        {
            update = false;

            for (i = 0; i < (Field.FIELD_GRID_X * Field.FIELD_GRID_Y); i++)
            {
                //チェックする優先度でないなら無視
                if (checkPrio != calcGrid[i]) continue;

                //このグリッドがチェックする優先度の場所
                checkX = i % Field.FIELD_GRID_X;
                checkY = i / Field.FIELD_GRID_X;

                //上下左右のチェック
                for (j = 0; j < (int)CheckDir.EnumMax; j++)
                {
                    //調べる場所の隣
                    tempX = checkX + CHECK_DIR_LIST[j][(int)CheckData.X];
                    tempY = checkY + CHECK_DIR_LIST[j][(int)CheckData.Y];

                    //それが場外なら無視
                    if ((tempX < 0) || (tempX >= Field.FIELD_GRID_X) || (tempY < 0) || (tempY >= Field.FIELD_GRID_Y)) continue;

                    //ここを調べる
                    tempGrid = tempX + tempY * Field.FIELD_GRID_X;

                    //隣が壁なら無視
                    if (Field.ObjectKind.Block == (Field.ObjectKind)Field.GRID_OBJECT_DATA[tempGrid]) continue;

                    //この場所の優先度が大きければ更新
                    if (calcGrid[tempGrid] > (checkPrio + 1))
                    {
                        calcGrid[tempGrid] = checkPrio + 1;
                        update = true;
                    }
                }
            }
            //チェックする優先度を+1
            checkPrio++;
        } while (update);

        //優先度テーブル
        int[] prioTable = new int[(int)CheckDir.EnumMax];

        //AI周辺の優先度をテーブルに保存
        for(i = 0; i < (int)CheckDir.EnumMax; i++)
        {
            tempX = aiX + CHECK_DIR_LIST[i][(int)CheckData.X];
            tempY = aiY + CHECK_DIR_LIST[i][(int)CheckData.Y];

            //グリッド外なら優先度を最低にする
            if((tempX < 0) || (tempX >= Field.FIELD_GRID_X) || (tempY < 0) || (tempY >= Field.FIELD_GRID_Y))
            {
                prioTable[i] = AI_PRIO_MIN;
                continue;
            }

            tempGrid = tempX + tempY * Field.FIELD_GRID_X;
            prioTable[i] = calcGrid[tempGrid];
        }

        //優先度テーブルのデバッグ出力
        {
            //デバッグ用文字列
            string temp = "";

            //AI周辺の優先度を取得
            temp += "PRIO TABLE\n";
            for(tempY = 0; tempY < Field.FIELD_GRID_Y; tempY++)
            {
                for (tempX = 0; tempX < Field.FIELD_GRID_X; tempX++)
                {
                    temp += "\t\t" + calcGrid[tempX + ((Field.FIELD_GRID_Y - 1 - tempY) * Field.FIELD_GRID_X)] + "";

                    if ((tempX == aiX) && (Field.FIELD_GRID_Y - 1 - tempY == aiY)) temp += "*";
                }
                temp += "\n";
            }
            temp += "\n";

            //移動方向別の優先度情報
            temp += "RESULT\n";
            for(i = 0; i < (int)CheckDir.EnumMax; i++)
            {
                temp += "\t" + prioTable[i] + "\t" + (CheckDir)i + "\n";
            }

            //出力
            Debug.Log("" + temp);
        }

        //4方向の優先度情報を返す
        return prioTable;
    }
}
