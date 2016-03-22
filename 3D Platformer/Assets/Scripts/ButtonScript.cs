using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void MainMenu() {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().MainMenu();
    }
    public void GameOver() {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().GameOver();
    }
    public void ExitGame() {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().ExitGame();
    }
    public void LevelOne() {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().LevelOne();
    }
    public void WinScreen() {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().WinScreen();
    }
    public void Unpause() {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().Unpause();
    }
}
