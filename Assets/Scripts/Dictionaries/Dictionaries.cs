using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class NodeIDToSpriteDictionary : SerializableDictionary<int, Sprite>
{
    public new Sprite this[int key]
    {
        get
        {
            return (ContainsKey(key)) ?
                base[key] : null;
        }
        set
        {
            if (ContainsKey(key))
            {
                base[key] = value;
            }
            else
            {
                return;
            }
        }
    }
}

[Serializable]
public class NodeIDToAudioClipDictionary : SerializableDictionary<int, AudioClip>
{
    public new AudioClip this[int key]
    {
        get
        {
            return (ContainsKey(key)) ?
                base[key] : null;
        }
        set
        {
            if (ContainsKey(key))
            {
                base[key] = value;
            }
            else
            {
                return;
            }
        }
    }
}

[Serializable]
public class CharStatsToValueDictionary : SerializableDictionary<CharacterStat, int>
{
    public CharStatsToValueDictionary(int basicValue = 1) : base()
    {
        Add(CharacterStat.Agility, basicValue);
        Add(CharacterStat.Perception, basicValue);
        Add(CharacterStat.Character, basicValue);
        Add(CharacterStat.Wits, basicValue);
        Add(CharacterStat.Physique, basicValue);
    }

    public new int this[CharacterStat key]
    {
        get
        {
            return (ContainsKey(key)) ?
                base[key] : -1;
        }
        set
        {
            if (ContainsKey(key))
            {
                base[key] = value;
            }
            else
            {
                return;
            }
        }
    }
}

[Serializable]
public class PlotValuesDictionary : SerializableDictionary<string, string>
{
    public new string this[string key]
    {
        get
        {
            key = key.ToLower();

            return (ContainsKey(key)) ?
                base[key].ToLower() : string.Empty;
        }
        set
        {
            key = key.ToLower();

            if (ContainsKey(key))
            {
                base[key] = value;
            }
            else
            {
                return;
            }
        }
    }
}

[Serializable]
public class StateTransitionDictionary : SerializableDictionary<StateTransition, string>
{
    public new string this[StateTransition key]
    {
        get
        {
            return (ContainsKey(key)) ?
                base[key].ToLower() : string.Empty;
        }
        set
        {
            if (ContainsKey(key))
            {
                base[key] = value;
            }
            else
            {
                return;
            }
        }
    }
}

[Serializable]
public class PossibleStateTransitionDictionary : SerializableDictionary<string, StateTransition[]>
{
    public new StateTransition[] this[string key]
    {
        get
        {
            key = key.ToLower();

            return (ContainsKey(key)) ?
                base[key] : null;
        }
        set
        {
            key = key.ToLower();

            if (ContainsKey(key))
            {
                base[key] = value;
            }
            else
            {
                return;
            }
        }
    }
}


[Serializable]
public class EquipmentItemToQuantity : SerializableDictionary<string, int>
{
    public static string ItemToDictionaryKey(ItemScript item) { return item.Name.ToLower(); }

    public new int this[string key]
    {
        get
        {
            string lower = key.ToLower();

            return
                ContainsKey(lower) ?
                    base[lower] : 0;
        }

        set
        {
            string lower = key.ToLower();

            if(ContainsKey(lower))
            {
                if (value >= 0)
                {
                    base[lower] = value;
                }
                else
                {
                    base[lower] = 0;
                }
            }
            else
            {

            }
        }
    }

    public int this[ItemScript key]
    {
        get
        {
            string lower = ItemToDictionaryKey(key);

            return
                ContainsKey(lower) ?
                    base[lower] : 0;
        }

        set
        {
            string lower = ItemToDictionaryKey(key);

            if (ContainsKey(lower))
            {
                if (value >= 0)
                {
                    base[lower] = value;
                }
                else
                {
                    base[lower] = 0;
                }
            }
            else
            {

            }
        }
    }
}
