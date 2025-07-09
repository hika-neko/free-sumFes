using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
	private Animator animator;

	void Awake()
	{
		animator = GetComponent<Animator>();
	}
	public void PlayAttack() => animator.SetTrigger("Attack");
	public void PlayHit()
	{

		animator.SetTrigger("Hit"); 
	}
	public void PlayDeath() 
	{
		animator.SetTrigger("Death"); 
	}
	public void DestroySelf() => Destroy(gameObject);
}
