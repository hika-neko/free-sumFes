using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
	private Vector2 direction;
	private Generator generatorRef; // ���������̎Q�Ƃ�ێ�
	private Vector3 spawnPosition; // �������ꂽ�ꏊ
	[SerializeField] private float moveSpeed;
	[SerializeField] private float maxDistance = 10f; // ���ꂷ���鋗��
	private float lifetime = 5f;
	private float timer = 0f;

	void Start()
	{
		spawnPosition = transform.position; // �����n�_���L�^
	}
	public void SetSpeed(float speed)
	{
		moveSpeed = speed;
	}

	public void SetDirection(Vector2 dir)
	{
		direction = dir.normalized;
	}
	public void SetGenerator(Generator generator)
	{
		generatorRef = generator;
	}
	public void SetLifetime(float time)
	{
		lifetime = time;
	}
	void Update()
	{
		// ���t���[���ړ�
		transform.Translate(direction * moveSpeed * Time.deltaTime);

		timer += Time.deltaTime;
		// �ړ������`�F�b�N�ix�� or �S�́j
		float distance = Mathf.Abs(transform.position.x - spawnPosition.x);
		if (distance >= maxDistance || timer >= lifetime)
		{
			if (generatorRef != null)
			{
				generatorRef.DecreaseSpawnCount();
			}
			Destroy(gameObject);
		}
	}
}
