using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextCodeManager : MonoBehaviour, IOpen, IWaitingCallBacks
{

	///==========================================================================================================
	///		GENERAL
	///==========================================================================================================

	#region Components

	[Header("Components")]

	public PhotonView pv;
	public GameObject validationBtn;

	public WaitingPlayers waitingRoom;
	public OpenWindowObj openWindowObj;
	public TextCodeVoteManager voteManager;

	public Image codeBckgndImg;
	public GameObject nextMissionBtn;

	#endregion

	#region Unity Built-in functions

	public void Awake()
	{
		pv = GetComponent<PhotonView>();
	}
	public void Start()
	{
		waitingRoom.Setup(this);
	}
	public void Update()
	{
		
		UpdateCodeInputs();
	}

	#endregion





	///==========================================================================================================
	///		CODE MANAGEMENT
	///==========================================================================================================

	#region Code Management variables

	[Header("Code Mangement")]

	string currentCode = "";
	public bool bIsOpen = false;
	public bool bIsUsed = false;
	public bool bIsAuthor = false;
	public bool bIsWaiting = false;

	public string correctCode = "";
	public List<Sprite> codeBckgnds;

	#endregion


	#region Code Management functions

	public void StartEditing()
	{
		if (bIsUsed || !bIsOpen) return;

		ChangeIsAuthor(true,false);
		ChangeIsUsed(true);
	}
	public void StopEditing()
	{
		if (!bIsUsed || !bIsOpen || !bIsAuthor) return;

		ChangeIsAuthor(false, false);
		ChangeIsUsed(false);
	}
	public void ResetCode()
	{
		ChangeCode("");
	}
	public void RemoveLastLetter()
	{
		string newCode = "";
		for (int i = 0; i < currentCode.Length - 1; i++)
			newCode += currentCode[i];

		ChangeCode(newCode);

		if (newCode.Length == 0) StopEditing();
	}

	public void UpdateCodeInputs()
	{
		if ((bIsUsed && !bIsAuthor) || bIsWaiting) return;

		if(Input.GetKeyDown(KeyCode.Backspace))
		{
			RemoveLastLetter();
			return;
		}


		string input = Input.inputString.ToUpper();
		if (input == "" || currentCode.Length >= correctCode.Length) return;


		if (!bIsUsed) StartEditing();

		print(input);
		ChangeCode(currentCode + input);
	}

	public void ValidateCode()
	{
		validationBtn.SetActive(false);
		waitingRoom.StartWaitingRoom(PhotonNetwork.LocalPlayer.NickName);
		waitingRoom.EnterWaitingRoom(true,true);
	}

	#endregion

	#region Code validation process functions

	public bool IsCodeCorrect()
	{
		return correctCode == currentCode;
	}


	[PunRPC]
	public void FailChaining()
	{
		print("Fail");
		StartCoroutine("FailActionChain");
	}
	public IEnumerator FailActionChain()
	{
		ChangeCodeBckGnd(1);

		yield return new WaitForSeconds(3);

		ChangeCodeBckGnd(0);
		CancelWaitingRoom();
	}

	[PunRPC]
	public void SuccessChaining()
	{
		StartCoroutine("SuccessActionChain");
		ScoreManager.instance.ChangeScore(100);
	}
	public IEnumerator SuccessActionChain()
	{
		ChangeCodeBckGnd(2);

		yield return new WaitForSeconds(2);

		nextMissionBtn.SetActive(true);
	}


	#endregion

	#region Variables chang sync functions

	[PunRPC]
	public void ChangeCode(string code, bool bRep = true)
	{
		if (bRep)
		{
			pv.RPC("ChangeCode", RpcTarget.All, code, false);
			return;
		}

		currentCode = code;

		// Display the validate btn when the code is long enough
		if (correctCode.Length == currentCode.Length && bIsAuthor)
			validationBtn.SetActive(true);
		else
			validationBtn.SetActive(false);

		UpdateDisplay();
	}
	[PunRPC]
	public void ChangeIsUsed(bool used, bool bRep = true)
	{
		if (bRep)
		{
			pv.RPC("ChangeIsUsed", RpcTarget.All, used, false);
			return;
		}
		bIsUsed = used;
	}
	[PunRPC]
	public void ChangeIsAuthor(bool author, bool bRep = true)
	{
		if (bRep)
		{
			pv.RPC("ChangeIsAuthor", RpcTarget.All, author, false);
			return;
		}
		bIsAuthor = author;
	}
	[PunRPC]
	public void ChangeIsWaiting(bool waiting, bool bRep = true)
	{
		if (bRep)
		{
			pv.RPC("ChangeIsWaiting", RpcTarget.All, waiting, false);
			return;
		}
		bIsWaiting = waiting;
	}

	#endregion







	///==========================================================================================================
	///		CODE DISPLAY
	///==========================================================================================================

	#region code display variables

	public TextCodeDisplayer displayer;
	public TextMeshProUGUI msgText;

	#endregion

	#region Code display functions

	public void UpdateDisplay()
	{
		displayer.DisplayText(currentCode);
	}

	public void ChangeCodeBckGnd(int index)
	{
		codeBckgndImg.sprite = codeBckgnds[index];
	}

	#endregion

	#region Msg display functions

	public IEnumerator DisplayMsgForTime(string msg, float duration, bool bRep)
	{
		DisplayMsg(msg, bRep);

		yield return new WaitForSeconds(duration);

		DisplayMsg("", bRep);
	}
	[PunRPC]
	public void DisplayMsg(string msg, bool bRep = true)
	{
		if (bRep)
		{
			pv.RPC("DisplayMsg", RpcTarget.All, msg, false);
			return;
		}

		msgText.text = msg;
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
		voteManager.DisplayProposition(currentCode);
	}
	public void EndWaitingRoom()
	{
		if(IsCodeCorrect())
		{
			pv.RPC("SuccessChaining", RpcTarget.All);
		}
		else
		{
			pv.RPC("FailChaining", RpcTarget.All);
		}
	}
	public void CancelWaitingRoom()
	{
		ResetCode();
		ChangeIsWaiting(false);
		ChangeIsAuthor(false);
		ChangeIsUsed(false);
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
		displayer.DisplayText(currentCode);
	}
	public void OnClose()
	{
		if (bIsAuthor)
		{
			ResetCode();
			ChangeIsUsed(false);
		}

		bIsOpen = false;
		ChangeIsAuthor(false, false);
	}

	#endregion

}
