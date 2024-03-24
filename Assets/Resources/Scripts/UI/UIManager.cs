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
    public static bool isUIActive = false;

    private List<GameObject> uiStack;

    public GameObject uiID;

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
        uiStack = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        BlackOutUpdate();

        if (uiStack.Count > 0)
        {
            uiID = uiStack[uiStack.Count - 1];
        }
        else
        {
            uiID = null;
        }        
    }

    public void ActiveBlackOut(float blackOutSpeed)
    {
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

    public bool GetUIActive()
    {
        return isUIActive;
    }

    public void SetUIActive(bool _isUIActive)
    {
        isUIActive = _isUIActive;
    }

    public void ActiveUI(GameObject uiID)
    {
        isUIActive = true;
        uiStack.Add(uiID);
    }

    public void UnActiveUI(GameObject uiID)
    {
        if (uiStack.Count > 0) { uiStack.RemoveAll(d => d.gameObject == uiID); }
        if (uiStack.Count == 0) { isUIActive = false; }
    }

    public bool CheckUITYPE(GameObject uiID)
    {
        GameObject nowUI = null;
        if(uiStack.Count > 0)
        {
            nowUI = uiStack[uiStack.Count - 1];
        }
        return (nowUI == uiID);
    }

}
