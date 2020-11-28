using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//user interface namespace
using UnityEngine.UI;
//scene management namespace
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    //current cube game obj
    [Header("Cube Object")]
    public GameObject currentCube;
    //last cube game object
    [Header("Last Cube Object")]
    public GameObject lastCube;
    //text object
    [Header("Text object")]
    public Text text;
    //level number integer
    [Header("Current Level")]
    public int Level;
    //boolean determining if game is over
    [Header("Boolean")]
    public bool Done;


    // Start is called before the first frame update
    void Start()
    {
        NewBlock();
    }

    // Update is called once per frame
    void Update()
    {
        if (Done)
            return;
        var time = Mathf.Abs(Time.realtimeSinceStartup % 2f - 1f);
        var pos1 = lastCube.transform.position + Vector3.up * 10f;
        var pos2 = pos1 + ((Level % 2 == 0)? Vector3.left : Vector3.forward) * 120;
        if(Level % 2 == 0)
        {
            currentCube.transform.position = Vector3.Lerp(pos2, pos1, time);
        }
        else
        {
            currentCube.transform.position = Vector3.Lerp(pos1, pos2, time);
        }
        if(Input.GetMouseButtonDown(0))
            NewBlock();
    }

    //function creates new blocks for the game
    void NewBlock()
{
    //if the last cube isnt destroyed
    if (lastCube != null)
    {
        //the current cube position = all 3 axis positions to nearest integer
        currentCube.transform.position = new Vector3(Mathf.Round(currentCube.transform.position.x),
            currentCube.transform.position.y, Mathf.Round(currentCube.transform.position.z));
        //current cube size = last cube size
        currentCube.transform.localScale = new Vector3(lastCube.transform.localScale.x -
            Mathf.Abs(currentCube.transform.position.x), lastCube.transform.localScale.y,
            lastCube.transform.localScale.z - Mathf.Abs(currentCube.transform.position.z -
            lastCube.transform.position.z));
        //current cubes positions equals to the current cubes x position
        //last cube's y position
        //z axis of 0.5
        currentCube.transform.position = Vector3.Lerp(currentCube.transform.position,
            lastCube.transform.position, 0.5f) + Vector3.up * 5f;
    }
    //is <= to 0 if the current cube size on the z axis is <= 0 (less than/equal to)
    if (currentCube.transform.localScale.x <= 0f || currentCube.transform.localScale.z <= 0f)
    {
        //done equals to true
        Done = true;
        //text is visible
        text.gameObject.SetActive(true);
        //text equals to the text of the final score & which level is played
        text.text = "Final Score: " + Level;
        //start corountine function X
        StartCoroutine(X());
        return;
    }
    lastCube = currentCube;
    currentCube = Instantiate(lastCube);
    currentCube.name = Level + "";
    currentCube.GetComponent<MeshRenderer>().material.SetColor(" Color", Color.HSVToRGB((
        Level / 100f) % 1f, 1f, 1f));
    Level++;
    Camera.main.transform.position = currentCube.transform.position + new Vector3(100, 100, 100);
    Camera.main.transform.LookAt(currentCube.transform.position);
}

    IEnumerator X()
    {
    yield return new WaitForSeconds(3f);
    SceneManager.LoadScene("SampleScene");
    }
}