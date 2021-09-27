using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//RequireComponent�ŃX�N���v�g�̃A�^�b�`�Ɠ����Ɏw�肵���R���|�[�l���g���ǉ������
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

    //�_���[�W���󂯂��Ƃ�
    void OnDamage()
    {
        PlayerVoice(damageVoice);

        //�����蔻���R���g���[���[���Ȃ����A��Ԃ��Q�[���I�[�o�[�ɂ���
        mCollider2D.enabled = false;
        mUnityChan2DController.enabled = false;
        Game.instance.state = Game.STATE.GAMEOVER;
    }

    //�W�����v����Ƃ�
    void Jump()
    {
        PlayerVoice(jumpVoice);
    }

    //�����Ɏw�肵���T�E���h���Đ�����
    void PlayerVoice(AudioClip clip)
    {
        mAudio.Stop();
        mAudio.PlayOneShot(clip);
    }
}
