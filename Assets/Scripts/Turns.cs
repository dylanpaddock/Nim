using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turns : MonoBehaviour {
    //Keeps track of the current player's turn.
    private enum Player {ONE, TWO}
    private Player current = Player.ONE; //Player with the current turn.
    private Text displayText; //Displays the current turn.
    public ComputerPlayer computerPlayer;

    private bool useComputer1 = false; //Is this player a computer? No.
    private bool useComputer2 = false; //Is this player a computer?
    private int mode; //Sets player 2 difficulty.
    //0: human, 1: easy, 2: medium, 3: hard
    public Text opponentName; //Displays the selected mode on the menu.

    //initialization
	void Start () {
        displayText = GetComponent<Text>();
        displayText.text = CurrentPlayer();
        mode = 0;
        toggleOpponent(); //easy AI as default opponent
	}

	// Update is called once per frame
	void Update () {
    }

    //Changes the active player. ONE <==> TWO
    public void ChangePlayer(){
        current = (current == Player.ONE)? Player.TWO : Player.ONE;
        displayText.text = CurrentPlayer();
    }

    //Gives the name of the active player as a string.
    public string CurrentPlayer(){
        return (current == Player.ONE)? "Player One" : "Player Two";
    }

    //Tells if the player is a controlled by a computer.
    private bool isComputer(Player p){
        return (p == Player.ONE && useComputer1) || (p == Player.TWO && useComputer2);
    }

    //Tells if the current turn is controlled by a computer.
    public bool isComputerTurn(){
        return isComputer(current);
    }

    //Changes the opponent's mode by stepping through each possibility.
    public void toggleOpponent(){
        mode += 1;
        string name;
        if (mode%4 == 0){
            useComputer2 = false;
            name = "human";
        }else if(mode%4 == 1){
            useComputer2 = true;
            computerPlayer.difficulty = ComputerPlayer.Difficulty.EASY;
            name = "easy AI";
        }else if(mode%4 == 2){
            useComputer2 = true;
            computerPlayer.difficulty = ComputerPlayer.Difficulty.MEDIUM;
            name = "medium AI";
        }else{
            useComputer2 = true;
            computerPlayer.difficulty = ComputerPlayer.Difficulty.HARD;
            name = "hard AI";
        }
        opponentName.text = "Change Opponent: " + name;
    }
}
