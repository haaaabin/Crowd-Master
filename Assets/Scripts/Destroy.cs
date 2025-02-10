using System.Collections;
using UnityEngine;

public class DestoryParticle : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(1f);
        Destroy(gameObject);
    }
}
