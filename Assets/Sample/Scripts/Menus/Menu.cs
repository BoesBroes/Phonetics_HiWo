using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public bool startWithMenuOpen = false;

    public GameObject[] allPanels;
    public GameObject startPanel;
    private GameObject currentOpenPanel;

    public void Start()
    {
        CloseAllPanels();
        if (startWithMenuOpen)
        {
            SwitchPanel(startPanel);
        }
    }

    public void OpenPanelOnTop(GameObject extraPanel)
    {
        extraPanel.SetActive(true);
    }

    //function to switch between panels in a menu
    public void SwitchPanel(GameObject nextPanel)
    {
        if (currentOpenPanel)
        {
            currentOpenPanel.SetActive(false);
        }

        nextPanel.gameObject.SetActive(true);
        currentOpenPanel = nextPanel;
    }

    private void CloseAllPanels()
    {
        foreach (GameObject panel in allPanels)
        {
            panel.SetActive(false);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
