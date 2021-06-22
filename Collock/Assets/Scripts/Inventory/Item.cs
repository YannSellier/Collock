using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
	public string itemName = "None";


	public Item(string itemName="None")
	{
		this.itemName = itemName;
	}

	public virtual void Use()
	{
		Debug.Log(itemName + " got Used");
	}
}
