using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplayer : MonoBehaviour
{

	///==========================================================================================================
	///		UNITY BUIL-IN
	///==========================================================================================================

	#region Unity Buil-in functions

	void Awake()
    {
		if (inv) inv.InitDisplayer(this);
    }
	private void Start()
	{
		UpdateDisplay();
	}
	void Update()
    {
        
    }

	#endregion





	///==========================================================================================================
	///		GENERAL
	///==========================================================================================================

	#region Inventory variables

	public Inventory inv;

	#endregion



	///==========================================================================================================
	///		INVENTORY DISPLAY
	///==========================================================================================================

	#region Inv display variables

	public Image imageHolder;

	private int selectedItemIndex = 0;

	#endregion


	#region Inv diaply functions

	public void UpdateDisplay()
	{
		KeepSelecInBound(selectedItemIndex);
		imageHolder.sprite = GetSelectedItemImg();
	}

	private Sprite GetSelectedItemImg()
	{
		print(inv);
		print(inv.GetItemAt(selectedItemIndex));
		return inv.ItemCount() > 0 ? inv.GetItemAt(selectedItemIndex).itemImage : null;
	}

	#endregion

	#region Item selection functions

	public void NextItem(int dir)
	{
		selectedItemIndex = KeepSelecInBound(selectedItemIndex + dir);
		UpdateDisplay();
	}
	public int KeepSelecInBound(int index)
	{
		return Mathf.Clamp(index, 0, inv.ItemCount()-1);
	}

	#endregion

	#region Item use functions

	public void UseItem()
	{
		inv.UseItem(selectedItemIndex);
	}

	#endregion
}
