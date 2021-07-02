using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PingManager : MonoBehaviour,IOpen
{
	public PhotonView pv;
	public Image pingImg;
	private bool bPingLaunched = false;

	public Sprite pingLaunchedImg;

	public void Awake()
	{
		pv = GetComponent<PhotonView>();
	}

	[PunRPC] public void LaunchPing(bool bRep = true)
	{
		if (bPingLaunched) return;


		if(bRep)
		{
			pv.RPC("LaunchPing", RpcTarget.All, false);
			return;
		}

		bPingLaunched = true;
		StartCoroutine("PingProcess");
	}

	IEnumerator PingProcess()
	{

		pingImg.sprite = pingLaunchedImg;

		yield return new WaitForSeconds(4);

		GetComponent<SuccessAction>().SuccessAct();
	}

	public void OnOpen()
	{
		print("Open");
	}
	public void OnClose()
	{

	}
}
