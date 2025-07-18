using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LoginUI;

public class KingMoneyManager : MonoBehaviour
{
	public static KingMoneyManager Instance;
	public int kingId = 1;

	public int money { get; private set; } = 0;

	void Awake()
	{
		if (Instance == null) Instance = this;
		else Destroy(gameObject);
	}

	public bool TryUseMoney(int amount)
	{
		if (money <= 0) return false;
		if (money >= amount)
		{
			money -= amount;
			// ここでサーバー通知とかも呼ぶ
			StartCoroutine(MoneyManager.Instance.ChangeMoneyOnServer(kingId, -amount));
			return true;
		}
		return false;
	}

	public void AddMoney(int amount)
	{
		money += amount;
		StartCoroutine(MoneyManager.Instance.ChangeMoneyOnServer(kingId, amount));
		// ここでサーバー通知とかも呼ぶ
	}
	public void SetKingInfo(KingInfo info)
	{
		kingId = info.king_id;
		money = info.money;
		Debug.Log($"KingID: {kingId}, 所持金: {money}");
	}
}
