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
		GameManager.instance.pv.RPC("OnSceneChange",RpcTarget.All);

		if (PhotonNetwork.IsConnected)
		{
				PhotonNetwork.LoadLevel(sceneToLoad);
		}
		else
			SceneManager.LoadScene(sceneToLoad);
	}
}
