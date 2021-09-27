using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public GameObject m_blockObject = null;                                  //�u���b�N�I�u�W�F�N�g
    public GameObject m_player1Object = null;                                //�v���C���[1�I�u�W�F�N�g
    public GameObject m_player2Object = null;                                //�v���C���[2�I�u�W�F�N�g

    public static readonly int FIELD_GRID_X = 9;                             //X�O���b�h��
    public static readonly int FIELD_GRID_Y = 9;                             //Y�O���b�h��
    public static readonly float BLOCK_SCALE = 2.0f;                         //�u���b�N�̃X�P�[��
    public static readonly Vector3 BLOCK_OFFSET = new Vector3(1, 1, 1);      //�u���b�N�z�u�̃I�t�Z�b�g

    //�}�b�v��z�u���̎��
    public enum ObjectKind
    {
        Empty,         //0
        Block,         //1
        Player1,       //2
        Player2        //3
    }

    //�z�u�f�[�^
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

    private GameObject m_blockParent = null;                   //���������u���b�N�̐e

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

    //�t�B�[���h�̏�����
    private void InitializeField()
    {
        //�u���b�N�̐e�̏�����
        m_blockParent = new GameObject();
        m_blockParent.name = "BlockParent";
        m_blockParent.transform.parent = transform;

        GameObject originalObject = null;           //��������u���b�N�̌��ƂȂ�I�u�W�F�N�g
        GameObject instanceObject;           //�u���b�N���Ƃ肠���������ϐ�
        Vector3 postion;                     //�����ʒu

        int gridX;
        int gridY;
        for(gridX = 0; gridX < FIELD_GRID_X; gridX++)
        {
            for (gridY = 0; gridY < FIELD_GRID_Y; gridY++)
            {
                //�e�ʒu�ɉ���u�����AoriginalOnject�ɉ�������
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

                //�u���b�N����
                postion = new Vector3(gridX * BLOCK_SCALE, 0, gridY * BLOCK_SCALE) + BLOCK_OFFSET;
                instanceObject = Instantiate(originalObject, postion, originalObject.transform.rotation) as GameObject;
                instanceObject.name = "" + originalObject.name + "(" + gridX + "," + gridY + ")";
                instanceObject.transform.localScale = Vector3.one * BLOCK_SCALE;
                instanceObject.transform.parent = m_blockParent.transform;
            }
        }
    }
}
