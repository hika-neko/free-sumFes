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
				Debug.LogWarning("King‚ªƒV[ƒ““à‚ÉŒ©‚Â‚©‚è‚Ü‚¹‚ñ‚Å‚µ‚½");
				return;
			}
		}
		UpdateUI(); // ‰Šú•\¦
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
