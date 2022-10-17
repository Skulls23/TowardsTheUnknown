using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillsBar : MonoBehaviour
{
    [SerializeField] private float skillSize = 0.025f;
    private TempUITestStats tempUITestStats;

    private void Awake()
    {
        tempUITestStats = GameObject.FindGameObjectWithTag("Player").GetComponent<TempUITestStats>();
    }

    private void Update()
    {
        UpdateSkillBar();
    }
    
    public void UpdateSkillBar()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        
        GetComponent<RectTransform>().anchorMin = new Vector2(0.5f - skillSize * tempUITestStats.lObjects.Count, GetComponent<RectTransform>().anchorMin.y);
        GetComponent<RectTransform>().anchorMax = new Vector2(0.5f + skillSize * tempUITestStats.lObjects.Count, GetComponent<RectTransform>().anchorMax.y);


        float previousAnchorMaxPoint = 0f;
        float anchorMaxXSize = (1f - previousAnchorMaxPoint) / tempUITestStats.lObjects.Count;
        for (int i = 0; i < tempUITestStats.lObjects.Count; i++)
        {
            GameObject skill = new GameObject();
            skill.transform.SetParent(transform);
            skill.name = i.ToString();
            skill.AddComponent<RectTransform>();
            
            skill.AddComponent<Image>();
            if(i%2 == 0)
                skill.GetComponent<Image>().color = Color.grey;
            
            skill.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            skill.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 10);
            skill.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.5f, 0.5f);
            skill.GetComponent<RectTransform>().anchorMin = new Vector2(previousAnchorMaxPoint, 0);
            skill.GetComponent<RectTransform>().anchorMax = new Vector2(previousAnchorMaxPoint + anchorMaxXSize, 1f);
            previousAnchorMaxPoint = skill.GetComponent<RectTransform>().anchorMax.x;
            skill.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
            skill.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        }
    }
}