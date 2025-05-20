using System.Linq;
using UnityEngine;
using TMPro;

public class RankPage : MonoBehaviour
{



    [SerializeField] Transform contentRoot;
    [SerializeField] GameObject rowPrefab;

    StageResultList allData;

    void Awake()
    {
        allData = StageResultSaver.LoadRank();
        RefreshRankList(1);
    }

    public void RefreshRankList(int idx)
    {

        foreach (Transform child in contentRoot)
        {
            Destroy(child.gameObject);
        }

        // ��ũ ������ ����
        var sortedData = allData.results
            .Where(r => r.stage == idx)
            .OrderByDescending(x => x.score)
            .ToList();

        // ��ũ ������ ����
        for (int i = 0; i < sortedData.Count; i++)
        {
            GameObject row = Instantiate(rowPrefab, contentRoot);
            TMP_Text rankText = row.GetComponentInChildren<TMP_Text>();
            rankText.text = $"stage {idx} : {i + 1}. {sortedData[i].playerName} - {sortedData[i].score}";
        }
    }
}