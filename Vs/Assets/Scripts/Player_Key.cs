using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Key : Player_Base
{
    public static GameObject m_mainPlayer = null;        //ユーザーが動かしているプレイヤー

    //初期化で自分の操作オブジェクトをm_mainPlayerとする
    private void Awake()
    {
        m_mainPlayer = gameObject;
    }

    //キーコードをpublicでInspectorから取得、これをP1とP2で変えることで入力を差別化
    public KeyCode KEYCODE_MOVE_LEFT  = KeyCode.A;
    public KeyCode KEYCODE_MOVE_UP    = KeyCode.W;
    public KeyCode KEYCODE_MOVE_RIGHT = KeyCode.D;
    public KeyCode KEYCODE_MOVE_DOWN  = KeyCode.S;
    public KeyCode KEYCODE_SHOOT      = KeyCode.Space;

    //各キーコードの入力があった時にm_playerInputをtrueに
    protected override void GetInput()
    {
        if      (Input.GetKey(KEYCODE_MOVE_LEFT )) m_playerInput[(int)PlayerInput.Move_Left ] = true;
        else if (Input.GetKey(KEYCODE_MOVE_RIGHT)) m_playerInput[(int)PlayerInput.Move_Right] = true;
        else if (Input.GetKey(KEYCODE_MOVE_UP   )) m_playerInput[(int)PlayerInput.Move_Up   ] = true;
        else if (Input.GetKey(KEYCODE_MOVE_DOWN )) m_playerInput[(int)PlayerInput.Move_Down ] = true;
        
        if (Input.GetKeyDown(KEYCODE_SHOOT)) m_playerInput[(int)PlayerInput.Shoot] = true;
    }
}
