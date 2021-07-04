using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCVoteManager : VoteManager
{
	public MultipleChoiceManager manager;
	public List<GameObject> selectionObjs;

	public override void VoteYes()
	{
		manager.openWindowObj.OpenWindow();
		manager.waitingRoom.EnterWaitingRoom(true);

		Open(false);
	}
	public override void VoteNo()
	{
		manager.openWindowObj.OpenWindow();
		manager.waitingRoom.pv.RPC("CancelWaitingRoom", RpcTarget.All);

		voteWindow.SetActive(false);

	}


	public override void DisplayProposition(string prop)
	{
		
	}

	public void DisplayChoice(int indexChoice)
	{
		for(int i =0; i < selectionObjs.Count; i++)
		{
			selectionObjs[i].SetActive(i == indexChoice);
		}
	}
}
