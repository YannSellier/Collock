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
	public GameObject launchBtn;
	public GameObject interventionBtn;

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
		pingImg.sprite = pingLaunchedImg;
		launchBtn.SetActive(false);
		interventionBtn.SetActive(true);
	}

	[PunRPC]
	public void Intervention(bool bRep = true)
	{
		if (bRep)
		{
			pv.RPC("Intervention", RpcTarget.All, false);
			return;
		}


		StartCoroutine("InterventionProcess");
	}
	IEnumerator InterventionProcess()
	{


		yield return new WaitForSeconds(0.5f);

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
