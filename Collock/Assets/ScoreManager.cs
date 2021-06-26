using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
	public static ScoreManager instance;
	private PhotonView pv;

	///==========================================================================================================
	///		GENERAL
	///==========================================================================================================

	#region Unity Buil-in functions

	void Awake()
	{
		if (!instance)
			instance = this;
		else
			Destroy(gameObject);

		pv = GetComponent<PhotonView>();
		displayText = GetComponentInChildren<TextMeshProUGUI>();
	}
	private void Start()
	{
		SetupScore();
	}
	void Update()
	{

	}

	#endregion




	///==========================================================================================================
	///		SCORE
	///==========================================================================================================

	#region variables

	private int score;
	private TextMeshProUGUI displayText;

	#endregion

	#region Score Management functions

	private void SetupScore()
	{
		score = 0;

		UpdateDisplay();
	}

	public int GetScore()
	{
		return score;
	}
	[PunRPC]
	public void ChangeScore(int change, bool bAlreadyRep=false)
	{
		if(!bAlreadyRep)
		{
			pv.RPC("ChangeScore", RpcTarget.All, change,true);
			return;
		}

		score += change;
		UpdateDisplay();
	}

	#endregion

	#region Displaying functions

	private void UpdateDisplay()
	{		
		displayText.text = score + " pts";
	}

	#endregion


}
