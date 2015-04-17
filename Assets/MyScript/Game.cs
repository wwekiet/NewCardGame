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
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class Game : MonoBehaviour
{
    //public Hashtable PlayerList = new Hashtable();
    public Hashtable CardInGame = new Hashtable();
    public Hashtable GameObjList = new Hashtable();
    public List<CardForm> CardList = new List<CardForm>();
    public GameObject mainPanel, mainPlayerPanel, handPanel, tempPanel, characterPanel;
    public GameObject playerPanel6, playerPanel1, playerPanel2, playerPanel3, playerPanel4, playerPanel5;
    public GameObject playerPanel7, playerPanel8, playerPanel9, playerPanel10, playerPanel11;
    public GameObject equipmentPanel;
    public Player myPlayer;
    public Player oppPlayer;
    public List<Player> playerList = new List<Player>();
    public Player playerTurn;
    private CardForm processingCard;
    public GameObject btnUse, btnCancel;
    //public bool Busy = false;
    //public bool Busy2 = false;
    //public bool Busy3 = false;
    //public bool Busy4 = false;
    //public bool Busy5 = false;
    //public bool Busy6 = false;

    public bool[] busy = new bool[] { false, false, false, false, false, false, false, false, false, false };
    public bool Modal = false;
    public string UsedAsStr = "";
    //public List<bool> Task = new List<bool>();
    Text phase, yourHealth, useButtonText;

    public delegate bool ConditionDelegate();
    public ConditionDelegate CustomCondition = null;

    public delegate void EventCustom();
    public EventCustom CancelClick = null;
    public EventCustom UseAsSomething = null;

    public CardForm ProcessingCard
    {
        get { return processingCard; }
        set
        {
            if (processingCard != null) processingCard.Form.HasBorder = false;
            processingCard = value;
            if (processingCard != null) processingCard.Form.HasBorder = true;
            if (processingCard != null)
            {
                //btnCancel.SetActive(true);
                bool condition = false;
                if (CustomCondition != null) condition = CustomCondition.Invoke();

                if (condition)
                {
                    btnUse.SetActive(true);
                }
                else if (processingCard is Attack || processingCard is MagicAttack)
                {
                    if (!processingCard.Form.Owner.MainAttack
                        && processingCard.Form.Owner.actionState == Player.ActionState.Free)
                    {
                        btnUse.SetActive(true);
                    }
                    else
                    {
                        btnUse.SetActive(false);
                    }
                }
                else if (processingCard is Dodge)
                {
                    if (processingCard.Form.Owner.actionState == Player.ActionState.WaitingDodge)
                    {
                        btnUse.SetActive(true);
                    }
                    else
                    {
                        btnUse.SetActive(false);
                    }
                }
                else if (processingCard is HolyGrail)
                {
                    if (processingCard.Form.Owner.actionState == Player.ActionState.WaitingBoD)
                    {
                        btnUse.SetActive(true);
                    }
                    else if (processingCard.Form.Owner.actionState == Player.ActionState.Free
                        && processingCard.Form.Owner.CurrentHealth < processingCard.Form.Owner.MaxHealth)
                    {
                        btnUse.SetActive(true);
                    }
                }
                else if (processingCard is CommandSeal)
                {
                    if (processingCard.Form.Owner.actionState == Player.ActionState.WaitingBoD)
                    {
                        btnUse.SetActive(true);
                    }
                    else if (processingCard.Form.Owner.actionState == Player.ActionState.Free
                        && !processingCard.Form.Owner.CommandSeal)
                    {
                        btnUse.SetActive(true);
                    }
                }
                else if (processingCard.Form.CardData.Type == Card.CardType.Weapon ||
                    processingCard.Form.CardData.Type == Card.CardType.Armor ||
                    processingCard.Form.CardData.Type == Card.CardType.MinusVehicle ||
                    processingCard.Form.CardData.Type == Card.CardType.PlusVehicle)
                {
                    if (processingCard.Form.Owner.actionState == Player.ActionState.Free)
                    {
                        btnUse.SetActive(true);
                    }
                }
                else
                {
                    btnUse.SetActive(false);
                }

                if (processingCard.Form.Owner.actionState == Player.ActionState.WaitingDiscard)
                {
                    useButtonText.text = "Discard";
                }
                else if (processingCard.Form.Owner.actionState == Player.ActionState.UseAs)
                {
                    useButtonText.text = "Use As " + UsedAsStr;
                }
                else
                {
                    useButtonText.text = "Use";
                }
            }
            else
            {
                //btnCancel.SetActive(false);
                btnUse.SetActive(false);
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        //Setup Global Game Variable
        mainPanel = GameObject.Find("Main Panel");
        mainPlayerPanel = GameObject.Find("MainPlayer Panel");
        playerPanel1 = GameObject.Find("Player 1 Panel");
        playerPanel2 = GameObject.Find("Player 2 Panel");
        playerPanel3 = GameObject.Find("Player 3 Panel");
        playerPanel4 = GameObject.Find("Player 4 Panel");
        playerPanel5 = GameObject.Find("Player 5 Panel");
        playerPanel6 = GameObject.Find("Player 6 Panel");
        playerPanel7 = GameObject.Find("Player 7 Panel");
        playerPanel8 = GameObject.Find("Player 8 Panel");
        playerPanel9 = GameObject.Find("Player 9 Panel");
        playerPanel10 = GameObject.Find("Player 10 Panel");
        playerPanel11 = GameObject.Find("Player 11 Panel");

        characterPanel = GameObject.Find("Character Panel");
        handPanel = GameObject.Find("Hand Card Panel");
        btnUse = GameObject.Find("Use Button");
        btnCancel = GameObject.Find("Cancel Button");
        tempPanel = GameObject.Find("Use Card Panel");
        equipmentPanel = GameObject.Find("Equipment Panel");
        phase = gameObject.transform.FindChild("Phase").transform.FindChild("Text").GetComponent<Text>();
        btnUse.SetActive(false);
        btnCancel.SetActive(false);
        yourHealth = characterPanel.transform.FindChild("Health").transform.FindChild("Text").GetComponent<Text>();
        useButtonText = gameObject.transform.FindChild("Use Button").transform.FindChild("Text").GetComponent<Text>();
        //Initilizing All Deck Cards
        InitListCard();

        List<Player> temp = new List<Player>();
        myPlayer = mainPlayerPanel.GetComponent<Player>();
        myPlayer.game = this;
        temp.Add(mainPlayerPanel.GetComponent<Player>());
        temp.Add(playerPanel1.GetComponent<Player>());
        temp.Add(playerPanel2.GetComponent<Player>());
        temp.Add(playerPanel3.GetComponent<Player>());
        temp.Add(playerPanel4.GetComponent<Player>());
        temp.Add(playerPanel5.GetComponent<Player>());
        temp.Add(playerPanel6.GetComponent<Player>());
        temp.Add(playerPanel7.GetComponent<Player>());
        temp.Add(playerPanel8.GetComponent<Player>());
        temp.Add(playerPanel9.GetComponent<Player>());
        temp.Add(playerPanel10.GetComponent<Player>());
        temp.Add(playerPanel11.GetComponent<Player>());

        busy[0] = false;
        busy[1] = false;
        busy[2] = false;
        busy[3] = false;
        busy[4] = false;
        busy[5] = false;

        //Draw 4 cards starting for each player
        //DrawXCard(4, myPlayer);
        //DrawXCard(4, oppPlayer);
        StartCoroutine(Starting(temp));
        //myPlayer.DrawPhase += DrawCard;
        //myPlayer.ChangePhase += ChangePhase;
        //oppPlayer.DrawPhase += DrawCard;
        //oppPlayer.ChangePhase += ChangePhase;

    }

    // Update is called once per frame
    void Update()
    {
        if (playerTurn != null) phase.text = playerTurn.GetPhase();
        if (yourHealth != null) yourHealth.text = myPlayer.CurrentHealth + "/" + myPlayer.MaxHealth;
        foreach (CardForm cf in CardList)
        {
            cf.Update();
        }
    }

    #region Function
    public IEnumerator Starting(List<Player> temp)
    {
        foreach (Player p in temp)
        {
            if (p.Status)
            {
                p.game = this;
                playerList.Add(p);
            }
            else
            {
                p.gameObject.SetActive(false);
            }
        }

        foreach (Player p in playerList)
        {
            DrawXCard(4, p);
            //if (p.AutoAI)
            //{
            //    Avalon avalon = new Avalon(Card.CardSuit.Heart, Card.CardNumber.Eight, Card.CardState.Equipment, p, this);
            //    CardList.Add(avalon);
            //    avalon.Form.FaceUp = true;
            //    avalon.Form.Active = true;
            //    avalon.Form.Owner = p;
            //    avalon.UseCard();
            //}
            yield return new WaitForSeconds(0.1f);
        }
        //Determine who goes first
        playerTurn = myPlayer;
        playerTurn.Turn = Player.PlayerTurn.Beginning;
    }
    public void DrawCard()
    {
        DrawXCard(2, playerTurn);
    }
    public void DrawOneCard()
    {
        DrawXCard(1, playerTurn);
    }
    public void DrawXCard(int number, Player player)
    {
        if (number == 0) return;
        System.Random rd = new System.Random();
        List<CardForm> deck = CardList.GetDeckList();
        int index = rd.Next(0, deck.Count - 1);
        CardForm cf = deck[index];
        cf.DrawFromDeck(number, player);
    }

    public void InitListCard()
    {
        CardList.Add(new Attack(Card.CardSuit.Club, Card.CardNumber.Eight, Card.CardState.None, null, this));
        CardList.Add(new Attack(Card.CardSuit.Club, Card.CardNumber.Five, Card.CardState.None, null, this));
        CardList.Add(new Attack(Card.CardSuit.Club, Card.CardNumber.Four, Card.CardState.None, null, this));
        CardList.Add(new Attack(Card.CardSuit.Diamond, Card.CardNumber.Jack, Card.CardState.None, null, this));
        CardList.Add(new Attack(Card.CardSuit.Heart, Card.CardNumber.King, Card.CardState.None, null, this));
        CardList.Add(new Attack(Card.CardSuit.Heart, Card.CardNumber.Ace, Card.CardState.None, null, this));
        CardList.Add(new Attack(Card.CardSuit.Diamond, Card.CardNumber.Ace, Card.CardState.None, null, this));
        CardList.Add(new Attack(Card.CardSuit.Spade, Card.CardNumber.Six, Card.CardState.None, null, this));
        CardList.Add(new Attack(Card.CardSuit.Spade, Card.CardNumber.Ten, Card.CardState.None, null, this));
        CardList.Add(new Attack(Card.CardSuit.Spade, Card.CardNumber.Three, Card.CardState.None, null, this));


        CardList.Add(new MagicAttack(Card.CardSuit.Club, Card.CardNumber.Eight, Card.CardState.None, null, this));
        CardList.Add(new MagicAttack(Card.CardSuit.Club, Card.CardNumber.Five, Card.CardState.None, null, this));
        CardList.Add(new MagicAttack(Card.CardSuit.Club, Card.CardNumber.Four, Card.CardState.None, null, this));
        CardList.Add(new MagicAttack(Card.CardSuit.Diamond, Card.CardNumber.Jack, Card.CardState.None, null, this));
        CardList.Add(new MagicAttack(Card.CardSuit.Heart, Card.CardNumber.King, Card.CardState.None, null, this));
        CardList.Add(new MagicAttack(Card.CardSuit.Heart, Card.CardNumber.Ace, Card.CardState.None, null, this));
        CardList.Add(new MagicAttack(Card.CardSuit.Diamond, Card.CardNumber.Ace, Card.CardState.None, null, this));
        CardList.Add(new MagicAttack(Card.CardSuit.Spade, Card.CardNumber.Six, Card.CardState.None, null, this));
        CardList.Add(new MagicAttack(Card.CardSuit.Spade, Card.CardNumber.Ten, Card.CardState.None, null, this));
        CardList.Add(new MagicAttack(Card.CardSuit.Spade, Card.CardNumber.Three, Card.CardState.None, null, this));

        //CardList.Add(new BerserCar(Card.CardSuit.Diamond, Card.CardNumber.Eight, Card.CardState.None, null, this));
        //CardList.Add(new GaeBuidhe(Card.CardSuit.Diamond, Card.CardNumber.Eight, Card.CardState.None, null, this));
        //CardList.Add(new GilgameshArmor(Card.CardSuit.Club, Card.CardNumber.Eight, Card.CardState.None, null, this));
        CardList.Add(new Hrunting(Card.CardSuit.Heart, Card.CardNumber.Eight, Card.CardState.None, null, this));
        //CardList.Add(new KanshouBakuya(Card.CardSuit.Heart, Card.CardNumber.Eight, Card.CardState.None, null, this));
        //CardList.Add(new Monohoshizao(Card.CardSuit.Club, Card.CardNumber.Eight, Card.CardState.None, null, this));
        //CardList.Add(new Pegasus(Card.CardSuit.Club, Card.CardNumber.Eight, Card.CardState.None, null, this));
        //CardList.Add(new Vimana(Card.CardSuit.Club, Card.CardNumber.Eight, Card.CardState.None, null, this));
        //CardList.Add(new YamahaVmax(Card.CardSuit.Spade, Card.CardNumber.Eight, Card.CardState.None, null, this));
        //CardList.Add(new PrelatiSpellbook(Card.CardSuit.Spade, Card.CardNumber.Eight, Card.CardState.None, null, this));
        //CardList.Add(new Avalon(Card.CardSuit.Heart, Card.CardNumber.Eight, Card.CardState.None, null, this));
        //CardList.Add(new SaberArmor(Card.CardSuit.Spade, Card.CardNumber.Eight, Card.CardState.None, null, this));


        CardList.Add(new AestusEstus(Card.CardSuit.Club, Card.CardNumber.Seven, Card.CardState.None, null, this));
        CardList.Add(new AestusEstus(Card.CardSuit.Club, Card.CardNumber.Seven, Card.CardState.None, null, this));
        CardList.Add(new AestusEstus(Card.CardSuit.Club, Card.CardNumber.Seven, Card.CardState.None, null, this));
        CardList.Add(new AestusEstus(Card.CardSuit.Club, Card.CardNumber.Seven, Card.CardState.None, null, this));
        CardList.Add(new AestusEstus(Card.CardSuit.Club, Card.CardNumber.Seven, Card.CardState.None, null, this));
        CardList.Add(new AestusEstus(Card.CardSuit.Club, Card.CardNumber.Seven, Card.CardState.None, null, this));
        CardList.Add(new AestusEstus(Card.CardSuit.Club, Card.CardNumber.Seven, Card.CardState.None, null, this));
        CardList.Add(new AestusEstus(Card.CardSuit.Club, Card.CardNumber.Seven, Card.CardState.None, null, this));

        //CardList.Add(new Dodge(Card.CardSuit.Spade, Card.CardNumber.Three, Card.CardState.None, null, this));
        //CardList.Add(new Dodge(Card.CardSuit.Heart, Card.CardNumber.Four, Card.CardState.None, null, this));
        //CardList.Add(new Dodge(Card.CardSuit.Spade, Card.CardNumber.Ace, Card.CardState.None, null, this));
        //CardList.Add(new Dodge(Card.CardSuit.Club, Card.CardNumber.Two, Card.CardState.None, null, this));
        //CardList.Add(new Dodge(Card.CardSuit.Heart, Card.CardNumber.Queen, Card.CardState.None, null, this));
        //CardList.Add(new Dodge(Card.CardSuit.Diamond, Card.CardNumber.Seven, Card.CardState.None, null, this));
        //CardList.Add(new Dodge(Card.CardSuit.Diamond, Card.CardNumber.Ten, Card.CardState.None, null, this));
        //CardList.Add(new Dodge(Card.CardSuit.Heart, Card.CardNumber.King, Card.CardState.None, null, this));

        CardList.Add(new HolyGrail(Card.CardSuit.Heart, Card.CardNumber.Three, Card.CardState.None, null, this));
        CardList.Add(new HolyGrail(Card.CardSuit.Heart, Card.CardNumber.Three, Card.CardState.None, null, this));
        CardList.Add(new HolyGrail(Card.CardSuit.Heart, Card.CardNumber.Three, Card.CardState.None, null, this));
        CardList.Add(new HolyGrail(Card.CardSuit.Heart, Card.CardNumber.Three, Card.CardState.None, null, this));
        CardList.Add(new HolyGrail(Card.CardSuit.Heart, Card.CardNumber.Three, Card.CardState.None, null, this));
        CardList.Add(new HolyGrail(Card.CardSuit.Heart, Card.CardNumber.Three, Card.CardState.None, null, this));
        CardList.Add(new HolyGrail(Card.CardSuit.Heart, Card.CardNumber.Three, Card.CardState.None, null, this));

        CardList.Add(new CommandSeal(Card.CardSuit.Spade, Card.CardNumber.Three, Card.CardState.None, "CommandSeal1", null, this));
        CardList.Add(new CommandSeal(Card.CardSuit.Club, Card.CardNumber.Three, Card.CardState.None, "CommandSeal2", null, this));
        CardList.Add(new CommandSeal(Card.CardSuit.Spade, Card.CardNumber.Three, Card.CardState.None, "CommandSeal3", null, this));
        CardList.Add(new CommandSeal(Card.CardSuit.Spade, Card.CardNumber.Three, Card.CardState.None, "CommandSeal4", null, this));
        CardList.Add(new CommandSeal(Card.CardSuit.Spade, Card.CardNumber.Three, Card.CardState.None, "CommandSeal5", null, this));
        CardList.Add(new CommandSeal(Card.CardSuit.Club, Card.CardNumber.Three, Card.CardState.None, "CommandSeal6", null, this));

        CardList.Shuffle();
    }

    public void UseCard()
    {
        if (useButtonText.text == "Use") ProcessingCard.UseCard();
        else if (useButtonText.text == "Discard") ProcessingCard.Discard();
        else
        {
            if (UseAsSomething != null) UseAsSomething.Invoke();
            UseAsSomething = null;
        }
        this.ProcessingCard = null;
    }

    public void UseAs()
    {
        if (UseAsSomething != null) UseAsSomething.Invoke();
        UseAsSomething = null;
        this.ProcessingCard = null;
    }

    public void Cancel()
    {
        btnCancel.SetActive(false);
        if (CancelClick != null) CancelClick.Invoke();
    }

    public virtual void PilesCollect()
    {
        StartCoroutine(Collect());
    }

    IEnumerator Collect()
    {
        yield return new WaitForSeconds(1);
        List<CardForm> usingCard = CardList.GetUsingList();
        foreach (CardForm cf in usingCard)
        {
            cf.Form.State = Card.CardState.Piles;
            cf.Form.CardData.State = Card.CardState.Piles;
            cf.Form.Active = false;
        }
    }

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

    public void CancelProcessingCard()
    {
        this.ProcessingCard = null;
    }

    public void Attack(int number, Player source, Player victim, bool physical)
    {
        StartCoroutine(AttackAction(number, source, victim, physical));
    }

    public int GetFreeTask()
    {
        for (int i = 0; i < busy.Count(); i++)
        {
            if (!busy[i]) return i;
        }
        return -1;
    }

    public int GetBusyTask()
    {
        for (int i = busy.Count()-1; i >= 0; i--)
        {
            if (busy[i]) return i;
        }
        return -1;
    }

    public IEnumerator AttackAction(int number, Player source, Player victim, bool physical)
    {
        //while (Busy || Busy2) yield return new WaitForSeconds(0.1f);
        int i = GetFreeTask();
        int j = GetBusyTask();
        //Boolean task = busy[GetFreeTask()];
        Debug.Log(GetFreeTask());
        busy[i] = true;
        if (source.AttackDamageModifier != null) StartCoroutine(source.AttackDamageModifier(number, source, victim));
        Debug.Log("Attack Damage Modifier");
        while (busy[i]) yield return new WaitForSeconds(0.1f);
        busy[i] = true;
        if (source.BeforeAttack != null) StartCoroutine(source.BeforeAttack(number, source, victim));
        Debug.Log("Before Attack");
        while (busy[i]) yield return new WaitForSeconds(0.1f);
        busy[i] = true;
        if (victim.BeforeAttacked != null) StartCoroutine(victim.BeforeAttacked(number, source, victim));
        Debug.Log("Before Attacked");
        while (busy[i]) yield return new WaitForSeconds(0.1f);

        if (!victim.IsDodge)
        {
            busy[i] = true;
            if (victim.DamageCalculation != null) StartCoroutine(victim.DamageCalculation(number, source, victim));
            Debug.Log("Damage Calculation");
            while (busy[i]) yield return new WaitForSeconds(0.1f);
            number = victim.DamageTaken;
            if (!physical)
            {
                busy[i] = true;
                if (source.CauseMagicDamage != null) StartCoroutine(source.CauseMagicDamage(number, source, victim));
                Debug.Log("Cause Magic Damage");
                while (busy[i]) yield return new WaitForSeconds(0.1f);
                busy[i] = true;
                if (victim.TakeMagicDamage != null) StartCoroutine(victim.TakeMagicDamage(number, source, victim));
                Debug.Log("Take Magic Damage");
            }
            else
            {
                busy[i] = true;
                if (source.CausePhysicDamage != null) StartCoroutine(source.CausePhysicDamage(number, source, victim));
                Debug.Log("Cause Physic Damage");
                while (busy[i]) yield return new WaitForSeconds(0.1f);
                busy[i] = true;
                if (victim.TakePhysicDamage != null) StartCoroutine(victim.TakePhysicDamage(number, source, victim));
                Debug.Log("Take Physic Damage");
            }
            while (busy[i]) yield return new WaitForSeconds(0.1f);
            busy[i] = true;
            if (source.EndAttack != null) StartCoroutine(source.EndAttack(number, source, victim));
            while (busy[i]) yield return new WaitForSeconds(0.1f);
            busy[i] = true;
            if (victim.EndAttack != null) StartCoroutine(victim.EndAttack(number, source, victim));
            Debug.Log("End Attack");
        }
        else
        {
            busy[i] = true;
            if (source.EndAttack != null) StartCoroutine(source.EndAttack(number, source, victim));
            while (busy[i]) yield return new WaitForSeconds(0.1f);
            busy[i] = true;
            if (victim.EndAttack != null) StartCoroutine(victim.EndAttack(number, source, victim));
            Debug.Log("End Attack");
        }
        if (j > 0) busy[j] = false;
        yield return new WaitForSeconds(0.1f);
    }

    public IEnumerator InflictDamage(int number, Player source, Player victim, bool physical)
    {
        int j = GetBusyTask();
        int i = GetFreeTask();
        busy[i] = true;
        if (source.DamageModifier != null) source.StartCoroutine(source.DamageModifier(number, source, victim));
        while (busy[i]) yield return new WaitForSeconds(0.1f);
        busy[i] = true;
        if (victim.DamageCalculation != null) victim.StartCoroutine(victim.DamageCalculation(number, source, victim));
        while (busy[i]) yield return new WaitForSeconds(0.1f);
        if(physical)
        {
            busy[i] = true;
            if (source.CausePhysicDamage != null) source.StartCoroutine(source.CausePhysicDamage(number, source, victim));
            while (busy[i]) yield return new WaitForSeconds(0.1f);
            busy[i] = true;
            if (victim.TakePhysicDamage != null) victim.StartCoroutine(victim.TakePhysicDamage(number, source, victim));
            while (busy[i]) yield return new WaitForSeconds(0.1f);
        }
        else
        {
            busy[i] = true;
            if (source.CauseMagicDamage != null) source.StartCoroutine(source.CauseMagicDamage(number, source, victim));
            while (busy[i]) yield return new WaitForSeconds(0.1f);
            busy[i] = true;
            if (victim.TakeMagicDamage != null) victim.StartCoroutine(victim.TakeMagicDamage(number, source, victim));
            while (busy[i]) yield return new WaitForSeconds(0.1f);
        }

        if(j>0)busy[j] = false;
        yield return new WaitForSeconds(0.1f);
    }
    public void ChangePhase()
    {
        //playerTurn = myPlayer;
        if (playerTurn.Turn == Player.PlayerTurn.OutTurn) playerTurn.Turn = Player.PlayerTurn.Beginning;
        else if (playerTurn.Turn == Player.PlayerTurn.Beginning) playerTurn.Turn = Player.PlayerTurn.Judgment;
        else if (playerTurn.Turn == Player.PlayerTurn.Judgment) playerTurn.Turn = Player.PlayerTurn.Draw;
        else if (playerTurn.Turn == Player.PlayerTurn.Draw) playerTurn.Turn = Player.PlayerTurn.Action;
        else if (playerTurn.Turn == Player.PlayerTurn.Action) playerTurn.Turn = Player.PlayerTurn.Discard;
        else if (playerTurn.Turn == Player.PlayerTurn.Discard) playerTurn.Turn = Player.PlayerTurn.End;
        else if (playerTurn.Turn == Player.PlayerTurn.End)
        {
            playerTurn.Turn = Player.PlayerTurn.OutTurn;

            //Determine next player
            int currentIndex = -1;
            for (int i = 0; i < playerList.Count; i++)
            {
                if (playerTurn == playerList[i]) currentIndex = i;
            }
            if (currentIndex < (playerList.Count - 1)) currentIndex++;
            else
            {
                currentIndex = 0;
            }
            playerTurn = playerList[currentIndex];
            playerTurn.Turn = Player.PlayerTurn.Beginning;
        }
    }

    public List<GameObject> GetPlayerInAttackRange()
    {
        List<GameObject> list = new List<GameObject>();
        foreach (Player p in playerList)
        {
            if (p == myPlayer)
            {
                continue;
            }
            else
            {
                int range = GetRange(myPlayer, p);
                //Debug.Log("Player " + p.name + ": " + range);
                int physicalRange = range + p.PlusDistance;
                int myAttackingRange = myPlayer.AttackingRange + myPlayer.MinusDistance;
                Debug.Log(physicalRange + "/" + myAttackingRange);
                if (range + p.PlusDistance <= myPlayer.AttackingRange + myPlayer.MinusDistance)
                {
                    list.Add(p.gameObject);
                }
            }
        }
        return list;
    }
    public int GetRange(Player source, Player victim)
    {
        int playerIndex1 = -1;
        int playerIndex2 = -1;
        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i] == source)
            {
                playerIndex1 = i;
            }
            else if (playerList[i] == victim)
            {
                playerIndex2 = i;
            }
        }

        if (playerIndex1 > -1 && playerIndex2 > -1)
        {
            int range1 = 0;
            int range2 = 0;
            if (playerIndex1 < playerIndex2)
            {
                do
                {
                    range1++;
                }
                while (playerIndex1 + range1 < playerIndex2);

                int rangeX = 0;
                int rangeY = 0;
                do
                {
                    rangeX++;
                }
                while (playerIndex1 - rangeX > 0);
                if (playerIndex1 == 0) rangeX = 0;
                do
                {
                    rangeY++;
                }
                while ((playerList.Count) - rangeY > playerIndex2);
                range2 = rangeX + rangeY;
            }
            else if (playerIndex1 > playerIndex2)
            {
                do
                {
                    range1++;
                }
                while (playerIndex1 - range1 > playerIndex2);

                int rangeX = 0;
                int rangeY = 0;
                do
                {
                    rangeX++;
                }
                while (playerIndex1 + rangeX < playerList.Count);
                if (playerIndex1 == playerList.Count - 1) rangeX = 0;
                do
                {
                    rangeY++;
                }
                while (rangeY < playerIndex2);
                range2 = rangeX + rangeY;
            }
            else
            {
                return 0;
            }

            if (range1 < range2 || range1 == range2) return range1;
            else return range2;
        }
        else
        {
            return 0;
        }
    }

    CardForm judgemntFinalResult = null;
    Action<CardForm> actionAfterJudgment = null;
    public void PerformJudgment(Player source, Action<CardForm> judgmentEffect)
    {
        System.Random rd = new System.Random();
        List<CardForm> deck = CardList.GetDeckList();
        int index = rd.Next(0, deck.Count - 1);
        if (index < deck.Count)
        {
            CardForm cf = deck[index];
            Action action = delegate()
            {
                //Start Judgment
                actionAfterJudgment = judgmentEffect;
                StartCoroutine(JudgmentChangeByRound(source, cf));
                StartCoroutine(JudgmentTakeEffect(judgmentEffect));
            };
            //StartCoroutine(JudgmentChangeByRound(source, cf, theNewValue => cf = theNewValue));
            cf.DoJudgment(action);
        }
    }
    public IEnumerator JudgmentChangeByRound(Player start, CardForm cf)
    {
        bool freeTask = busy[GetFreeTask()];
        int currentIndex = -1;
        for (int i = 0; i < playerList.Count; i++)
        {
            if (start == playerList[i]) currentIndex = i;
        }
        //int startIndex = currentIndex;

        if (playerList[currentIndex].OnJudgment != null) { freeTask = true; playerList[currentIndex].StartCoroutine(playerList[currentIndex].OnJudgment()); }
        while (freeTask) yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < playerList.Count; i++)
        {
            GameObject panel = playerList[currentIndex].gameObject;
            Outline border = panel.GetComponent<Outline>();
            if (border != null)
            {
                border.effectColor = Color.blue;
                border.enabled = true;
            }

            if (playerList[currentIndex].BeforeJudgmentTakeEffect != null) { freeTask = true; playerList[currentIndex].StartCoroutine(playerList[currentIndex].BeforeJudgmentTakeEffect()); }
            while (freeTask) yield return new WaitForSeconds(0.1f);

            #region JudgmentChange of Character 1 Ability Check
            if (playerList[currentIndex].Character1 != null)
            {
                foreach (CharacterAbility ability in playerList[currentIndex].Character1.Ability)
                {
                    if (ability != null)
                    {
                        if (ability.Form == CharacterAbility.AbilityForm.JudgmentChange && ability.Status)
                        {
                            //Invoke ability or effect that happend before the judgment take an effect
                            freeTask = true;
                            StartCoroutine(ability.Ability(0, playerList[currentIndex], playerList[currentIndex]));

                            //Use Busy2 to stop the queuing
                            while (freeTask) yield return new WaitForSeconds(0.1f);

                            if (playerList[currentIndex].Character1 is Illya && ability.Used)
                            {
                                if (border != null) border.enabled = false;
                                ability.Used = false;
                                StopCoroutine(JudgmentTakeEffect(actionAfterJudgment));
                                PerformJudgment(start, actionAfterJudgment);
                                yield break;
                            }
                        }
                    }
                }
            }
            #endregion

            #region JudgmentChange of Character 2 Ability Check
            if (playerList[currentIndex].Character2 != null)
            {
                foreach (CharacterAbility ability in playerList[currentIndex].Character2.Ability)
                {
                    if (ability != null)
                    {
                        if (ability.Form == CharacterAbility.AbilityForm.JudgmentChange && ability.Status)
                        {
                            //Invoke ability or effect that happend before the judgment take an effect
                            freeTask = true;
                            StartCoroutine(ability.Ability(0, playerList[currentIndex], playerList[currentIndex]));

                            //Use Busy2 to stop the queuing
                            while (freeTask) yield return new WaitForSeconds(0.1f);

                            if (playerList[currentIndex].Character2 is Illya && ability.Used)
                            {
                                if (border != null) border.enabled = false;
                                ability.Used = false;
                                StopCoroutine(JudgmentTakeEffect(actionAfterJudgment));
                                PerformJudgment(start, actionAfterJudgment);
                                yield break;
                            }
                        }
                    }
                }
            }
            #endregion

            yield return new WaitForSeconds(1f);
            if (border != null) border.enabled = false;
            if (currentIndex < (playerList.Count - 1))
            {
                currentIndex++;
            }
            else
            {
                currentIndex = 0;
            }
        }
        judgemntFinalResult = cf;
    }
    public IEnumerator JudgmentTakeEffect(Action<CardForm> actionAfterJudgment)
    {
        while (judgemntFinalResult == null)
        {
            yield return new WaitForSeconds(0.1f);
        }
        //Judgment Done
        actionAfterJudgment.Invoke(judgemntFinalResult);
        if (GetBusyTask() < 0) PilesCollect();
        judgemntFinalResult = null;
        actionAfterJudgment = null;
    }
    #endregion
}

