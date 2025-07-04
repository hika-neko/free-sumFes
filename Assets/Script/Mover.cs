using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
	private Vector2 direction;
	private Generator generatorRef; // ←生成元の参照を保持
	private Vector3 spawnPosition; // 生成された場所
	private SpriteRenderer sr; // 生成された場所
	[SerializeField] private float speed = 5f;
	[SerializeField] private float maxDistance = 10f; // 離れすぎる距離

	void Start()
	{
		spawnPosition = transform.position; // 生成地点を記録
	}

	public void SetDirection(Vector2 dir)
	{
		direction = dir.normalized;
	}
	public void SetGenerator(Generator generator)
	{
		generatorRef = generator;
	}
	void Update()
	{
		// 毎フレーム移動
		transform.Translate(direction * speed * Time.deltaTime);

		// 移動距離チェック（x軸 or 全体）
		float distance = Mathf.Abs(transform.position.x - spawnPosition.x);
		if (distance >= maxDistance)
		{
			if (generatorRef != null)
			{
				generatorRef.DecreaseSpawnCount();
			}
			Destroy(gameObject);
		}
	}
}
