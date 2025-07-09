using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
	[SerializeField] private float spawnInterval = 10f;
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
		var enemyList = EnemyManager.Instance.enemyList;
		if(enemyList == null || enemyList.Count == 0)
		{
			Debug.LogWarning("EnemySpawer: enemyListが空かnullやで");
			return;
		}

		Enemy selected = enemyList[Random.Range(0,enemyList.Count)];

		GameObject prefab = Resources.Load<GameObject>($"Enemy/{selected.prefab_name}");
		if (prefab == null)
		{
			Debug.LogWarning($"EnemyPrefabが無いで:{selected.prefab_name}");
			return;
		}
		Camera cam = Camera.main;
		Vector3 camLeft = cam.ViewportToWorldPoint(new Vector3(0, 0.25f, 0));
		Vector3 camRight = cam.ViewportToWorldPoint(new Vector3(1, 0.25f, 0));
		float spawnOffsetX = 2.0f;

		int side = Random.Range(0, 2);
		Vector3 spawnPos;
		bool flipX = false;

		if(side == 0)
		{
			spawnPos = new Vector3(camLeft.x - spawnOffsetX, camLeft.y, 0);
			flipX = false;
		}
		else
		{
			spawnPos = new Vector3(camRight.x + spawnOffsetX, camLeft.y, 0);
			flipX = true;
		}

		GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);
		SpriteRenderer eneSR = enemy.GetComponent<SpriteRenderer>();
		if(eneSR != null)
		{
			eneSR.flipX = flipX;
		}
		EnemyMover mover = enemy.GetComponent<EnemyMover>();
		if (mover != null)
		{
			mover.SetDirection(fromRight: side == 1);
			mover.SetMoveSpeed(selected.speed);
		}
		EnemyStatus status = enemy.GetComponent<EnemyStatus>();
		if (status != null)
		{
			status.Initialize(selected.health_point); // Enemyデータのhpをセット
		}
	}
}
