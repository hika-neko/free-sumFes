//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;
//using UnityEngine.Networking;

//[System.Serializable]
//public class KingCreateResult
//{
//	public int king_id;
//}


//public class AddKing : MonoBehaviour
//{
//	void Start()
//	{
//		StartCoroutine(AddNewKing());
//	}

//	IEnumerator AddNewKing()
//	{
//		using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/Unity�A�g/add_king.php", new WWWForm()))
//		{
//			yield return www.SendWebRequest();

//			if (www.result != UnityWebRequest.Result.Success)
//			{
//				Debug.LogError("�f�[�^�擾���s: " + www.error);
//			}
//			else
//			{
//				string json = www.downloadHandler.text;
//				KingCreateResult king = JsonUtility.FromJson<KingCreateResult>(json);
//				Debug.Log("fighter�� " + king.king_id+ " �擾");
//			}
//		}

//	}
//}
