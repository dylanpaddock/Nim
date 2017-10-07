using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerPlayer : MonoBehaviour {
    //Plays the game for the computer. Hard difficulty is optimal play and will
    //win any game when taking the first turn. Easy and Medium difficulties
    //have a chance of making a nonoptimal (random) move.

    public enum Difficulty {EASY, MEDIUM, HARD};
    public Difficulty difficulty; //How close to optimal the computer plays.
    public float waitTime; //How long to wait before removing stones.
    private float easyChance = .2f; //Probability of making nonoptimal moves.
    private float mediumChance = .1f; //Probability of making nonoptimal moves.

    public Pieces pieces; //Access to all groups and stones.
    public List<GameObject> selectedList; //Contains the selected stones.
    public Group selectedGroup; //Contains the selected group.

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

    //Given the index of a group in a list of groups and a number of stones to
    //remove, add that many stones in the group to the selection.
    private void SelectStones(int groupIndex, int stonesToRemove){
        selectedGroup = pieces.groups[groupIndex].GetComponent<Group>();
        selectedList = new List<GameObject>();
        for (int i=0; i < stonesToRemove; i++){
            selectedList.Add(selectedGroup.stonesList[i]);
        }
    }

    //Make a selection of a random number of stones from a random group. Always
    //selects at least one stone. The input is a list of ints representing the
    //current state of the board: each int is the size of a group.
    private void SelectRandom(List<int> stonesList){
        int groupIndex = Random.Range(0, stonesList.Count);
        if (stonesList[groupIndex] == 0){
            Debug.LogError("Error: randomed to an empty group.");
        }
        int stonesToRemove = Random.Range(1, stonesList[groupIndex] + 1);
        SelectStones(groupIndex, stonesToRemove);
    }

    //Makes a selection of the optimal move, if it exists or according to
    //difficulty. Otherwise chooses a random move.
    public void ChooseStones(){
        List<int> stonesList = new List<int>();//Accumulate size of each pile.
        foreach (GameObject g in pieces.groups){
            stonesList.Add(g.GetComponent<Group>().stonesList.Count);
        }
        //Calculate the Nim sum: the sizes of all piles XORed together. A value
        //of zero means no winning move is possible under optimal play.
        int nimsum = 0;
        foreach( int pileSize in stonesList){
            nimsum = nimsum ^ pileSize;
        }
        float r = Random.value;
        //Choosing a random move (for difficulty reasons or no optimal move).
        if (nimsum == 0 || (difficulty == Difficulty.EASY && easyChance < r) ||
        (difficulty == Difficulty.MEDIUM && mediumChance < r)){
            SelectRandom(stonesList);
        }else{
            //Find move the optimal move that sets the Nim sum to 0.
            for (int i = 0; i < stonesList.Count; i++){
                if ((stonesList[i]^nimsum) < stonesList[i]){
                    SelectStones(i, stonesList[i] - (stonesList[i]^nimsum));
                    return;
                }
            }
            //This code should never execute.
            Debug.LogError("Error: No optimal move was found.");
        }
    }

}
