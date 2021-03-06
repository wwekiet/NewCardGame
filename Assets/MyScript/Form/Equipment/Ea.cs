﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Ea : Weapon
{
    public Ea(Card.CardSuit suit, Card.CardNumber number, Card.CardState state, Player owner, Game g)
        : base("Ea - Sword of Rupture", "Ea", "", 2, suit, number, state, owner, g)
    {
        this.DamageIncrease += EaEffect;
	}

    private System.Collections.IEnumerator EaEffect(int number, Player source, Player victim, Game.DamageType dmgType)
    {
        int busy = game.GetBusyTask();

        if (victim.lastDamageCard != null) victim.lastDamageCard.LossOfHealth = true;

        if (busy >= 0) game.busy[busy] = false;
        yield return new WaitForSeconds(0.1f);
    }

    public override void Ability()
    {
        base.Ability();
    }

    public override void UseCard()
    {
        //Equipped();
        base.UseCard();
    }

    public override void Discard()
    {
        UnEquipped();
        base.Discard();
    }

    public override void Equipped()
    {
        base.Equipped();
    }

    public override void UnEquipped()
    {
        base.UnEquipped();
    }
}