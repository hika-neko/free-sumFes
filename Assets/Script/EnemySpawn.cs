using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
	[SerializeField] private GameObject enemyPrefab;
	[SerializeField] private float spawnInterval = 5f;
	private float timer = 0f;

	void Update()
	{
		timer += Time.deltaTime;
		if (timer >= spawnInterval)
		{
			SpawnEnemy();
			timer = 0f;
		}
	}

	void SpawnEnemy()
	{
		Instantiate(enemyPrefab, transform.position, Quaternion.identity);
	}
}
