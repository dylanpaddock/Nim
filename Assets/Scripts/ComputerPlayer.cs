using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerPlayer : MonoBehaviour {
    public enum Difficulty {EASY, MEDIUM, HARD};
    public Difficulty difficulty;
    public Pieces pieces;
    public float waitTime;
    public float easyChance = .2f;
    public float mediumChance = .1f;

    public List<GameObject> selectedList;
    public Group selectedGroup;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

    private void SelectStones(int groupIndex, int stonesToRemove){//given the number of stones, select them
        //choose stones to remove
        Debug.Log("[Comp] group: " + groupIndex + ", stones: " + stonesToRemove);
        selectedGroup = pieces.groups[groupIndex].GetComponent<Group>();
        selectedList = new List<GameObject>();
        for (int i=0; i < stonesToRemove; i++){
            selectedList.Add(selectedGroup.stonesList[i]);
        }
    }

    private void SelectRandom(List<int> stonesList){//choose a number of stones randomly and select them
        bool isEmpty = true;
        int groupIndex = -1;
        while (isEmpty){
            groupIndex = Random.Range(0, stonesList.Count);
            isEmpty = stonesList[groupIndex] == 0;
            Debug.Log("randomed to empty group, try again...");
        }
        int stonesToRemove = Random.Range(1, stonesList[groupIndex] + 1);
        Debug.Log("[Comp] random group: " + groupIndex + ", stones: " + stonesToRemove);
        SelectStones(groupIndex, stonesToRemove);
    }

    public void ChooseStones(){//choose stones acording to difficulty
        //for Easy, choose randomly 50%
        //for medium, choose randomly 20%
        //for hard, choose optimal move
        List<int> stonesList = new List<int>();//accumulate size of each pile
        foreach (GameObject g in pieces.groups){
            stonesList.Add(g.GetComponent<Group>().stonesList.Count);
        }
        int nimsum = 0;
        foreach( int pileSize in stonesList){
            nimsum = nimsum ^ pileSize;
        }
        float r = Random.value;
        if (nimsum == 0 || (difficulty == Difficulty.EASY && easyChance < r) || (difficulty == Difficulty.MEDIUM && mediumChance < r)){
            //choose randomly if no good move or random chance
            SelectRandom(stonesList);
        }else{
            //find move to set nimsum to 0
            for (int i = 0; i < stonesList.Count; i++){
                if ((stonesList[i]^nimsum) < stonesList[i]){
                    Debug.Log("[Comp] stonesList[i]: "+stonesList[i] + ", nimsum: " + nimsum);
                    SelectStones(i, stonesList[i] - (stonesList[i]^nimsum));
                    return;
                }
            }
            Debug.Log("[Comp] no selection found");
        }
    }

}
