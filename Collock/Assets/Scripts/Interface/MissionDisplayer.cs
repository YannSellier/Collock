using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MissionDisplayer : MonoBehaviour, IWaitingCallBacks
{
	public WaitingPlayers waitingSystem;
	[HideInInspector]public PhotonView pv;
	public bool BWaitForOthers = false;
	public bool bStartTimers = true;


	public void Awake()
	{
		pv = GetComponent<PhotonView>();
		waitingSystem.Setup(this);
	}

	public void StartMission()
	{
		print("Start mission");

		if (BWaitForOthers)
		{

			//if (!waitingSystem.bWaitingRoomOpen)
			//{
				//pv.RPC("SetupWaitingRoom", RpcTarget.All);
				waitingSystem.StartWaitingRoom(PhotonNetwork.LocalPlayer.NickName);
			//}

			waitingSystem.EnterWaitingRoom(true);

		}
		else
		{

			if (bStartTimers) GameManager.instance.StartTimers();
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
		if(bStartTimers) GameManager.instance.StartTimers();
		CloseDisplay();
	}
	public void StartVote()
	{

	}
	public void CancelWaitingRoom()
	{
		
	}
	public void OnEnterWaitingRoom()
	{

	}

	public void CloseDisplay()
	{
		Destroy(gameObject);
	}

}
