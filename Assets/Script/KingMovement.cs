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
		{Phase.Castle, "èÈâ∫í¨" },
		{Phase.Expedition, "âìê™" },
		{Phase.Saloon, "éèÍ"},
		{Phase.WeaponShop, "íbñËâÆ"}
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
	private bool nearTalker;
	private bool unlockNow = false;
	Collider2D nearDoorCollider;
	Collider2D nearTalkerCollider;

	private float moneyTime = 2.5f;
	private float timer = 0f;

	public bool IsMoveEnabled { get; set; } = true;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
		if(LoginUI.IsLoginUIActive)
		{
			IsMoveEnabled = false;
		}
	}

	void Update()
	{
		UpdatePhaseText(currentPhase);
		if (IsMoveEnabled)
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
			if (Input.GetButtonDown("Jump") && frontDoor)
			{
				DoorTrigger doorTrigger = nearDoorCollider.GetComponent<DoorTrigger>();
				int doorId = doorTrigger.GetDoorId();
				switch (doorId)
				{
					// èÈ-íbñËâÆ
					case 0:
						currentPhase = Phase.WeaponShop;
						SceneSwitch.Instance.LoadModeScene(SceneSwitch.Phase.WeaponShop, "Castle");
						break;

					// èÈ-éèÍ
					case 1:
						currentPhase = Phase.Saloon;
						SceneSwitch.Instance.LoadModeScene(SceneSwitch.Phase.Saloon, "Castle");
						break;

					// íbñËâÆ-èÈ
					case 2:
						currentPhase = Phase.Castle;
						SceneSwitch.Instance.LoadModeScene(SceneSwitch.Phase.Castle, "WeaponShop");
						break;

					// éèÍ-èÈ
					case 3:
						currentPhase = Phase.Castle;
						SceneSwitch.Instance.LoadModeScene(SceneSwitch.Phase.Castle, "Saloon");
						break;

					default:
						Debug.LogWarning("ñ¢íËã`ÇÃTalk ID: " + doorId);
						break;
				}
			}
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
			break;
				
			case Phase.Saloon:

			if (Input.GetButtonDown("Submit") && nearTalker && !unlockNow)
			{
				unlockNow = true;
				IsMoveEnabled = false;
				TalkTrigger talkerTrigger = nearTalkerCollider.GetComponent<TalkTrigger>();
				if(talkerTrigger != null)
				{
					int talkerId = talkerTrigger.GetTalkerId();
					if (talkerId != 1) return;
					else if(talkerId == 1)
					{
						FighterUnlock.Instance.Open();
					}
				}
			}

			else if(Input.GetButtonDown("Cancel") && nearTalker && unlockNow)
			{
				unlockNow = false;
				FighterUnlock.Instance.Close();
				IsMoveEnabled = true;
			}
			break;

			case Phase.WeaponShop: 
			
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

	private void OnTriggerStay2D(Collider2D other)
	{
		if(other.CompareTag("Door"))
		{
			frontDoor = true;
			nearDoorCollider = other;
		}
		else if(other.CompareTag("Talker"))
		{
			nearTalker = true;
			nearTalkerCollider = other;
		}
	}
	private void OnTriggerExit2D(Collider2D other)
	{
		if(other.CompareTag("Door"))
		{
			frontDoor = false;
			nearDoorCollider = null;
		}
		else if (other.CompareTag("Talker"))
		{
			nearTalker = false;
			nearTalkerCollider = null;
		}
	}
}