using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragDropOp: MonoBehaviour 
{

	private Item item;
	private Inventory invStart;

	public Image itemImg;



	public void SetupDragDrop(Item item, Inventory invStart)
	{
		this.invStart = invStart;
		this.item = item;

		invStart.RemoveItem(item);

		itemImg.sprite = item.itemImage;
		itemImg.gameObject.SetActive(true);
	}


	
	public void UpdatePosition(Vector2 mousePos)
	{
		itemImg.transform.position = mousePos;
	}


	public void EndOp(DDTrigger ddSlot)
	{

		if(ddSlot && ddSlot.inv.CanAddItem())
		{
			ddSlot.inv.AddItem(item);
		}
		else
		{
			invStart.AddItem(item);
		}
		itemImg.gameObject.SetActive(false);
	}

}
