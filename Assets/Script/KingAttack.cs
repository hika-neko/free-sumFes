using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingAttack : MonoBehaviour
{
	public enum CallingFighter
	{
		Commoner,
		Warrior,
		AdvanceWarrior,
	}

	[SerializeField] private CallingFighter selectFighter = CallingFighter.Commoner;
	[SerializeField] private Animator animator;
	[SerializeField] private AudioSource kingAudio;
	[SerializeField] private AudioClip[] kingAudios;
	[SerializeField] private Generator generator;
	[SerializeField] private SpriteRenderer sr;

	public List<Fighter> fighterList => FighterManager.Instance.fighterList;
	private KingMovement kingMovement;

	void Start()
	{
		kingMovement = GetComponent<KingMovement>();
		if (animator == null) animator = GetComponent<Animator>();
		if (kingAudio == null) kingAudio = GetComponent<AudioSource>();
		if (sr == null) sr = GetComponent<SpriteRenderer>();
	}

	void Update()
	{
		if (!kingMovement.IsMoveEnabled) return;

		if (Input.GetKeyDown(KeyCode.F1))
		{
			selectFighter = CallingFighter.Commoner;
			PlaySoundAndLog(0, "•½–¯Wait");
		}
		if (Input.GetKeyDown(KeyCode.F2))
		{
			selectFighter = CallingFighter.Warrior;
			PlaySoundAndLog(0, "íŽmWait");
		}
		if (Input.GetKeyDown(KeyCode.F3))
		{
			selectFighter = CallingFighter.AdvanceWarrior;
			PlaySoundAndLog(0, "ã‹‰íŽmWait");
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			Fighter selectedData = GetSelectedFighter(selectFighter.ToString());
			if (selectedData != null && selectedData.unlocked == 1 && KingMoneyManager.Instance.money >= selectedData.cost)
			{
				KingMoneyManager.Instance.TryUseMoney(selectedData.cost);

				generator.Spawner(sr.flipX, selectedData.attack, selectedData.fighter_id);

				kingAudio.clip = kingAudios[1];
				kingAudio.Play();
				animator.SetTrigger("Attack");
			}
			else
			{
				Debug.Log("¢Š«Ž¸”sF‰ð•ú‚³‚ê‚Ä‚¢‚È‚¢‚©ŠŽ‹à•s‘«");
			}
		}
	}


	private Fighter GetSelectedFighter(string kind)
	{
		return fighterList.Find(f => f.kind == kind);
	}

	private void PlaySoundAndLog(int audioIndex, string log)
	{
		kingAudio.clip = kingAudios[audioIndex];
		kingAudio.Play();
		Debug.Log(log);
	}
}
