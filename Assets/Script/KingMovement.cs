using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KingMovement : MonoBehaviour
{
	public enum Phase
	{
		Castle,
		Expedition,
		Saloon,
		WeaponShop
	}

	private Dictionary<Phase, string> phaseNames = new Dictionary<Phase, string>
	{
		{Phase.Castle, "城下町" },
		{Phase.Expedition, "遠征" },
		{Phase.Saloon, "酒場"},
		{Phase.WeaponShop, "鍛冶屋"}
	};

	[SerializeField] public Phase currentPhase = Phase.Castle;
	[SerializeField] private float moveSpeed = 5f;
	[SerializeField] private float jumpForce = 5f;
	[SerializeField] private LayerMask groundLayer;
	[SerializeField] private Transform groundCheck;
	[SerializeField] private float groundCheckRadius = 0.3f;
	[SerializeField] TextMeshProUGUI phaseText;
	[SerializeField] SceneSwitch sceneSwitch;

	private Rigidbody2D rb;
	private Vector2 movement;
	private SpriteRenderer sr;
	private Animator animator;
	private bool isGrounded;
	private bool frontDoor;

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
		UpdatePhaseText(currentPhase);
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

		if (Input.GetKeyDown(KeyCode.Tab))
		{
			currentPhase = currentPhase == Phase.Castle ? Phase.Expedition : Phase.Castle;
			sceneSwitch.ToggleMode();
		}


		switch (currentPhase)
		{
			case Phase.Castle:
				timer += Time.deltaTime;
				if (timer >= moneyTime)
				{
					KingMoneyManager.Instance.AddMoney(100000);
					timer = 0f;
				}

				// 数字キーで解放処理
				for (int i = 1; i <= 9; i++)
				{
					if (Input.GetKeyDown(KeyCode.Alpha0 + i) || Input.GetKeyDown(KeyCode.Keypad0 + i))
					{
						Debug.Log($"解放キーが押されました: Fighter ID = {i}");
						TryUnlockFighterById(i);
					}
				}
				break;

			case Phase.Expedition:
				if (Input.GetButtonDown("Jump") && isGrounded && IsMoveEnabled)
				{
					rb.velocity = new Vector2(rb.velocity.x, jumpForce);
					animator.SetTrigger("Jump");
				}
				break;
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
	public void UpdatePhaseText(Phase currentPhase)
	{
		string displayName = phaseNames.ContainsKey(currentPhase)
			? phaseNames[currentPhase]
			: currentPhase.ToString(); // fallback

		phaseText.text = "Phase:" + displayName;
	}
	public void TryUnlockFighterById(int id)
	{
		if (FighterManager.Instance.fighterList == null)
		{
			Debug.Log("fighterListがnull");
			return;
		}

		Fighter target = FighterManager.Instance.fighterList.Find(f => f.fighter_id == id);

		if (target == null)
		{
			Debug.Log("Fighter targetがnull (id = " + id + ")");
			return;
		}

		Debug.Log("Fighter見つかった: id = " + id + ", unlock = " + target.unlocked);

		if (target.unlocked == 0)
		{
			if (KingMoneyManager.Instance.TryUseMoney(target.unlock_cost))
			{
				target.unlocked = 1;
				StartCoroutine(FighterManager.Instance.UnlockFighterOnServer(target.fighter_id));
				//	KingMoneyManager.Instance.TryUseMoney(target.unlock_cost);
				Debug.Log("解放完了: id = " + id);
			}
			else
			{
				Debug.Log("お金足りません: cost = " + target.unlock_cost);
			}
		}
		else
		{
			Debug.Log("すでに解放済み: id = " + id);
		}
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		Debug.Log(other.gameObject.name +", " + other.gameObject.tag);
		if (/*Input.GetButtonDown("Jump") */
			Input.GetKeyDown(KeyCode.Space) && other.CompareTag("Door"))
		{
			Debug.Log("上押したよ");
			DoorTrigger doorTrigger = other.GetComponent<DoorTrigger>();
			if (doorTrigger != null)
			{
				int num = doorTrigger.GetDoorId();
				Debug.Log(num);
				if (num == 1)
				{
					currentPhase = Phase.Saloon;
				}
				else if (num == 2)
				{
					currentPhase = Phase.WeaponShop;
				}
			}
		}
	}
}