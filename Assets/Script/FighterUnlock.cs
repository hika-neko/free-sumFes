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
		Debug.Log("Fighter�����ʃI�[�v��");
	}
	public void Close() 
	{
		imageLoop.StopTalkMaster();
		isOpen = false;
		transform.parent.gameObject.SetActive(false);
		Debug.Log("Fighter�����ʃN���[�Y");
	}
	public void TryUnlockFighterById(int id)
	{
		if (FighterManager.Instance.fighterList == null)
		{
			Debug.Log("fighterList��null");
			return;
		}

		Fighter target = FighterManager.Instance.fighterList.Find(f => f.fighter_id == id);

		if (target == null)
		{
			Debug.Log($"Fighter��������Ȃ�: id = {id}");
			return;
		}

		if (target.unlocked == 1)
		{
			Debug.Log($"���łɉ���ς�: id = {id}");
			return;
		}

		if (KingMoneyManager.Instance.TryUseMoney(target.unlock_cost))
		{
			target.unlocked = 1;
			Debug.Log($"�������: id = {id}");
			FighterManager.Instance.UnlockFighterOnServer(id);
		}
		else
		{
			Debug.Log($"����������܂���: �K�v = {target.unlock_cost}");
		}
	}
}