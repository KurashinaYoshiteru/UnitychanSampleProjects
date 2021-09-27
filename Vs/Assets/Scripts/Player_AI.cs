using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AI : Player_Base
{
    //�`�F�b�N����
    private enum CheckDir
    {
        Left,
        Up,
        Right,
        Down,
        EnumMax
    }

    //�`�F�b�N���
    private enum CheckData
    {
        X,
        Y,
        EnumMax
    }

    //�`�F�b�N����
    private static readonly int[][] CHECK_DIR_LIST = new int[(int)CheckDir.EnumMax][]
    {
        new int[(int)CheckData.EnumMax]{-1,  0},
        new int[(int)CheckData.EnumMax]{ 0,  1},
        new int[(int)CheckData.EnumMax]{ 1,  0},
        new int[(int)CheckData.EnumMax]{ 0, -1}
    };

    private static readonly int AI_PRIO_MIN = 99;                 //AI�D��x�ōł��Ⴂ�l
    private static readonly float AI_INTERVAL_MIN = 0.5f;         //AI�v�l�Ԋu�̍ŒZ
    private static readonly float AI_INTERVAL_MAX = 0.8f;         //AI�v�l�Ԋu�̍Œ�
    private static readonly float AI_IGNORE_DISTANCE = 2.0f;      //�v���C���[�ւ̐ڋߌ��E
    private static readonly float SHOOT_INTERVAL = 1.0f;          //�ˌ��Ԋu

    private float m_aiInterval = 0f;                              //���݂�AI�̎v�l�Ԋu
    private float m_shootInterval = 0f;                           //���݂̎ˌ��̊Ԋu
    private PlayerInput m_pressInput = PlayerInput.Move_Left;     //���݂�AI�̓���

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
        //���[�U�[���������Ă���v���C���[���擾
        GameObject mainObject = Player_Key.m_mainPlayer;
        if (mainObject == null) return;

        //�v�l�X�V����
        m_aiInterval -= Time.deltaTime;
        m_shootInterval -= Time.deltaTime;

        //�v���C���[��AI�̋������Z�o
        Vector3 aiSubPosition = transform.position - mainObject.transform.position;
        aiSubPosition.y = 0f;

        //�������������瓮��
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

    //X���W�̈ʒu���O���b�h�ɕϊ�(float��int)
    private int GetGridX(float posX)
    {
        return Mathf.Clamp((int)(posX / Field.BLOCK_SCALE), 0, Field.FIELD_GRID_X - 1);
    }

    //Y���W�̈ʒu���O���b�h�ɕϊ�(float��int)
    private int GetGridY(float posZ)
    {
        return Mathf.Clamp((int)(posZ / Field.BLOCK_SCALE), 0, Field.FIELD_GRID_Y - 1);
    }

    //AI���ړ�����Ƃ��̗D��x�Z�o
    private int[] GetMovePrioTable()
    {
        int i, j;

        //AI�̈ʒu�Z�o
        Vector3 aiPosition = transform.position;
        int aiX = GetGridX(aiPosition.x);
        int aiY = GetGridY(aiPosition.z);

        //�v���C���[�̈ʒu�Z�o
        GameObject mainObject = Player_Key.m_mainPlayer;
        Vector3 playerPosition = mainObject.transform.position;
        int playerX = GetGridX(playerPosition.x);
        int playerY = GetGridY(playerPosition.z);
        int playerGrid = playerX + playerY * Field.FIELD_GRID_X;

        //�O���b�h�̊e�ʒu�̗D��x���i�[����z��
        int[] calcGrid = new int[(Field.FIELD_GRID_X * Field.FIELD_GRID_Y)];
        for(i = 0; i < Field.FIELD_GRID_X * Field.FIELD_GRID_Y; i++)
        {
            calcGrid[i] = AI_PRIO_MIN;
        }

        //�v���C���[�����݂���ꏊ��1������
        calcGrid[playerGrid] = 1;

        int checkPrio = 1;     //�`�F�b�N����D��x��1����
        int checkX;
        int checkY;
        int tempX;
        int tempY;
        int tempGrid;

        //�`�F�b�N������true
        bool update;
        do
        {
            update = false;

            for (i = 0; i < (Field.FIELD_GRID_X * Field.FIELD_GRID_Y); i++)
            {
                //�`�F�b�N����D��x�łȂ��Ȃ疳��
                if (checkPrio != calcGrid[i]) continue;

                //���̃O���b�h���`�F�b�N����D��x�̏ꏊ
                checkX = i % Field.FIELD_GRID_X;
                checkY = i / Field.FIELD_GRID_X;

                //�㉺���E�̃`�F�b�N
                for (j = 0; j < (int)CheckDir.EnumMax; j++)
                {
                    //���ׂ�ꏊ�̗�
                    tempX = checkX + CHECK_DIR_LIST[j][(int)CheckData.X];
                    tempY = checkY + CHECK_DIR_LIST[j][(int)CheckData.Y];

                    //���ꂪ��O�Ȃ疳��
                    if ((tempX < 0) || (tempX >= Field.FIELD_GRID_X) || (tempY < 0) || (tempY >= Field.FIELD_GRID_Y)) continue;

                    //�����𒲂ׂ�
                    tempGrid = tempX + tempY * Field.FIELD_GRID_X;

                    //�ׂ��ǂȂ疳��
                    if (Field.ObjectKind.Block == (Field.ObjectKind)Field.GRID_OBJECT_DATA[tempGrid]) continue;

                    //���̏ꏊ�̗D��x���傫����΍X�V
                    if (calcGrid[tempGrid] > (checkPrio + 1))
                    {
                        calcGrid[tempGrid] = checkPrio + 1;
                        update = true;
                    }
                }
            }
            //�`�F�b�N����D��x��+1
            checkPrio++;
        } while (update);

        //�D��x�e�[�u��
        int[] prioTable = new int[(int)CheckDir.EnumMax];

        //AI���ӂ̗D��x���e�[�u���ɕۑ�
        for(i = 0; i < (int)CheckDir.EnumMax; i++)
        {
            tempX = aiX + CHECK_DIR_LIST[i][(int)CheckData.X];
            tempY = aiY + CHECK_DIR_LIST[i][(int)CheckData.Y];

            //�O���b�h�O�Ȃ�D��x���Œ�ɂ���
            if((tempX < 0) || (tempX >= Field.FIELD_GRID_X) || (tempY < 0) || (tempY >= Field.FIELD_GRID_Y))
            {
                prioTable[i] = AI_PRIO_MIN;
                continue;
            }

            tempGrid = tempX + tempY * Field.FIELD_GRID_X;
            prioTable[i] = calcGrid[tempGrid];
        }

        //�D��x�e�[�u���̃f�o�b�O�o��
        {
            //�f�o�b�O�p������
            string temp = "";

            //AI���ӂ̗D��x���擾
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

            //�ړ������ʂ̗D��x���
            temp += "RESULT\n";
            for(i = 0; i < (int)CheckDir.EnumMax; i++)
            {
                temp += "\t" + prioTable[i] + "\t" + (CheckDir)i + "\n";
            }

            //�o��
            Debug.Log("" + temp);
        }

        //4�����̗D��x����Ԃ�
        return prioTable;
    }
}
