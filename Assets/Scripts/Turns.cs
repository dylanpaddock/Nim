using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turns : MonoBehaviour {
    private enum Player {ONE, TWO}
    private Player current = Player.ONE;
    private Text displayText;
    public ComputerPlayer computerPlayer;

    private bool useComputer1 = false;//player 1 is a computer
    private bool useComputer2 = false;//player 2 is a computer
    private int state = 1;
    public Text opponentName;
	// Use this for initialization
	void Start () {
        displayText = GetComponent<Text>();
        state = 0;
        toggleOpponent(); //easy AI as default opponent
	}

	// Update is called once per frame
	void Update () {

        //set string to "player one/two"
	}

    void OnGUI(){
        displayText.text = CurrentPlayer();
    }

    public void ChangePlayer(){
        current = (current == Player.ONE)? Player.TWO : Player.ONE;
    }

    public string CurrentPlayer(){
        return (current == Player.ONE)? "Player One" : "Player Two";
    }

    private bool isComputer(Player p){
        return (p == Player.ONE && useComputer1) || (p == Player.TWO && useComputer2);

    }

    public bool isComputerTurn(){
        return isComputer(current);
    }

    public void toggleOpponent(){
        state += 1;
        string name;
        Debug.Log(state);
        if (state%4 == 0){
            useComputer2 = false;
            name = "human";
        }else if(state%4 == 1){
            //easy ai
            useComputer2 = true;
            computerPlayer.difficulty = ComputerPlayer.Difficulty.EASY;
            name = "easy AI";
        }else if(state%4 == 2){
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
