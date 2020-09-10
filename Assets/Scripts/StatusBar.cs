using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatusBar : MonoBehaviour
{
    public TextMeshProUGUI lifePoints;
    public TextMeshProUGUI money;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        lifePoints.text = GameState.instance.lives.ToString();
        money.text = GameState.instance.currency.ToString();
    }
}
