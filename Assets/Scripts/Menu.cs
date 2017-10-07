using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {
    //Controls the game's menu for starting or ending a game.
    Text displayText; //The menu's text object to display.
    public UnitSelector us;
    public Pieces pieces;
    private bool first = true; //This is the first game.

	// Use this for initialization
	void Start () {
        displayText = GetComponent<Text>();
        displayText.text = "Menu";

	}

	// Update is called once per frame
	void Update () {

	}

    //Ends the current game, shows the winner, and displays the menu.
    public void EndGame(string s){
        displayText.text = s;
        us.Pause();
        gameObject.SetActive(true);
    }

    //Starts the game, makes the stones, and hides the menu.
    public void StartGame(){

        //The first game is always the default layout and the rest random.
        if (first){
            pieces.ResetPieces(new List<int>() {5,3,4});
            first = false;
        } else {
            pieces.ResetPieces();
        }

        us.Unpause();
        gameObject.SetActive(false);
    }

    //Closes the application.
    public void CloseGame(){
        Application.Quit();
    }
}
