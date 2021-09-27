using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour 
{
    private static readonly float  bulletMoveSpeed = 10.0f;        //�e�ۑ��x
    public GameObject hitEffectPrefab = null;                      //�Փˎ��̃G�t�F�N�g

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //�ړ�
        {
            Vector3 vecAddPos = (Vector3.forward * bulletMoveSpeed);
            transform.position += transform.rotation * vecAddPos * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //�Փ˃G�t�F�N�g����
        if(hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }
}
