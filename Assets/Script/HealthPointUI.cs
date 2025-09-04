using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPointUI : MonoBehaviour
{
    [SerializeField] List<Image> hearts;
    private Color fullcolor = Color.white;
    private Color emptycolor = Color.black;
    [SerializeField] KingMovement kingHp;

	private void Start()
	{
		if(kingHp != null)
		{
			UpdateHpUI();
		}
	}

	private void Update()
	{
		if(kingHp != null)
		{
			UpdateHpUI();
		}
	}

	void UpdateHpUI()
	{
		int currentHp = Mathf.Clamp(kingHp.king_hp, 0, hearts.Count);
		for(int i = 0;i < hearts.Count;i++)
		{
			hearts[i].color = (i < currentHp) ? fullcolor : emptycolor;
		}
	}
}
