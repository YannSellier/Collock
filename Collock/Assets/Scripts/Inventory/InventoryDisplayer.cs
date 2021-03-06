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
		if(bUpdateDisplayOnStart)
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

	public int selectedItemIndex = 0;
	public bool bUpdateDisplayOnStart = true;

	#endregion


	#region Inv diaply functions

	public Item GetSelectedItem()
	{
		return inv.GetItemAt(selectedItemIndex);
	}

	public void UpdateDisplay()
	{
		KeepSelecInBound(selectedItemIndex);
		Sprite sprite = GetSelectedItemImg();
		imageHolder.sprite = sprite;
		

		float imgAlbedo = imageHolder.sprite == null ? 0 : 1;
		imageHolder.color = new Color(1f, 1f, 1f, imgAlbedo);
	}

	private Sprite GetSelectedItemImg()
	{
		return inv.ItemCount() > selectedItemIndex ? inv.GetItemAt(selectedItemIndex).itemImage : null;
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
		if (inv.ItemCount() == 0) return 0;
		return Mathf.Clamp(index, 0, inv.ItemCount()-1);
	}
	public void ResetSelection()
	{
		selectedItemIndex = 0;
	}

	#endregion

	#region Item use functions

	public void UseItem()
	{
		inv.UseItem(selectedItemIndex);
	}

	#endregion
}
