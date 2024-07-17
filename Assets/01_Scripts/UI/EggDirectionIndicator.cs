using System;
using UnityEngine;

public class EggDirectionIndicator : MonoBehaviour
{
    Chicken player;
    Transform playerTransform;
    Transform egg;
    Camera playerCamera;

    [SerializeField] RectTransform directionIndicatorUI; // UI element to indicate the direction
    [SerializeField] float heightScaler = 50;
    [SerializeField] GameObject eggArrow;

    private void Start()
    {
        playerCamera = Camera.main;
        FindPlayerAndEgg();
        GameManager.Instance.OnSpawnChicken += FindPlayerAndEgg;
    }

    void Update()
    {
        ShowEggArrow();

        Vector3 playerToEgg = egg.position - playerTransform.position;
        playerToEgg.y = 0; // Ignore y difference

        // Calculate the angle between the player's forward direction and the direction to the egg
        float angle = Vector3.Angle(playerTransform.forward, playerToEgg);

        // If the egg is within the camera's field of view, hide the indicator
        Vector3 screenPoint = playerCamera.WorldToViewportPoint(egg.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

        if (onScreen || player.HasEgg)
        {
            directionIndicatorUI.gameObject.SetActive(false);
            return;
        }

        directionIndicatorUI.gameObject.SetActive(true);

        // Determine if the egg is to the left or right of the player
        Vector3 crossProduct = Vector3.Cross(playerTransform.forward, playerToEgg);
        bool isLeft = crossProduct.y < 0;

        // Position the indicator at the edge of the screen
        Vector3 viewportCenter = new Vector3(0.5f, 0.5f, 0);
        float horizontalInset = directionIndicatorUI.rect.width; // Slightly inset from the edge (0.5 - 0.05)
        float verticalInset = directionIndicatorUI.rect.height; // Slightly inset from the edge (0.5 - 0.05)

        if (isLeft)
        {
            directionIndicatorUI.anchorMin = new Vector2(0, 0.5f);
            directionIndicatorUI.anchorMax = new Vector2(0, 0.5f);
            directionIndicatorUI.anchoredPosition = new Vector3(horizontalInset, 0, 0);
        }
        else
        {
            directionIndicatorUI.anchorMin = new Vector2(1, 0.5f);
            directionIndicatorUI.anchorMax = new Vector2(1, 0.5f);
            directionIndicatorUI.anchoredPosition = new Vector3(-horizontalInset, 0, 0);
        }

        // Adjust for vertical positioning based on y difference
        float yDifference = (egg.position.y - playerTransform.position.y) * heightScaler;
        float maxHeight = Screen.height * 0.5f * verticalInset; // Half the screen height for normalization
        float verticalPosition = Mathf.Clamp(yDifference, -maxHeight, maxHeight);

        directionIndicatorUI.anchoredPosition = new Vector3(directionIndicatorUI.anchoredPosition.x, verticalPosition, 0);
    }

    private void ShowEggArrow()
    {
        eggArrow.transform.position = egg.position;

        float distanceEggToPlayer = Vector3.Distance(egg.position, playerTransform.position);
        eggArrow.transform.localScale = Vector3.one * distanceEggToPlayer;

        eggArrow.SetActive(!player.HasEgg);
    }

    void FindPlayerAndEgg()
    {
        player = GameManager.Instance.Player;
        playerTransform = player.transform;
        egg = player.Egg.transform;
    }
}
