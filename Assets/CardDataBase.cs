using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDataBase : MonoBehaviour
{
    public List<Card> cardList = new List<Card>();

    private void Awake() {
        cardList.Add(new Card(CardTypeEnum.Empty));
        cardList.Add(new Card(CardTypeEnum.Empty));
        cardList.Add(new Card(CardTypeEnum.Wire));
    }
}
