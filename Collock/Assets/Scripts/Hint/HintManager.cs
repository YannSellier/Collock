using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintManager : MonoBehaviour
{
	public static HintManager instance;

	///==========================================================================================================
	///		GENERAL
	///==========================================================================================================

	#region Components

	[Header("Components")]

	public PhotonView pv;
	public HintDisplayer displayer;

	public HintBank hintBank;
	#endregion

	#region Unity Built-in functions

	public void Awake()
	{
		if (!instance)
			instance = this;
		else
			Destroy(gameObject);

		pv = GetComponent<PhotonView>();
	}
	public void Start()
	{
		hintBank.Setup();
	}

	public void Update()
	{
		UpdateHintTimer();
		UpdateInputs();
	}

	#endregion

	#region Input functions

	public void UpdateInputs()
	{
		if(bHintOpen && Input.GetMouseButtonDown(0))
			OpenHint(-1, false);
	}

	#endregion






	///==========================================================================================================
	///		HINT
	///==========================================================================================================

	#region Hint variables

	public bool bHintOpen = false;

	public int currentHint = -1;

	private float currentTime = 0;
	private float timeDelta = 0;

	#endregion

	#region Hint timer functions

	public void UpdateHintTimer()
	{
		timeDelta += Time.deltaTime;
		if(timeDelta >= 1)
		{
			timeDelta -= 1;
			currentTime += 1;

			if(!bHintOpen)
				CheckForHints(currentTime);
		}
	}
	public void CheckForHints(float time)
	{
		for(int i = 0; i < hintBank.hints.Count; i ++)
		{
			Hint hint = hintBank.hints[i];
			if (hint.bActive && hint.displayTime <= time)
				OpenHint(i);
		}
	}

	#endregion

	#region Hint management functions

	[PunRPC]
	public void OpenHint(int indexHint, bool bRep = true)
	{
		if(bRep)
		{
			pv.RPC("OpenHint", RpcTarget.All, indexHint, false);
			return;
		}

		bHintOpen = indexHint != -1;
		currentHint = indexHint;
		string msg = "";
		if(currentHint != -1)
		{
			Hint hint = hintBank.hints[indexHint];
			hint.bActive = false;

			msg = hint.msg;
		}

		DisplayHint(msg);
	}

	#endregion





	///==========================================================================================================
	///		DISPLAY
	///==========================================================================================================

	#region Hint display variables



	#endregion

	#region Hint display functions

	public void DisplayHint(string msg)
	{
		if (!displayer) return;

		displayer.DisplayHint(msg);
	}

	#endregion

}
