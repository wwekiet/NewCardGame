//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{
	Hashtable PlayerList = new Hashtable();
	Hashtable CardInGame = new Hashtable();
    GameObject mainPanel;
	public Game()
	{

	}

	// Use this for initialization
	void Start () {
        
	}

	// Update is called once per frame
	void Update () {
        //foreach (DictionaryEntry item in CardInGame) {
        //    CardForm cf =(CardForm)item.Value;
        //    if(cf.Visible)cf.Update();
        //}
        mainPanel = GameObject.Find("Main Panel");
	}

	public void CreateCard()
	{
        GameObject go = new GameObject("Attack Card Form");
        go.transform.SetParent(mainPanel.transform);
        go.AddComponent(typeof(Attack));

        //Card atkCard = new Card(Card.CardType.Basic, "Dodge", "Dodge", "Used to attack one player.",
        //    Card.CardSuit.Heart, Card.CardNumber.Ace, Card.CardState.Hand, null);
        //CardForm cf = new CardForm(atkCard, 0, 0, 500, 700);
	}

    public static void MoveCard(CardForm cf)
	{
		//GameObject mainPanel = GameObject.Find ("Main Panel");
        if (cf.Position.x == 0) cf.Move(new Vector2(-200, 0), 5, null);
        else if (cf.Position.x == -200) cf.Move(new Vector2(0, 0), 5, null);
	}

    #region Function
    public static void CommandProcess(Command cm)
    {
        if (cm == null)
            return;
        else
        {
            if (cm.Command_Code == CommandCode.Attack)
            {
                //Attack card = 
            }
        }
    }

    public void Heal(int number, Guid source, Guid target)
    {
        //Player sourcePlayer = (Player)PlayerList [source];
        Player targetPlayer = (Player)PlayerList[target];
        if (targetPlayer != null)
        {
            targetPlayer.CurrentHealth += number;
        }
    }

    public void CausePhysicalDamage(int number, Guid source, Guid target)
    {
        Player targetPlayer = (Player)PlayerList[target];
        if (targetPlayer != null)
        {
            targetPlayer.CurrentHealth -= number;
        }
    }

    public void CauseMagicDamage(int number, Guid source, Guid target)
    {
        Player targetPlayer = (Player)PlayerList[target];
        if (targetPlayer != null)
        {
            targetPlayer.CurrentHealth -= number;
        }
    }

    public void LossOfHealth(int number, Guid source, Guid target)
    {
        Player targetPlayer = (Player)PlayerList[target];
        if (targetPlayer != null)
        {
            targetPlayer.CurrentHealth -= number;
        }
    } 
    #endregion
}

