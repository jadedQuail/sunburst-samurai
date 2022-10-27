using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemText : MonoBehaviour
{

    // Basically with this script you can place text or an image above an object (but it's the location within the HUD, not the world)

    public Text nameLabel;
    public Image worldImage;

    [SerializeField] string weaponName;
    [SerializeField] Sprite keySprite;

    private void Start()
    {
        // Start off with the name label off
        nameLabel.gameObject.SetActive(false);
    }

    // Function that turns the in-world text on
    public void TextOn()
    {
        if (nameLabel == null) return;

        Vector3 namePos = Camera.main.WorldToScreenPoint(this.transform.position);
        nameLabel.transform.position = namePos;

        nameLabel.text = weaponName;
        nameLabel.gameObject.SetActive(true);
    }

    // Function that turns on the in-world image, if it exists
    public void ImageOn()
    {
        if (worldImage == null) return;

        Vector3 imgPos = Camera.main.WorldToScreenPoint(this.transform.position);
        worldImage.transform.position = imgPos;

        worldImage.sprite = keySprite;
        worldImage.gameObject.SetActive(true);
    }

    // Function that turns the in-world text off
    public void TextOff()
    {
        nameLabel.gameObject.SetActive(false);
    }

    // Function that turns the in-world image off
    public void ImageOff()
    {
        worldImage.gameObject.SetActive(false);
    }
}
