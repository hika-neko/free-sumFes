using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkTrigger : MonoBehaviour
{
	[SerializeField] private int talker_id;
	public int GetTalkerId()
	{
		return talker_id;
	}
}
