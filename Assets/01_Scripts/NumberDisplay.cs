using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberDisplay : MonoBehaviour
{
    public GameObject digitPrefab;  // Prefab containing the sprite renderer for a digit
    public List<Sprite> numberSprites;  // List of sprites for digits 0-9

    private List<GameObject> digitObjects = new List<GameObject>();

    public void SetNumber(int number)
    {
        // Clear existing digits
        foreach (GameObject digitObject in digitObjects)
        {
            Destroy(digitObject);
        }
        digitObjects.Clear();

        // Convert number to string to process each digit
        string numberStr = number.ToString();

        // Create and place new digit sprites
        for (int i = 0; i < numberStr.Length; i++)
        {
            char digitChar = numberStr[i];
            int digit = int.Parse(digitChar.ToString());

            // Instantiate a new digit object
            GameObject digitObject = Instantiate(digitPrefab, transform);

            // Set the sprite to the corresponding digit sprite
            Image image = digitObject.GetComponent<Image>();
            image.sprite = numberSprites[digit];

            // Add to list of digit objects
            digitObjects.Add(digitObject);
        }
    }
}