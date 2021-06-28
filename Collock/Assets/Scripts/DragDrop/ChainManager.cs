using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChainManager : DragDropWindowManager, IWaitingCallBacks
{
	public List<string> correctChain;

	public WaitingPlayers waitingRoom;
	

	public override void OnDrop()
	{
		base.OnDrop();

		foreach (var inv in inventories)
		{
			if (inv.ItemCount() == 0) return;
		}

		print("Open validation window");
		OpenValidationWindow(true);
	}

	public bool IsChainCorrect()
	{
		for(int i = 0; i < correctChain.Count; i++)
		{
			if (correctChain[i] != inventories[i].GetItemAt(0).itemName)
				return false;
		}

		return true;
	}



	public void ValidateChain()
	{
		waitingRoom.StartWaitingRoom();
		OpenValidationWindow(false);
	}
	

	public void VoteNo()
	{
		waitingRoom.pv.RPC("CancelWaitingRoom",RpcTarget.All);
	}
	public void VoteYes()
	{
		waitingRoom.EnterWaitingRoom(true);
		
	}




	#region Waiting callbacks

	public void StartVote()
	{
		
	}
	public void CancelWaitingRoom()
	{
		CancelDragDropOp();
	}
	public void EndWaitingRoom()
	{
		
	}

	#endregion
}
