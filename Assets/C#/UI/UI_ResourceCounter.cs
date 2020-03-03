using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class UI_ResourceCounter : NetworkBehaviour
{

    private RectTransform rect;
    private Vector3 originalScale;
    private bool animating = false;
    private float startTime;
    private const float pulseLength = 0.2f;
    private const float pulseScale = 0.2f; /* Fraction added to 100% of scale */

    void Start()
    {
        rect = this.GetComponent<RectTransform>();
        originalScale = rect.localScale;
    }
    
    void LateUpdate()
    {
        if (animating)
        {
            float elapsed = Time.time - startTime;
            if (elapsed >= pulseLength)
            {
                animating = false;
                rect.localScale = originalScale;
            }
            else
            {
                if (elapsed <= pulseLength / 2.0f)
                {
                    rect.localScale = (elapsed / pulseLength * pulseScale + 1) * originalScale;
                }
                else
                {
                    rect.localScale = ((1 - elapsed / pulseLength) * pulseScale + 1) * originalScale;
                }
            }
        }
    }

    public void pulseAnimation()
    {
        if (!animating)
        {
            originalScale = rect.localScale;
        }
        else
        {
            rect.localScale = originalScale;
        }
        animating = true;
        startTime = Time.time;
    }

}
