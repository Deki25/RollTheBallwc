﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelMenuMenager : MonoBehaviour
{
    public GameObject LevelPanel;
    public GameObject LevelButton;

    public Sprite[] levelThumbnail;

    private void Awake()
    {
        levelThumbnail = Resources.LoadAll<Sprite>("Levels");
        int level = 1;
        foreach (Sprite thumbnail in levelThumbnail)
        {
            GameObject container = Instantiate(LevelButton, LevelPanel.transform) as GameObject;
            container.GetComponent<Image>().sprite = thumbnail;

            int currentLevel = level;
            container.GetComponentInChildren<Text>().text = "Level " + currentLevel;
            string levelName = thumbnail.name;
            container.GetComponent<Button>().onClick.AddListener(() => LoadLevel(levelName));
            level++;
        }
    }

    private void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void Back(string goTo)
    {
        SceneManager.LoadScene(goTo);
    }
}


//public Button[] levelsButton;


//private void Awake()
//{
//    PlayerPrefs.DeleteAll();
//    int levelReached = PlayerPrefs.GetInt("levelReached", 1);
//    for (int i = 0; i < levelsButton.Length; i++)
//    {
//        if (i + 1 > levelReached)
//            levelsButton[i].interactable = false;
//    }
//}

//public void SelectLevel(string levelName)
//{
//    SceneManager.LoadScene(levelName);
//}