using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
	public enum Mode { Castle, Fight }

	[SerializeField] private Mode currentMode = Mode.Castle;

	private string castleSceneName = "Castle";
	private string fightSceneName = "Stage";
	private string currentSceneName = "";

	void Start()
	{
		LoadModeScene(currentMode);
	}

	void Update()
	{

	}

	public void ToggleMode()
	{
		// ���[�h�؂�ւ�
		currentMode = currentMode == Mode.Castle ? Mode.Fight : Mode.Castle;
		LoadModeScene(currentMode);
	}

	void LoadModeScene(Mode mode)
	{
		// ���̃V�[�����A�����[�h�i���݂��Ă���΁j
		if (!string.IsNullOrEmpty(currentSceneName))
		{
			SceneManager.UnloadSceneAsync(currentSceneName);
		}

		// �V�������[�h�̃V�[�������擾
		string newScene = (mode == Mode.Castle) ? castleSceneName : fightSceneName;

		// Additive�Ń��[�h
		SceneManager.LoadScene(newScene, LoadSceneMode.Additive);
		currentSceneName = newScene;

		// ���[�h��ɃA�N�e�B�u�V�[���ɐݒ�i�I�v�V�����j
		StartCoroutine(SetActiveAfterLoad(newScene));
	}

	System.Collections.IEnumerator SetActiveAfterLoad(string sceneName)
	{
		yield return null;
		Scene loaded = SceneManager.GetSceneByName(sceneName);
		if (loaded.IsValid())
		{
			SceneManager.SetActiveScene(loaded);
		}
	}
}
