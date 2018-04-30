using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Background", menuName = "VN-Project/Journal Entry")]
public class JournalEntryScript : ScriptableObject
{
    public string EntryID;

    [Multiline]
    [SerializeField]    
    private string journalEntryText;

    private static int MaxEntryLength = 200;

    public string JournalEntryText
    {
        get
        {
            StringBuilder sb = new StringBuilder(journalEntryText);
            if(TextWithinLimit)
            {
                sb.Length = MaxEntryLength;
            }

            return sb.ToString();

        }
    }

    public bool TextWithinLimit { get { return journalEntryText.Length > MaxEntryLength; } }

    public JournalEntry GetEntry(WorldDate date)
    {
        return
            new JournalEntry(this.EntryID, this.JournalEntryText, date);
    }
}

[System.Serializable]
public class JournalEntry
{
    [SerializeField]
    private string entryID;

    [SerializeField]
    private string entryText;

    [SerializeField]
    private WorldDate dateOfCreation;

    public string ID { get { return entryID; } }
    public string Text { get { return entryText; } }
    public WorldDate DateOfAdding { get { return dateOfCreation; } }

    public JournalEntry(string id, string text, WorldDate date)
    {
        entryID = id;
        entryText = text;
        dateOfCreation = date;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine(dateOfCreation.ToString());
        sb.AppendLine();
        sb.AppendLine(entryText);

        return sb.ToString();
    }
}

public static class JournalEntryExtension
{
    public static int Compare(this JournalEntry entryA, JournalEntry entryB)
    {        
        bool AisLater = entryA.DateOfAdding.CompareTo(entryB.DateOfAdding, InequalityTypes.Greater, false, false);
        bool BisLater = entryB.DateOfAdding.CompareTo(entryA.DateOfAdding, InequalityTypes.Greater, false, false);

        return
            AisLater ? 1 : (BisLater ? -1 : 0);
    }
}

[System.Serializable]
public class Journal
{
    [SerializeField]
    private List<JournalEntry> entries;

    private HashSet<string> presentEntriesIDs;

    public Journal()
    {
        if (entries == null)
        {
            entries = new List<JournalEntry>();
        }
                
        InstantiateEntriesIndexer();
    }

    public bool TryInsertEntry(JournalEntry entry)
    {
        if(presentEntriesIDs == null)
        {
            InstantiateEntriesIndexer();
        }

        bool result = !EntryIsInJournal(entry);

        if(result)
        {
            AddEntry(entry);
        }

        return result;
    }

    public JournalEntry[] GetJournalEntries()
    {
        entries.Sort(JournalEntryExtension.Compare);
        return entries.ToArray();
    }

    private bool EntryIsInJournal(JournalEntry entry)
    {
        return presentEntriesIDs.Contains(entry.ID);
    }

    private void AddEntry(JournalEntry entry)
    {
        entries.Add(entry);
        presentEntriesIDs.Add(entry.ID);
    }

    private void InstantiateEntriesIndexer()
    {
        presentEntriesIDs = new HashSet<string>();

        if(entries != null)
        {
            foreach(JournalEntry entry in entries)
            {
                if (!presentEntriesIDs.Contains(entry.ID))
                {
                    presentEntriesIDs.Add(entry.ID);
                }
            }
        }
    }
}