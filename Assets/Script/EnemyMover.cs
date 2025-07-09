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

	public void SetDirection(bool fromRight)
	{
		moveDirection = fromRight ? Vector2.left : Vector2.right;

		// 見た目を反転
		var sr = GetComponent<SpriteRenderer>();
		if (sr != null)
		{
			sr.flipX = fromRight; // 右から来たときだけ反転
		}
	}
	public void SetMoveSpeed(float speed)
	{
		moveSpeed = speed;
	}

	void Update()
	{
		// 接地判定
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

		if (isGrounded)
		{
			transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
		}
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		// 地面には触れても消さない
		if (collision.CompareTag("Ground") || collision.CompareTag("Player")
			|| collision.CompareTag("Gc"))
		{
			// 地面に触っただけなので何もしない
			return;
		}
	}
}
