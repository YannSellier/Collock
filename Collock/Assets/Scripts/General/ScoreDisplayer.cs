using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplayer : MonoBehaviour
{


	private TextMeshProUGUI displayText;

	// Start is called before the first frame update
	void Awake()
    {
		displayText = GetComponentInChildren<TextMeshProUGUI>();
		ScoreManager.instance.DeclareDisplayer(this);
    }

	public void UpdateDisplay(int score)
	{
		displayText.text = score + " pts";
	}

}
