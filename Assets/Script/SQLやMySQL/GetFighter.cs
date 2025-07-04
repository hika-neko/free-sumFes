using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class Fighter
{
    public int fighter_id;              // id
    public string kind;         // ���
    public int cost;            // �R�X�g
    public int attack;          // �U����
    public float speed;         // ���x
    public string prefab_name;  // �v���n�u��
    public int unlocked;        // ����ς݂�
	public int unlock_cost;      // �������R�X�g
}

[System.Serializable]
public class FighterListWrapper
{
    public List<Fighter> fighter;
}


public class GetFighter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetFighterData());
    }

    IEnumerator GetFighterData()
    {
        string url = "http://localhost/Unity�A�g/get_fighters_info.php";
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if(www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("�ʐM���s" + www.error);
            yield break;
        }

        string json = www.downloadHandler.text;
		FighterListWrapper data = JsonUtility.FromJson<FighterListWrapper>(json);
        foreach(var f in data.fighter)
        {
            Debug.Log($"ID:{f.fighter_id} ���:{f.kind} �R�X�g:{f.cost} �U����:{f.attack} " +
                $"���x:{f.speed} �v���n�u��:{f.prefab_name} �A�����b�N:{f.unlocked} �������R�X�g:{f.unlock_cost}");
        }
    }
}
