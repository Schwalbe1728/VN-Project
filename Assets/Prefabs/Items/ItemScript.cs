using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Equipment Item")]
public class ItemScript : ScriptableObject
{    
    [SerializeField]
    private new string name;
    [SerializeField]    
    private int value;
    [SerializeField]
    private EquipmentType type;

    public string Name { get { return name; } }
    public int AbstractValue { get { return value; } }
    public EquipmentType EqType { get { return type; } }

    #region Weapon Specific
    public bool IsWeapon
    {
        get
        {
            return EqType == EquipmentType.OneHandedWeapon || EqType == EquipmentType.TwoHandedWeapon;
        }
    }

    [SerializeField]
    private int bonusDamageMin;

    [SerializeField]
    private int bonusDamageMax;

    public int BonusDamage
    {
        get { return IsWeapon ? Random.Range(bonusDamageMin, bonusDamageMax + 1) : 0; }
    }

    public int MinDamage { get { return IsWeapon ? bonusDamageMin : 0; } }
    public int MaxDamage { get { return IsWeapon ? bonusDamageMax : 0; } }

    #endregion

    #region Edibles Specific
    public bool IsEdible { get { return EqType == EquipmentType.Consumables; } }    

    [SerializeField]
    private int vitalityHealed;

    [SerializeField]
    private int sanityHealed;    
    
    #endregion
    
    private static float useTimeVariance = 0.2f;
    [SerializeField]
    private int timeConsumed;

    public bool IsUsable { get { return IsEdible || IsWeapon; } }

    public int TimeConsumedMin { get { return IsUsable ? Mathf.RoundToInt(timeConsumed * (1.0f - useTimeVariance)) : 0; } }
    public int TimeConsumedMax { get { return IsUsable ? Mathf.RoundToInt(timeConsumed * (1.0f + useTimeVariance)) : 0; } }
    private int TimeConsumed { get { return Random.Range(TimeConsumedMin, TimeConsumedMax + 1); } }

    public void Use()
    {
        TimeManagerScript.AdvanceTime(TimeConsumed);

    }
}

public enum EquipmentType
{
    OneHandedWeapon,
    TwoHandedWeapon,
    Clothes,
    Consumables,
    Money,
    Quest
}

public static class ItemExtension
{
    private const int AbstractToPence = 4;
    private const int PenceToShilling = 12;
    private const int ShillingToPound = 20;    

    public static string MonetaryValueInCoin(this ItemScript item, int quantity = 1)
    {
        int totalValue = item.AbstractValue * quantity;

        int pounds, shillings;
        float pences;

        ToCoins(totalValue, out pounds, out shillings, out pences);

        StringBuilder sb = new StringBuilder();

        if(pounds > 0)
        {
            sb.Append(pounds);
            sb.Append(" Pounds");
        }

        if(shillings > 0)
        {
            if(sb.Length > 0)
            {
                sb.Append(", ");
            }

            sb.Append(shillings);
            sb.Append(" Shillings");
        }

        if (pences > 0)
        {
            if (sb.Length > 0)
            {
                sb.Append(", ");
            }

            sb.AppendFormat("{0:0.00} Pences", pences);
        }

        if(totalValue == 0)
        {
            sb.Append("N/A");
        }

        return sb.ToString();
    }

    public static void GetValuesInCoin(this ItemScript item, out int pounds, out int shillings, out float pences)
    {
        int value = item.AbstractValue;

        ToCoins(value, out pounds, out shillings, out pences);
    }

    public static void GetValuesInCoin(this ItemScript[] items, out int pounds, out int shillings, out float pences)
    {
        int value = 0;
        foreach(ItemScript item in items) { value += item.AbstractValue; }

        ToCoins(value, out pounds, out shillings, out pences);
    }

    public static float ValueToPence(this ItemScript item)
    {        
        return ToPence(item.AbstractValue);
    }

    public static int ValueToShilling(this ItemScript item)
    {
        return ToShilling(item.AbstractValue);
    }

    public static int ValueToPound(this ItemScript item)
    {
        return ToPound(item.AbstractValue);
    }

    public static void ToCoins(int value, out int pounds, out int shillings, out float pences)
    {
        pounds = ToPound(value);
        value -= pounds * ShillingToPound * PenceToShilling * AbstractToPence;

        shillings = ToShilling(value);
        value -= shillings * PenceToShilling * AbstractToPence;

        pences = ToPence(value);
    }

    private static float ToPence(float val)
    {
        return (val / AbstractToPence);
    }

    private static int ToShilling(float val)
    {
        return Mathf.FloorToInt(val / (AbstractToPence * PenceToShilling));
    }

    private static int ToPound(float val)
    {
        return Mathf.FloorToInt(val / (AbstractToPence * PenceToShilling * ShillingToPound));
    }    
}