using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KingMovement : MonoBehaviour
{
	private float invisibleTime = 0.4f;
	private float invisibleTimer;
	private bool isInvisible;
	[SerializeField] private float moveSpeed = 5f;
	[SerializeField] private float jumpForce = 5f;
	[SerializeField] private LayerMask groundLayer;
	[SerializeField] private Transform groundCheck;
	[SerializeField] private float groundCheckRadius = 0.3f;
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

	public int king_hp = 3;
	public bool IsMoveEnabled;
	public int kingId;
	private bool isDead;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
		if (LoginUI.IsLoginUIActive)
		{
			IsMoveEnabled = false;
		}
	}

	void Update()
	{
		// ñ≥ìGéûä‘ÇÃÉJÉEÉìÉg
		if (isInvisible)
		{
			invisibleTimer += Time.deltaTime;
			if (invisibleTimer >= invisibleTime)
			{
				isInvisible = false;
				gameObject.tag = "Player"; // É^ÉOÇñﬂÇ∑
				invisibleTimer = 0f;
				Debug.Log("ñ≥ìGèIóπ");
			}
		}
		if (Input.GetKeyDown(KeyCode.F12))
		{
			TakeDamage(1);
		}
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
						PhaseManager.Instance.CurrentPhase = PhaseManager.Phase.WeaponShop;
						SceneSwitch.Instance.LoadModeScene(PhaseManager.Phase.WeaponShop, "Castle");
						break;

					// èÈ-éèÍ
					case 1:
						PhaseManager.Instance.CurrentPhase = PhaseManager.Phase.Saloon;
						SceneSwitch.Instance.LoadModeScene(PhaseManager.Phase.Saloon, "Castle");
						break;

					// íbñËâÆ-èÈ
					case 2:
						PhaseManager.Instance.CurrentPhase = PhaseManager.Phase.Castle;
						SceneSwitch.Instance.LoadModeScene(PhaseManager.Phase.Castle, "WeaponShop");
						break;

					// éèÍ-èÈ
					case 3:
						PhaseManager.Instance.CurrentPhase = PhaseManager.Phase.Castle;
						SceneSwitch.Instance.LoadModeScene(PhaseManager.Phase.Castle, "Saloon");
						break;

					case 4:
					// éèÍ-èÈ
					PhaseManager.Instance.CurrentPhase = PhaseManager.Phase.Expedition;
					SceneSwitch.Instance.LoadModeScene(PhaseManager.Phase.Expedition, "Castle");
					break;

					default:
						Debug.LogWarning("ñ¢íËã`ÇÃDoor ID: " + doorId);
						break;
				}
			}
		}

		switch (PhaseManager.Instance.CurrentPhase)
		{
			case PhaseManager.Phase.Castle:
			if (!LoginUI.IsLoginUIActive)
			{
				timer += Time.deltaTime;
				if (timer >= moneyTime)
				{
					KingMoneyManager.Instance.AddMoney(100);
					timer = 0f;
				}
			}
			break;
				
			case PhaseManager.Phase.Saloon:
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

			case PhaseManager.Phase.WeaponShop: 
			
			break;

			case PhaseManager.Phase.Expedition:
			//Debug.Log("isGrouned + " + isGrounded + "/ isMoveEnabled + " + IsMoveEnabled);
			if (Input.GetButtonDown("Jump")&& IsMoveEnabled)
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
	public void TakeDamage(int damage)
	{
		if(isDead || isInvisible) return;
		king_hp -= damage;
		if(king_hp <= 0)
		{
			Death();
		}
		else
		{
			animator.SetTrigger("Damage");
		}
		isInvisible = true;
		gameObject.tag = "Untagged";
	}
	private void Death()
	{
		isDead = true;
		animator.SetTrigger("Death");
		KingMoneyManager.Instance.DicreaseMoney();
		StartCoroutine(ReturnToCastle());
	}
	private IEnumerator ReturnToCastle()
	{
		yield return new WaitForSeconds(1f); // éÄñSÉAÉjÉÅÇÃçƒê∂ë“Çø

		// ÉVÅ[ÉìëJà⁄
		PhaseManager.Instance.CurrentPhase = PhaseManager.Phase.Castle;
		SceneSwitch.Instance.LoadModeScene(PhaseManager.Phase.Castle, "Forest");
		animator.Play("Idle");
		// HPïúäàÇ»Ç«
		king_hp = 3;
		isDead = false;
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