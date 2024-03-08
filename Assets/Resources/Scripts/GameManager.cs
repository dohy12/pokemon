using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    private void Awake()
    {
        EditorReady();
        instance = this; 
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void EditorReady()
    {
        GameObject camera = GameObject.Find("Main Camera");
        if (camera != null)
        {
            camera.SetActive(false);
        }

        transform.position = Vector3.zero;
    }
}
