using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HintDisplayer : MonoBehaviour
{
	public GameObject displayWindow;
	public TextMeshProUGUI msgText;


	public void DisplayHint(string msg)
	{
		msgText.text = msg;
		displayWindow.SetActive(msg != "");
	}
}
