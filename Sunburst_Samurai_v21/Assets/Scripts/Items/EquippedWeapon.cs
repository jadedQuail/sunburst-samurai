using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedWeapon : MonoBehaviour
{
    GameObject player;
    [SerializeField] string equippedWeaponName;
    [SerializeField] Sprite equippedWeaponSprite;
    [SerializeField] float weaponDamage;
    [SerializeField] ParticleSystem weaponTrail;

    [SerializeField] int buyValue;
    [SerializeField] int sellValue;

    private bool currentlyHeld = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Function that gives the name of the weapon to the HUD
    public string GetWeaponName()
    {
        return equippedWeaponName;
    }

    // Function that gives the weapon image to the HUD
    public Sprite GetWeaponSprite()
    {
        return equippedWeaponSprite;
    }

    public int GetWeaponSellAmount()
    {
        return sellValue;
    }

    // Function that returns the item's buy value
    public int GetWeaponBuyAmount()
    {
        return buyValue;
    }

    // Function that gives the weapon damage to the HUD
    public float GetWeaponDamage()
    {
        return weaponDamage;
    }

    // Function that stops the weapon trail
    public void StopWeaponTrail()
    {
        weaponTrail.Stop();
    }

    // Function that starts the weapon trail
    public void StartWeaponTrail()
    {
        weaponTrail.Play();
    }

    // Function that indicates whether this weapon is being held or not
    public bool GetIsHeld()
    {
        return currentlyHeld;
    }

    // Function that allows other scripts to set whether or not this item is being held
    public void SetIsHeld(bool value)
    {
        currentlyHeld = value;
    }
}
