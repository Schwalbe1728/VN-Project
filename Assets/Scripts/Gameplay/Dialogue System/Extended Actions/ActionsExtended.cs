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
    #region Change Music
    [SerializeField]
    private bool changeMusicSet;
    [SerializeField]
    private ChangeMusicAction changeMusic;

    public bool ChangeMusicSet { get { return changeMusicSet; } set { changeMusicSet = value; } }
    public ChangeMusicAction ChangeMusic { get { return changeMusic; } }
    #endregion
    #region Change Ambience
    [SerializeField]
    private bool changeAmbienceSet;
    [SerializeField]
    private ChangeAmbienceAction changeAmbience;

    public bool ChangeAmbienceSet { get { return changeAmbienceSet; } set { changeAmbienceSet = value; } }
    public ChangeAmbienceAction ChangeAmbience { get { return changeAmbience; } }
    #endregion

    #region Give Item
    [SerializeField]
    private bool giveItemSet;
    [SerializeField]
    private GiveItemAction giveItem;
    public bool GiveItemSet { get { return giveItemSet; } set { giveItemSet = value; } }
    public GiveItemAction GiveItem { get { return giveItem; } }
    #endregion

    #region Take Item
    [SerializeField]
    private bool takeItemSet;
    [SerializeField]
    private TakeItemAction takeItem;
    public bool TakeItemSet { get { return takeItemSet; } set { takeItemSet = value; } }
    public TakeItemAction TakeItem { get { return takeItem; } }
    #endregion

    public override void DoAction()
    {
        if (hurtPlayerSet) hurtPlayer.DoAction();
        if (hurtPlayerSanitySet) hurtPlayerSanity.DoAction();
        if (changeMusicSet) changeMusic.DoAction();
        if (changeAmbienceSet) changeAmbience.DoAction();
        if (giveItemSet) giveItem.DoAction();
        if (takeItemSet) takeItem.DoAction();
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

    public void SetChangeMusicAction(AudioClip clip)
    {
        changeMusic = new ChangeMusicAction();
        changeMusic.Clip = clip;        
        changeMusicSet = true;
    }

    public void ClearChangeMusicAction() { changeMusicSet = false; }

    public void SetChangeAmbienceAction(AudioClip clip)
    {
        changeAmbience = new ChangeAmbienceAction();
        changeAmbience.Clip = clip;
        changeAmbienceSet = true;
    }

    public void ClearChangeAmbienceAction() { changeAmbienceSet = false; }

    public void SetGiveItemAction(/*EquipmentItem item*/)
    {
        giveItem = new GiveItemAction();
        //giveItem.Item = item;
        giveItemSet = true;
    }

    public void ClearGiveItemAction() { GiveItemSet = false; }

    public void SetTakeItemAction(/*EquipmentItem item*/)
    {
        takeItem = new TakeItemAction();
        //TakeItem.Item = item;
        takeItemSet = true;
    }

    public void ClearTakeItemAction() { TakeItemSet = false; }
}

[System.Serializable]
public class HurtPlayerAction : DialogueActionBase
{
    public int Damage;

    public override void DoAction()
    {
        // TODO: Znajdź obiekt gracza i zadaj graczowi obrażenia

        GameObject.Find("Game Info Component").GetComponent<CharacterInfoScript>().ChangeHealth(-Damage);
    }
}

[System.Serializable]
public class HurtPlayerSanityAction : DialogueActionBase
{
    public int Damage;

    public override void DoAction()
    {
        // TODO: Znajdź obiekt gracza i zadaj graczowi obrażenia
        GameObject.Find("Game Info Component").GetComponent<CharacterInfoScript>().ChangeSanity(-Damage);
    }
}
#region Audio Changing
[System.Serializable]
public abstract class ChangeAudioAction : DialogueActionBase
{
    public AudioClip Clip;    
}

[System.Serializable]
public class ChangeMusicAction : ChangeAudioAction
{   
    public override void DoAction()
    {
        SoundManagerScript sms = GameObject.Find("Settings").GetComponent<SoundManagerScript>();
        sms.PlayMusic(Clip);
    }
}

[System.Serializable]
public class ChangeAmbienceAction : ChangeAudioAction
{    
    public override void DoAction()
    {
        SoundManagerScript sms = GameObject.Find("Settings").GetComponent<SoundManagerScript>();
        sms.PlayAmbience(Clip);
    }
}
#endregion

#region Equipment Changes
public abstract class EquipmentChangeAction : DialogueActionBase
{
    //public EquipmentItem Item;
}

[System.Serializable]
public class GiveItemAction : EquipmentChangeAction
{
    public override void DoAction()
    {
        base.DoAction();
    }
}

[System.Serializable]
public class TakeItemAction : EquipmentChangeAction
{
    public override void DoAction()
    {
        base.DoAction();
    }
}

#endregion