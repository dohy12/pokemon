using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideUI : MonoBehaviour
{
    public bool isActive = false;
    public RectTransform rectTransform;
    public float slideUIpos = 0;
    public float slideUIposTime = 0f;
    public Vector2 slideUIstartPos;
    public Vector2 slideUIendPos;
    public Vector2 slideUIdirec;
    public float slideUIdistance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SlideUiInit()
    {
        isActive = false;
        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = slideUIstartPos;

        var tmp = slideUIendPos - slideUIstartPos;
        slideUIdirec = tmp.normalized;
        slideUIdistance = tmp.magnitude;
    }

    public void SlideUiActive()
    {
        isActive = true;
        slideUIpos = 0f;
        slideUIposTime = 0f;
    }

    public void SlideUiUnActive()
    {
        isActive = false;
        slideUIposTime = 0.5f;
    }

    public void SlideUiUpdate()
    {
        if (isActive)
        {
            if (slideUIpos < slideUIdistance)
            {
                slideUIposTime += Time.deltaTime * 2;
                slideUIpos = -(slideUIdistance * 4) * slideUIposTime * slideUIposTime + (slideUIdistance * 4) * slideUIposTime;
                if (slideUIpos > slideUIdistance - 2f)
                {
                    slideUIpos = slideUIdistance;
                }
                SetUIPos();
            }
        }
        else
        {
            if (slideUIpos > 0)
            {
                slideUIposTime += Time.deltaTime * 2;
                slideUIpos = -(slideUIdistance * 4) * slideUIposTime * slideUIposTime + (slideUIdistance * 4) * slideUIposTime;
                if (slideUIpos < 2)
                {
                    slideUIpos = 0f;
                }
                SetUIPos();
            }
        }
    }

    private void SetUIPos()
    {
        rectTransform.anchoredPosition = slideUIstartPos + slideUIdirec * slideUIpos;
    }
}
