using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragDropOp: MonoBehaviour 
{
	PhotonView pv;
	private Item item;
	private Inventory invStart;

	public Image itemImg;

	public void Awake()
	{
		pv = GetComponent<PhotonView>();
	}

	public void SetupDragDrop(Item item, int invIndex)
	{
		

		invStart = GameManager.instance.localPlayer.invs[invIndex];
		this.item = item;

		RemoveItemFromInv(item.itemName, invIndex);

		itemImg.sprite = item.itemImage;
		itemImg.gameObject.SetActive(true);
	}

	[PunRPC]
	public void RemoveItemFromInv(string itemName, int invIndex, bool bRep = true)
	{
		if (bRep)
		{
			pv.RPC("RemoveItemFromInv", RpcTarget.All, itemName, invIndex, false);
			return;
		}


		GameManager.instance.localPlayer.invs[invIndex].RemoveItem(itemName);
	}
	[PunRPC]
	public void AddItemToInv(int indexItem, int invIndex, bool bRep = true)
	{
		if (bRep)
		{
			pv.RPC("AddItemToInv", RpcTarget.All, indexItem, invIndex, false);
			return;
		}

		print("Add item " + indexItem + " to inv " + invIndex);

		Player p = GameManager.instance.localPlayer;
		p.invs[invIndex].AddItem(p.itemBank.ddItems[indexItem]);
	}




	public void UpdatePosition(Vector2 mousePos)
	{
		itemImg.transform.position = mousePos;
	}


	public void EndOp(DDTrigger ddSlot)
	{
		Player p = GameManager.instance.localPlayer;
		int indexItem = p.itemBank.GetDDItemIndex(item);
		print("try to add " + indexItem);
		itemImg.gameObject.SetActive(false);
		if (ddSlot && ddSlot.bAuthorizeDrop)
		{
			int indexInv = p.GetIndexInv(ddSlot.inv);
			if (ddSlot.inv.CanAddItem())
			{
				AddItemToInv(indexItem, indexInv);
			}
			else
			{
				Item targetItem = ddSlot.inv.items[0];
				int indexTargetItem = p.itemBank.GetDDItemIndex(targetItem);
				int indexInvStart = p.GetIndexInv(invStart);

				RemoveItemFromInv(targetItem.itemName, indexInv);
				AddItemToInv(indexTargetItem, indexInvStart);
				AddItemToInv(indexItem, indexInv);
			}
		}
		else
		{
			int indexInv = p.GetIndexInv(invStart);
			AddItemToInv(indexItem, indexInv);
		}
	}

}
