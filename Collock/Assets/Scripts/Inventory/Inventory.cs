using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{


///===========================================================================================================================================================
///		INVENTORY VARIABLES
///===========================================================================================================================================================

		
#region Items variables

	// Items list functions
	private List<Item> items;
	public Item GetItemAt(int index) { return index < items.Count && index > 0 ? items[index] : null; }
	public int GetItemIndex(Item item) { return items.IndexOf(item); }

#endregion







///===========================================================================================================================================================
///		INVENTORY FUNCTIONS
///===========================================================================================================================================================


	#region Initializing

	public void Init(List<Item> items = null)
	{
		this.items = items != null ? items : new List<Item>();
	}

	#endregion

	#region Item List interactions

	public int AddItem(Item item)
	{
		items.Add(item);
		return items.Count - 1;
	}
	public bool RemoveItem(Item item)
	{
		return items.Remove(item);
	}

	#endregion
}
