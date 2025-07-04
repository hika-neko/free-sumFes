using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KingMove : MonoBehaviour
{
	public enum Fighter
	{
		Commoner,
		Warrior,
		AdvanceWarrior,
	}
	public enum Mode
	{
		Fight,
		Castle,
	}


	private Fighter selectedFighter = Fighter.Commoner;
	[SerializeField] Mode currentMode = Mode.Fight;

	[SerializeField] float moveSpeed = 5f;
	[SerializeField] float jumpForce = 5f;
	[SerializeField] private LayerMask groundLayer;
	[SerializeField] private Transform groundCheck;
	[SerializeField] private float groundCheckRadius = 0.3f;
	[SerializeField] public bool isMove;
	private bool isGrounded;

	private Rigidbody2D rb;
	private Vector2 movement;
	
	private SpriteRenderer sr;
	[HideInInspector] public int money = 0;
	[SerializeField] Animator animator;
	[SerializeField] string attackName;
	[SerializeField] Generator generator;
	// �U���J�n�E�I���C�x���g
	//public event Action OnAttackStarted;
	//public event Action OnAttackFinished;

	[SerializeField] AudioSource kingAudio;
	[SerializeField] AudioClip[] kingAudios;
	[SerializeField] string[] kingAudiosMemo;

	private float moneyTime = 2.5f;
	float timer = 0f;
	void Start()
	{
		isMove = true;
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
	}

	void Update()
	{
		movement.x = Input.GetAxis("Horizontal");
		movement.Normalize();
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

		if (Mathf.Abs(movement.x) < 0.01f)
		{
			animator.SetFloat("MoveX", 0f);
		}
		else
		{
			animator.SetFloat("MoveX", Mathf.Abs(movement.x), 0.05f, Time.deltaTime);
		}
		// ���E���]
		if (movement.x != 0)
		{
			sr.flipX = movement.x < 0;
		}

		animator.SetBool("IsGrounded", isGrounded);
		// �W�����v����
		if (Input.GetButtonDown("Jump") && isGrounded)
		{
			rb.velocity = new Vector2(rb.velocity.x, jumpForce);
			animator.SetTrigger("Jump");
		}

		switch (currentMode)
		{
			case Mode.Castle:
			if (currentMode == Mode.Castle)
			{
				timer += Time.deltaTime;
				if (timer >= moneyTime)
				{
					money += 1000;
					timer = 0f;
				}
			}
			break;

			case Mode.Fight:
			if (Input.GetKeyDown(KeyCode.F1))
			{
				kingAudio.clip = null;
				kingAudio.clip = kingAudios[0];
				selectedFighter = Fighter.Commoner;
				Debug.Log("����Wait");
				kingAudio.Play();
			}
			if (Input.GetKeyDown(KeyCode.F2))
			{
				kingAudio.clip = null;
				kingAudio.clip = kingAudios[0];
				selectedFighter = Fighter.Warrior;
				Debug.Log("��mWait");
				kingAudio.Play();
			}
			if (Input.GetKeyDown(KeyCode.F3))
			{
				kingAudio.clip = null;
				kingAudio.clip = kingAudios[0];
				selectedFighter = Fighter.AdvanceWarrior;
				Debug.Log("�㋉��mWait");
				kingAudio.Play();
			}

			if (isMove)
			{
				// Enter�L�[�����ŗ����̏E�� or �n��������s��
				if (Input.GetKeyDown(KeyCode.Space))
				{
					switch (selectedFighter)
					{
						case Fighter.Commoner:
						kingAudio.clip = null;
						kingAudio.clip = kingAudios[1];
						generator.Spawner(sr.flipX, 4, 0);
						break;

						case Fighter.Warrior:
						kingAudio.clip = null;
						kingAudio.clip = kingAudios[1];
						generator.Spawner(sr.flipX, 5, 1);
						break;

						case Fighter.AdvanceWarrior:
						kingAudio.clip = null;
						kingAudio.clip = kingAudios[2];
						generator.Spawner(sr.flipX, 10, 2);
						break;
					}
					kingAudio.Play();
					Debug.Log(kingAudio.clip.name);
					animator.SetTrigger("Attack");
				}
			}
			break;
		}
	}

	void FixedUpdate()
	{
		Vector2 velocity = rb.velocity;
		velocity.x = movement.normalized.x * moveSpeed;  // ���ړ��ݒ�
		rb.velocity = velocity;
	}

	public void SetMoveEnabled(bool enabled)
	{
		isMove = enabled;
	}
}
