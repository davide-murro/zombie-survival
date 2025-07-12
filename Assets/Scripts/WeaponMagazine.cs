using UnityEngine;

public class WeaponMagazine : MonoBehaviour
{
    [SerializeField] int capacity = 20;

    public int ammoAmount = 20;

    public int GetCapacity()
    {
        return capacity;
    }
    public int GetCurrentAmmo()
    {
        return ammoAmount;
    }

    public void UseAmmo(int amount)
    {
        ammoAmount -= amount;
    }

    public void ReloadAmmo(int amount)
    {
        if (ammoAmount + amount > capacity)
            ammoAmount = capacity;
        else
            ammoAmount += amount;
    }
}
