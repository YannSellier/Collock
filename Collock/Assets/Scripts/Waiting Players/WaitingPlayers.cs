using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaitingPlayers : MonoBehaviour
{
	///==========================================================================================================
	///		GENERAL
	///==========================================================================================================

	#region Conponents variables

	[HideInInspector] public PhotonView pv;

	#endregion

	#region Unity's functions

	public void Awake()
	{
		pv = GetComponent<PhotonView>();
	}

	public void Update()
	{
	}

	#endregion




	///==========================================================================================================
	///		WAITING PLAYERS
	///==========================================================================================================

	#region Waiting players variables

	private int playersWaiting = 0;
	private int totalPlayers = 0;
	private bool bInWaitingRoom = false;
	public bool bWaitingRoomOpen = false;
	private string firstPlayerName = "";


	public IWaitingCallBacks waitingCallbacks;


	public GameObject waitingRoomWindow;
	public TextMeshProUGUI playersText;

	public GameObject voteWindow;
	public TextMeshProUGUI voteText;

	#endregion

	#region Waiting functions

	[PunRPC] public void Setup(IWaitingCallBacks waitingCallbacks)
	{
		print("Setup waiting room");

		this.waitingCallbacks = waitingCallbacks;

		playersWaiting = 0;
		totalPlayers = PhotonNetwork.CurrentRoom.PlayerCount;
		bWaitingRoomOpen = true;
	}
	public void StartWaitingRoom()
	{
		print("Start Waiting room");

		pv.RPC("StartVote", RpcTarget.Others);

		UpdateWaitingRoom();
	}
	public void EnterWaitingRoom(bool bEnter)
	{
		if (bEnter == bInWaitingRoom) return;

		print("Enter waiting room " + bEnter);
		bInWaitingRoom = bEnter;

		if (playersWaiting == 0) pv.RPC("SetFirstPlayer", RpcTarget.All, PhotonNetwork.LocalPlayer.NickName);

		if (bEnter) pv.RPC("AddWaitingPlayer",RpcTarget.All);
		DisplayWaitingRoom(bEnter);
	}

	[PunRPC] public void AddWaitingPlayer()
	{
		playersWaiting++;
		UpdateWaitingRoom();
	}
	public void UpdateWaitingRoom()
	{
		UpdateWaitingRoomDisplay();

		if (playersWaiting == totalPlayers)
			EndWaitingRoom();
			//pv.RPC("EndWaitingRoom", RpcTarget.All);
	}

	[PunRPC] public void SetFirstPlayer(string name)
	{
		firstPlayerName = name;
	}


	#endregion

	#region Callbacks functions

	[PunRPC] public void StartVote()
	{
		if(voteWindow)
		{
			voteText.text = firstPlayerName + " vous a proposé la solution suivante :";
			voteWindow.SetActive(true);
		}

		if (waitingCallbacks == null) return;

		waitingCallbacks.StartVote();
	}
	[PunRPC] public void CancelWaitingRoom()
	{
		bWaitingRoomOpen = false;
		StartCoroutine("ShowClosingMessage", "Un joueur a refusé votre proposition");
		if (voteWindow)
			voteWindow.SetActive(false);

		if (waitingCallbacks == null) return;

		waitingCallbacks.CancelWaitingRoom();
	}
	[PunRPC] public void EndWaitingRoom()
	{
		bWaitingRoomOpen = false;
		EnterWaitingRoom(false);
		if (voteWindow)
			voteWindow.SetActive(false);

		if (waitingCallbacks == null) return;

		waitingCallbacks.EndWaitingRoom();
	}

	IEnumerator ShowClosingMessage(string msg)
	{
		playersText.text = msg;

		yield return new WaitForSeconds(2);

		EnterWaitingRoom(false);
	}

	#endregion

	#region WaitingRoom display functions

	public void DisplayWaitingRoom(bool bDisplay)
	{
		waitingRoomWindow.SetActive(bDisplay);
	}
	public void UpdateWaitingRoomDisplay()
	{
		playersText.text = playersWaiting + "/" + totalPlayers + " joueurs prêt";
	}

	#endregion


}
