using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public GameObject m_blockObject = null;            //迷路を構成するブロックオブジェクト
    public GameObject m_playerObject = null;           //操作プレイヤーオブジェクト
    public GameObject m_goalObject = null;             //ゴールオブジェクト
    public GameObject m_targetObject = null;           //ターゲットオブジェクト
    public GameObject m_stageClearObject = null;       //ステージクリア時に表示されるオブジェクト

    public bool m_createAtOnce = true;                 //デバッグ用、falseは少しずつフィールド生成

    public enum StageClear
    {
        Goal,
        Target
    }

    public StageClear m_stageClear = StageClear.Goal;

    //チェック方向
    public enum CheckDir           //← ↑ → ↓ の順番
    {
        Left,         //左
        Up,           //上
        Right,        //右
        Down,         //下
        EnumMax,

        None = -1
    }

    //チェック情報
    private enum CheckData
    {
        X,           //X軸
        Y,           //Y軸
        EnumMax
    }

    //チェック方向
    private static readonly int[][] CHECK_DIR_LIST = new int[(int)CheckDir.EnumMax][]
    {   //                               X   Y
        new int[(int)CheckData.EnumMax]{-1,  0},
        new int[(int)CheckData.EnumMax]{ 0, -1},
        new int[(int)CheckData.EnumMax]{ 1,  0},
        new int[(int)CheckData.EnumMax]{ 0,  1}
    };

    //チェック方向の反対側
    private static readonly CheckDir[] REVERSE_DIR_LIST = new CheckDir[(int)CheckDir.EnumMax]
    {
        CheckDir.Right,
        CheckDir.Down,
        CheckDir.Left,
        CheckDir.Up
    };

    //チェックする順番
    private static readonly CheckDir[] CHECK_ORDER_LIST = new CheckDir[(int)CheckDir.EnumMax]
    {
        CheckDir.Up,
        CheckDir.Down,
        CheckDir.Left,
        CheckDir.Right
    };

    private static readonly int MAZE_LINE_X = 8;                                            //迷路のX方向通路数
    private static readonly int MAZE_LINE_Y = 8;                                            //迷路のY方向通路数
    private static readonly int MAZE_GRID_X = MAZE_LINE_X * 2 + 1;                          //迷路のX配列数
    private static readonly int MAZE_GRID_Y = MAZE_LINE_Y * 2 + 1;                          //迷路のY配列数
    private static readonly int EXEC_MAZE_COUNT_MAX = MAZE_LINE_X * MAZE_LINE_Y / 2;        //ブロックを１つずつ生成する際の試行回数
    private static readonly float MAZE_BLOCK_SCALE = 2.0f;                                  //迷路に使うブロック１つの大きさ
    private static readonly int TARGET_NUM = 5;                                             //ターゲット数

    private bool[][] m_mazeGrid = null;                  //迷路の配列、trueはブロックで埋める
    private GameObject m_blockParent = null;             //迷路に使うブロックの親
    private int m_makeMazeCounter = 0;                       //ブロックを１つずつ生成する際のカウンタ
    private bool m_stageClearedFlag = false;          //ステージクリアに必要なオブジェクト(Target/Goal)を生成したらtrue

    private void Awake()
    {
        //迷路の初期化
        InitializeMaze();

        //一気に迷路を作り場合の処理
        if (m_createAtOnce)
        {
            for (int i = 0; i < EXEC_MAZE_COUNT_MAX; i++)
            {
                ExecMaze();
            }
            CreateMaze();
        }

        //プレイヤー生成
        CreatePlayer();

        //クリア条件生成
        switch (m_stageClear)
        {
            case StageClear.Goal:
                CreateGoal();
                break;
            case StageClear.Target:
                CreateTarget();
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //一気に迷路を作らない場合の処理
        if (!m_createAtOnce)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //一手ずつ迷路を生成
                ExecMaze();
                //ブロック生成
                CreateMaze();
            }
        }

        //ステージクリア処理
        if (!m_stageClearedFlag)
        {
            if (Game.IsStageCleared())
            {
                CreateStageClear();
                m_stageClearedFlag = true;
            }
        }
    }

    void InitializeMaze()
    {
        //迷路配列の宣言
        m_mazeGrid = new bool[MAZE_GRID_X][];
        for (int gridX = 0; gridX < MAZE_GRID_X; gridX++)
        {
            m_mazeGrid[gridX] = new bool[MAZE_GRID_Y];
        }

        //迷路配列の初期化、最初からブロックで埋めるところ(外郭と柱)をtrueでそれ以外はfalse
        bool blockFlag;
        for(int gridX = 0; gridX < MAZE_GRID_X; gridX++)
        {
            for (int gridY = 0; gridY < MAZE_GRID_Y; gridY++)
            {
                blockFlag = false;
                if((gridX == 0) || (gridY == 0) || (MAZE_GRID_X - 1 == gridX) || (MAZE_GRID_Y - 1 == gridY))
                {
                    blockFlag = true;
                }
                else if((gridX % 2 == 0) && (gridY % 2 == 0))
                {
                    blockFlag = true;
                }
                m_mazeGrid[gridX][gridY] = blockFlag;
            }
        }
    }

    private void ExecMaze()
    {
        //迷路の作成が完了していたら処理終了
        if (m_makeMazeCounter >= EXEC_MAZE_COUNT_MAX) return;

        int counter = m_makeMazeCounter;        //チェック開始番号
        m_makeMazeCounter++;

        int lineMax = Mathf.Max(MAZE_LINE_X, MAZE_LINE_Y);       //XとYの通路数のうち大きい方
        int start1 = counter / lineMax * 2;                      //チェック開始位置
        int start2 = counter % lineMax * 2;                      //ココヨクワカラナイ
        int gridX_A = 0;
        int gridY_A = 0;
        int gridX_B;
        int gridY_B;
        int gridX_C;
        int gridY_C;

        CheckDir checkDirNow;       //チェックする方向
        CheckDir checkDirNG;        //１つ前のチェック方向

        //上下左右の端から１本ずつ枝を伸ばし、壁を生成する
        for(int i = 0; i < (int)CheckDir.EnumMax; i++)
        {
            checkDirNow = CHECK_ORDER_LIST[i];                //今チェックする方向

            //ココヨクワカラナイ
            switch (checkDirNow)
            {
                case CheckDir.Left:
                    gridX_A = MAZE_GRID_X - 1 - start1;
                    gridY_A = MAZE_GRID_Y - 1 - start2;
                    break;
                case CheckDir.Up:
                    gridX_A = MAZE_GRID_X - 1 - start2;
                    gridY_A = MAZE_GRID_Y - 1 - start1;
                    break;
                case CheckDir.Right:
                    gridX_A = start1;
                    gridY_A = start2;
                    break;
                case CheckDir.Down:
                    gridX_A = start2;
                    gridY_A = start1;
                    break;
                default:
                    Debug.LogError("存在しない方向(" + checkDirNow + ")");
                    gridX_A = -1;
                    gridX_A = -1;
                    break;
            }

            //場外チェック
            if ((gridX_A < 0) || (gridX_A >= MAZE_GRID_X) || (gridY_A < 0) || (gridY_A >= MAZE_GRID_Y)) continue;
            
            //壁がある柱にぶつかるまでループ
            for(; ; )
            {
                //チェックする柱(開始位置から２つ隣のブロック)
                gridX_B = gridX_A + CHECK_DIR_LIST[(int)checkDirNow][(int)CheckData.X] * 2;
                gridY_B = gridY_A + CHECK_DIR_LIST[(int)checkDirNow][(int)CheckData.Y] * 2;

                //任意のブロックの周囲を調べ、他のブロックとつながっていないかチェック
                if (IsConnectedBlock(gridX_B, gridY_B))
                {
                    break;
                }

                //開始位置とチェック位置の間にブロックを置く
                gridX_C = gridX_A + CHECK_DIR_LIST[(int)checkDirNow][(int)CheckData.X];
                gridY_C = gridY_A + CHECK_DIR_LIST[(int)checkDirNow][(int)CheckData.Y];

                //ブロックを配置
                SetBlock(gridX_C, gridY_C, true);

                //次はつないだ柱から検索
                gridX_A = gridX_B;
                gridY_A = gridY_B;

                //戻ってはいけない方向をセット
                checkDirNG = REVERSE_DIR_LIST[(int)checkDirNow];

                //次に調べる柱をランダムで決定
                checkDirNow = CHECK_ORDER_LIST[Random.Range(0, (int)CheckDir.EnumMax)];

                //戻ろうとしたら反対側にする
                if(checkDirNow == checkDirNG)
                {
                    checkDirNow = REVERSE_DIR_LIST[(int)checkDirNow];
                }
            }
        }
    }

    //指定した場所にブロックを置く
    private void SetBlock(int gridX, int gridY, bool blockFlag)
    {
        m_mazeGrid[gridX][gridY] = blockFlag;
    }

    //指定した場所にブロックがあるかチェックする
    private bool IsBlock(int gridX, int gridY)
    {
        return m_mazeGrid[gridX][gridY];
    }

    //指定したブロックの上下左右にブロックがあるかチェックする
    private bool IsConnectedBlock(int gridX, int gridY)
    {
        bool connectedFlag = false;                 //周りにあればtrue

        int checkX;
        int checkY;

        for (int i = 0; i < (int)CheckDir.EnumMax; i++)
        {
            //チェックするブロックの位置
            checkX = gridX + CHECK_DIR_LIST[i][(int)CheckData.X];
            checkY = gridY + CHECK_DIR_LIST[i][(int)CheckData.Y];

            //場外チェック
            if ((checkX < 0) || (checkX >= MAZE_GRID_X) || (checkY < 0) || (checkY >= MAZE_GRID_Y)) continue;

            //既にブロックがあればループ終了
            if(IsBlock(checkX, checkY))
            {
                connectedFlag = true;
                break;
            }
        }

        return connectedFlag;
    }

    //迷路生成
    private void CreateMaze()
    {
        //前にブロックがいたら削除
        if (m_blockParent)
        {
            Destroy(m_blockParent);
            m_blockParent = null;
        }

        //ブロックの親を設定
        m_blockParent = new GameObject();
        m_blockParent.name = "BlockParent";
        m_blockParent.transform.parent = transform;

        GameObject blockObject;       //生成後のブロックオブジェクト
        Vector3 position;             //ブロック生成位置

        //[X, Y]がtrueの場所にブロック生成
        for(int gridX = 0; gridX < MAZE_GRID_X; gridX++)
        {
            for (int gridY = 0; gridY < MAZE_GRID_Y; gridY++)
            {
                if(IsBlock(gridX, gridY))
                {
                    position = new Vector3(gridX, 0, gridY) * MAZE_BLOCK_SCALE;
                    blockObject = Instantiate(m_blockObject, position, Quaternion.identity) as GameObject;
                    blockObject.name = "Block(" + gridX + ", " + gridY + ")";
                    blockObject.transform.localScale = Vector3.one * MAZE_BLOCK_SCALE;
                    blockObject.transform.parent = m_blockParent.transform;
                }
            }
        }
    }

    //プレイヤー生成
    private void CreatePlayer()
    {
        Instantiate(m_playerObject, new Vector3(1, 0, 1) * MAZE_BLOCK_SCALE, Quaternion.identity);
    }

    //ゴール生成
    private void CreateGoal()
    {
        Vector3 position = new Vector3(MAZE_GRID_X - 2, 0, MAZE_GRID_Y - 2) * MAZE_BLOCK_SCALE;
        Instantiate(m_goalObject, position, Quaternion.identity);
    }

    //ターゲット生成
    private void CreateTarget()
    {
        Vector3 position;
        for(int i = 0; i < TARGET_NUM; i++)
        {
            position = new Vector3(Random.Range(0, MAZE_LINE_X * 2) + 1, 0, Random.Range(0, MAZE_LINE_Y * 2) + 1) * MAZE_BLOCK_SCALE;
            Instantiate(m_targetObject, position, Quaternion.identity);  
        }
    }

    //ステージクリア表示生成
    private void CreateStageClear()
    {
        Instantiate(m_stageClearObject, Vector3.zero, Quaternion.identity);
    }
}
