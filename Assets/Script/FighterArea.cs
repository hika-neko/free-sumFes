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
		{ "Warrior", "��m" },
		{ "AdvanceWarrior", "�㋉��m" },
		{ "Archer", "�|��" },
		{ "Assassin", "�ÎE��" },
		{ "Wizard", "�����m" },
		{ "PlagueDoctor", "�u�a��t" },
		// �K�v�ɉ����Ēǉ�
	};

	private void Awake()
	{
		if (fighterKindToSprite == null)
		{
			fighterKindToSprite = new Dictionary<string, Sprite>
			{
				{"Warrior",Resources.Load<Sprite>("FighterIcons/1.��m")},
		{"AdvanceWarrior",Resources.Load<Sprite>("FighterIcons/2.�㋉��m")},
		{"Archer",Resources.Load<Sprite>("FighterIcons/3.�|��")},
		{"Assassin",Resources.Load<Sprite>("FighterIcons/4.�ÎE��")},
		{"Wizard",Resources.Load<Sprite>("FighterIcons/5.�����m")},
		{"PlagueDoctor",Resources.Load<Sprite>("FighterIcons/6.�u�a��t")}
			};
		}
	}


	public void SetInfo(Fighter data)
	{
		string displayName;
		if (!FighterKindToDisplayName.TryGetValue(data.fighter_kind, out displayName))
		{
			displayName = data.fighter_kind; // �Ή����Ȃ���΂��̂܂�
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
			Debug.LogWarning("�X�v���C�g����: " + data.fighter_kind);
			iconImage.sprite = null;
		}
	}
}
