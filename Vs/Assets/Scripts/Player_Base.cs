using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Base : HitObject
{
    //�v���C���[����̎��
    protected enum PlayerInput
    {
        Move_Left,
        Move_Up,
        Move_Right,
        Move_Down,
        Shoot,
        EnumMax
    }

    //�㉺���E�̈ړ�����
    private static readonly float MOVE_ROTATION_Y_LEFT  = -90f;
    private static readonly float MOVE_ROTATION_Y_UP    =   0f;
    private static readonly float MOVE_ROTATION_Y_RIGHT =  90f;
    private static readonly float MOVE_ROTATION_Y_DOWN  = 180f;

    //�ړ����x
    public float MOVE_SPEED = 5.0f;

    public GameObject playerObject = null;           //���������f��(unitychan)
    public GameObject bulletObject = null;           //�e�̃v���n�u
    public GameObject hitEffectPrefab = null;        //�q�b�g�G�t�F�N�g

    private float m_rotationY = 0.0f;                                         //�v���C���[�̌��݂̉�]�p�x
    protected bool[] m_playerInput = new bool[(int)PlayerInput.EnumMax];      //�v���C���[�̓���
    protected bool m_playerDeadFlag = false;                                  //�v���C���[���S�t���O

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_playerDeadFlag) return;       //���S�t���O��true�Ȃ�S�Ă̏����𖳎�

        ClearInput();                       //true�ɂȂ��Ă������͂�false�Ƀ��Z�b�g
        GetInput();                         //���͂��擾
        CheckMove();                        //���͂ɉ������ړ�����
    }

    //���͂̃��Z�b�g
    void ClearInput()
    {
        for(int i = 0; i < (int)PlayerInput.EnumMax; i++)
        {
            m_playerInput[i] = false;
        }
    }

    //���͂̃`�F�b�N
    protected virtual void GetInput()
    {
        //�p����ŃI�[�o�[���C�h
    }

    //�ړ��A�ˌ�����
    private void CheckMove()
    {
        Animator animator = playerObject.GetComponent<Animator>();    //�A�j���[�^�[(���J�j��)�R���|�[�l���g
        float moveSpeed = MOVE_SPEED;                                 //�ړ����x
        bool shootFlag = false;                                       //���˃t���O

        //�ړ��Ɖ�](PlayerInput�ŋL�q)
        {
            //�ړ��̓��͂�����΃v���C���[�̌�����ݒ�
            if      (m_playerInput[(int)PlayerInput.Move_Left ]) m_rotationY = MOVE_ROTATION_Y_LEFT;
            else if (m_playerInput[(int)PlayerInput.Move_Up   ]) m_rotationY = MOVE_ROTATION_Y_UP;
            else if (m_playerInput[(int)PlayerInput.Move_Right]) m_rotationY = MOVE_ROTATION_Y_RIGHT;
            else if (m_playerInput[(int)PlayerInput.Move_Down ]) m_rotationY = MOVE_ROTATION_Y_DOWN;

            //���͂��Ȃ���Α��x0�Ƃ���
            else moveSpeed = 0;

            transform.rotation = Quaternion.Euler(0, m_rotationY, 0);                                    //�����Ă��������Rotate��ς���
            transform.position += transform.rotation * Vector3.forward * moveSpeed * Time.deltaTime;     //���t���[��moveSpeed�̕��ړ�������
        }

        //�ˌ�
        {
            //�ˌ��{�^���̓��͂�����Βe�𐶐�
            if (m_playerInput[(int)PlayerInput.Shoot])
            {
                shootFlag = true;
                Vector3 vecBulletPos = transform.position;
                vecBulletPos += transform.rotation * Vector3.forward;
                vecBulletPos.y = 1.0f;
                Instantiate(bulletObject, vecBulletPos, transform.rotation);
            }
            else
            {
                shootFlag = false;
            }
        }

        //���J�j��
        {
            animator.SetFloat("Speed", moveSpeed);       //�ړ���
            animator.SetBool("Shoot", shootFlag);        //�ˌ��t���O
        }
    }

    //����̒e�ɓ����������̏���
    private void OnTriggerEnter(Collider hitCollider)
    {
        //��������OK�Ȃ��̂łȂ���Ώ����I��
        if (IsHitOK(hitCollider.gameObject) == false) return;

        //�e�ɓ����������Ƃ����J�j���ɓ`���A�j���[�V�������Đ�������
        {
            Animator animator = playerObject.GetComponent<Animator>();
            animator.SetBool("Dead", true);
        }

        //�q�b�g�G�t�F�N�g������ΐ���
        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, transform.position, transform.rotation);
        }

        //���S�t���O��true��
        m_playerDeadFlag = true;
    }
}
