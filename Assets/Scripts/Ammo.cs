using System;
using UnityEngine;

[Serializable]
class AmmoSlot
{
    public AmmoType ammoType;
    public int ammoAmount;
}

public class Ammo : MonoBehaviour
{
    [SerializeField] AmmoSlot[] ammoSlots;

    AmmoSlot GetAmmoSlot(AmmoType type)
    {
        foreach (AmmoSlot slot in ammoSlots)
        {
            if (slot.ammoType == type)
            {
                return slot;
            }
        }

        return null;
    }

    public int GetCurrentAmmo(AmmoType type)
    {
        return GetAmmoSlot(type).ammoAmount;
    }

    public void UseAmmo(AmmoType type, int amount)
    {
        GetAmmoSlot(type).ammoAmount -= amount;
    }
}
