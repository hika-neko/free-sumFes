using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KingMovement : MonoBehaviour
{
	[SerializeField] private float moveSpeed = 5f;
	[SerializeField] private float jumpForce = 5f;
	[SerializeField] private LayerMask groundLayer;
	[SerializeField] private Transform groundCheck;
	[SerializeField] private float groundCheckRadius = 0.3f;

	private Rigidbody2D rb;
	private Vector2 movement;
	private SpriteRenderer sr;
	private Animator animator;
	private bool isGrounded;

	private float moneyTime = 2.5f;
	private float timer = 0f;

	public bool IsMoveEnabled { get; private set; } = true;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
	}

	void Update()
	{
		movement.x = Input.GetAxis("Horizontal");
		movement.Normalize();

		isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

		animator.SetBool("IsGrounded", isGrounded);

		if (Mathf.Abs(movement.x) < 0.01f)
		{
			animator.SetFloat("MoveX", 0f);
		}
		else
		{
			animator.SetFloat("MoveX", Mathf.Abs(movement.x), 0.05f, Time.deltaTime);
		}

		if (movement.x != 0)
		{
			sr.flipX = movement.x < 0;
		}

		if (Input.GetButtonDown("Jump") && isGrounded && IsMoveEnabled)
		{
			rb.velocity = new Vector2(rb.velocity.x, jumpForce);
			animator.SetTrigger("Jump");
		}

		timer += Time.deltaTime;
		if (timer >= moneyTime)
		{
			KingMoneyManager.Instance.AddMoney(1000);
			timer = 0f;
		}

		// �����L�[�ŉ������
		for (int i = 1; i <= 9; i++)
		{
			if (Input.GetKeyDown(KeyCode.Alpha0 + i) || Input.GetKeyDown(KeyCode.Keypad0 + i))
			{
				Debug.Log($"����L�[��������܂���: Fighter ID = {i}");
				TryUnlockFighterById(i);
			}
		}
	}

	void FixedUpdate()
	{
		Vector2 velocity = rb.velocity;
		velocity.x = movement.normalized.x * moveSpeed;
		rb.velocity = velocity;
	}

	public void SetMoveEnabled(bool enabled)
	{
		IsMoveEnabled = enabled;
	}

	public void TryUnlockFighterById(int id)
	{
		if (FighterManager.Instance.fighterList == null)
		{
			Debug.Log("fighterList��null");
			return;
		}
		
		Fighter target = FighterManager.Instance.fighterList.Find(f => f.fighter_id == id);

		if (target == null)
		{
			Debug.Log("Fighter target��null (id = " + id + ")");
			return;
		}

		Debug.Log("Fighter��������: id = " + id + ", unlock = " + target.unlocked);

		if (target.unlocked == 0)
		{
			if (KingMoneyManager.Instance.TryUseMoney(target.unlock_cost))
			{
				target.unlocked = 1;
				StartCoroutine(FighterManager.Instance.UnlockFighterOnServer(target.fighter_id));
				Debug.Log("�������: id = " + id);
			}
			else
			{
				Debug.Log("��������܂���: cost = " + target.unlock_cost);
			}
		}
		else
		{
			Debug.Log("���łɉ���ς�: id = " + id);
		}
	}
}
