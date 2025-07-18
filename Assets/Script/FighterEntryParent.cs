using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterEntryParent : MonoBehaviour
{
	public static FighterEntryParent Instance;
	[SerializeField] FighterArea[] fighterAreas;

	private void Awake()
	{
		fighterAreas = GetComponentsInChildren<FighterArea>(includeInactive: true);
	}

	private void Start()
	{
		List<Fighter> fighters = FighterManager.Instance.GetFighterList();
		SetUp(fighters);
	}
	public void SetUp(List<Fighter> fighterList)
	{
		for (int i =0; i < fighterAreas.Length; i++)
		{
			int fighterIndex = i + 1;
			if(fighterIndex >= fighterList.Count)
			{
				Debug.LogWarning("FighterList doesn't have enough");
			}
			fighterAreas[i].SetInfo(fighterList[fighterIndex]);
		}
	}
}
