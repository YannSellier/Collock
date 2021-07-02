using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragDropWindowManager: MonoBehaviour, IOpen
{
	public PhotonView pv;

	public List<Inventory> inventories;
	public GameObject validationWindow;


	public bool bIsUsed = false;
	public bool bIsAuthor = false;

	public void Awake()
	{
		pv = GetComponent<PhotonView>();
	}

	public void OnOpen()
	{
		GameManager.instance.localPlayer.OpenDDInterface(true);
	}

	public void OnClose()
	{
		GameManager.instance.localPlayer.OpenDDInterface(true);

		CancelDragDropOp();
	}


	public void CancelDragDropOp()
	{
		SetIsAuthor(false);
		SetIsUsed(false);

		CancelAllContent();
	}


	public virtual void OnDrop()
	{
		if(!bIsUsed)
		{
			SetIsAuthor(true, false);
			SetIsUsed(true);
		}
	}
	public virtual void OpenValidationWindow(bool bOpen)
	{
		validationWindow.SetActive(bOpen);
	}

	public void CancelAllContent()
	{
		for(int i = 0; i < inventories.Count; i++)
		{
			inventories[i].InvTransfer(GameManager.instance.localPlayer.inv);
			inventories[i].UpdateAllDisplay();
		}
	}


	


	[PunRPC] public void SetIsUsed(bool newState, bool bRep=true)
	{
		if(bRep)
		{
			pv.RPC("SetIsUsed", RpcTarget.All, newState, false);
			return;
		}

		bIsUsed = newState;
	}
	[PunRPC] public void SetIsAuthor(bool newState, bool bRep=true)
	{
		if (bRep)
		{
			pv.RPC("SetIsAuthor", RpcTarget.All, newState, false);
			return;
		}

		bIsAuthor = newState;
	}

}
