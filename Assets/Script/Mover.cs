using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
	private Vector2 direction;
	private Generator generatorRef; // ���������̎Q�Ƃ�ێ�
	private Vector3 spawnPosition; // �������ꂽ�ꏊ
	private SpriteRenderer sr; // �������ꂽ�ꏊ
	[SerializeField] private float speed = 5f;
	[SerializeField] private float maxDistance = 10f; // ���ꂷ���鋗��

	void Start()
	{
		spawnPosition = transform.position; // �����n�_���L�^
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
		// ���t���[���ړ�
		transform.Translate(direction * speed * Time.deltaTime);

		// �ړ������`�F�b�N�ix�� or �S�́j
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
