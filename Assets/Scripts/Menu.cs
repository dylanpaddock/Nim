using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {
    Text displayText;
    string current;
    public UnitSelector us;
    public Pieces pieces;
    private bool first = true;

	// Use this for initialization
	void Start () {
        displayText = GetComponent<Text>();
        current = "Menu";
	}

	// Update is called once per frame
	void Update () {

	}

    void OnGUI(){
        displayText.text = current;
    }

    public void EndGame(string s){
        current = s;
        us.Pause();
        gameObject.SetActive(true);
    }

    public void StartGame(){
        //start game: make stones, hide menu
        Debug.Log("starting game");

        //make stones: first game is default, the rest random.
        if (first){
            pieces.ResetPieces(new List<int>() {5,3,4});
            first = false;
        } else {
            pieces.ResetPieces();
        }

        us.Unpause();
        gameObject.SetActive(false);
    }
}
