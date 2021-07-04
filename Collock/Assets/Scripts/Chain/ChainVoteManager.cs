using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChainVoteManager : VoteManager
{
	public ChainManager manager;
	public List<Image> itemImgs;

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

	public void DisplayChain(List<Sprite> imgs)
	{
		print("display chain with " + imgs.Count);
		for(int i = 0; i < imgs.Count; i++)
		{
			print("display " + imgs[i]);
			itemImgs[i].sprite = imgs[i];
		}
	}
}
