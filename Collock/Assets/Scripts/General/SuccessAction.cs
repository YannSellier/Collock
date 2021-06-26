using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccessAction : MonoBehaviour
{
	public int sceneToLoad = 2;

	public void SuccessAct()
	{
		if(PhotonNetwork.IsMasterClient)
			PhotonNetwork.LoadLevel(sceneToLoad);
	}
}
