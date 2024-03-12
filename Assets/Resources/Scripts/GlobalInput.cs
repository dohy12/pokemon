using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalInput : MonoBehaviour
{
    static public GlobalInput globalInput;
    public float vertical { get; private set; }
    public float horizontal { get; private set; }

    public float verticalRaw { get; private set; }
    public float horizontalRaw { get; private set; }

    public bool aButtonDown { get; private set; }

    public Vector2 keyCheck;

    void Awake()
    {
        globalInput = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");

        verticalRaw = Input.GetAxisRaw("Vertical");
        horizontalRaw = Input.GetAxisRaw("Horizontal");

        aButtonDown = Input.GetButtonDown("AButton");

        keyCheck = new Vector2(vertical, horizontal);

    }
}
