using UnityEngine;

public class PlayerUI : MonoBehaviour {

	[SerializeField]
	GameObject pauseMenu;

	void Start () 
	{
		PauseMenu.IsOn = false;
	}

	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			TogglePauseMenu ();
		}

	}

	public void TogglePauseMenu()
	{
		pauseMenu.SetActive (!pauseMenu.activeSelf);
		PauseMenu.IsOn = pauseMenu.activeSelf;
	}
}
