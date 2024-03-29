using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Money : MonoBehaviour
{
    public static Money instance;
    public Vector2 startPos;
    public Vector2 endPos;
    private RectTransform rectT;
    private TMP_Text txt;

    private GameDataManager data;


    private void Awake()
    {
        instance = this;
        
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        
    }

    public void Active()
    {
        rectT.anchoredPosition = endPos;
        ShowMoney();
    }

    public void UnActive()
    {
        rectT.anchoredPosition = startPos;
    }

    public void Init()
    {
        data = GameDataManager.instance;

        rectT = (RectTransform)transform;
        rectT.anchoredPosition = startPos;
        txt = transform.GetChild(0).GetComponent<TMP_Text>();
    }

    private void ShowMoney() { txt.text = data.money.ToString()  + " ¿ø"; }

    public void SetMoney(int money)
    { 
        data.money = money;
        ShowMoney();
    }

    public int GetMoney() {  return data.money; }
}
