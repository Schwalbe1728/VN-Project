using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DialogueAction
{
    #region Game State

    [SerializeField]
    private bool advanceTimeSet;
    [SerializeField]
    private AdvanceTimeAction advanceTime;
    public bool AdvanceTimeSet { get { return advanceTimeSet; } set { advanceTimeSet = value; } }
    public AdvanceTimeAction AdvanceTime { get { return advanceTime; } }

    #endregion
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

    #region Use Item
    [SerializeField]
    private bool useItemSet;
    [SerializeField]
    private UseItemAction useItem;
    public bool UseItemSet { get { return useItemSet; } set { useItemSet = value; } }
    public UseItemAction UseItem { get { return useItem; } }
    #endregion

    [SerializeField]
    private bool updateJournalSet;
    [SerializeField]
    private UpdateJournalAction updateJournal;
    public bool UpdateJournalSet { get { return updateJournalSet; } set { updateJournalSet = value; } }
    public UpdateJournalAction UpdateJournal { get { return updateJournal; } }


    public override void DoAction()
    {
        if (advanceTimeSet) advanceTime.DoAction();
        if (hurtPlayerSet) hurtPlayer.DoAction();
        if (hurtPlayerSanitySet) hurtPlayerSanity.DoAction();
        if (changeMusicSet) changeMusic.DoAction();
        if (changeAmbienceSet) changeAmbience.DoAction();
        if (giveItemSet) giveItem.DoAction();
        if (takeItemSet) takeItem.DoAction();
        if (useItemSet) useItem.DoAction();
        if (updateJournalSet) updateJournal.DoAction();
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

    public void SetGiveItemAction(ItemScript item, int quantity)
    {
        giveItem = new GiveItemAction();
        giveItem.Item = item;
        giveItem.Quantity = quantity;
        giveItemSet = true;
    }

    public void ClearGiveItemAction() { GiveItemSet = false; }

    public void SetTakeItemAction(ItemScript item, int quantity)
    {
        takeItem = new TakeItemAction();
        takeItem.Item = item;
        takeItem.Quantity = quantity;
        takeItemSet = true;
    }

    public void ClearTakeItemAction() { TakeItemSet = false; }

    public void SetUseItemAction(ItemScript item)
    {
        useItem = new UseItemAction();
        useItem.Item = item;
        useItemSet = true;
    }

    public void ClearUseItemAction() { UseItemSet = false; }

    public void SetAdvanceTimeAction(int seconds, bool allowVariance, float varianceValue)
    {
        advanceTime = new AdvanceTimeAction();
        advanceTime.Seconds = seconds;
        advanceTime.VarianceSet = allowVariance;
        advanceTime.VarianceValue = varianceValue;
        advanceTimeSet = true;
    }

    public void ClearAdvanceTimeAction()
    {
        AdvanceTimeSet = false;
    }

    public void SetUpdateJournalAction(JournalEntryScript entry)
    {
        updateJournal = new UpdateJournalAction();
        updateJournal.Entry = entry;
        updateJournalSet = true;
    }

    public void ClearUpdateJournalAction()
    {
        updateJournalSet = false;
    }

    public override string ToString()
    {
        int set = 0;

        if (AdvanceTimeSet) set++;
        if (ChangeAmbienceSet) set++;
        if (ChangeMusicSet) set++;
        if (GiveItemSet) set++;
        if (HurtPlayerSanitySet) set++;
        if (HurtPlayerSet) set++;
        if (TakeItemSet) set++;
        if (UseItemSet) set++;
        if (UpdateJournalSet) set++;

        return "Defined Actions: " + set.ToString();
    }
}

[System.Serializable]
public class AdvanceTimeAction : DialogueActionBase
{
    public int Seconds;
    public bool VarianceSet;
    public float VarianceValue;

    public override void DoAction()
    {
        if (VarianceSet)
        {
            TimeManagerScript.AdvanceTimeWithRandomVariance(VarianceValue, Seconds);
        }
        else
        {
            TimeManagerScript.AdvanceTime(Seconds);
        }
    }
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
    public ItemScript Item;
}

[System.Serializable]
public class GiveItemAction : EquipmentChangeAction
{
    public int Quantity;

    public override void DoAction()
    {
        PlayerEquipmentScript equipment = 
            GameObject.Find("Game Info Component").GetComponent<PlayerEquipmentScript>();

        equipment.AddItems(Item, Quantity);
    }
}

[System.Serializable]
public class TakeItemAction : EquipmentChangeAction
{
    public int Quantity;

    public override void DoAction()
    {
        PlayerEquipmentScript equipment =
            GameObject.Find("Game Info Component").GetComponent<PlayerEquipmentScript>();

        equipment.TakeItem(Item, Quantity, true);
    }
}

[System.Serializable]
public class UseItemAction : EquipmentChangeAction
{
    public override void DoAction()
    {
        PlayerEquipmentScript equipment =
            GameObject.Find("Game Info Component").GetComponent<PlayerEquipmentScript>();

        equipment.UseItem(Item);
    }
}

[System.Serializable]
public class UpdateJournalAction : DialogueActionBase
{
    public JournalEntryScript Entry;

    public override void DoAction()
    {
        //sprawdź czy wpis został już dodany do dziennika       

        JournalEntry entry = Entry.GetEntry(TimeManagerScript.GetDate());
        JournalUIScript.TryInsertEntry(entry);
        //wstaw do dziennika
    }
}

#endregion