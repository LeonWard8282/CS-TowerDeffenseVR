using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NodoUI : MonoBehaviour
{
    private Nodo target;

    public GameObject ui;

    public TMP_Text upgradeCost;

    public Button upgradeButton;

    public Button sellButton;

    public void SetTarget(Nodo _target)
    {
        this.target = _target;

        transform.position = target.GetBuildPosition();

        if (!target.isUpgraded)
        {
            upgradeCost.text = "$ " + target.turretBluePrint.upgradeCost;

            upgradeButton.interactable = true;
        }
        else
        {
            upgradeCost.text = "Maxed out";
            upgradeButton.interactable = false;
        }




        ui.SetActive(true);

    }

    public void Hide()
    {
        ui.SetActive(false);
    }

    public void Upgrade()
    {
        target.UpgradeTurret();
        BuildManager.instance.DeselectNode();

    }

}
