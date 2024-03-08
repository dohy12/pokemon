using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    private Image blackout;
    private bool isBlackout;
    private float blackOutAlpha;
    private float blackOutSpeed;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        blackout = transform.Find("BlackOut").GetComponent<Image>();
        blackout.gameObject.SetActive(false);
        isBlackout = false;
    }

    // Update is called once per frame
    void Update()
    {
        BlackOutUpdate();
    }


    public void ActiveBlackOut(float blackOutSpeed)
    {
        Debug.Log("test");
        blackout.gameObject.SetActive(true);
        this.blackOutSpeed = blackOutSpeed;
        blackOutAlpha = 0;
        isBlackout = true;
        blackout.color = new Color(0, 0, 0, 0);
    }

    private void UnActiveBlackOut()
    {
        blackout.color = new Color(0, 0, 0, 0);
        isBlackout = false;
        blackOutAlpha = 0;
        blackout.gameObject.SetActive(false);        
    }

    private void BlackOutUpdate()
    {
        if (isBlackout)
        {
            blackout.color = new Color(0, 0, 0, blackOutAlpha);

            blackOutAlpha += Time.deltaTime * blackOutSpeed * 1.5f;
            if (blackOutAlpha > 1.5f)
            {
                blackOutAlpha = 1.5f;
                blackOutSpeed *= -1f;
            }

            if (blackOutAlpha < 0f)
            {
                UnActiveBlackOut();
            }
        }
        
    }
}
