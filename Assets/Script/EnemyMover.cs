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

	public void SetDirection(bool fromRight)
	{
		moveDirection = fromRight ? Vector2.left : Vector2.right;

		// �����ڂ𔽓]
		var sr = GetComponent<SpriteRenderer>();
		if (sr != null)
		{
			sr.flipX = fromRight; // �E���痈���Ƃ��������]
		}
	}
	public void SetMoveSpeed(float speed)
	{
		moveSpeed = speed;
	}

	void Update()
	{
		// �ڒn����
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

		if (isGrounded)
		{
			transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
		}
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		// �n�ʂɂ͐G��Ă������Ȃ�
		if (collision.CompareTag("Ground") || collision.CompareTag("Player")
			|| collision.CompareTag("Gc"))
		{
			// �n�ʂɐG���������Ȃ̂ŉ������Ȃ�
			return;
		}
	}
}
