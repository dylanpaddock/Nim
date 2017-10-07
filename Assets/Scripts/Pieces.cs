using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pieces : MonoBehaviour {
    //A container to access all groups and stones in the game. Handles board
    //reset as well.
    public Object groupPrefab; //Prefabricated empty group.
    public Object stonePrefab; //Prefabricated stone.

    //Maximum numbers of groups and stones to generate.
    private List<int> groupBounds = new List<int>() {3,13};
    private List<int> stoneBounds = new List<int>() {1,32};

    public List<GameObject> groups;
    public UnitSelector us;

    //initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
	}

    //Reset the board randomly. Creates new groups and stones with numbers
    //within the range of groupBounds and stoneBounds.
    public void ResetPieces(){
        //Random number of groups.
        int numGroups = Random.Range(groupBounds[0], groupBounds[1]);
        List<int> li = new List<int>();
        //Random stones in each group.
        for (int i = 0; i<numGroups; i++){
            li.Add(Random.Range(stoneBounds[0], stoneBounds[1]));
        }
        ResetPieces(li);
    }

    //Resets the board according to a specified list of group sizes.
    public void ResetPieces(List<int> numPieces){
        //Remove old groups from the board.
        if (groups != null){
            foreach (GameObject g in groups){
                g.SetActive(false);
                Object.Destroy(g);
            }
        }
        groups = new List<GameObject>();
        for (int i = 0; i < numPieces.Count; i++){
            //Set up and instantiate each group of stones with screen position
            // spaced evenly.
            Vector3 groupPos = Camera.main.ScreenToWorldPoint(new Vector3(
                           Screen.width*(1f + i)*1f/(numPieces.Count + 1f),
                           Screen.height/2, 100));
            Quaternion rot = Quaternion.identity;
            GameObject newGroup = (GameObject) Instantiate(groupPrefab,
                                                    groupPos, rot, transform);
            List<GameObject> stones = new List<GameObject>();
            for (int j = 0; j < numPieces[i]; j++){
                //Find the stone position.
                float yOffset = ((1f + j)*1f/(numPieces[i] + 1f) - .5f)*2f;
                Vector3 stonePos = new Vector3(groupPos.x,
                                   yOffset*newGroup.transform.localScale.y, 0);
                //Instantiate the new stone.
                GameObject newStone = (GameObject) Instantiate(stonePrefab,
                                      stonePos, rot, newGroup.transform);
                stones.Add(newStone);
            }
            //Add stones to groups and group to list of groups.
            Group g = newGroup.GetComponent<Group>();
            g.stonesList = stones;
            groups.Add(newGroup);
        }
    }
}
