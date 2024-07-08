using UnityEngine;
using UnityEngine.UI;

public class TutorialSlideShow : MonoBehaviour
{
    [SerializeField] Image showTutorialImage;

    [SerializeField] Sprite[] tutorialImages;

    private void Start()
    {
        showTutorialImage.sprite = tutorialImages[0];
    }
    public void Next()
    {
        if (showTutorialImage.sprite == tutorialImages[tutorialImages.Length - 1])
        {
            showTutorialImage.sprite = tutorialImages[0];
        }
        else
        {
            for (int i = 0; i < tutorialImages.Length; i++)
            {
                if (showTutorialImage.sprite == tutorialImages[i])
                {
                    showTutorialImage.sprite = tutorialImages[i + 1];
                    break;
                }
            }
        }
    }

    public void Previous()
    {
        if (showTutorialImage.sprite == tutorialImages[0])
        {
            showTutorialImage.sprite = tutorialImages[tutorialImages.Length - 1];
        }
        else
        {
            for (int i = 0; i < tutorialImages.Length; i++)
            {
                if (showTutorialImage.sprite == tutorialImages[i])
                {
                    showTutorialImage.sprite = tutorialImages[i - 1];
                    break;
                }
            }
        }
    }
}
