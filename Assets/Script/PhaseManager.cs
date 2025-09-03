using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
	public enum Phase
	{
		Castle,
		Expedition,
		Saloon,
		WeaponShop
	}

	public static PhaseManager Instance { get; private set; }

	private Phase currentPhase = Phase.Castle;
	public Phase CurrentPhase
	{
		get => currentPhase;
		set
		{
			if (currentPhase != value)
			{
				currentPhase = value;
				OnPhaseChanged?.Invoke(currentPhase);
			}
		}
	}
	public delegate void PhaseChanged(Phase newPhase);
	public event PhaseChanged OnPhaseChanged;
	private void Awake()
	{
		// シングルトンの初期化
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject); // 必要なら
		}
		else
		{
			Destroy(gameObject);
		}
	}
}