public static class Extension
{
    public static object GetItemByIndex(this Hashtable list, int index)
    {
        int i = 0;
        foreach (DictionaryEntry item in list)
        {
            if (i == index)
            {
                return item;
            }
            i++;
        }
        return null;
    }

    public static List<CardForm> GetHandList(this List<CardForm> list, Player player)
    {
        List<CardForm> hand = new List<CardForm>();
        foreach (CardForm cf in list)
        {
            if (cf.Form.State == Card.CardState.Hand && cf.Form.Owner == player)
            {
                hand.Add(cf);
            }
        }
        hand = hand.OrderBy(x => x.Form.LastInteract).ToList();
        return hand;
    }

    public static List<CardForm> GetDeckList(this List<CardForm> list)
    {
        List<CardForm> hand = new List<CardForm>();
        foreach (CardForm cf in list)
        {
            if (cf.Form.State == Card.CardState.None)
            {
                hand.Add(cf);
            }
        }
        hand = hand.OrderBy(x => x.Form.LastInteract).ToList();
        return hand;
    }

    public static List<CardForm> GetUsingList(this List<CardForm> list)
    {
        List<CardForm> usingList = new List<CardForm>();
        foreach (CardForm cf in list)
        {
            if (cf.Form.State == Card.CardState.Using)
            {
                usingList.Add(cf);
            }
        }
        usingList = usingList.OrderBy(x => x.Form.LastInteract).ToList();
        return usingList;
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

}


