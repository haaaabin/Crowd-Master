using UnityEngine;

public class Stair : MonoBehaviour
{
    private int stairIndex;

    private void Start()
    {
        stairIndex = transform.GetSiblingIndex();
    }

    public int GetStairIndex()
    {
        return stairIndex;
    }
}