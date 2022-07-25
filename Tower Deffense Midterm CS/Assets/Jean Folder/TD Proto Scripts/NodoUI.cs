using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodoUI : MonoBehaviour
{
    private Nodo target;

    public GameObject ui;

    public void SetTarget(Nodo _target)
    {
        this.target = _target;

        transform.position = target.GetBuildPosition();
        ui.SetActive(true);

    }

    public void Hide()
    {
        ui.SetActive(false);
    }

}
