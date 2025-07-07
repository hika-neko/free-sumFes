using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
	[SerializeField] private float moveSpeed = 1.5f;
	private Vector2 moveDirection = Vector2.left; // �E�����ɐi��
	[SerializeField] private Transform groundCheck; // �ڒn�m�F�ʒu
	[SerializeField] private float groundCheckRadius = 0.3f;
	[SerializeField] private LayerMask groundLayer;

	private bool isGrounded;

	void Update()
	{
		// �ڒn����
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

		if (isGrounded)
		{
			transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
		}
	}
}
