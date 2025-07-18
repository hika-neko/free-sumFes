using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FighterArea : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI nameText;
	[SerializeField] private TextMeshProUGUI costText;
	[SerializeField] private TextMeshProUGUI lockText;
	[SerializeField] private Image iconImage;

	private static Dictionary<string, Sprite> fighterKindToSprite;

	private static readonly Dictionary<string, string> FighterKindToDisplayName = new Dictionary<string, string>
	{
		{ "Warrior", "戦士" },
		{ "AdvanceWarrior", "上級戦士" },
		{ "Archer", "弓兵" },
		{ "Assassin", "暗殺者" },
		{ "Wizard", "魔導士" },
		{ "PlagueDoctor", "疫病医師" },
		// 必要に応じて追加
	};

	private void Awake()
	{
		if (fighterKindToSprite == null)
		{
			fighterKindToSprite = new Dictionary<string, Sprite>
			{
				{"Warrior",Resources.Load<Sprite>("FighterIcons/1.戦士")},
		{"AdvanceWarrior",Resources.Load<Sprite>("FighterIcons/2.上級戦士")},
		{"Archer",Resources.Load<Sprite>("FighterIcons/3.弓兵")},
		{"Assassin",Resources.Load<Sprite>("FighterIcons/4.暗殺者")},
		{"Wizard",Resources.Load<Sprite>("FighterIcons/5.魔導士")},
		{"PlagueDoctor",Resources.Load<Sprite>("FighterIcons/6.疫病医師")}
			};
		}
	}


	public void SetInfo(Fighter data)
	{
		string displayName;
		if (!FighterKindToDisplayName.TryGetValue(data.fighter_kind, out displayName))
		{
			displayName = data.fighter_kind; // 対応がなければそのまま
		}
		nameText.text = displayName;
		costText.text = data.unlock_cost.ToString();
		lockText.text = data.unlocked == 1 ? "Unlocked" : "Locked";

		Sprite icon;
		if(fighterKindToSprite.TryGetValue(data.fighter_kind, out icon) 
			&& icon != null)
		{
			iconImage.sprite = icon;
		}
		else
		{
			Debug.LogWarning("スプライト無し: " + data.fighter_kind);
			iconImage.sprite = null;
		}
	}
}
