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
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MagicAttack: CardForm
{
    public MagicAttack(Card.CardSuit suit, Card.CardNumber number, Card.CardState state, Player owner, Game g)
        : base(new Card(Card.CardType.Basic, "Magical ATTACK", "MagicAttack", "Used to attack one player with magical damage.",
        suit, number, state, owner),g)
    {

    }


    public void PerformAttack(GameObject panelClick)
    {
        Outline border = panelClick.GetComponent<Outline>();
        if (border!=null) border.enabled = false;
        EventTrigger et = panelClick.GetComponent<EventTrigger>();
        if (et!=null) et.delegates = null;
        Vector3 position = game.tempPanel.transform.localPosition;
        Player source = this.Form.Owner;
        Player target = panelClick.GetComponent<Player>();

        //if (this.Form.Owner == game.myPlayer) RefreshHand(game.myPlayer);
        this.Form.Owner = null;
        this.Form.FaceUp = true;
        this.Form.CardData.State = Card.CardState.Using;
        this.Form.State = Card.CardState.Using;
        Action action = delegate()
        {
            this.Form.UpdateLastInteract();
            RefreshHand(game.myPlayer);
            RefreshPiles();
            game.Attack(1, source, target);
        };
        this.Form.Move(new Vector2(position.x, position.y), MoveSpeed, action);

        //player.CurrentHealth -= 1;
    }

    public override void UseCard()
    {
        Outline border = game.playerPanel6.GetComponent<Outline>();
        border.enabled = true;
        EventTrigger et = game.playerPanel6.GetComponent<EventTrigger>();
        EventTrigger.TriggerEvent trigger = new EventTrigger.TriggerEvent();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) => { PerformAttack(game.playerPanel6); });
        et.delegates = new System.Collections.Generic.List<EventTrigger.Entry>();
        et.delegates.Add(entry);
        //base.UseCard();
    }


}


