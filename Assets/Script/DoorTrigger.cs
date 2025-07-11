using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
	[SerializeField] private int door_id;
	public int GetDoorId()
	{
		return door_id;
	}
}
