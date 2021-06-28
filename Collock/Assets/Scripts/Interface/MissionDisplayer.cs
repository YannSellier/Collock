using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MissionDisplayer : MonoBehaviour, IWaitingCallBacks
{
	public WaitingPlayers waitingSystem;
	[HideInInspector]public PhotonView pv;
	public bool BWaitForOthers = false;


	public void Awake()
	{
		pv = GetComponent<PhotonView>();
	}

	public void StartMission()
	{
		print("Start mission");

		if (BWaitForOthers)
		{

			if (!waitingSystem.bWaitingRoomOpen)
			{
				pv.RPC("SetupWaitingRoom", RpcTarget.All);
				waitingSystem.StartWaitingRoom("");
			}

			waitingSystem.EnterWaitingRoom(true);

		}
		else
		{
			CloseDisplay();
		}
	}
	[PunRPC]
	public void SetupWaitingRoom()
	{
		print("Setup waiting room ");
		waitingSystem.Setup(this);
	}

	public void EndWaitingRoom()
	{
		TimerManager.instance.StartTimer();
		waitingSystem.EnterWaitingRoom(false);
		GameManager.instance.localPlayer.OpenWindow(false);
		CloseDisplay();
	}
	public void StartVote()
	{

	}
	public void CancelWaitingRoom()
	{
		
	}

	public void CloseDisplay()
	{
		Destroy(gameObject);
	}

}
