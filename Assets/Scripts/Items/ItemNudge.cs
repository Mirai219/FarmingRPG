using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemNudge : MonoBehaviour
{
    private WaitForSeconds pause;
    private bool isAnimating = false;

    private void Awake()
    {
        pause = new WaitForSeconds(0.04f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isAnimating)
        {
            if(other.gameObject.transform.position.x < transform.position.x) //玩家从左边进入
            {
                StartCoroutine(RotateClock());
            }
            else
            {
                StartCoroutine(RotateAntiClock());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isAnimating)
        {
            if (collision.gameObject.transform.position.x > transform.position.x) //玩家从左边进入
            {
                StartCoroutine(RotateClock());
            }
            else
            {
                StartCoroutine(RotateAntiClock());
            }
        }
    }

    private IEnumerator RotateAntiClock()
    {
        isAnimating = true;

        for(int i = 0; i < 4; ++i)
        {
            gameObject.transform.GetChild(0).Rotate(0f, 0f, 2f);

            yield return pause;
        }

        for(int i=0; i < 5; ++i)
        {
            gameObject.transform.GetChild(0).Rotate(0f, 0f, -2f);

            yield return pause;
        }

        gameObject.transform.GetChild(0).Rotate(0f, 0f, 2f);

        yield return pause;

        isAnimating = false;
    }

    private IEnumerator RotateClock()
    {
        isAnimating = true;

        for (int i = 0; i < 4; ++i)
        {
            gameObject.transform.GetChild(0).Rotate(0f, 0f, -2f);

            yield return pause;
        }

        for (int i = 0; i < 5; ++i)
        {
            gameObject.transform.GetChild(0).Rotate(0f, 0f, 2f);

            yield return pause;
        }

        gameObject.transform.GetChild(0).Rotate(0f, 0f, -2f);

        yield return pause;

        isAnimating = false;
    }
}
