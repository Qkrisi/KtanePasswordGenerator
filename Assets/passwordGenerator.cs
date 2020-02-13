﻿using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using KModkit;

public class passwordGenerator : MonoBehaviour {
	public KMBombInfo Bomb;
	public KMSelectable[] keypad;
	public KMSelectable clearButton;
	public KMSelectable submitButton;
	public TextMesh Screen;
	
	private int pressedNumber = 0;
	private bool inputMode = false;
	private string submitKey = "";
	private string generatedInput = "";
	private string convertedPorts = "";
	private string convertedBatteries = "";
	private string convertedIndicators = "";
	private string convertedLetter = "";
	private string symbol ="";

	// Logging
	static int moduleIdCounter = 1;
	int moduleId;
	

	// Use this for initialization
	void Awake () {
		
		moduleId = moduleIdCounter++;

		// Assigning buttons
		foreach (KMSelectable key in keypad)
		{
			KMSelectable pressedKey = key;
			key.OnInteract += delegate () { PressKey(pressedKey);
			return false; };
		}
		clearButton.OnInteract += delegate () 
			{ resetInput(clearButton);  return false; };
		submitButton.OnInteract += delegate () 	
			{ submitInput(submitButton); return false; };
		ScreenDisplay(submitKey);
		inputMode = true;
		Debug.LogFormat("[Password Generator #{0}]: The module is waiting for submit button press to start the calculation.", moduleId);
	}
	// When a key is pressed
	void PressKey (KMSelectable key)
	{
		GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, key.transform);
		key.AddInteractionPunch(0.25f);
		if (inputMode == true)
		{
			submitKey += key.GetComponentInChildren<TextMesh>().text.ToString();
			ScreenDisplay(submitKey);
			pressedNumber = submitKey.Length;	
			checkInput(pressedNumber);
		}
	}
	
	// Displays the screen
	private void ScreenDisplay(string str) 
	{
		Screen.text = str;
	}
	// Reset the input
	private void resetInput (KMSelectable clearButton)
		{
		GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, clearButton.transform);
		clearButton.AddInteractionPunch(0.25f);
		if (inputMode == true)
			{	
			submitKey = "";
			pressedNumber = 0;
			ScreenDisplay(submitKey);
			}
		}
	// Submit the number
	private void submitInput (KMSelectable submitButton)
		{
		GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, submitButton.transform);
		submitButton.AddInteractionPunch(0.25f);
		if (inputMode == true)
			{
			if (Bomb.GetModuleNames().Contains("Bamboozled Again") || Bomb.GetModuleNames().Contains("Ultimate Cycle") || Bomb.GetModuleNames().Contains("UltraStores"))
			{
				generatedInput = "*DEAD*";
				Debug.LogFormat("[Password Generator #{0}]: Bamboozled Again, UltraStores or Ultimate Cycle present, ignoring all other rules.", moduleId);
				Debug.LogFormat("[Password Generator #{0}]: The correct input is *DEAD*.", moduleId);
			}
			else 
			{
				Debug.LogFormat("[Password Generator #{0}]: Submit button was pressed! Generating input..", moduleId);
				//Part I: First letter of SN
				char firstletter = Bomb.GetSerialNumberLetters().First();
				var firstCharPos = char.ToUpperInvariant(firstletter) - 'A' + 1;
				Debug.LogFormat("[Password Generator #{0}]: The numerical position of the first character of serial number is {1}", moduleId, firstCharPos);
				
				if (firstCharPos % 6 + 1 == 1)
				{ convertedLetter = "A"; }
				else if (firstCharPos % 6 + 1 == 2)
				{ convertedLetter = "B"; }
				else if (firstCharPos % 6 + 1 == 3)
				{ convertedLetter = "C"; }
				else if (firstCharPos % 6 + 1 == 4)
				{ convertedLetter = "D"; }
				else if (firstCharPos % 6 + 1 == 5)
				{ convertedLetter = "E"; }
				else if (firstCharPos % 6 + 1 == 6)
				{ convertedLetter = "F"; }
				Debug.LogFormat("[Password Generator #{0}]: The calculated answer for Part I is {1}.", moduleId, convertedLetter);
				
				//Part II: Indicators, Batt, Ports
				if (Bomb.GetBatteryCount() % 6 + 1 == 1)
				{ convertedBatteries = "A"; }
				else if (Bomb.GetBatteryCount() % 6 + 1 == 2)
				{ convertedBatteries = "B"; }
				else if (Bomb.GetBatteryCount() % 6 + 1 == 3)
				{ convertedBatteries = "C"; }
				else if (Bomb.GetBatteryCount() % 6 + 1 == 4)
				{ convertedBatteries = "D"; }
				else if (Bomb.GetBatteryCount() % 6 + 1 == 5)
				{ convertedBatteries = "E"; }
				else if (Bomb.GetBatteryCount() % 6 + 1 == 6)
				{ convertedBatteries = "F"; }

				if (Bomb.GetPortCount() % 6 + 1 == 1)			
					{convertedPorts = "A";	}
				else if (Bomb.GetPortCount() % 6 + 1 == 2)
					{convertedPorts = "B";	}
				else if (Bomb.GetPortCount() % 6 + 1 == 3)
					{convertedPorts = "C";	}
				else if (Bomb.GetPortCount() % 6 + 1 == 4)
					{convertedPorts = "D";	}
				else if (Bomb.GetPortCount() % 6 + 1 == 5)
					{convertedPorts = "E";	}
				else if (Bomb.GetPortCount() % 6 + 1 == 6)
					{convertedPorts = "F";	}

				if (Bomb.GetIndicators().Count() % 6 + 1 == 1)
					{ convertedIndicators = "A"; }
				else if (Bomb.GetIndicators().Count() % 6 + 1 == 2)
					{ convertedIndicators = "B"; }
				else if (Bomb.GetIndicators().Count() % 6 + 1 == 3)
					{ convertedIndicators = "C"; }
				else if (Bomb.GetIndicators().Count() % 6 + 1 == 4)
					{ convertedIndicators = "D"; }
				else if (Bomb.GetIndicators().Count() % 6 + 1 == 5)
					{ convertedIndicators = "E"; }
				else if (Bomb.GetIndicators().Count() % 6 + 1 == 6)
					{ convertedIndicators = "F"; }

				Debug.LogFormat("[Password Generator #{0}]: Number of batteries = {1}, indicators = {2}, and ports = {3}", moduleId, Bomb.GetBatteryCount(), Bomb.GetIndicators().Count(), Bomb.GetPortCount());
				
				string correctPart2 = "";
				if ( Bomb.GetIndicators().Count() == Bomb.GetBatteryCount() || Bomb.GetIndicators().Count() == Bomb.GetPortCount() || Bomb.GetBatteryCount() == Bomb.GetPortCount()  )
				{
				Debug.LogFormat("[Password Generator #{0}]: There are equal number of batteries, indicators or ports, reversing the order", moduleId);
				correctPart2 = convertedPorts + convertedIndicators + convertedBatteries;
				Debug.LogFormat("[Password Generator #{0}]: The calculated answer for Part II is {1}", moduleId, correctPart2);
				}
				else
				{
				Debug.LogFormat("[Password Generator #{0}]: There are different number of batteries, indicators and ports.", moduleId);
				correctPart2 = convertedBatteries + convertedIndicators + convertedPorts;

				Debug.LogFormat("[Password Generator #{0}]: The calculated answer for Part II is {1}", moduleId, correctPart2);
				}
				// Part III: Symbols
				if (Bomb.GetModuleNames().Contains("Question Mark"))
					{
						symbol = "?"; 
						Debug.LogFormat("[Password Generator #{0}]: Rule 1 applied: There are Question Mark module on the bomb.", moduleId);
					}
				else if (Bomb.GetModuleNames().Contains("Astrology"))
					{	
						symbol = "*";
						Debug.LogFormat("[Password Generator #{0}]: Rule 2 applied: There are Astrology module on the bomb.", moduleId);
					}
				else if (Bomb.GetModuleNames().Contains("Logic") || Bomb.GetModuleNames().Contains("logic") || Bomb.GetModuleNames().Contains("Boolean") || Bomb.GetModuleNames().Contains("boolean") )
					{	
						symbol = "&";
						Debug.LogFormat("[Password Generator #{0}]: Rule 3 applied: There are at least one module with 'Logic' or 'Boolean' in its name on the bomb.", moduleId);
					}
				else if (Bomb.GetModuleNames().Contains("Code") || Bomb.GetModuleNames().Contains("code"))
					{	
						symbol = "/" ;
						Debug.LogFormat("[Password Generator #{0}]: Rule 4 applied: There are at least one module with 'Code' in its name on the bomb.", moduleId);
					}
				else if (Bomb.GetModuleNames().Contains("Alphabet") || Bomb.GetModuleNames().Contains("alphabet"))
					{	
						symbol = "@";
						Debug.LogFormat("[Password Generator #{0}]: Rule 5 applied: There are at least one module with 'Alphabet' in its name on the bomb.", moduleId);
					}
				else
					{	
						symbol = "-";
						Debug.LogFormat("[Password Generator #{0}]: Otherwise rule.", moduleId);
					}
				// Part IV: Solved, Unsolved, Minutes Remaining
				int solvedCount = Bomb.GetSolvedModuleNames().Count;
				int unsolvedCount = Bomb.GetModuleNames().Count - Bomb.GetSolvedModuleNames().Count;
				int minutesRemaining = (int) Bomb.GetTime() / 60;

				Debug.LogFormat("[Password Generator #{0}] The submit button was pressed at {1} solve(s), {2} unsolved and {3} min(s) remaining.", moduleId, solvedCount, unsolvedCount, minutesRemaining);
				int correctPart4 = (solvedCount * unsolvedCount * minutesRemaining) % 100;
				Debug.LogFormat("[Password Generator #{0}] The calculated answer for Part IV is {1}.", moduleId, correctPart4);


				// Part V: Last digit
				int lastDigit = Bomb.GetSerialNumberNumbers().Last();
				Debug.LogFormat("[Password Generator #{0}]: The last digit of the Serial Number is {1}.", moduleId, lastDigit);

				//Contenating the calculated answer
				generatedInput = convertedLetter + correctPart2 + symbol + correctPart4 + lastDigit;
				Debug.LogFormat("[Password Generator #{0}]: The final input is {1}.", moduleId, generatedInput);
			}
			//Whether is the input correct
			if (submitKey == generatedInput)
			{
				GetComponent<KMBombModule>().HandlePass();
				Debug.LogFormat("[Password Generator #{0}]: You have inputted correct answer. Module solved.", moduleId);
				submitKey = "SOLVED";
				ScreenDisplay(submitKey);
				inputMode = false;
			}
			else 
			{
				GetComponent<KMBombModule>().HandleStrike();
				Debug.LogFormat("[Password Generator #{0}]: You have inputted {1}, which is a wrong answer. Module striked and reset.", moduleId, submitKey);
				StartCoroutine(DisplayError(submitKey));
				pressedNumber = 0;
				ScreenDisplay(submitKey);
				generatedInput = "";
			}
		}
	}
	// Strike if received too many inputs
	void checkInput(int pressedNumber)
	{
		if (pressedNumber > 7)
		{
			GetComponent<KMBombModule>().HandleStrike();
			StartCoroutine(DisplayBlink(submitKey));	
			pressedNumber = 0;
			ScreenDisplay(submitKey);
			Debug.LogFormat("[Password Generator #{0}]: You have inputted too many characters. Module striked and reset.", moduleId);

		}
	}
    	IEnumerator DisplayBlink(string checkInput) {
		inputMode = false;
		for (var i = 0; i < 3; i++) {
			submitKey = "-------";
			ScreenDisplay(submitKey);
            yield return new WaitForSeconds(0.5f);
			submitKey = "";
			ScreenDisplay(submitKey);
            yield return new WaitForSeconds(0.5f);
		}
		inputMode = true;
		}
		IEnumerator DisplayError(string submitInput) 
		{
			inputMode = false;
			yield return new WaitForSeconds(0.1f);
			submitKey = "-WRONG-";
			ScreenDisplay(submitKey);
			yield return new WaitForSeconds(3f);
			submitKey = "-WRONG";
			ScreenDisplay(submitKey);
			yield return new WaitForSeconds(0.5f);
			submitKey = "-WRON";
			ScreenDisplay(submitKey);
			yield return new WaitForSeconds(0.5f);
			submitKey = "-WRO";
			ScreenDisplay(submitKey);
			yield return new WaitForSeconds(0.5f);
			submitKey = "-WR";
			ScreenDisplay(submitKey);
			yield return new WaitForSeconds(0.5f);
			submitKey = "-W";
			ScreenDisplay(submitKey);
			yield return new WaitForSeconds(0.5f);
			submitKey = "-";
			ScreenDisplay(submitKey);
			yield return new WaitForSeconds(0.5f);
			submitKey = "";
			ScreenDisplay(submitKey);
			yield return new WaitForSeconds(0.5f);
			inputMode = true;
		}
}