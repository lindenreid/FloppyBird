using UnityEngine;

public class Pipe : MonoBehaviour
{
    [SerializeField] private GameObject _topPipe;
    [SerializeField] private GameObject _bottomPipe;

    public void Setup ()
    {
        Debug.Log("pipe.setup");
    }
}