using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : HitObject
{
    private static readonly float bulletMoveSpeed = 10.0f;         //�e��
    public GameObject hitEffectPrefab = null;                      //�q�b�g�G�t�F�N�g�v���n�u

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //�ړ�
        {
            Vector3 vecAddPos = Vector3.forward * bulletMoveSpeed;
            transform.position += transform.rotation * vecAddPos * Time.deltaTime;
        }
    }

    //�e�������ɓ����������̏���
    private void OnTriggerEnter(Collider hitCollider)
    {
        //��������OK�Ȃ��̂łȂ���Ώ����I��
        if (IsHitOK(hitCollider.gameObject) == false) return;
        
        //�q�b�g�G�t�F�N�g������ΐ���
        if(hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }

}
