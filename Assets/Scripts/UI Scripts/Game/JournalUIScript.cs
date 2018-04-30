using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalUIScript : MonoBehaviour
{
    public delegate void UpdatedMyJournal(JournalEntry entry);

    private static UpdatedMyJournal OnUpdatedMyJournal;
    private static Journal journal;

    [SerializeField]
    private AudioClip journalUpdatedSound;
    private AudioSource sfxAudioSource;

	public static void SetJournal(Journal _journal)
    {
        journal = _journal;                       
    }

    public static void RegisterToOnUpdatedJournal(UpdatedMyJournal ev)
    {
        OnUpdatedMyJournal += ev;
    }
	
	public static void TryInsertEntry(JournalEntry entry)
    {
        if(journal.TryInsertEntry(entry))
        {
            FireOnUpdatedMyJournal(entry);
        }
    }

    void Awake()
    {
        if(journal == null)
        {
            SetJournal(new Journal());
        }

        StartCoroutine(SetSFXAudioSource());
        RegisterToOnUpdatedJournal(PlayUpdatedMyJournalSound);
    }

    void Update()
    {
        /// TEST
        /// 
        if(Input.GetKeyUp(KeyCode.Alpha0))
        {
            if(journal != null)
            {
                JournalEntry[] entries = journal.GetJournalEntries();

                foreach(JournalEntry entry in entries)
                {
                    Debug.Log(entry.ToString());
                }
            }
        }
    }

    private void PlayUpdatedMyJournalSound(JournalEntry entry)
    {
        if(sfxAudioSource != null)
        {
            sfxAudioSource.PlayOneShot(journalUpdatedSound);
        }
    }

    private static void FireOnUpdatedMyJournal(JournalEntry entry)
    {
        if(OnUpdatedMyJournal != null)
        {
            OnUpdatedMyJournal(entry);
        }
    }

    private IEnumerator SetSFXAudioSource()
    {
        GameObject temp = null;

        while(temp == null)
        {
            temp = GameObject.Find("SFX Source");
            yield return new WaitForEndOfFrame();
        }
        
        sfxAudioSource = temp.GetComponent<AudioSource>();

        Debug.Log("SFX Source found and set");
    }
}
