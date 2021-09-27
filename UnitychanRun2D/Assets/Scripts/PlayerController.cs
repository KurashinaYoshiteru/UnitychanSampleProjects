using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//RequireComponentでスクリプトのアタッチと同時に指定したコンポーネントも追加される
[RequireComponent(typeof(AudioSource))]

public class PlayerController : MonoBehaviour
{

    public AudioClip jumpVoice;
    public AudioClip damageVoice;

    private AudioSource mAudio;
    private UnityChan2DController mUnityChan2DController;
    private Collider2D mCollider2D;

    // Start is called before the first frame update
    void Start()
    {
        mAudio = GetComponent<AudioSource>();
        mUnityChan2DController = GetComponent<UnityChan2DController>();
        mCollider2D = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //ダメージを受けたとき
    void OnDamage()
    {
        PlayerVoice(damageVoice);

        //当たり判定やコントローラーをなくし、状態をゲームオーバーにする
        mCollider2D.enabled = false;
        mUnityChan2DController.enabled = false;
        Game.instance.state = Game.STATE.GAMEOVER;
    }

    //ジャンプするとき
    void Jump()
    {
        PlayerVoice(jumpVoice);
    }

    //引数に指定したサウンドを再生する
    void PlayerVoice(AudioClip clip)
    {
        mAudio.Stop();
        mAudio.PlayOneShot(clip);
    }
}
