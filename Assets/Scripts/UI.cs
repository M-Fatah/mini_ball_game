using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
	public Text scoreText;
	public GameObject gameOverUI;
	public AudioClip buttonClip;

	private int currentScore = 0;
	private Coin[] coins;
	private Player player;

	void Start()
	{
		player = FindObjectOfType<Player>();
		coins = FindObjectsOfType<Coin>();

		if(player)
			player.OnPlayerDeath += GameOver;
		
		for(int i = 0; i < coins.Length; i++)
			coins[i].OnCoinCollected += UpdateScore;

		scoreText.text = currentScore.ToString("D4");
	}

	void UpdateScore(int newScore)
	{
		currentScore += newScore;
		scoreText.text = currentScore.ToString("D4");
	}

	void GameOver()
	{
		if(gameOverUI)
		{
			gameOverUI.SetActive(true);
		}
	}

	public void LoadScene(int index)
	{
		if(buttonClip)
			SoundsManager.instance.PlaySFX(buttonClip);
			
		SceneManager.LoadScene(index);
	}

}
