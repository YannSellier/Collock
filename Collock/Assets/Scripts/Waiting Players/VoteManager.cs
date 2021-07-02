using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VoteManager : MonoBehaviour
{
	public GameObject voteWindow;

	public void Open(bool bOpen)
	{
		voteWindow.SetActive(bOpen);
	}

	public abstract void VoteYes();
	public abstract void VoteNo();
	public abstract void DisplayProposition(string prop);
	

}
