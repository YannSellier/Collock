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

	public int playersWaiting = 0;
	private int totalPlayers = 0;
	public bool bInWaitingRoom = false;
	public bool bWaitingRoomOpen = false;
	private string firstPlayerName = "";

	public SolutionDisplayer solutionDisplayer;

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
	[PunRPC] public void ChangeNbPlayer(int nb,bool bRep = true)
	{
		if(bRep)
		{
			pv.RPC("ChangeNbPlayer", RpcTarget.All, nb, false);
			return;
		}
		print("change nb player: " + nb);

		playersWaiting = nb;

		if(bInWaitingRoom)
			UpdateWaitingRoom();
	}
	public void StartWaitingRoom(string player)
	{
		print("Start Waiting room");
		pv.RPC("SetFirstPlayer", RpcTarget.All, player);
		pv.RPC("StartVote", RpcTarget.Others);

		UpdateWaitingRoom();
	}
	public void EnterWaitingRoom(bool bEnter, bool bFirst = false)
	{
		if (bEnter == bInWaitingRoom) return;

		print("Enter waiting room " + bEnter);
		bInWaitingRoom = bEnter;
		DisplayWaitingRoom(bEnter);


		if (bEnter)
		{
			//if (playersWaiting == 0)
			//	pv.RPC("SetFirstPlayer", RpcTarget.All, PhotonNetwork.LocalPlayer.NickName);

			AddWaitingPlayer();

			if (waitingCallbacks != null) waitingCallbacks.OnEnterWaitingRoom();
		}

	}

	public void AddWaitingPlayer()
	{
		
		print("add player to " + playersWaiting);
		ChangeNbPlayer(playersWaiting + 1);
	}
	public void UpdateWaitingRoom()
	{
		UpdateWaitingRoomDisplay();

		if (playersWaiting >= totalPlayers)
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
		if(voteWindow && solutionDisplayer)
		{
			voteText.text = firstPlayerName + " vous a proposé la solution suivante :";
			voteWindow.SetActive(true);
			

			DisplaySolution();
		}

		if (waitingCallbacks == null) return;

		GameManager.instance.localPlayer.OpenWindow(true);
		waitingCallbacks.StartVote();
	}
	[PunRPC] public void CancelWaitingRoom()
	{
		bWaitingRoomOpen = false;
		ChangeNbPlayer(0);
		StartCoroutine("ShowClosingMessage", "Un joueur a refusé la proposition");
		if (voteWindow)
			voteWindow.SetActive(false);

		if (waitingCallbacks == null) return;

		waitingCallbacks.CancelWaitingRoom();
	}
	public void EndWaitingRoom()
	{

		bWaitingRoomOpen = false;
		EnterWaitingRoom(false);
		ChangeNbPlayer(0);
		if (voteWindow)
			voteWindow.SetActive(false);



		if (waitingCallbacks == null) return;

		waitingCallbacks.EndWaitingRoom();
	}

	IEnumerator ShowClosingMessage(string msg)
	{
		playersText.text = msg;

		yield return new WaitForSeconds(3);

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
		playersText.text = playersWaiting + "/" + totalPlayers + " joueurs";
	}

	#endregion




	///==========================================================================================================
	///		VOTING
	///==========================================================================================================

	public int[] solution;

	public void DisplaySolution()
	{
		if (solutionDisplayer == null) return;

		solutionDisplayer.Init();
		solutionDisplayer.DisplaySolution(solution);
	}

	[PunRPC]
	public void SetSolution(int[] solution, bool bRep = true)
	{
		if(bRep)
		{
			pv.RPC("SetSolution", RpcTarget.All, solution, false);
			return;
		}

		this.solution = solution;
	}

}
