using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory : MonoBehaviour
{

	///==========================================================================================================
	///		UNITY BUIL-IN
	///==========================================================================================================

	#region Components

	public PhotonView pv;

	#endregion

	#region Unity Buil-in functions

	void Awake()
	{
		pv = GetComponent<PhotonView>();
		items = new List<Item>();
	}
	private void Start()
	{
	}
	void Update()
	{

	}

	#endregion




	///==========================================================================================================
	///		INVENTORY MANAGEMENT
	///==========================================================================================================


	#region Inventory variables

	// Items list functions
	public List<Item> items;
	public int ItemCount() { return items.Count; }
	public Item GetItemAt(int index) { return index < items.Count && index >= 0 ? items[index] : null; }
	public int GetItemIndex(Item item) { return items.IndexOf(item); }

	private List<InventoryDisplayer> displayers;

	public int maxItem = 20;

	#endregion


	#region Initializing

	public void Init(List<Item> items = null)
	{
		this.items = items != null ? items : new List<Item>();
	}
	public void InitDisplayer(InventoryDisplayer id)
	{
		if (displayers == null) displayers = new List<InventoryDisplayer>();
		displayers.Add(id);
	}

	#endregion

	#region Item List interactions

	public bool CanAddItem()
	{
		return maxItem > items.Count;
	}

	public int AddItem(Item item, bool updateDisplay = true)
	{
		if (!CanAddItem()) return -1;

		items.Add(item);

		if (updateDisplay)
			UpdateAllDisplay();	

		return items.Count - 1;
	}
	public bool RemoveItem(Item item, bool updateDisplay = true)
	{
		bool bSuccess = items.Remove(item);

		if (updateDisplay)
			UpdateAllDisplay();


		return bSuccess;
	}
	public void RemoveItem(string itemName, bool updateDisplay = true)
	{
		foreach(var item in items)
		{
			if(item.itemName == itemName)
			{

				ResetDisplayerSelection();
				RemoveItem(item, updateDisplay);
				return;
			}
		}
	}

	public void InvTransfer(Inventory newInv)
	{
		while(items.Count > 0)
		{
			newInv.AddItem(items[0]);
			RemoveItem(items[0]);
		}
	}

	#endregion

	#region Item use functions

	public void UseItem(int indexItem)
	{
		items[indexItem].Use();
	}

	#endregion

	#region Inv display

	public void UpdateAllDisplay()
	{
		if (displayers == null) return;
		foreach(var displayer in displayers)
		{
			if (displayer && displayer.enabled) displayer.UpdateDisplay();
		}
	}

	public void ResetDisplayerSelection()
	{
		foreach (var displayer in displayers)
			if(displayer)
				displayer.ResetSelection();
	}

	#endregion


	#region Sync functions

	[PunRPC]
	public void RPC_RemoveItem(int indexItem,bool bRep)
	{
		if(bRep)
		{
			pv.RPC("RPC_RemoveItem", RpcTarget.All, indexItem, false);
			return;
		}

		RemoveItem(items[indexItem]);
	}

	#endregion





}
