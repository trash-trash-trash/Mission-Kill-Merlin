using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthView : MonoBehaviour
{
    public Health health;
    
    public Slider slider;

    [Header("Status UI")]
    public GameObject statusTextPrefab;
    public Transform gridParentObj; 
    public List<GameObject> spawnedObj = new List<GameObject>();

    void Start()
    {
        slider.maxValue = health.maxHP;
        slider.value = health.CurrentHP;

        health.AnnounceHP += SetHP;
        health.AnnounceHealthStatus += UpdateStatuses;

        RefreshStatuses();
    }

    private void SetHP(int hp)
    {
        slider.value = hp;
    }

    private void UpdateStatuses(List<HealthStatus> aHealthStatusList)
    {
        RefreshStatuses();
    }

    private void RefreshStatuses()
    {
        var counts = new Dictionary<HealthStatus, int>();

        foreach (HealthStatus status in health.Statuses)
        {
            if (!counts.ContainsKey(status))
                counts[status] = 0;

            counts[status]++;
        }

        int index = 0;
        foreach (KeyValuePair<HealthStatus, int> pair in counts)
        {
            GameObject obj;

            if (index < spawnedObj.Count)
            {
                obj = spawnedObj[index];
                obj.SetActive(true);
            }
            else
            {
                obj = Instantiate(statusTextPrefab, gridParentObj);
                spawnedObj.Add(obj);
            }

            TMP_Text text = obj.GetComponent<TMP_Text>();
            if (text != null)
            {
                text.text = $"{pair.Key} x{pair.Value}";
            }

            index++;
        }
        
        for (int i = index; i < spawnedObj.Count; i++)
        {
            spawnedObj[i].SetActive(false);
        }
    }

    void OnDisable()
    {
        health.AnnounceHP -= SetHP;
        health.AnnounceHealthStatus -= UpdateStatuses;
    }
}