using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SuccessAction : MonoBehaviour
{
	public int sceneToLoad = 2;

	public void SuccessAct()
	{
		if (PhotonNetwork.IsConnected)
		{
			if (PhotonNetwork.IsMasterClient)
				PhotonNetwork.LoadLevel(sceneToLoad);
		}
		else
			SceneManager.LoadScene(sceneToLoad);
	}
}
