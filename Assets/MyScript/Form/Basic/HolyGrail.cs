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

public class HolyGrail : CardForm
{
    public HolyGrail(Card.CardSuit suit, Card.CardNumber number, Card.CardState state, Player owner, Game g)
        : base(new Card(Card.CardType.Basic, "Holy Grail", "HolyGrail", "Used to regain one unit of health or save one player when they are on the brink of death.",
        suit, number, state, owner),g)
    {
        this.UseCondition += useCondition;
    }

    private bool useCondition()
    {
        Player owner = this.Form.Owner;
        if (owner != null && owner.actionState == Player.ActionState.Free && owner.CurrentHealth < owner.MaxHealth)
        {
            return true;
        }
        else if (owner != null && owner.actionState == Player.ActionState.WaitingBoD)
        {
            return true;
        }
        else if (owner != null && owner.actionState == Player.ActionState.WaitingSave)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void UseCard()
    {
        Player source = this.Form.Owner;
        if (source.actionState == Player.ActionState.Free) PerformHealing();
        else if (source.actionState == Player.ActionState.WaitingSave) PerformSaving();
        //base.UseCard();
    }
}


