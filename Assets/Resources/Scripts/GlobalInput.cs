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

    public bool bButtonDown { get; private set; }

    public bool bButton { get; private set; }

    public bool startButtonDown { get; private set; }

    private float stunTime = 0f;

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

        bButtonDown = Input.GetButtonDown("BButton");
        bButton = Input.GetButton("BButton");

        startButtonDown = Input.GetButtonDown("Submit");

        if (stunTime > 0)
        {
            stunTime -= Time.deltaTime;
            InputStun();
        }

    }

    public void InputStun()
    {
        vertical = 0f;
        horizontal = 0f;

        verticalRaw = 0f;
        horizontalRaw = 0f;

        aButtonDown = false;

        bButtonDown = false;

        startButtonDown = false;
    }


    public void InputStun(float stunTime)
    {
        InputStun();
        this.stunTime = stunTime;
    }
}
