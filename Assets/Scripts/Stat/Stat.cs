using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Stat : MonoBehaviour
{
    [Header("Stat Info")]
    public float maxHp;
    public float currentHp;
    public float attackPower;
    public int effectiveRange;

    
    protected CvsReader csv;

    //[Header("Stage Info")]
    //public StageManager stageManager;

    public Image[] blinkImages;
    public Image attackedImage;
    Color halfA = new Color(139, 0, 0, 0.5f);
    Color fullA = new Color(1, 1, 1, 1f);
    public void Awake()
    {
        csv = Managers.Instance.GetComponent<CvsReader>();
        //stageManager = FindObjectOfType<StageManager>();
        blinkImages = gameObject.GetComponentsInChildren<Image>();
    }
    public abstract void SetStatData(int index);
    public abstract void AddDamage(GameObject damage);

    
    public IEnumerator AlphaBlink()
    {
        yield return new WaitForSeconds(0.1f);

        foreach (Image image in blinkImages)
        {
            image.color = halfA;
        }

        yield return new WaitForSeconds(0.1f);

        foreach (Image image in blinkImages)
        {
            image.color = fullA;
        }

    }
    
    public IEnumerator Shake()
    {
        float t = 1f;
        float shakePower = 1f;
        Vector3 origin = transform.position;

        while (t > 0f)
        {
            t -= 0.05f;
            transform.position = origin + (Vector3)Random.insideUnitCircle * shakePower * t;

            yield return null;
        }
        transform.position = origin;

        yield return new WaitForSeconds(0.3f);
    }
}
