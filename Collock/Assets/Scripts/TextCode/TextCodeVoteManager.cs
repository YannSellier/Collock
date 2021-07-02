using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextCodeVoteManager : VoteManager
{
	public TextCodeManager manager;
	public TextCodeDisplayer displayer;

	
	public override void VoteYes()
	{
		manager.openWindowObj.OpenWindow();
		manager.waitingRoom.EnterWaitingRoom(true);

		voteWindow.SetActive(false);
	}
	public override void VoteNo()
	{
		manager.openWindowObj.OpenWindow();
		manager.waitingRoom.pv.RPC("CancelWaitingRoom", RpcTarget.All);

		voteWindow.SetActive(false);

	}


	public override void DisplayProposition(string prop)
	{
		displayer.DisplayText(prop);
	}

}
