using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UiManager : MonoBehaviour
{
	public Image liveImageDisplay;
	public TextMeshProUGUI scoreText;
	public Sprite[] lives;
	public GameObject titleScreen;
	public int score;


	public void UpdateLives(int currentLives)
	{
		Debug.Log("Current Lives: " + currentLives);
		liveImageDisplay.sprite = lives[currentLives];


	}


	public void UpdateScore()
	{
		score +=10;
		scoreText.text = "Score: " + score;
	}

	public void ShowTitleScreen()
	{
		titleScreen.SetActive(true);
	}

	public void HideTitleScreen()
	{

		titleScreen.SetActive(false);
		scoreText.text = "Score:00";
	}

}
