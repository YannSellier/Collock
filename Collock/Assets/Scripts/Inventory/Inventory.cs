using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory : MonoBehaviour
{

	///==========================================================================================================
	///		UNITY BUIL-IN
	///==========================================================================================================

	#region Unity Buil-in functions

	void Awake()
	{
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
		foreach(var displayer in displayers)
		{
			if (displayer) displayer.UpdateDisplay();
		}
	}

	#endregion





}
