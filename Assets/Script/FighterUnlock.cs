using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterUnlock : MonoBehaviour
{
	public static FighterUnlock Instance;
	[SerializeField] ImageLoop imageLoop;

	void Awake()
	{
		Instance = this; 
		transform.parent.gameObject.SetActive(false);
	}

	public void Open()
	{
		imageLoop.TalkMaster();
		transform.parent.gameObject.SetActive(true);
		Debug.Log("Fighter解放画面オープン");
	}
	public void Close() 
	{
		imageLoop.StopTalkMaster();
		transform.parent.gameObject.SetActive(false);
		Debug.Log("Fighter解放画面クローズ");
	}
}