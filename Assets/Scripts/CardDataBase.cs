using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDataBase : MonoBehaviour
{
    public List<Card> cardList = new List<Card>();
    public List<GameDefinition> gameDefinitions = new List<GameDefinition>(); 
    private void Awake() {
        gameDefinitions.Add(new GameDefinition(4,15,4,1,1,3));
        gameDefinitions.Add(new GameDefinition(5,19,54,1,2,3));

//        cardList.Add(new Card(CardTypeEnum.Empty));
//        cardList.Add(new Card(CardTypeEnum.Empty));
//        cardList.Add(new Card(CardTypeEnum.Wire));
    }
}
