using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
	public static BossManager Instance;
	[SerializeField] GameObject boss;
	[SerializeField] Vector3 bossAppear = new Vector3(0,0,0);

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void SpawnBoss()
	{
		EnemySpawn enemySpawn = FindObjectOfType<EnemySpawn>();
		if (enemySpawn != null)
		{
			enemySpawn.StopSpawning();
		}
		if(boss != null)
		{
			Instantiate(boss, bossAppear, Quaternion.identity);
		}
		else
		{
			Debug.LogWarning("ÇªÇ‡ÇªÇ‡ë∂ç›ÇµÇ‚ÇÁÇÒ");
		}
	}
}
