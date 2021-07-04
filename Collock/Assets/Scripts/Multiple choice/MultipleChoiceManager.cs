using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultipleChoiceManager : MonoBehaviour, IOpen, IWaitingCallBacks
{




	///==========================================================================================================
	///		GENERAL
	///==========================================================================================================

	#region Components


	[Header("Components")]

	public PhotonView pv;
	public WaitingPlayers waitingRoom;
	public OpenWindowObj openWindowObj;
	public MCVoteManager voteManager;

	public GameObject validationBtn;
	public GameObject nextMissionBtn;

	#endregion

	#region Built-in functions

	public void Awake()
	{
		pv = GetComponent<PhotonView>();
	}
	public void Start()
	{
		waitingRoom.Setup(this);
	}

	#endregion






	///==========================================================================================================
	///		CHOICE MANAGEMENT
	///==========================================================================================================

	#region Choice management variables

	private int currentChoice = -1;
	public int correctchoice;

	public bool bIsOpen = false;	
	public bool bIsWaiting = false;

	#endregion

	#region Choice management functions

	public void SelectChoice(int indexChoice)
	{
		ChangeChoice(indexChoice);
	}

	[PunRPC]
	public void ChangeChoice(int indexChoice, bool bRep = true)
	{
		if (bIsWaiting) return;


		if(bRep)
		{
			pv.RPC("ChangeChoice", RpcTarget.All, indexChoice, false);
			return;
		}

		if (indexChoice == currentChoice)
			indexChoice = -1;

		currentChoice = indexChoice;
		UpdateValidationBtnVisiblity();

		UpdateSelectionDisplay();
	}


	public void ValidateChoice()
	{
		validationBtn.SetActive(false);
		waitingRoom.StartWaitingRoom(PhotonNetwork.LocalPlayer.NickName);
		waitingRoom.EnterWaitingRoom(true, true);
	}

	#endregion

	#region Validation process functions

	public bool IsChoiceCorrect()
	{
		return correctchoice == currentChoice;
	}


	[PunRPC]
	public void FailChoice()
	{
		print("Fail");
		StartCoroutine("FailActionChain");
	}
	public IEnumerator FailActionChain()
	{
		ChangeSelectionDisplay(1,currentChoice);

		yield return new WaitForSeconds(3);

		ChangeSelectionDisplay(0, currentChoice);
		CancelWaitingRoom();
	}

	[PunRPC]
	public void SuccessChoice()
	{
		StartCoroutine("SuccessActionChain");
		ScoreManager.instance.ChangeScore(100);
	}
	public IEnumerator SuccessActionChain()
	{
		ChangeSelectionDisplay(2, currentChoice);

		yield return new WaitForSeconds(2);

		nextMissionBtn.SetActive(true);
	}


	#endregion


	#region Variables change sync functions

	[PunRPC]
	public void ChangeIsWaiting(bool waiting, bool bRep = true)
	{
		if (bRep)
		{
			pv.RPC("ChangeIsWaiting", RpcTarget.All, waiting, false);
			return;
		}
		bIsWaiting = waiting;

		UpdateValidationBtnVisiblity();
	}

	#endregion





	///==========================================================================================================
	///		DISPLAY
	///==========================================================================================================

	#region Choice display variables

	public List<GameObject> choiceObjs;

	public List<Sprite> selectionImgs;

	#endregion

	#region Choice display functions

	public void UpdateValidationBtnVisiblity()
	{
		validationBtn.SetActive(currentChoice != -1 && !bIsWaiting);
	}

	public void UpdateSelectionDisplay()
	{
		for(int i = 0; i < choiceObjs.Count; i++)
		{
			choiceObjs[i].transform.GetChild(1).gameObject.SetActive(i == currentChoice);
		}
	}

	public void ChangeSelectionDisplay(int indexImg, int indexChoice)
	{
		if (currentChoice == -1) return;

		choiceObjs[indexChoice].transform.GetChild(1).GetComponent<Image>().sprite = selectionImgs[indexImg];
	}

	#endregion




	///==========================================================================================================
	///		WAITING
	///==========================================================================================================

	#region Waiting Callback functions

	public void OnEnterWaitingRoom()
	{
		openWindowObj.ChangeCanClose(false);
		ChangeIsWaiting(true, false);
	}
	public void StartVote()
	{
		voteManager.Open(true);
		voteManager.DisplayChoice(currentChoice);
	}
	public void EndWaitingRoom()
	{
		if (IsChoiceCorrect())
		{
			pv.RPC("SuccessChoice", RpcTarget.All);
		}
		else
		{
			pv.RPC("FailChoice", RpcTarget.All);
		}
	}
	public void CancelWaitingRoom()
	{
		ChangeChoice(-1);
		ChangeIsWaiting(false);
		openWindowObj.ChangeCanClose(true);
	}

	#endregion



	///==========================================================================================================
	///		OPEN WINDOW
	///==========================================================================================================

	#region Open window callback functions

	public void OnOpen()
	{
		bIsOpen = true;
		UpdateValidationBtnVisiblity();
		UpdateSelectionDisplay();
	}
	public void OnClose()
	{
		bIsOpen = false;
	}

	#endregion




}
