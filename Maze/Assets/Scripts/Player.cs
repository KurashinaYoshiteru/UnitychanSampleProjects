using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject playerObject = null;                         //プレイヤーモデルオブジェクト
    public GameObject bulletObject = null;                         //弾オブジェクト
    public Transform bulletStartPosition = null;                   //弾の発射位置

    private static readonly float MOVE_Z_FRONT = 5.0f;             //前進移動量
    private static readonly float MOVE_Z_BACK = -2.0f;             //後退移動量
    private static readonly float ROTATION_Y_KEY = 360.0f;         //キー操作による回転量
    private static readonly float ROTATION_Y_MOUSE = 720.0f;       //マウス操作による回転量

    private float m_rotationY = 0.0f;                               //プレイヤーオブジェクトの現在の角度
    private bool m_mouseLockFlag = true;                            //マウスを固定するフラグ



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
        //回転
        float addRotationY = 0.0f;    //1フレームの回転量
        {
            //キー入力による回転量算出
            if (Input.GetKey(KeyCode.Q))
            {
                addRotationY = -ROTATION_Y_KEY;
            }
            else if (Input.GetKey(KeyCode.E))
            {
                addRotationY = ROTATION_Y_KEY;
            }

            //マウス入力による回転量算出
            if (m_mouseLockFlag)
            {
                addRotationY += Input.GetAxis("Mouse X") * ROTATION_Y_MOUSE;
            }

            //算出した回転量を現在の角度に加算
            m_rotationY += addRotationY * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, m_rotationY, 0);
        }

        //移動      
        Vector3 addPosition = Vector3.zero;     //1フレームの移動量
        { 
            //キー入力による移動量算出
            Vector3 vecInput = new Vector3(0, 0, Input.GetAxisRaw("Vertical"));
            if(vecInput.z > 0)
            {
                addPosition.z = MOVE_Z_FRONT;
            }
            else if(vecInput.z < 0)
            {
                addPosition.z = MOVE_Z_BACK;
            }

            //算出した移動量を現在の位置に加算
            transform.position += ((transform.rotation * addPosition) * Time.deltaTime);     //「transform.rotation * (移動量(Vector3))」で+Zの分だけ正面に進む
        }

        //射撃
        bool shootFlag;    //弾発射時にtrue、アニメーションで使用
        {
            if (Input.GetButtonDown("Fire1"))
            {
                shootFlag = true;
                if (bulletStartPosition != null)
                {
                    //弾の生成位置算出
                    Vector3 vecBulletPos = bulletStartPosition.position;
                    vecBulletPos += transform.rotation * Vector3.forward;
                    vecBulletPos.y = 1.0f;

                    //弾の生成
                    Instantiate(bulletObject, vecBulletPos, transform.rotation);
                }
            }
            else
            {
                shootFlag = false;
            }
        }

        //アニメーション
        {
            Animator animator = playerObject.GetComponent<Animator>();

            animator.SetFloat("SpeedZ", addPosition.z);    //移動量
            animator.SetBool("Shoot", shootFlag);          //射撃フラグ
        }
    }
}
