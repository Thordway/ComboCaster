using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public bool menu = false;
	public bool paused = false;
	public Canvas pauseMenu;
	private CursorLockMode wantedMode = CursorLockMode.Locked;
    public bool lockCursor = true;

    public int numOfEnemies = 5;
	// Use this for initialization
	void Start () {
        if (GameObject.FindGameObjectsWithTag("GameController").Length >= 2) {
            Destroy(gameObject);
        }
        else {
            DontDestroyOnLoad(gameObject);
        }
        if (menu == false) {
		    pauseMenu.enabled = false;
		    SetCursorState();
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (menu == false) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                if (paused)
                    Unpause();
                else
                    Pause();
            }
            if (Input.GetKeyDown(KeyCode.L)) {
                lockCursor = !lockCursor;
                SetCursorState();
                if (lockCursor == false) {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }

            }
            if (numOfEnemies <= 0)
                WinScreen();
        }
	}

	// Apply requested cursor state
	void SetCursorState (){
        if (lockCursor == true)
        {
            Cursor.lockState = wantedMode;
            //Hide cursor when locking
            Cursor.visible = (CursorLockMode.Locked != wantedMode);
        }
	}
	public void Pause() {
		wantedMode = CursorLockMode.None;
		SetCursorState ();
        GameObject.FindGameObjectWithTag("Pause").GetComponent<Canvas>().enabled = true;// pauseMenu.enabled = true;
		Time.timeScale = 0;
		paused = true;
	}
	public void Unpause() {
		wantedMode = CursorLockMode.Locked;
		SetCursorState ();
        GameObject.FindGameObjectWithTag("Pause").GetComponent<Canvas>().enabled = false;
		Time.timeScale = 1;
		paused = false;
	}

	public void MainMenu() {
        menu = true;
        paused = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu",LoadSceneMode.Single);
	}
    public void GameOver() {
        menu = true;
        paused = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1;
        SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
    }
    public void ExitGame() {
        Application.Quit();
    }
    public void LevelOne() {
        numOfEnemies = 5;
        menu = false;
        paused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        SceneManager.LoadScene("LevelOne", LoadSceneMode.Single);
    }
    public void WinScreen() {
        menu = true;
        paused = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1;
        SceneManager.LoadScene("WinScreen", LoadSceneMode.Single);
    }
}
