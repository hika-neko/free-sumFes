using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using TMPro;

public class MoneyConut : MonoBehaviour
{
	[SerializeField] KingMove king;
	[SerializeField] TextMeshProUGUI moneyText;

	private void Start()
	{
		if(king == null)
		{
			king = FindObjectOfType<KingMove>();
			if (king == null)
			{
				Debug.LogWarning("King���V�[�����Ɍ�����܂���ł���");
			}
		}
	}

	private void Update()
	{
		UpdateUI();
	}
	
	void UpdateUI()
	{
		moneyText.text = "Money:" + king.money.ToString();
	}
}
