using UnityEngine;

public class Settings : MonoBehaviour
{
    [SerializeField] GameObject[] panels;

    GameObject currentPanel;

    private void Start()
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }

        SetPanel(FindObjectOfType<OpenCloseSettings>().SettingsStartIndex);
        currentPanel.SetActive(true);
    }

    public void SetPanel(int index)
    {
        if (currentPanel != null) currentPanel.SetActive(false);
        currentPanel = panels[index];
        currentPanel.SetActive(true);
    }
}
