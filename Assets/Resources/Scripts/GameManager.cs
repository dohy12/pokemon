using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static Dictionary<int, Vector2> direcToVector;

    private void Awake()
    {
        EditorReady();
        instance = this;
        SetDirecVector();
        SetResolution();
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

    private void SetDirecVector()
    {
        direcToVector = new Dictionary<int, Vector2>();
        direcToVector.Add(0, new Vector2(0f, -1f));
        direcToVector.Add(1, new Vector2(-1f, 0f));
        direcToVector.Add(2, new Vector2(0f, 1f));
        direcToVector.Add(3, new Vector2(1f, 0f));
    }

    private void SetResolution()
    {
        int setWidth = 832; // 화면 너비
        int setHeight = 576; // 화면 높이

        Screen.SetResolution(setWidth, setHeight, false);
    }
}
