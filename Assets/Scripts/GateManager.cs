using UnityEngine;
using TMPro;

public class GateManager : MonoBehaviour
{
    public TextMeshPro gateNo;
    public int randomNumber;
    public bool multiply;

    // Start is called before the first frame update
    void Start()
    {
        if (multiply)
        {
            randomNumber = Random.Range(1, 3);
            gateNo.text = "X" + randomNumber;
        }
        else
        {
            randomNumber = Random.Range(10, 60);

            //짝수로 맞추기
            if (randomNumber % 2 != 0)
                randomNumber += 1;

            gateNo.text = randomNumber.ToString();
        }
    }

}
