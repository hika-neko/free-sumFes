using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
	[SerializeField] private float moveSpeed = 1.5f;
	private Vector2 moveDirection = Vector2.left; // 右→左に進む
	[SerializeField] private Transform groundCheck; // 接地確認位置
	[SerializeField] private float groundCheckRadius = 0.3f;
	[SerializeField] private LayerMask groundLayer;

	private bool isGrounded;

	void Update()
	{
		// 接地判定
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

		if (isGrounded)
		{
			transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
		}
	}
}
