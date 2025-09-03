using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingAttack : MonoBehaviour
{
	public enum SelectFighter
	{
		Commoner,
		Warrior,
		AdvanceWarrior,
		Archer,
		Assassin,
		Wizard,
		PlagueDoctor
	}

	[SerializeField] private SelectFighter selectedFighter = SelectFighter.Commoner;
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
		if (!kingMovement.IsMoveEnabled ||
			PhaseManager.Instance.CurrentPhase != PhaseManager.Phase.Expedition) return;

		if (Input.GetKeyDown(KeyCode.F1))
		{
			selectedFighter = SelectFighter.Commoner;
			PlaySoundAndLog(0, "•½–¯Wait");
		}
		if (Input.GetKeyDown(KeyCode.F2))
		{
			selectedFighter = SelectFighter.Warrior;
			PlaySoundAndLog(0, "íŽmWait");
		}
		if (Input.GetKeyDown(KeyCode.F3))
		{
			selectedFighter = SelectFighter.AdvanceWarrior;
			PlaySoundAndLog(0, "ã‹‰íŽmWait");
		}
		if (Input.GetKeyDown(KeyCode.F4))
		{
			selectedFighter = SelectFighter.Archer;
			PlaySoundAndLog(0, "‹|•ºWait");
		}
		if (Input.GetKeyDown(KeyCode.F5))
		{
			selectedFighter = SelectFighter.Assassin;
			PlaySoundAndLog(0, "ˆÃŽEŽÒWait");
		}
		if (Input.GetKeyDown(KeyCode.F6))
		{
			selectedFighter = SelectFighter.Wizard;
			PlaySoundAndLog(0, "–‚“±ŽmWait");
		}
		if (Input.GetKeyDown(KeyCode.F7))
		{
			selectedFighter = SelectFighter.PlagueDoctor;
			PlaySoundAndLog(0, "‰u•aˆãŽtWait");
		}

		if (Input.GetButtonDown("Attack"))
		{
			Fighter selectedData = GetSelectedFighter(selectedFighter);
			if(selectedData != null)
			{
				int spawnLevel = selectedData.fighter_level;
				int totalCost;

				if(spawnLevel == 1)
				{
					totalCost = selectedData.cost;
				}
				else
				{
					float discountRate = 0.95f;
					totalCost = Mathf.CeilToInt(selectedData.cost * spawnLevel * discountRate);
				}

				if (selectedData.unlocked == 1 && KingMoneyManager.Instance.money >= totalCost)
				{
					KingMoneyManager.Instance.TryUseMoney(totalCost);
					generator.Spawner(sr.flipX, spawnLevel, selectedData.fighter_id);

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
	}
	private void PlaySoundAndLog(int audioIndex, string log)
	{
		kingAudio.clip = kingAudios[audioIndex];
		kingAudio.Play();
		Debug.Log(log);
	}
	private Fighter GetSelectedFighter(SelectFighter selected)
	{
		// enum‚Ì–¼‘Oi"Commoner"‚È‚Çj‚Æ kind ‚ð”äŠr
		return fighterList.Find(f => f.fighter_kind == selected.ToString());
	}
}
