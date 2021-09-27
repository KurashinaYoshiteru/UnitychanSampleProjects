using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creator : MonoBehaviour
{
    public GameObject prefab;                        //��������v���n�u
    public Vector3 randomPosRange = Vector3.up;      //��������ʒu���痣�ꂽ�����_���Ȉʒu
    public Vector3 velocity = Vector3.left;          //�������̑���
    public float offsetTime = 0f;                    //�ŏ��ɐ�������܂ł̎���
    public float intervalTime = 3f;                  //�����Ԋu
    public float leftTime = 5f;                      //���������I�u�W�F�N�g��������܂ł̎���

    private float mTime = 0f;                        //���̃N���X�ŊǗ����鎞��

    // Start is called before the first frame update
    void Start()
    {
        mTime = -offsetTime;
    }

    // Update is called once per frame
    void Update()
    {
        //�Q�[�����̏�ԂłȂ���ΐ������Ȃ�
        if (Game.instance.state != Game.STATE.MOVE) return;

        mTime += Time.deltaTime;

        //���Ԃ�0�ȉ��Ȃ珈���I��
        if (mTime < 0f) return;

        //�����Ԋu�𒴂�����ΏۃI�u�W�F�N�g�𐶐�
        if (mTime >= intervalTime)
        {
            //�����ʒu�������_���Ɏw��
            Vector3 randomPos = Vector3.one;
            randomPos.x = Random.Range(-randomPosRange.x, randomPosRange.x);
            randomPos.y = Random.Range(-randomPos.y, randomPos.y);
            Vector3 pos = transform.position + randomPos;

            //�����A�w�莞�Ԍ����
            GameObject obj = Instantiate(prefab, pos, transform.rotation) as GameObject;

            obj.GetComponent<Rigidbody2D>().velocity = velocity;

            Destroy(obj, leftTime);

            //���ԃ��Z�b�g
            mTime = 0f;
        }
    }
}
