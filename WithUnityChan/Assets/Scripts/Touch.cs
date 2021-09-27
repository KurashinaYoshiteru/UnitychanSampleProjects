using UnityEngine;
using System.Collections;

public class Touch : MonoBehaviour {
	
	public AudioClip voice_01;
	public AudioClip voice_02;
	
	private Animator animator;
	private AudioSource univoice;

	private int motionIdol = Animator.StringToHash("Base Layer.Idol");

    private void Start()
    {
		animator = GetComponent<Animator>();
		univoice = GetComponent<AudioSource>();
    }

    private void Update()
    {
		animator.SetBool("Touch", false);
		animator.SetBool("TouchHead", false);

		Ray ray;
		RaycastHit hit;

		GameObject hitObject;

		if (animator.GetCurrentAnimatorStateInfo(0).nameHash == motionIdol)
		{
			animator.SetBool("Motion_Idle", true);
		}
		else
		{
			animator.SetBool("Motion_Idle", false);
		}

		if (Input.GetMouseButtonDown(0))
        {
			//マウスカーソルの位置からカメラの画面を通してレイを飛ばす
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);


			if (Physics.Raycast(ray, out hit, 100))
			{
				hitObject = hit.collider.gameObject;

				if(hitObject.gameObject.tag == "Head")
                {
					animator.SetBool("TouchHead", true);

					univoice.clip = voice_01;
					univoice.Play();

					animator.SetBool("Face_Happy", true);
					animator.SetBool("Face_Angry", false);

					DispMsg.dispMessage("おはよう！\n今日も元気に頑張っていこう！");
				}
				else if(hitObject.gameObject.tag == "Breast")
				{
					animator.SetBool("Touch", true);

					univoice.clip = voice_02;
					univoice.Play();

					animator.SetBool("Face_Happy", false);
					animator.SetBool("Face_Angry", true);

					DispMsg.dispMessage("きゃっ！");
				}
            }
        }
	}
}
