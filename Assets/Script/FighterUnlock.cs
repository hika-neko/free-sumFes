using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FighterUnlock : MonoBehaviour
{
	public static FighterUnlock Instance;
	[SerializeField] ImageLoop imageLoop;
	public bool isOpen { get; private set; }

	void Awake()
	{
		Instance = this; 
		transform.parent.gameObject.SetActive(false);
	}

	public void Open()
	{
		imageLoop.TalkMaster();
		isOpen = true;
		transform.parent.gameObject.SetActive(true);
		Debug.Log("Fighter解放画面オープン");
	}
	public void Close() 
	{
		imageLoop.StopTalkMaster();
		isOpen = false;
		transform.parent.gameObject.SetActive(false);
		Debug.Log("Fighter解放画面クローズ");
	}
	public void TryUnlockFighterById(int id)
	{
		if (FighterManager.Instance.fighterList == null)
		{
			Debug.Log("fighterListがnull");
			return;
		}

		Fighter target = FighterManager.Instance.fighterList.Find(f => f.fighter_id == id);

		if (target == null)
		{
			Debug.Log($"Fighterが見つからない: id = {id}");
			return;
		}

		if (target.unlocked == 1)
		{
			Debug.Log($"すでに解放済み: id = {id}");
			return;
		}

		if (KingMoneyManager.Instance.TryUseMoney(target.unlock_cost))
		{
			target.unlocked = 1;
			Debug.Log($"解放成功: id = {id}");
			FighterManager.Instance.UnlockFighterOnServer(id);
		}
		else
		{
			Debug.Log($"お金が足りません: 必要 = {target.unlock_cost}");
		}
	}
}