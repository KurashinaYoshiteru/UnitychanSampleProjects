using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public GameObject m_blockObject = null;            //���H���\������u���b�N�I�u�W�F�N�g
    public GameObject m_playerObject = null;           //����v���C���[�I�u�W�F�N�g
    public GameObject m_goalObject = null;             //�S�[���I�u�W�F�N�g
    public GameObject m_targetObject = null;           //�^�[�Q�b�g�I�u�W�F�N�g
    public GameObject m_stageClearObject = null;       //�X�e�[�W�N���A���ɕ\�������I�u�W�F�N�g

    public bool m_createAtOnce = true;                 //�f�o�b�O�p�Afalse�͏������t�B�[���h����

    public enum StageClear
    {
        Goal,
        Target
    }

    public StageClear m_stageClear = StageClear.Goal;

    //�`�F�b�N����
    public enum CheckDir           //�� �� �� �� �̏���
    {
        Left,         //��
        Up,           //��
        Right,        //�E
        Down,         //��
        EnumMax,

        None = -1
    }

    //�`�F�b�N���
    private enum CheckData
    {
        X,           //X��
        Y,           //Y��
        EnumMax
    }

    //�`�F�b�N����
    private static readonly int[][] CHECK_DIR_LIST = new int[(int)CheckDir.EnumMax][]
    {   //                               X   Y
        new int[(int)CheckData.EnumMax]{-1,  0},
        new int[(int)CheckData.EnumMax]{ 0, -1},
        new int[(int)CheckData.EnumMax]{ 1,  0},
        new int[(int)CheckData.EnumMax]{ 0,  1}
    };

    //�`�F�b�N�����̔��Α�
    private static readonly CheckDir[] REVERSE_DIR_LIST = new CheckDir[(int)CheckDir.EnumMax]
    {
        CheckDir.Right,
        CheckDir.Down,
        CheckDir.Left,
        CheckDir.Up
    };

    //�`�F�b�N���鏇��
    private static readonly CheckDir[] CHECK_ORDER_LIST = new CheckDir[(int)CheckDir.EnumMax]
    {
        CheckDir.Up,
        CheckDir.Down,
        CheckDir.Left,
        CheckDir.Right
    };

    private static readonly int MAZE_LINE_X = 8;                                            //���H��X�����ʘH��
    private static readonly int MAZE_LINE_Y = 8;                                            //���H��Y�����ʘH��
    private static readonly int MAZE_GRID_X = MAZE_LINE_X * 2 + 1;                          //���H��X�z��
    private static readonly int MAZE_GRID_Y = MAZE_LINE_Y * 2 + 1;                          //���H��Y�z��
    private static readonly int EXEC_MAZE_COUNT_MAX = MAZE_LINE_X * MAZE_LINE_Y / 2;        //�u���b�N���P����������ۂ̎��s��
    private static readonly float MAZE_BLOCK_SCALE = 2.0f;                                  //���H�Ɏg���u���b�N�P�̑傫��
    private static readonly int TARGET_NUM = 5;                                             //�^�[�Q�b�g��

    private bool[][] m_mazeGrid = null;                  //���H�̔z��Atrue�̓u���b�N�Ŗ��߂�
    private GameObject m_blockParent = null;             //���H�Ɏg���u���b�N�̐e
    private int m_makeMazeCounter = 0;                       //�u���b�N���P����������ۂ̃J�E���^
    private bool m_stageClearedFlag = false;          //�X�e�[�W�N���A�ɕK�v�ȃI�u�W�F�N�g(Target/Goal)�𐶐�������true

    private void Awake()
    {
        //���H�̏�����
        InitializeMaze();

        //��C�ɖ��H�����ꍇ�̏���
        if (m_createAtOnce)
        {
            for (int i = 0; i < EXEC_MAZE_COUNT_MAX; i++)
            {
                ExecMaze();
            }
            CreateMaze();
        }

        //�v���C���[����
        CreatePlayer();

        //�N���A��������
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
        //��C�ɖ��H�����Ȃ��ꍇ�̏���
        if (!m_createAtOnce)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //��肸���H�𐶐�
                ExecMaze();
                //�u���b�N����
                CreateMaze();
            }
        }

        //�X�e�[�W�N���A����
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
        //���H�z��̐錾
        m_mazeGrid = new bool[MAZE_GRID_X][];
        for (int gridX = 0; gridX < MAZE_GRID_X; gridX++)
        {
            m_mazeGrid[gridX] = new bool[MAZE_GRID_Y];
        }

        //���H�z��̏������A�ŏ�����u���b�N�Ŗ��߂�Ƃ���(�O�s�ƒ�)��true�ł���ȊO��false
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
        //���H�̍쐬���������Ă����珈���I��
        if (m_makeMazeCounter >= EXEC_MAZE_COUNT_MAX) return;

        int counter = m_makeMazeCounter;        //�`�F�b�N�J�n�ԍ�
        m_makeMazeCounter++;

        int lineMax = Mathf.Max(MAZE_LINE_X, MAZE_LINE_Y);       //X��Y�̒ʘH���̂����傫����
        int start1 = counter / lineMax * 2;                      //�`�F�b�N�J�n�ʒu
        int start2 = counter % lineMax * 2;                      //�R�R���N���J���i�C
        int gridX_A = 0;
        int gridY_A = 0;
        int gridX_B;
        int gridY_B;
        int gridX_C;
        int gridY_C;

        CheckDir checkDirNow;       //�`�F�b�N�������
        CheckDir checkDirNG;        //�P�O�̃`�F�b�N����

        //�㉺���E�̒[����P�{���}��L�΂��A�ǂ𐶐�����
        for(int i = 0; i < (int)CheckDir.EnumMax; i++)
        {
            checkDirNow = CHECK_ORDER_LIST[i];                //���`�F�b�N�������

            //�R�R���N���J���i�C
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
                    Debug.LogError("���݂��Ȃ�����(" + checkDirNow + ")");
                    gridX_A = -1;
                    gridX_A = -1;
                    break;
            }

            //��O�`�F�b�N
            if ((gridX_A < 0) || (gridX_A >= MAZE_GRID_X) || (gridY_A < 0) || (gridY_A >= MAZE_GRID_Y)) continue;
            
            //�ǂ����钌�ɂԂ���܂Ń��[�v
            for(; ; )
            {
                //�`�F�b�N���钌(�J�n�ʒu����Q�ׂ̃u���b�N)
                gridX_B = gridX_A + CHECK_DIR_LIST[(int)checkDirNow][(int)CheckData.X] * 2;
                gridY_B = gridY_A + CHECK_DIR_LIST[(int)checkDirNow][(int)CheckData.Y] * 2;

                //�C�ӂ̃u���b�N�̎��͂𒲂ׁA���̃u���b�N�ƂȂ����Ă��Ȃ����`�F�b�N
                if (IsConnectedBlock(gridX_B, gridY_B))
                {
                    break;
                }

                //�J�n�ʒu�ƃ`�F�b�N�ʒu�̊ԂɃu���b�N��u��
                gridX_C = gridX_A + CHECK_DIR_LIST[(int)checkDirNow][(int)CheckData.X];
                gridY_C = gridY_A + CHECK_DIR_LIST[(int)checkDirNow][(int)CheckData.Y];

                //�u���b�N��z�u
                SetBlock(gridX_C, gridY_C, true);

                //���͂Ȃ��������猟��
                gridX_A = gridX_B;
                gridY_A = gridY_B;

                //�߂��Ă͂����Ȃ��������Z�b�g
                checkDirNG = REVERSE_DIR_LIST[(int)checkDirNow];

                //���ɒ��ׂ钌�������_���Ō���
                checkDirNow = CHECK_ORDER_LIST[Random.Range(0, (int)CheckDir.EnumMax)];

                //�߂낤�Ƃ����甽�Α��ɂ���
                if(checkDirNow == checkDirNG)
                {
                    checkDirNow = REVERSE_DIR_LIST[(int)checkDirNow];
                }
            }
        }
    }

    //�w�肵���ꏊ�Ƀu���b�N��u��
    private void SetBlock(int gridX, int gridY, bool blockFlag)
    {
        m_mazeGrid[gridX][gridY] = blockFlag;
    }

    //�w�肵���ꏊ�Ƀu���b�N�����邩�`�F�b�N����
    private bool IsBlock(int gridX, int gridY)
    {
        return m_mazeGrid[gridX][gridY];
    }

    //�w�肵���u���b�N�̏㉺���E�Ƀu���b�N�����邩�`�F�b�N����
    private bool IsConnectedBlock(int gridX, int gridY)
    {
        bool connectedFlag = false;                 //����ɂ����true

        int checkX;
        int checkY;

        for (int i = 0; i < (int)CheckDir.EnumMax; i++)
        {
            //�`�F�b�N����u���b�N�̈ʒu
            checkX = gridX + CHECK_DIR_LIST[i][(int)CheckData.X];
            checkY = gridY + CHECK_DIR_LIST[i][(int)CheckData.Y];

            //��O�`�F�b�N
            if ((checkX < 0) || (checkX >= MAZE_GRID_X) || (checkY < 0) || (checkY >= MAZE_GRID_Y)) continue;

            //���Ƀu���b�N������΃��[�v�I��
            if(IsBlock(checkX, checkY))
            {
                connectedFlag = true;
                break;
            }
        }

        return connectedFlag;
    }

    //���H����
    private void CreateMaze()
    {
        //�O�Ƀu���b�N��������폜
        if (m_blockParent)
        {
            Destroy(m_blockParent);
            m_blockParent = null;
        }

        //�u���b�N�̐e��ݒ�
        m_blockParent = new GameObject();
        m_blockParent.name = "BlockParent";
        m_blockParent.transform.parent = transform;

        GameObject blockObject;       //������̃u���b�N�I�u�W�F�N�g
        Vector3 position;             //�u���b�N�����ʒu

        //[X, Y]��true�̏ꏊ�Ƀu���b�N����
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

    //�v���C���[����
    private void CreatePlayer()
    {
        Instantiate(m_playerObject, new Vector3(1, 0, 1) * MAZE_BLOCK_SCALE, Quaternion.identity);
    }

    //�S�[������
    private void CreateGoal()
    {
        Vector3 position = new Vector3(MAZE_GRID_X - 2, 0, MAZE_GRID_Y - 2) * MAZE_BLOCK_SCALE;
        Instantiate(m_goalObject, position, Quaternion.identity);
    }

    //�^�[�Q�b�g����
    private void CreateTarget()
    {
        Vector3 position;
        for(int i = 0; i < TARGET_NUM; i++)
        {
            position = new Vector3(Random.Range(0, MAZE_LINE_X * 2) + 1, 0, Random.Range(0, MAZE_LINE_Y * 2) + 1) * MAZE_BLOCK_SCALE;
            Instantiate(m_targetObject, position, Quaternion.identity);  
        }
    }

    //�X�e�[�W�N���A�\������
    private void CreateStageClear()
    {
        Instantiate(m_stageClearObject, Vector3.zero, Quaternion.identity);
    }
}
