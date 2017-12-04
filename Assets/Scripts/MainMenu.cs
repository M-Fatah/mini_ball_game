using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour 
{
	public AudioClip buttonClip;

	public void LoadScene(int index)
	{
		SoundsManager.instance.PlaySFX(buttonClip);

		SceneManager.LoadScene(index);
	}

    public void QuitGame()
    {
		SoundsManager.instance.PlaySFX(buttonClip);

        Application.Quit();
    }
}
