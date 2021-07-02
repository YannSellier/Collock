using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextCodeDisplayer : MonoBehaviour
{
	public List<TextMeshProUGUI> textMeshes;


	public void Awake()
	{
		ResetDisplay();
	}



	public void DisplayText(string text)
	{
		int i = 0;
		for (i = 0; i < text.Length && i < textMeshes.Count; i++)
		{
			DisplayLetter(i, ""+text[i]);
		}
		while(i<textMeshes.Count)
		{
			DisplayLetter(i, "");
			i++;
		}
	}
	public void DisplayLetter(int indexLetter, string letter)
	{
		textMeshes[indexLetter].text = letter;
	}

	public void ResetDisplay()
	{
		DisplayText("");
	}

	


}
