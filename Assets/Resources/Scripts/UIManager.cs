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

    private Stack<TYPE> uiStack;

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
        uiStack = new Stack<TYPE>();
    }

    // Update is called once per frame
    void Update()
    {
        BlackOutUpdate();
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

    public void ActiveUI(TYPE type)
    {
        isUIActive = true;
        uiStack.Push(type);
    }

    public void UnActiveUI()
    {
        Invoke("PopStack", 0.1f);//Áö¿¬        
    }

    private void PopStack()
    {
        if (uiStack.Count > 0) { uiStack.Pop(); }
        if (uiStack.Count == 0){ isUIActive = false; }
    }

    public bool CheckUITYPE(TYPE type)
    {
        var nowUI = TYPE.NONE;
        if(uiStack.Count > 0)
        {
            nowUI = uiStack.Peek();
        }
        return (nowUI == type);
    }


    public enum TYPE
    {
        NONE,
        DIALOG,
        MENU,
        POKEDEX1,
        POKEDEX2,
        POKEMON1,
        POKEMON2,
        BAG
    }
}
