using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ButtonKickController : MonoBehaviour
{
    Coroutine a;
    // Start is called before the first frame update
    protected void OnEnable()
    {
        if (a != null)
        {
            StopCoroutine(a);
        }
        a = StartCoroutine(Zoom());
    }
    IEnumerator Zoom()
    {
        while (true)
        {
            this.transform.DOScale(Vector3.one * 1.2f, 0.5f).OnComplete(() =>
            {
                this.transform.DOScale(Vector3.one * 0.8f, 0.5f);
            });
            yield return new WaitForSeconds(1);
        }
    }
    public void OnClick()
    {
        this.gameObject.SetActive(false);
        GameController.instant.Kick();
    }
    
}
