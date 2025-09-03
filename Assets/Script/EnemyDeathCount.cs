using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathCount : MonoBehaviour
{
	public static EnemyDeathCount Instance;
	[Header("撃破数")] public int killCount = 0;
	[Header("ボス出現に要する撃破数")] public int bossAppearNum;
	private bool bossSpawned = false;

	private void Awake()
	{
		if(Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void AddKill()
	{
		killCount++;
		Debug.Log($"敵を倒した: {killCount} / {bossAppearNum}");

		if(!bossSpawned && killCount >= bossAppearNum)
		{
			bossSpawned = true;
			SpawnBoss();
		}
	}

	public void SetBossAppearNum(int num)
	{
		bossAppearNum = num;
	}

	private void SpawnBoss()
	{
		Debug.Log("ボス出現");
		if(BossManager.Instance != null)
		{
			BossManager.Instance.SpawnBoss();
		}
		else
		{
			Debug.LogWarning("BossManagerがnullやね");
		}
	}
}
