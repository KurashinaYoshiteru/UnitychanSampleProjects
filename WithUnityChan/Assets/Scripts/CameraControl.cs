using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	
	GameObject cameraParent;

	Vector3		defaultPosition;	//デフォルト座標保存用
	Quaternion	defaultRotation;	//デフォルト角度保存用
	float		defaultZoom;		//デフォルトズーム保存用

	// Use this for initialization
	void Start () {
	
		//カメラの親オブジェクトを取得
		cameraParent = GameObject.Find("CameraParent");
	
		//デフォルト位置を保存
		defaultPosition = Camera.main.transform.position;
		defaultRotation = cameraParent.transform.rotation;
		defaultZoom = Camera.main.fieldOfView;
	}
	
	// Update is called once per frame
	void Update () {
	
		//カメラ移動
		if( Input.GetMouseButton(0) ){

			Camera.main.transform.Translate(Input.GetAxisRaw("Mouse X") / 10, Input.GetAxisRaw("Mouse Y") / 10, 0);
		}
		
		//カメラ回転
		if( Input.GetMouseButton(1) ){
		
			cameraParent.transform.Rotate(Input.GetAxisRaw("Mouse Y") * 10, Input.GetAxisRaw("Mouse X") * 10, 0);
		}
		
		//ズームイン・ズームアウト
		Camera.main.fieldOfView += (20 * Input.GetAxis("Mouse ScrollWheel") );
		
		if(Camera.main.fieldOfView < 10){
			
			Camera.main.fieldOfView = 10;
		}
		
		//カメラ位置リセット
		if( Input.GetMouseButton(2) ){

			Camera.main.transform.position = defaultPosition;
			cameraParent.transform.rotation = defaultRotation;
			Camera.main.fieldOfView = defaultZoom;
		}
	}
}

