using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject playerObject = null;                         //�v���C���[���f���I�u�W�F�N�g
    public GameObject bulletObject = null;                         //�e�I�u�W�F�N�g
    public Transform bulletStartPosition = null;                   //�e�̔��ˈʒu

    private static readonly float MOVE_Z_FRONT = 5.0f;             //�O�i�ړ���
    private static readonly float MOVE_Z_BACK = -2.0f;             //��ވړ���
    private static readonly float ROTATION_Y_KEY = 360.0f;         //�L�[����ɂ���]��
    private static readonly float ROTATION_Y_MOUSE = 720.0f;       //�}�E�X����ɂ���]��

    private float m_rotationY = 0.0f;                               //�v���C���[�I�u�W�F�N�g�̌��݂̊p�x
    private bool m_mouseLockFlag = true;                            //�}�E�X���Œ肷��t���O



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckMove();
    }

    private void CheckMove()
    {
        //��]
        float addRotationY = 0.0f;    //1�t���[���̉�]��
        {
            //�L�[���͂ɂ���]�ʎZ�o
            if (Input.GetKey(KeyCode.Q))
            {
                addRotationY = -ROTATION_Y_KEY;
            }
            else if (Input.GetKey(KeyCode.E))
            {
                addRotationY = ROTATION_Y_KEY;
            }

            //�}�E�X���͂ɂ���]�ʎZ�o
            if (m_mouseLockFlag)
            {
                addRotationY += Input.GetAxis("Mouse X") * ROTATION_Y_MOUSE;
            }

            //�Z�o������]�ʂ����݂̊p�x�ɉ��Z
            m_rotationY += addRotationY * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, m_rotationY, 0);
        }

        //�ړ�      
        Vector3 addPosition = Vector3.zero;     //1�t���[���̈ړ���
        { 
            //�L�[���͂ɂ��ړ��ʎZ�o
            Vector3 vecInput = new Vector3(0, 0, Input.GetAxisRaw("Vertical"));
            if(vecInput.z > 0)
            {
                addPosition.z = MOVE_Z_FRONT;
            }
            else if(vecInput.z < 0)
            {
                addPosition.z = MOVE_Z_BACK;
            }

            //�Z�o�����ړ��ʂ����݂̈ʒu�ɉ��Z
            transform.position += ((transform.rotation * addPosition) * Time.deltaTime);     //�utransform.rotation * (�ړ���(Vector3))�v��+Z�̕��������ʂɐi��
        }

        //�ˌ�
        bool shootFlag;    //�e���ˎ���true�A�A�j���[�V�����Ŏg�p
        {
            if (Input.GetButtonDown("Fire1"))
            {
                shootFlag = true;
                if (bulletStartPosition != null)
                {
                    //�e�̐����ʒu�Z�o
                    Vector3 vecBulletPos = bulletStartPosition.position;
                    vecBulletPos += transform.rotation * Vector3.forward;
                    vecBulletPos.y = 1.0f;

                    //�e�̐���
                    Instantiate(bulletObject, vecBulletPos, transform.rotation);
                }
            }
            else
            {
                shootFlag = false;
            }
        }

        //�A�j���[�V����
        {
            Animator animator = playerObject.GetComponent<Animator>();

            animator.SetFloat("SpeedZ", addPosition.z);    //�ړ���
            animator.SetBool("Shoot", shootFlag);          //�ˌ��t���O
        }
    }
}
