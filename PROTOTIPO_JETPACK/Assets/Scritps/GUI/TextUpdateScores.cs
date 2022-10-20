using UnityEngine;
using TMPro;

public class TextUpdateScores : MonoBehaviour
{
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        GameManager.OnCollectionScore += this.AddValue;
    }

    private void OnDestroy()
    {
        GameManager.OnCollectionScore -= this.AddValue;
    }

    private void OnEnable()
    {
        text.text = GameManager.Instance.Scores+"";
    }

    void AddValue(int valeu)
    {
        text.text = valeu + "";
    }
}