using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pieces : MonoBehaviour {
    public Object groupPrefab;
    public Object stonePrefab;

    private List<int> groupBounds = new List<int>() {3,13};
    private List<int> stoneBounds = new List<int>() {1,32};


    public List<int> numPieces;
    public List<GameObject> groups;
    public UnitSelector us;
	// Use this for initialization
	void Start () {
        //ResetPieces();
	}

	// Update is called once per frame
	void Update () {

	}

    public void ResetPieces(){//randomly
        //random number of groups
        int numGroups = Random.Range(groupBounds[0], groupBounds[1]);
        List<int> li = new List<int>();

        for (int i = 0; i<numGroups; i++){
            li.Add(Random.Range(stoneBounds[0], stoneBounds[1]));
        }
        ResetPieces(li);
    }

    public void ResetPieces(List<int> numPieces){
        this.numPieces = numPieces;
        //remove old groups
        if (groups != null){
            foreach (GameObject g in groups){
                g.SetActive(false);
                Object.Destroy(g);
            }
        }
        groups = new List<GameObject>();
        //check if groups/stones within bounds
        for (int i = 0; i < numPieces.Count; i++){
            //set up and instantiate each group of stones
            //screen position spaced evenly
            Vector3 gpos = Camera.main.ScreenToWorldPoint(new Vector3((1f + i)*1f/(numPieces.Count + 1f)*Screen.width,Screen.height/2, 100));
            Quaternion rot = Quaternion.identity;
            GameObject newGroup = (GameObject) Instantiate(groupPrefab, gpos, rot, transform);

            List<GameObject> stones = new List<GameObject>();
            for (int j = 0; j < numPieces[i]; j++){
                //Debug.Log("stone...");
                //find position
                Vector3 spos = new Vector3(gpos.x,((1f + j)*1f/(numPieces[i] + 1f)-.5f)*2f*newGroup.transform.localScale.y, 0);
                //instantiate stone
                GameObject newStone = (GameObject) Instantiate(stonePrefab, spos, rot, newGroup.transform);
                //add stones to groups and master list
                //us.ObjList.Add(newStone);
                stones.Add(newStone);
            }
            Group g = newGroup.GetComponent<Group>();
            g.stonesList = stones;
            groups.Add(newGroup);
        }
        //update groups
        //us.GroupList = groups;
    }
}
