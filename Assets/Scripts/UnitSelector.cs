using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelector : MonoBehaviour {

    private bool isSelecting = false;
    private Vector3 mousePos1;
    private bool paused;
    private float startTime;

    //public List<GameObject> ObjList;//
    public Group selectedGroup;//current group that is selected or null
    public List<GameObject> selectedList;//list of stones currently selected or null

    public Pieces pieces;//list of all groups
    private List<GameObject> groupsInBounds;

    public Turns turnController;
    public ComputerPlayer computerPlayer;
    public Menu menu;
	// Use this for initialization
    void Start () {
        Pause();
        groupsInBounds = new List<GameObject>();
	}

	// Update is called once per frame
	void Update () {
        if (paused){
            return;
        }

        if (!turnController.isComputerTurn()){//human turn
            if (Input.GetMouseButtonDown(0)){// on click: clear old, start new selection
                isSelecting = true;
                mousePos1 = Input.mousePosition;
                //deselect objects in old list: pieces.groups,
                foreach (GameObject gameObject in pieces.groups){
                    Group g = gameObject.GetComponent<Group>();
                    if (g==null){
                        Debug.Log("SOMETHING WENT WRONG!!!");
                    }else if (g.isSelected){
                        //deselect group and stones
                        g.isSelected = false;
                        foreach (GameObject go in g.stonesList){
                            Stone s = go.GetComponent<Stone>();
                            s.isSelected = false;
                        }
                    }
                }
            }
            if (isSelecting){//while mouse down, select group and stones

                //check if any group is within drag box
                //if found, choose best in order of selection
                foreach (GameObject g in pieces.groups){
                    bool hit = intersectsSelection(g);
                    bool inList = groupsInBounds.Contains(g);
                    if(hit && !inList){ //group is in selection box and not in list
                        groupsInBounds.Add(g);
                    }else if (!hit && inList){//group is out of selection box and in list
                        g.GetComponent<Group>().isSelected = false;
                        groupsInBounds.Remove(g);
                    }
                }
                //highlight first group as selected if exists
                //go through list. if item is null or empty, do nothing. if item is a group with stones, select it
                bool found = false;
                for (int i = 0; i<groupsInBounds.Count && !found; i++){
                    Group g = groupsInBounds[i].GetComponent<Group>();
                    if (groupsInBounds[i] != null && g.stonesList.Count > 0){
                        //find first group with active stones
                        selectedGroup = g;
                        selectedGroup.isSelected = true;
                        found = true;
                    }
                }
                Debug.Log("number of groups selected: " + groupsInBounds.Count);
            }
            if (Input.GetMouseButtonUp(0)){//on release: add selected items to list

                //select objects in new list
                if (selectedGroup != null){
                    selectedList = new List<GameObject>();
                    foreach (GameObject gameObject in selectedGroup.stonesList){
                        if (isWithinSelection(gameObject)){
                            Stone s = gameObject.GetComponent<Stone>();
                            if (s != null){
                                s.isSelected = true;
                                selectedList.Add(gameObject);
                            }
                        }
                    }
                }
                isSelecting = false;
            }
        }else {//computer turn


            if (!isSelecting){//

                computerPlayer.ChooseStones();
                selectedGroup = computerPlayer.selectedGroup;
                selectedGroup.isSelected = true;
                selectedList = computerPlayer.selectedList;
                startTime = Time.time;
                isSelecting = true;
                foreach (GameObject gameObject in selectedList){
                    Stone s = gameObject.GetComponent<Stone>();
                    if (s != null){
                        s.isSelected = true;
                    }

                }
            }else if (isSelecting && (startTime + computerPlayer.waitTime < Time.time)){
                removeSelected();
            }
        }
    }

    void OnGUI(){
        if (isSelecting && !turnController.isComputerTurn()){//draw box
            Rect r = Utils.GetScreenRect(mousePos1, Input.mousePosition);
            Utils.DrawScreenRect(r, new Color(.9f, .0f, .0f, .15f));
            Utils.DrawScreenRectBorder(r, 2, new Color(.9f, .2f, .1f, .9f));
        }
    }

    public bool isWithinSelection(GameObject gameObject){
        if (!isSelecting){
            return false;
        }

        Camera camera = Camera.main;
        Bounds bounds = Utils.GetViewportBounds(camera, mousePos1, Input.mousePosition);
        Vector3 diag = bounds.max - bounds.min;
        diag.z = 0;
        //if selection area is very small (no drag)
        //if (diag.magnitude < .01f){
        //    int layerMask = 1 << 8;
        //    Ray ray1 = camera.ScreenPointToRay(mousePos1);
        //    Ray ray2 = camera.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;
        //    if(Physics.Raycast(ray1, out hit, Mathf.Infinity, layerMask)){
        //        if (hit.collider.gameObject == gameObject){
        //            return true;
        //        }
        //    }
        //
        //    if(Physics.Raycast(ray2, out hit, Mathf.Infinity, layerMask)){
        //        if (hit.collider.gameObject == gameObject){
        //            return true;
        //        }
        //    }
        //}

        return bounds.Contains(camera.WorldToViewportPoint(gameObject.transform.position));
    }

    public bool intersectsSelection(GameObject gameObject){
        if (!isSelecting){
            return false;
        }

        Camera camera = Camera.main;
        Bounds selectionBounds = Utils.GetViewportBounds(camera, mousePos1, Input.mousePosition);
        //Collider collider = gameObject.GetComponent<Collider>();
        Renderer r = gameObject.GetComponent<Renderer>();
        Bounds objBounds = Utils.GetViewportBoundsWorldspace(camera, r.bounds.min, r.bounds.max);//convert to viewport space
        return selectionBounds.Intersects(objBounds);

    }

    public void removeSelected(){
        if (selectedList != null && selectedList.Count > 0){
            Debug.Log("removing stones");
            foreach (GameObject s in selectedList){
                //ObjList.Remove(s);
                s.SetActive(false);
                selectedGroup.stonesList.Remove(s);
                Object.Destroy(s);
            }
            if (selectedGroup.Size() == 0){
                pieces.groups.Remove(selectedGroup.gameObject);//remove empty group
                selectedGroup.gameObject.SetActive(false);
                Object.Destroy(selectedGroup.gameObject);
            }

            if (pieces.groups.Count == 0){ // all groups are empty
                menu.enabled = true;
                menu.EndGame(turnController.CurrentPlayer()+" wins!!!");
            }

            turnController.ChangePlayer();
            //deselect
        }
        isSelecting = false;
        selectedList = null;
        if (selectedGroup != null){
            selectedGroup.isSelected = false;
        }
        selectedGroup = null;
    }

    public void Pause(){
        paused = true;
    }

    public void Unpause(){
        paused = false;
    }
}
