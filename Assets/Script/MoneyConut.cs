using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using TMPro;

public class MoneyConut : MonoBehaviour
{
	[SerializeField] KingMove king;
	[SerializeField] TextMeshProUGUI moneyText;
	private int lastDisplayedMoney = -1;

	private void Start()
	{
		if(king == null)
		{
			king = FindObjectOfType<KingMove>();
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
		if (king != null && king.money != lastDisplayedMoney)
		{
			UpdateUI();
		}
	}
	
	void UpdateUI()
	{
		lastDisplayedMoney = king.money;
		moneyText.text = "Money:" + king.money.ToString();
	}
}
