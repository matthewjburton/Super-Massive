using TMPro;
using UnityEngine;

public class Matter : Particle
{
    MatterStats matterStats;

    protected override void Start()
    {
        base.Start();
        matterStats = stats as MatterStats;
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Matter matter))
        {
            if (matter.fusions.Equals(fusions))
            {
                Fuse(other.gameObject);
                HandleFusionText();
            }
        }
    }

    void HandleFusionText()
    {
        GameObject fusionText = Instantiate(matterStats.fusionTextPrefab, transform.position, Quaternion.identity);

        int index = Mathf.Min(fusions - 1, matterStats.fusionTextList.Length - 1);

        fusionText.GetComponentInChildren<TextMeshProUGUI>().text = matterStats.fusionTextList[index];
        fusionText.GetComponentInChildren<TextMeshProUGUI>().fontSize = 6 + (index * 2);
        fusionText.GetComponentInChildren<TextMeshProUGUI>().color = matterStats.fusionColorList[index];
    }
}
