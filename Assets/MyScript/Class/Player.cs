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
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public string PlayerName = "";
    public string IpAdress = "";
    public bool Status = true;
    public GameObject CharacterObject1;
    public GameObject CharacterObject2;
    public Character Character1;
    public Character Character2;
    public Guid PlayerID;
    public int HandLimit = 0;
    public int CurrentHealth = 0;
    public double MaxHealth = 0;
    public int AttackingRange = 0;
    public int MinusDistance = 0;
    public int PlusDistance = 0;
    private PlayerTurn turn = PlayerTurn.OutTurn;
    public bool AutoAI = false;

    public bool DodgeAble = true;
    public bool IsDodge = false;
    public bool AdditionDodge = false;
    public bool MainAttack = false;
    public bool CommandSeal = false;
    public int DamageIncrease = 0;
    public int DamageDecrease = 0;
    public bool DoubleDamage = false;
    public int DamageTaken = 0;
    public bool Attacked = false;

    public GameObject Weapon, Armor, PlusVehicle, MinusVehicle;
    //public CardForm myWeapon = null, myArmor = null, myPlusVehicle = null, myMinusvehicle = null;
    public GameObject Delay1, Delay2, Delay3, Delay4;
    public CardForm myDelay1 = null, myDelay2 = null, myDelay3 = null, myDelay4 = null;
    Text health, maxHealth, hand;

    public CardForm lastDamageCard = null;
    public Player lastDamagePlayer;
    public Player targetPlayer;
    public Game game;

    public delegate void PhaseDelegate();
    public PhaseDelegate ChangePhase = null;
    public PhaseDelegate BeginningOfTurn = null;
    public PhaseDelegate DrawPhase = null;
    public PhaseDelegate JudgmentPhase = null;
    public PhaseDelegate ActionPhase = null;
    public PhaseDelegate DiscardPhase = null;
    public PhaseDelegate EndPhase = null;
    public PhaseDelegate OnWaitingAction = null;

    public delegate IEnumerator EffectDelegate(int number, Player source, Player victim);
    public EffectDelegate BeforeAttack = null;
    public EffectDelegate BeforeAttacked = null;
    public EffectDelegate TakeMagicDamage = null;
    public EffectDelegate TakePhysicDamage = null;
    public EffectDelegate CauseMagicDamage = null;
    public EffectDelegate CausePhysicDamage = null;
    public EffectDelegate Healing = null;
    public EffectDelegate AfterHealing = null;
    public EffectDelegate BrinkOfDeath = null;
    public EffectDelegate EndAttack = null;


    public delegate IEnumerator ModifierDelegate(int number, Player source, Player victim);
    public ModifierDelegate AttackDamageModifier = null;
    public ModifierDelegate DamageModifier = null;
    public ModifierDelegate DamageCalculation = null;
    public ModifierDelegate HealModifier = null;

    public delegate IEnumerator SequentiallyDelegate();
    public SequentiallyDelegate OnJudgment = null;
    public SequentiallyDelegate BeforeJudgmentTakeEffect = null;
    public SequentiallyDelegate AfterJudgmentTakeEffect = null;
    public SequentiallyDelegate Discard = null;

    public enum ActionState
    {
        None, Free, WaitingDodge, WaitingTool, WaitingRhoAias, WaitingBoD, WaitingSave, WaitingDiscard,
        UseAs, WaitingAttack,
    }

    public enum PlayerTurn
    {
        Beginning = 0,
        Judgment = 1,
        Draw = 2,
        Action = 3,
        Discard = 4,
        End = 5,
        OutTurn = 6,
    }

    public ActionState actionState = ActionState.None;

    public PlayerTurn Turn
    {
        get { return turn; }
        set
        {
            turn = value;
            if (turn == PlayerTurn.Beginning)
            {
                if (BeginningOfTurn != null) BeginningOfTurn.Invoke();
                //if (ChangePhase != null) ChangePhase.Invoke();
            }
            else if (turn == PlayerTurn.Judgment)
            {
                if (JudgmentPhase != null) JudgmentPhase.Invoke();
                //if (ChangePhase != null) ChangePhase.Invoke();
            }
            else if (turn == PlayerTurn.Draw)
            {
                if (DrawPhase != null) DrawPhase.Invoke();
            }
            else if (turn == PlayerTurn.Action)
            {
                if (ActionPhase != null) ActionPhase.Invoke();
                this.actionState = ActionState.Free;
            }
            else if (turn == PlayerTurn.Discard)
            {
                if (DiscardPhase != null) DiscardPhase.Invoke();
                this.actionState = ActionState.None;
            }
            else if (turn == PlayerTurn.End)
            {
                if (EndPhase != null) EndPhase.Invoke();
            }
            else if (turn == PlayerTurn.OutTurn)
            {
                MainAttack = false;
            }
            if (turn != PlayerTurn.Action && turn != PlayerTurn.OutTurn)
            {
                System.Threading.Timer time = new System.Threading.Timer(delegate(object sender)
                {
                    if (ChangePhase != null) OnWaitingAction += ChangePhase;
                });
                time.Change(500, 0);
            }
        }
    }

    #region Method
    public string GetPhase()
    {
        if (turn == PlayerTurn.Beginning) return "Beginning of Turn";
        if (turn == PlayerTurn.Judgment) return "Judgment Phase";
        if (turn == PlayerTurn.Draw) return "Draw Phase";
        if (turn == PlayerTurn.Action) return "Action Phase";
        if (turn == PlayerTurn.Discard) return "Discard Phase";
        if (turn == PlayerTurn.End) return "End Phase";

        return "End Turn";
    }
    public CardForm GetWeapon()
    {
        Equipment weapon = null;
        if (this.Weapon != null) weapon = this.Weapon.GetComponent<Equipment>();
        if (weapon != null && weapon.Form != null) return weapon.Form;
        else return null;
    }
    public CardForm GetArmor()
    {
        Equipment armor = null;
        if (this.Armor != null) armor = this.Armor.GetComponent<Equipment>();
        if (armor != null && armor.Form != null) return armor.Form;
        else return null;
    }
    public CardForm GetPlusVehicle()
    {
        Equipment plus = null;
        if (this.PlusVehicle != null) plus = this.PlusVehicle.GetComponent<Equipment>();
        if (plus != null && plus.Form != null) return plus.Form;
        else return null;
    }
    public CardForm GetMinusVehicle()
    {
        Equipment minus = null;
        if (this.MinusVehicle != null) minus = this.MinusVehicle.GetComponent<Equipment>();
        if (minus != null && minus.Form != null) return minus.Form;
        else return null;
    }
    public void DiscardEquipment(CardForm cf)
    {
        Equipment weapon = null, armor = null, plus = null, minus = null;
        if (this.Weapon != null) weapon = this.Weapon.GetComponent<Equipment>();
        if (this.Armor != null) armor = this.Armor.GetComponent<Equipment>();
        if (this.PlusVehicle != null) plus = this.PlusVehicle.GetComponent<Equipment>();
        if (this.MinusVehicle != null) minus = this.MinusVehicle.GetComponent<Equipment>();
        if (weapon != null && weapon.Form != null && weapon.Form == cf)
        {
            weapon.Form = null;
            this.Weapon.SetActive(false);
        }
        if (armor != null && armor.Form != null && armor.Form == cf)
        {
            armor.Form = null;
            this.Armor.SetActive(false);
        }
        if (plus != null && plus.Form != null && plus.Form == cf)
        {
            plus.Form = null;
            this.PlusVehicle.SetActive(false);
        }
        if (minus != null && minus.Form != null && minus.Form == cf)
        {
            minus.Form = null;
            this.MinusVehicle.SetActive(false);
        }
    }
    #endregion

    #region Event Handler
    private IEnumerator afterHealing(int number, Player source, Player victim)
    {
        int busy = game.GetBusyTask();
        int free = game.GetFreeTask();

        game.busy[busy] = false;
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator brinkOfDeath(int number, Player source, Player victim)
    {
        int busy = game.GetBusyTask();
        int free = game.GetFreeTask();
        actionState = ActionState.WaitingBoD;

        //do something

        game.busy[busy] = false;
        yield return new WaitForSeconds(0.1f);
    }

    private IEnumerator attackDamageModifier(int number, Player source, Player victim)
    {
        int busy = game.GetBusyTask();
        int free = game.GetFreeTask();
        Debug.Log(free);
        if (CommandSeal)
        {
            source.DamageIncrease += 1;
            CommandSeal = false;
        }
        CardForm weapon = GetWeapon();
        if (weapon != null && weapon.DamageIncrease != null) { game.busy[free] = true; StartCoroutine(weapon.DamageIncrease(number, source, victim)); }
        while (game.busy[free]) yield return new WaitForSeconds(0.1f);
        if (Character1 != null) { game.busy[free] = true; StartCoroutine(Character1.AbilityActive(CharacterAbility.AbilityForm.AttackDamageModifier, number, source, victim)); }
        while (game.busy[free]) yield return new WaitForSeconds(0.1f);
        if (Character2 != null) { game.busy[free] = true; StartCoroutine(Character2.AbilityActive(CharacterAbility.AbilityForm.AttackDamageModifier, number, source, victim)); }
        while (game.busy[free]) yield return new WaitForSeconds(0.1f);
        game.busy[busy] = false;
        yield return new WaitForSeconds(0.1f);
    }

    private IEnumerator damageModifier(int number, Player source, Player victim)
    {
        int busy = game.GetBusyTask();
        int free = game.GetFreeTask();

        game.busy[busy] = false;
        yield return new WaitForSeconds(0.1f);
    }

    private IEnumerator healing(int number, Player source, Player victim)
    {
        int busy = game.GetBusyTask();
        int free = game.GetFreeTask();
        if (HealModifier != null) { game.busy[free] = true; StartCoroutine(HealModifier(number, source, victim)); }
        while (game.busy[free]) yield return new WaitForSeconds(0.1f);
        if (victim.CurrentHealth < victim.MaxHealth)
        {
            victim.CurrentHealth += number;
            if (AfterHealing != null) { game.busy[free] = true; StartCoroutine(AfterHealing(number, source, victim)); }
            while (game.busy[free]) yield return new WaitForSeconds(0.1f);
        }
        game.busy[busy] = false;
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator beforeAttack(int number, Player source, Player victim)
    {
        int busy = game.GetBusyTask();
        int free = game.GetFreeTask();
        actionState = ActionState.None;
        targetPlayer = victim;
        MainAttack = true;
        game.busy[busy] = false;
        yield return new WaitForSeconds(0.1f);
    }

    private IEnumerator beforeAttacked(int number, Player source, Player victim)
    {
        int busy = game.GetBusyTask();
        IsDodge = false;
        //game.Busy = true;
        actionState = ActionState.WaitingDodge;
        targetPlayer = source;
        if (source.DodgeAble)
        {
            if (!AutoAI)
            {
                if (victim == game.myPlayer)
                {

                    game.btnCancel.SetActive(true);
                    game.CancelClick += delegate()
                    {
                        IsDodge = false;
                        game.btnCancel.SetActive(false);
                        game.CancelClick = null;
                        game.busy[busy] = false;
                    };
                    yield return new WaitForSeconds(3);

                    if (game.btnCancel.activeSelf)
                    {
                        game.btnCancel.SetActive(false);
                        game.CancelClick = null;
                    }
                }
            }
            else
            {
                List<CardForm> hand = game.CardList.GetHandList(this);
                foreach (CardForm cf in hand)
                {
                    if (cf is Dodge)
                    {
                        Dodge dodgeCard = cf as Dodge;
                        dodgeCard.UseCard();
                        if (!source.AdditionDodge) IsDodge = true;
                    }
                }
            }
        }
        game.busy[busy] = false;
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator damageCalculation(int number, Player source, Player victim)
    {
        int busy = game.GetBusyTask();
        int free = game.GetFreeTask();
        int damage = (number + source.DamageIncrease) - victim.DamageDecrease;
        victim.DamageTaken = damage;
        if (victim.CurrentHealth - damage > 0) victim.CurrentHealth -= damage;
        else
        {
            victim.CurrentHealth = 0;
            int negativeHealth = victim.CurrentHealth - damage;
            if (victim.BrinkOfDeath != null) { game.busy[free] = true; victim.StartCoroutine(victim.BrinkOfDeath(negativeHealth, source, victim)); }
            while (game.busy[free]) yield return new WaitForSeconds(0.1f);
        }
        source.DamageIncrease = 0;
        victim.DamageDecrease = 0;
        game.busy[busy] = false;
        yield return new WaitForSeconds(0.1f);
    }

    private IEnumerator takeMagicDamage(int number, Player source, Player victim)
    {
        //while (game.Busy || game.Busy2) yield return new WaitForSeconds(0.1f);
        int busy = game.GetBusyTask();
        int free = game.GetFreeTask();
        lastDamagePlayer = source;

        CardForm armor = GetArmor();
        if (armor != null && armor.TakeMagicDamage != null) { game.busy[free] = true; StartCoroutine(armor.TakeMagicDamage(number, source, victim)); }
        while (game.busy[free]) yield return new WaitForSeconds(0.1f);

        if (Character1 != null) { game.busy[free] = true; StartCoroutine(Character1.AbilityActive(CharacterAbility.AbilityForm.TakeMagicDamage, number, source, victim));}
        while (game.busy[free]) yield return new WaitForSeconds(0.1f);
        if (Character1 != null) { game.busy[free] = true; StartCoroutine(Character1.AbilityActive(CharacterAbility.AbilityForm.TakeMagicDamage, number, source, victim)); }
        while (game.busy[free]) yield return new WaitForSeconds(0.1f);
        victim.DamageTaken = 0;
        game.busy[busy] = false;
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator takePhysicDamage(int number, Player source, Player victim)
    {
        int busy = game.GetBusyTask();
        int free = game.GetFreeTask();
        lastDamagePlayer = source;

        CardForm armor = GetArmor();
        if (armor != null && armor.TakePhysicDamage != null) { game.busy[free] = true; StartCoroutine(armor.TakePhysicDamage(number, source, victim)); }
        while (game.busy[free]) yield return new WaitForSeconds(0.1f);

        if (Character1 != null) { game.busy[free] = true; StartCoroutine(Character1.AbilityActive(CharacterAbility.AbilityForm.TakePhysicalDamage, number, source, victim));}
        while (game.busy[free]) yield return new WaitForSeconds(0.1f);
        if (Character1 != null) { game.busy[free] = true; StartCoroutine(Character1.AbilityActive(CharacterAbility.AbilityForm.TakePhysicalDamage, number, source, victim)); }
        while (game.busy[free]) yield return new WaitForSeconds(0.1f);
        victim.DamageTaken = 0;
        game.busy[busy] = false;
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator causeMagicDamage(int number, Player source, Player victim)
    {
        //while (game.Busy || game.Busy2) yield return new WaitForSeconds(0.1f);
        int busy = game.GetBusyTask();
        int free = game.GetFreeTask();
        lastDamagePlayer = source;

        if (victim.Attacked)
        {
            victim.Attacked = false;
            CardForm weapon = GetWeapon();
            if (weapon != null && weapon.CauseMagicDamage != null) { game.busy[free] = true; StartCoroutine(weapon.CauseMagicDamage(number, source, victim)); }
            while (game.busy[free]) yield return new WaitForSeconds(0.1f);
        }

        if (Character1 != null) { game.busy[free] = true; StartCoroutine(Character1.AbilityActive(CharacterAbility.AbilityForm.CauseMagicDamage, number, source, victim)); }
        while (game.busy[free]) yield return new WaitForSeconds(0.1f);

        if (Character2 != null) { game.busy[free] = true; StartCoroutine(Character2.AbilityActive(CharacterAbility.AbilityForm.CauseMagicDamage, number, source, victim)); }
        while (game.busy[free]) yield return new WaitForSeconds(0.1f);

        game.busy[busy] = false;
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator causePhysicDamage(int number, Player source, Player victim)
    {
        int busy = game.GetBusyTask();
        int free = game.GetFreeTask();
        lastDamagePlayer = source;

        if (victim.Attacked)
        {
            victim.Attacked = false;
            CardForm weapon = GetWeapon();
            if (weapon != null && weapon.CausePhysicDamage != null) { game.busy[free] = true; StartCoroutine(weapon.CausePhysicDamage(number, source, victim)); }
            while (game.busy[free]) yield return new WaitForSeconds(0.1f);
        }

        if (Character1 != null) { game.busy[free] = true; StartCoroutine(Character1.AbilityActive(CharacterAbility.AbilityForm.CausePhysicalDamage, number, source, victim));}
        while (game.busy[free]) yield return new WaitForSeconds(0.1f);
        if (Character2 != null) { game.busy[free] = true; StartCoroutine(Character2.AbilityActive(CharacterAbility.AbilityForm.CausePhysicalDamage, number, source, victim)); }
        while (game.busy[free]) yield return new WaitForSeconds(0.1f);
        game.busy[busy] = false;
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator endAttack(int number, Player source, Player victim)
    {
        int busy = game.GetBusyTask();
        int free = game.GetFreeTask();
        targetPlayer = null;
        if (turn == PlayerTurn.Action)
        {
            actionState = ActionState.Free;
        }
        else
        {
            actionState = ActionState.None;
        }
        victim.lastDamageCard = null;
        game.busy[busy] = false;
        if (game.GetBusyTask() < 0) game.PilesCollect();
        yield return new WaitForSeconds(0.1f);
    }

    private IEnumerator onJudgment()
    {
        int busy = game.GetBusyTask();
        int free = game.GetFreeTask();
        //while (game.Busy || game.Busy2) yield return new WaitForSeconds(0.1f);
        #region OnJudgment of Character 1 Ability Check
        if (this.Character1 != null)
        {
            foreach (CharacterAbility ability in this.Character1.Ability)
            {
                if (ability != null)
                {
                    if (ability.Form == CharacterAbility.AbilityForm.OnJudgment && ability.Status)
                    {
                        //Invoke ability or effect that happend before the judgment take an effect
                        game.busy[free] = true;
                        StartCoroutine(ability.Ability(0, this, this));

                        //Use Busy2 to stop the queuing
                        while (game.busy[free]) yield return new WaitForSeconds(0.1f);
                    }
                }
            }
        }
        #endregion

        #region OnJudgment of Character 2 Ability Check
        if (this.Character2 != null)
        {
            foreach (CharacterAbility ability in this.Character2.Ability)
            {
                if (ability != null)
                {
                    if (ability.Form == CharacterAbility.AbilityForm.OnJudgment && ability.Status)
                    {
                        //Invoke ability or effect that happend before the judgment take an effect
                        game.busy[free] = true;
                        StartCoroutine(ability.Ability(0, this, this));

                        //Use Busy2 to stop the queuing
                        while (game.busy[free]) yield return new WaitForSeconds(0.1f);
                    }
                }
            }
        }
        #endregion

        game.busy[busy] = false;
    }

    private IEnumerator beforeJudgmentTakeEffect()
    {
        int busy = game.GetBusyTask();
        int free = game.GetFreeTask();

        game.busy[busy] = false;
        yield return new WaitForSeconds(0.1f);
    }

    private IEnumerator afterJudgmentTakeEffect()
    {
        int busy = game.GetBusyTask();
        int free = game.GetFreeTask();

        game.busy[busy] = false;
        yield return new WaitForSeconds(0.1f);
    }
    #endregion

    void Start()
    {
        //GameObject btnHealth = gameObject.transform.FindChild("P6Health").GetComponent();
        try
        {
            if (gameObject.transform.FindChild("Weapon") != null) Weapon = gameObject.transform.FindChild("Weapon").gameObject;
            if (gameObject.transform.FindChild("Armor") != null) Armor = gameObject.transform.FindChild("Armor").gameObject;
            if (gameObject.transform.FindChild("Plus Vehicle") != null) PlusVehicle = gameObject.transform.FindChild("Plus Vehicle").gameObject;
            if (gameObject.transform.FindChild("Minus Vehicle") != null) MinusVehicle = gameObject.transform.FindChild("Minus Vehicle").gameObject;
            if (gameObject.transform.FindChild("Delay1") != null) Delay1 = gameObject.transform.FindChild("Delay1").gameObject;
            if (gameObject.transform.FindChild("Delay2") != null) Delay2 = gameObject.transform.FindChild("Delay2").gameObject;
            if (gameObject.transform.FindChild("Delay3") != null) Delay3 = gameObject.transform.FindChild("Delay3").gameObject;
            if (gameObject.transform.FindChild("Delay4") != null) Delay4 = gameObject.transform.FindChild("Delay4").gameObject;
            if (gameObject.transform.FindChild("Character1") != null) CharacterObject1 = gameObject.transform.FindChild("Character1").gameObject;
            if (gameObject.transform.FindChild("Character2") != null) CharacterObject2 = gameObject.transform.FindChild("Character2").gameObject;
            Transform healthObject = gameObject.transform.FindChild("Health");
            if (healthObject != null) health = healthObject.transform.FindChild("Text").GetComponent<Text>();
            Transform handObject = gameObject.transform.FindChild("Hand");
            if (handObject != null) hand = handObject.transform.FindChild("Text").GetComponent<Text>();

        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        if (Weapon != null) Weapon.SetActive(false);
        if (Armor != null) Armor.SetActive(false);
        if (PlusVehicle != null) PlusVehicle.SetActive(false);
        if (MinusVehicle != null) MinusVehicle.SetActive(false);
        if (Delay1 != null) Delay1.SetActive(false);
        if (Delay2 != null) Delay2.SetActive(false);
        if (Delay3 != null) Delay3.SetActive(false);
        if (Delay4 != null) Delay4.SetActive(false);

        if (CharacterObject1 != null)
        {
            Character1 = CharacterObject1.GetComponent<Character>();
            //CharacterObject1.SetActive(false);
            //Character1.game = this.game;
            //Character1.PlayerOwner = this;
        }
        if (CharacterObject2 != null)
        {
            Character2 = CharacterObject2.GetComponent<Character>();
            //CharacterObject2.SetActive(false);
            //Character2.game = this.game;
            //Character2.PlayerOwner = this;
        }

        this.Healing += healing;
        this.AfterHealing += afterHealing;
        this.BrinkOfDeath += brinkOfDeath;
        this.AttackDamageModifier += attackDamageModifier;
        this.DamageCalculation += damageCalculation;
        this.BeforeAttack += beforeAttack;
        this.BeforeAttacked += beforeAttacked;
        this.EndAttack += endAttack;
        this.TakeMagicDamage += takeMagicDamage;
        this.TakePhysicDamage += takePhysicDamage;
        this.CauseMagicDamage += causeMagicDamage;
        this.CausePhysicDamage += causePhysicDamage;
        this.DamageModifier += damageModifier;
        this.OnJudgment += onJudgment;
        this.BeforeJudgmentTakeEffect += beforeJudgmentTakeEffect;
        this.AfterJudgmentTakeEffect += afterJudgmentTakeEffect;
    }

    void Update()
    {
        try
        {
            if (health != null) health.text = this.CurrentHealth.ToString() + " / " + this.MaxHealth.ToString();
            List<CardForm> handList = game.CardList.GetHandList(this);
            if (hand != null) hand.text = handList.Count.ToString();
            if (OnWaitingAction != null)
            {
                OnWaitingAction.Invoke();
                OnWaitingAction = null;
            }
            if (game != null && DrawPhase == null)
            {
                this.DrawPhase += game.DrawCard;
            }
            if (game != null && ChangePhase == null)
            {
                this.ChangePhase += game.ChangePhase;
            }
            if (CharacterObject1 != null) Character1 = CharacterObject1.GetComponent<Character>();
            if (CharacterObject2 != null) Character2 = CharacterObject2.GetComponent<Character>();
            if (this.Character1 != null)
            {
                this.Character1.game = this.game;
                this.Character1.PlayerOwner = this;
            }
            if (this.Character2 != null)
            {
                this.Character2.game = this.game;
                this.Character2.PlayerOwner = this;
            }
        }
        catch { }
    }

}

