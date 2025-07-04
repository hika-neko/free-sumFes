using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KingMove : MonoBehaviour
{
	public enum CallingFighter
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


	private Fighter selectedFighter;
	public List<Fighter> fighterList => FighterManager.Instance.fighterList;
	[SerializeField] CallingFighter selectFighter = CallingFighter.Commoner;
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
	// 攻撃開始・終了イベント
	//public event Action OnAttackStarted;
	//public event Action OnAttackFinished;

	[SerializeField] AudioSource kingAudio;
	[SerializeField] AudioClip[] kingAudios;
	[SerializeField] string[] kingAudiosMemo;

	[SerializeField] private int kingId = 1;

	private float moneyTime = 2.5f;
	float timer = 0f;
	void Start()
	{
		isMove = true;
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
	}
	private Fighter GetSelectedFighter(string kind)
	{
		return fighterList.Find(f => f.kind == kind);
	}

	private bool CanSummon(Fighter f)
	{
		return f.unlocked == 1 && money >= f.cost;
	}

	void Update()
	{
		movement.x = Input.GetAxis("Horizontal");
		movement.Normalize();
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
		
		Fighter selectedData = GetSelectedFighter(selectFighter.ToString());

		if (Mathf.Abs(movement.x) < 0.01f)
		{
			animator.SetFloat("MoveX", 0f);
		}
		else
		{
			animator.SetFloat("MoveX", Mathf.Abs(movement.x), 0.05f, Time.deltaTime);
		}
		// 左右反転
		if (movement.x != 0)
		{
			sr.flipX = movement.x < 0;
		}

		animator.SetBool("IsGrounded", isGrounded);
		// ジャンプ入力
		if (Input.GetButtonDown("Jump") && isGrounded)
		{
			rb.velocity = new Vector2(rb.velocity.x, jumpForce);
			animator.SetTrigger("Jump");
		}

		switch (currentMode)
		{
			case Mode.Castle:
				timer += Time.deltaTime;
				if (timer >= moneyTime)
				{
					AddMoney(1000);
					timer = 0f;
				}

				if(Input.GetKeyDown(KeyCode.S) && selectedData.unlocked == 0)
				{
					if(TryUseMoney(selectedData.unlock_cost))
					{
						selectedData.unlocked = 1;
						StartCoroutine(FighterManager.Instance.UnlockFighterOnServer(selectedData.fighter_id));
					}
				}
			break;

			case Mode.Fight:
			if (Input.GetKeyDown(KeyCode.F1))
			{
				kingAudio.clip = null;
				kingAudio.clip = kingAudios[0];
				selectFighter = CallingFighter.Commoner;
				Debug.Log("平民Wait");
				kingAudio.Play();
			}
			if (Input.GetKeyDown(KeyCode.F2))
			{
				kingAudio.clip = null;
				kingAudio.clip = kingAudios[0];
				selectFighter = CallingFighter.Warrior;
				Debug.Log("戦士Wait");
				kingAudio.Play();
			}
			if (Input.GetKeyDown(KeyCode.F3))
			{
				kingAudio.clip = null;
				kingAudio.clip = kingAudios[0];
				selectFighter = CallingFighter.AdvanceWarrior;
				Debug.Log("上級戦士Wait");
				kingAudio.Play();
			}

			if (currentMode == Mode.Fight && isMove)
			{
				if(Input.GetKeyDown(KeyCode.Space))
				{

					if (selectedData != null && CanSummon(selectedData) && currentMode == Mode.Fight)
					{
						money -= selectedData.cost;
						MoneyManager.Instance.ChangeMoney(kingId, -selectedData.cost);

						generator.Spawner(sr.flipX, selectedData.attack, selectedData.fighter_id);

						kingAudio.clip = kingAudios[1];
						kingAudio.Play();
						animator.SetTrigger("Attack");
					}
					else
					{
						Debug.Log("召喚失敗：解放されていないか所持金不足");
					}
				}
			}
			break;
		}
	}

	void FixedUpdate()
	{
		Vector2 velocity = rb.velocity;
		velocity.x = movement.normalized.x * moveSpeed;  // 横移動設定
		rb.velocity = velocity;
	}

	public void SetMoveEnabled(bool enabled)
	{
		isMove = enabled;
	}

	public bool TryUseMoney(int amount)
	{
		if (money >= amount)
		{
			money -= amount;
			MoneyManager.Instance.ChangeMoney(kingId, -amount);
			return true;
		}
		return false;
	}

	public void AddMoney(int amount)
	{
		money += amount;
		MoneyManager.Instance.ChangeMoney(kingId, amount);
	}
}
