using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DDTrigger : MonoBehaviour
{

	public Inventory inv;
	public InventoryDisplayer invDisplay;
	public int indexSlot;

	public bool bAuthorizeDrag = true;
	public bool bAuthorizeDrop = true;
}
