using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardPickup : Pickable
{
	public Item item;

	public override Item GetItem() { return item; }
}
