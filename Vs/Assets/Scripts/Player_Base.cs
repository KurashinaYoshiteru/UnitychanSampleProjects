using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Base : HitObject
{
    //プレイヤー操作の種類
    protected enum PlayerInput
    {
        Move_Left,
        Move_Up,
        Move_Right,
        Move_Down,
        Shoot,
        EnumMax
    }

    //上下左右の移動方向
    private static readonly float MOVE_ROTATION_Y_LEFT  = -90f;
    private static readonly float MOVE_ROTATION_Y_UP    =   0f;
    private static readonly float MOVE_ROTATION_Y_RIGHT =  90f;
    private static readonly float MOVE_ROTATION_Y_DOWN  = 180f;

    //移動速度
    public float MOVE_SPEED = 5.0f;

    public GameObject playerObject = null;           //動かすモデル(unitychan)
    public GameObject bulletObject = null;           //弾のプレハブ
    public GameObject hitEffectPrefab = null;        //ヒットエフェクト

    private float m_rotationY = 0.0f;                                         //プレイヤーの現在の回転角度
    protected bool[] m_playerInput = new bool[(int)PlayerInput.EnumMax];      //プレイヤーの入力
    protected bool m_playerDeadFlag = false;                                  //プレイヤー死亡フラグ

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_playerDeadFlag) return;       //死亡フラグがtrueなら全ての処理を無視

        ClearInput();                       //trueになっていた入力をfalseにリセット
        GetInput();                         //入力を取得
        CheckMove();                        //入力に応じた移動処理
    }

    //入力のリセット
    void ClearInput()
    {
        for(int i = 0; i < (int)PlayerInput.EnumMax; i++)
        {
            m_playerInput[i] = false;
        }
    }

    //入力のチェック
    protected virtual void GetInput()
    {
        //継承先でオーバーライド
    }

    //移動、射撃処理
    private void CheckMove()
    {
        Animator animator = playerObject.GetComponent<Animator>();    //アニメーター(メカニム)コンポーネント
        float moveSpeed = MOVE_SPEED;                                 //移動速度
        bool shootFlag = false;                                       //発射フラグ

        //移動と回転(PlayerInputで記述)
        {
            //移動の入力があればプレイヤーの向きを設定
            if      (m_playerInput[(int)PlayerInput.Move_Left ]) m_rotationY = MOVE_ROTATION_Y_LEFT;
            else if (m_playerInput[(int)PlayerInput.Move_Up   ]) m_rotationY = MOVE_ROTATION_Y_UP;
            else if (m_playerInput[(int)PlayerInput.Move_Right]) m_rotationY = MOVE_ROTATION_Y_RIGHT;
            else if (m_playerInput[(int)PlayerInput.Move_Down ]) m_rotationY = MOVE_ROTATION_Y_DOWN;

            //入力がなければ速度0とする
            else moveSpeed = 0;

            transform.rotation = Quaternion.Euler(0, m_rotationY, 0);                                    //向いている方向にRotateを変える
            transform.position += transform.rotation * Vector3.forward * moveSpeed * Time.deltaTime;     //毎フレームmoveSpeedの分移動させる
        }

        //射撃
        {
            //射撃ボタンの入力があれば弾を生成
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

        //メカニム
        {
            animator.SetFloat("Speed", moveSpeed);       //移動量
            animator.SetBool("Shoot", shootFlag);        //射撃フラグ
        }
    }

    //相手の弾に当たった時の処理
    private void OnTriggerEnter(Collider hitCollider)
    {
        //当たってOKなものでなければ処理終了
        if (IsHitOK(hitCollider.gameObject) == false) return;

        //弾に当たったことをメカニムに伝えアニメーションを再生させる
        {
            Animator animator = playerObject.GetComponent<Animator>();
            animator.SetBool("Dead", true);
        }

        //ヒットエフェクトがあれば生成
        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, transform.position, transform.rotation);
        }

        //死亡フラグをtrueに
        m_playerDeadFlag = true;
    }
}
