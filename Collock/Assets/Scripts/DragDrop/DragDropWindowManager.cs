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

	public List<DDTrigger> ddts;

	public bool bIsUsed = false;
	public bool bIsAuthor = false;

	public bool bCancelContentOnClose = true;

	public void Awake()
	{
		pv = GetComponent<PhotonView>();
	}

	public virtual void OnOpen()
	{
		GameManager.instance.localPlayer.OpenDDInterface(true);
	}

	public void OnClose()
	{
		GameManager.instance.localPlayer.OpenDDInterface(true);

		CancelDragDropOp();

		OpenValidationWindow(false);
	}


	public void CancelDragDropOp()
	{
		SetIsAuthor(false);
		SetIsUsed(false);

		if(bCancelContentOnClose) CancelAllContent();
	}


	public virtual void OnDrag(DDTrigger ddt)
	{

		//if (!ddts.Contains(ddt)) return;

		//OnDragFromThis(ddt);
	}
	public virtual void OnDragFromThis(DDTrigger ddt)
	{
		CheckForValidationPossibility();
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
		if(validationWindow)
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


	public void CheckForValidationPossibility()
	{
		if (!bIsAuthor && bIsUsed) return;

		foreach (var inv in inventories)
		{
			if (inv.ItemCount() == 0)
			{
				OpenValidationWindow(false);
				return;
			}
		}
		OpenValidationWindow(true);
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
