using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWaitingCallBacks
{
	public void OnEnterWaitingRoom();
	public void CancelWaitingRoom();
	public void StartVote();
	public void EndWaitingRoom();
}
