using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDPickup : Pickable
{
	public DDItem ddItem;
	public override Item GetItem() { return ddItem; }
}
