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
	private List<Item> items;
	public int ItemCount() { return items.Count; }
	public Item GetItemAt(int index) { return index < items.Count && index >= 0 ? items[index] : null; }
	public int GetItemIndex(Item item) { return items.IndexOf(item); }

	private InventoryDisplayer displayer;

	#endregion


	#region Initializing

	public void Init(List<Item> items = null)
	{
		this.items = items != null ? items : new List<Item>();
	}
	public void InitDisplayer(InventoryDisplayer id)
	{
		displayer = id;
	}

	#endregion

	#region Item List interactions

	public int AddItem(Item item, bool updateDisplay = true)
	{
		items.Add(item);

		if (updateDisplay && displayer)
			displayer.UpdateDisplay();	

		return items.Count - 1;
	}
	public bool RemoveItem(Item item, bool updateDisplay = true)
	{
		bool bSuccess = items.Remove(item);

		if (updateDisplay && displayer)
			displayer.UpdateDisplay();

		return bSuccess;
	}

	#endregion

	#region Item use functions

	public void UseItem(int indexItem)
	{
		items[indexItem].Use();
	}

	#endregion





}
