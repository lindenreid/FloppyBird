using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private TMP_Text _text;

    // Hook into events before Start() methods called.
    private void Awake()
    {
        gameController.PointsChanged += HandlePointsChanged;
    }

    // Change points UI display when player points change.
    public void HandlePointsChanged(int points)
    {
        _text.text = points.ToString();
    }
}
