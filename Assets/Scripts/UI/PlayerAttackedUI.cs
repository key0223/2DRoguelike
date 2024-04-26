using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttackedUI : MonoBehaviour
{
    Image attakedImage;

    Color zeroA = new Color(0, 0, 0, 0f);
    Color halfA = new Color(0.3f, 0.04f, 0.05f, 0.5f);
    Color fullA = new Color(0.3f, 0.04f, 0.05f, 1f);

    Color baseColor;
    void Awake()
    {
        attakedImage = GetComponent<Image>();
        baseColor = attakedImage.color;
    }

    public void OnAttacked()
    {
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeOut()
    {
        float alpha = 1f;

        while(alpha >0 )
        {
            alpha -= 0.1f;
            yield return new WaitForSeconds(0.05f);
            attakedImage.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
        }
    }

    public IEnumerator FadeIn()
    {
        float alpha = 0;

        while(alpha<1)
        {
            alpha += 0.1f;
            yield return new WaitForSeconds(0.05f);
            attakedImage.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
        }

        StartCoroutine(FadeOut());
    }
}
