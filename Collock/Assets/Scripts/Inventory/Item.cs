using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
	public string itemName = "None";
	public Sprite itemImage;

	public Item(string itemName="None",Sprite itemImage=null)
	{
		this.itemName = itemName;
		this.itemImage = itemImage;
	}

	public virtual void Use()
	{
		Debug.Log(itemName + " got Used");
	}
}
