using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaOtter_Blocking_of_View : MonoBehaviour
{
    private GameObject Child;
    private SpriteRenderer SpriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        Child = gameObject.transform.GetChild(1).gameObject;
        SpriteRenderer = Child.GetComponent<SpriteRenderer>();
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BlockingOfView"))
        {
            
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
            StartCoroutine(nameof(FadeIn));
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("BlockingOfView"))
        {
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    public IEnumerator FadeIn() // ���İ� 0���� 1�� ��ȯ
    {
        SpriteRenderer.color = new Color(0, 0, 0, 0);
        while (SpriteRenderer.color.a < 1.0f)
        {
            SpriteRenderer.color = new Color(0,0,0, SpriteRenderer.color.a + (Time.deltaTime *2f));
            yield return null;
        }
        //StartCoroutine(FadeTextToZeroAlpha());
    }

}
