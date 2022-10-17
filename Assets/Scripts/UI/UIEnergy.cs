using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEnergy : MonoBehaviour
{
    private TempUITestStats tempUITestStats;

    private void Awake()
    {
        tempUITestStats = GameObject.FindGameObjectWithTag("Player").GetComponent<TempUITestStats>();
    }

    private void Update()
    {
        UpdateEnergyUI();
    }

    public void UpdateEnergyUI()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        
        float previousAnchorMaxPoint = 0f;
        float anchorMaxXSize = (1f - previousAnchorMaxPoint) / tempUITestStats.energyMax;

        for (int i = 0; i < tempUITestStats.energyMax; i++)
        {
            GameObject energy = new GameObject();
            energy.transform.SetParent(transform);
            energy.name = i.ToString();
            energy.AddComponent<RectTransform>();

            energy.AddComponent<Image>();

            energy.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            energy.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 10);
            energy.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.5f, 0.5f);
            energy.GetComponent<RectTransform>().anchorMin = new Vector2(previousAnchorMaxPoint, 0);
            energy.GetComponent<RectTransform>().anchorMax = new Vector2(previousAnchorMaxPoint + anchorMaxXSize, 1f);
            previousAnchorMaxPoint = energy.GetComponent<RectTransform>().anchorMax.x;
            energy.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
            energy.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);

            if (i < tempUITestStats.energyCurrent)
            {
                energy.GetComponent<Image>().color = Color.green;
            }
            else
            {
                energy.GetComponent<Image>().color = Color.red;
            }
        }
    }
}