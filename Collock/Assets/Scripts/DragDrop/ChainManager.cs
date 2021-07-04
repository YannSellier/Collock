using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class ChainManager : DragDropWindowManager, IWaitingCallBacks
{
	public List<string> correctChain;

	public WaitingPlayers waitingRoom;
	public ChainItemsBank chainItemBank;

	public ChainVoteManager voteManager;


	public Sprite[] chainImgs = new Sprite[3];

	public GameObject successWindow;

	public OpenWindowObj openWindowObj;

	public int[] currentProposition;


	public void Start()
	{
		waitingRoom.Setup(this);
	}

	public override void OnOpen()
	{
		base.OnOpen();
		UpdateChainDisplay();
		CheckForValidationPossibility();
	}

	public override void OnDrop()
	{
		base.OnDrop();

		CheckForValidationPossibility();

		//SetSolution();
		//pv.RPC("LoadProposition", RpcTarget.Others, waitingRoom.solution);

	}
	public int[] GetConfig()
	{
		int[] config = new int[chainImgs.Length];
		for(int i = 0; i < chainImgs.Length; i++)
		{
			config[i] = chainItemBank.imgs.IndexOf(chainImgs[i]);
		}
		return config;
	}
	[PunRPC] public void LoadProposition(int[] sol)
	{
		print("Load Porposition");
		/*Inventory playerInv = GameManager.instance.localPlayer.inv;
		for(int i = 0; i < sol.Length; i ++)
		{
			Item solItem = null;
			foreach (var item in playerInv.items)
			{
				if (sol[i] == chainItemBank.imgs.IndexOf(item.itemImage))
				{
					solItem = item;
					break;
				}
			}

			inventories[i].AddItem(solItem);
			playerInv.RemoveItem(solItem);
		}*/

		for (int i = 0; i < sol.Length; i++)
		{
			ChangeSlotImg(i, sol[i]);
		}

	}
	[PunRPC] public void ChangeSlotImg(int indexSlot, int indexImg)
	{

		Image img = inventories[indexSlot].GetComponent<Image>();
		if (indexImg == -1)
		{
			img.color = new Color(1, 1, 1, 0);			
		} 
		else
		{
			img.sprite = chainItemBank.imgs[indexImg];
			img.color = new Color(1, 1, 1, 1);
			print("slot " + indexSlot + " filled with img " + indexImg + " On " + img);
		}
	}
	[PunRPC] public void SaveProposition(int[] proposition)
	{
		currentProposition = proposition;
	}


	public void ValidateChain()
	{

		SetIsAuthor(true,false);
		SetIsUsed(true);

		waitingRoom.StartWaitingRoom(PhotonNetwork.LocalPlayer.NickName);
		waitingRoom.EnterWaitingRoom(true,true);
		OpenValidationWindow(false);

	}
	public void SetSolution()
	{
		List<Sprite> imgs = new List<Sprite>();
		foreach (var inv in inventories)
			imgs.Add(inv.items[0].itemImage);

		waitingRoom.SetSolution(chainItemBank.ConvertSolution(imgs));
	}
	

	public void VoteNo()
	{

		openWindowObj.OpenWindow();
		waitingRoom.pv.RPC("CancelWaitingRoom",RpcTarget.All);
	}
	public void VoteYes()
	{

		openWindowObj.OpenWindow();
		waitingRoom.EnterWaitingRoom(true);

	}

	#region ChainDisplay

	public void UpdateChainDisplay()
	{
		foreach (var inv in inventories)
			inv.UpdateAllDisplay();
	}

	#endregion


	#region Waiting callbacks

	public void OnEnterWaitingRoom()
	{
		print("Enter waiting room with author=" + bIsAuthor);
		//if (!bIsAuthor)
		//{
			openWindowObj.OpenWindow();
		//}
	}
	public void StartVote()
	{
		List<Sprite> imgs = new List<Sprite>();
		foreach (var inv in inventories)
			imgs.Add(inv.items[0].itemImage);
		print("imgs: " + imgs.Count);

		voteManager.Open(true);
		voteManager.DisplayChain(imgs);
	}
	public void CancelWaitingRoom()
	{
		CancelDragDropOp();

		openWindowObj.ChangeCanClose(true);

		CheckForValidationPossibility();
	}
	public void EndWaitingRoom()
	{
		print("End waiting room before author");
		//if (!bIsAuthor)
		//{
		//	return;
		//}
		print("End waiting room after author");



		if (IsChainCorrect())
		{
			print("Chain correct");
			SuccessChaining();
			//pv.RPC("SuccessChaining", RpcTarget.All);
		}
		else
		{
			print("Wrong chain");
			//pv.RPC("FailChaining", RpcTarget.All);
			FailChaining();
		}
	}

	#endregion

	#region Chain validation



	[PunRPC]
	public void FailChaining()
	{
		print("Fail");
		StartCoroutine("FailActionChain");
	}
	public IEnumerator FailActionChain()
	{
		ShowError();

		yield return new WaitForSeconds(3);

		openWindowObj.ChangeCanClose(true);

		CancelWaitingRoom();
		ShowError(false);
	}

	[PunRPC]
	public void SuccessChaining()
	{
		StartCoroutine("SuccessActionChain");
		ScoreManager.instance.ChangeScore(100);
	}
	public IEnumerator SuccessActionChain()
	{
		ShowError();

		yield return new WaitForSeconds(4);

		GetComponent<SuccessAction>().SuccessAct();
	}


	public bool IsChainCorrect()
	{
		for (int i = 0; i < correctChain.Count; i++)
		{
			if (!IsNodeCorrect(i))
				return false;
		}

		return true;
	}
	public bool IsNodeCorrect(int indexNode)
	{
		return correctChain[indexNode] == inventories[indexNode].items[0].itemName;
	}
	public void ShowError(bool bShow = true)
	{
		for (int i = 0; i < inventories.Count; i++)
		{
			if (!bShow)
			{
				pv.RPC("SetNodeState", RpcTarget.All, i, 0);
			}
			else
			{
				int state = IsNodeCorrect(i) ? 2 : 1;
				pv.RPC("SetNodeState", RpcTarget.All, i, state);
			}
		}
	}
	[PunRPC] public void SetNodeState(int indexNode, int state)
	{
		bool b0 = false;
		bool b1 = false;

		if (state == 1)
			b0 = true;
		else if (state == 2)
			b1 = true;


		inventories[indexNode].transform.GetChild(0).gameObject.SetActive(b0);
		inventories[indexNode].transform.GetChild(1).gameObject.SetActive(b1);
	}

	public void ShowErrorToAll()
	{

	}





	#endregion
}
