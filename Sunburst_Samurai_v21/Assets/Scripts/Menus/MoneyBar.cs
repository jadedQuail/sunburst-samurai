using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyBar : MonoBehaviour
{
    public Text moneyText;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMoneyBar();
    }

    void UpdateMoneyBar()
    {
        moneyText.text = "$" + player.GetComponent<Money>().GetMoney().ToString();
    }
}
