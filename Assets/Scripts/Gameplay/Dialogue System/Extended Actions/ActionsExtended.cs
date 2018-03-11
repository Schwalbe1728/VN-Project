using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DialogueAction
{
    #region Hurt Player
    [SerializeField]
    private bool hurtPlayerSet;
    [SerializeField]
    private HurtPlayerAction hurtPlayer;

    public bool HurtPlayerSet { get { return hurtPlayerSet; } set { hurtPlayerSet = value; } }
    public HurtPlayerAction HurtPlayer { get { return hurtPlayer; } }
    #endregion
    #region Hurt Player's Sanity
    [SerializeField]
    private bool hurtPlayerSanitySet;
    [SerializeField]
    private HurtPlayerSanityAction hurtPlayerSanity;

    public bool HurtPlayerSanitySet { get { return hurtPlayerSanitySet; } set { hurtPlayerSanitySet = value; } }
    public HurtPlayerSanityAction HurtPlayerSanity { get { return hurtPlayerSanity; } }
    #endregion

    public override void DoAction()
    {
        if (hurtPlayerSet) hurtPlayer.DoAction();
        if (hurtPlayerSanitySet) hurtPlayerSanity.DoAction();
    }

    public void SetHurtPlayerAction(int damage)
    {
        hurtPlayer = new HurtPlayerAction();
        hurtPlayer.Damage = damage;
        hurtPlayerSet = true;
    }

    public void ClearHurtPlayerAction() { hurtPlayerSet = false; }

    public void SetHurtPlayerSanityAction(int damage)
    {
        hurtPlayerSanity = new HurtPlayerSanityAction();
        hurtPlayerSanity.Damage = damage;
        hurtPlayerSanitySet = true;
    }

    public void ClearHurtPlayerSanityAction() { hurtPlayerSanitySet = false; }
}

[System.Serializable]
public class HurtPlayerAction : DialogueActionBase
{
    public int Damage;

    public override void DoAction()
    {
        // TODO: Znajdź obiekt gracza i zadaj graczowi obrażenia
    }
}

[System.Serializable]
public class HurtPlayerSanityAction : DialogueActionBase
{
    public int Damage;

    public override void DoAction()
    {
        // TODO: Znajdź obiekt gracza i zadaj graczowi obrażenia
    }
}