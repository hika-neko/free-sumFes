using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
	public static SceneSwitch Instance { get; private set; }

	public enum Phase
	{
		Castle, 
		Fight,
		Saloon,
		WeaponShop
	}

	private Dictionary<(string toScene, string fromScene), Vector3> spawnPoints = new Dictionary<(string, string), Vector3>()
	{
		// 遷移先　遷移元　どの座標
		{("Saloon", "Castle"), new Vector3(-5, -2, 0)},
		{("WeaponShop", "Castle"), new Vector3(-5, -2, 0) },
		{("Castle", "Saloon"), new Vector3(15.5f, -2, 0) },
		{("Castle", "WeaponShop"), new Vector3(0.1f, -2, 0) },
		{("Castle", "Castle"), new Vector3(-0.1f, -2, 0) },
	};


	[SerializeField] private Phase currentMode = Phase.Castle;

	private string castleSceneName = "Castle";
	private string fightSceneName = "Stage";
	private string barSceneName = "Saloon";
	private string shopSceneName = "WeaponShop";
	private string currentSceneName = "";
	private string lastSceneName = "";

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			// DontDestroyOnLoad(gameObject); // 必要なら
		}
		else
		{
			Destroy(gameObject);
		}
	}

	void Start()
	{
		LoadModeScene(currentMode, castleSceneName);
	}

	public void LoadModeScene(Phase phase, string fromScene)
	{
		lastSceneName = fromScene;

		// 新しいモードのシーン名を取得
		string newScene = phase switch
		{
			Phase.Castle => newScene = castleSceneName,
			Phase.Saloon => newScene = barSceneName,
			Phase.WeaponShop => newScene = shopSceneName,
			_ => ""
		};

		if (string.IsNullOrEmpty(newScene))
		{
			Debug.LogWarning("未対応のモード: " + phase);
			return;
		}

		// ロード後にアクティブシーンに設定（オプション）
		StartCoroutine(SwitchScene(newScene));
	}
	private Vector3 GetSpawnPositionForScene(string toScene, string fromScene)
	{
		if (spawnPoints.TryGetValue((toScene, fromScene), out Vector3 pos))
		{
			return pos;
		}
		else
		{
			Debug.LogWarning($"スポーン位置未定義: {toScene} ← {fromScene}");
			return Vector3.zero;
		}
	}

	private IEnumerator SwitchScene(string newScene)
	{
		if (!string.IsNullOrEmpty(currentSceneName))
		{
			AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(currentSceneName);
			while (!unloadOp.isDone)
			{
				yield return null;
			}
		}

		AsyncOperation loadOp=SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive);
		while (!loadOp.isDone) 
		{
			yield return null;
		}
		currentSceneName = newScene;

		yield return SetActiveAfterLoad(newScene);
	}
	IEnumerator SetActiveAfterLoad(string sceneName)
	{
		yield return null;

		Scene loaded = SceneManager.GetSceneByName(sceneName);
		if (loaded.IsValid())
		{
			SceneManager.SetActiveScene(loaded);

			GameObject player = GameObject.FindWithTag("Player");
			if (player != null)
			{
				Vector3 spawnPos = GetSpawnPositionForScene(sceneName, lastSceneName);
				player.transform.position = spawnPos;
			}
		}
		else
		{
			Debug.LogWarning($"playerなし.");
		}
	}
}
