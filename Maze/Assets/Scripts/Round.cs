using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round : MonoBehaviour
{
    private float rotationNow = 0.0f;    //1フレームあたりの回転量
    public float rotationAdd = 90.0f;    //1秒あたりの回転量


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rotationNow += rotationAdd * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, rotationNow, 0);
    }
}
