using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using TMPro;

public class MoneyConut : MonoBehaviour
{
	[SerializeField] KingMovement king;
	[SerializeField] KingMoneyManager kingMoney;
	[SerializeField] TextMeshProUGUI moneyText;
	private int lastDisplayedMoney = -1;

	private void Start()
	{
		if(king == null)
		{
			king = FindObjectOfType<KingMovement>();
			if (king == null)
			{
				Debug.LogWarning("King���V�[�����Ɍ�����܂���ł���");
				return;
			}
		}
		UpdateUI(); // �����\��
	}

	private void Update()
	{
		if (king != null && kingMoney.money != lastDisplayedMoney)
		{
			UpdateUI();
		}
	}
	
	void UpdateUI()
	{
		lastDisplayedMoney = kingMoney.money;
		moneyText.text = "Money:" + kingMoney.money.ToString();
	}
}
