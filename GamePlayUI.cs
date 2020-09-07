using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GamePlayUI : MonoBehaviour
{
    public static GamePlayUI instance;
    public GameObject answers;
    public GameObject resetButton;
    public GameObject puzzlePanel;
    public GameObject endGamePanel;
    public TextMeshProUGUI endGameText;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        AudioController.instance.PlayAudio(AudioType.ST2);
    }
    public void RightAnswer()
    {
        AudioController.instance.PlayAudio(AudioType.SFX1);
        Character.mainCharacter.SetTransition(Transition.Jump, Character.mainCharacter.GetComponent<Animator>());
        foreach (Transform slider in puzzlePanel.transform)
        {
            if (slider.GetComponent<Slider>().value == 0)
            {
                slider.GetComponent<Slider>().value = 1;
                return;
            }
        }
    }
    public void ShowAnswers(bool enabled)
    {
        Character.mainCharacter.DisplayDialogue();
        answers.SetActive(enabled);
    }
    public void ResetLevel()
    {
        foreach (Transform slider in puzzlePanel.transform)
        {
            slider.GetComponent<Slider>().value = 0;
        }
        Character.mainCharacter.Reset();
        answers.SetActive(false);
    }
    public void SetEndGameText(string text)
    {
        endGameText.text = text;
    }
    public void SetEndGamePanel(bool enabled)
    {
        endGamePanel.SetActive(enabled);
    }
    public void PlayEndGameSFX()
    {
        AudioController.instance.PlayAudio(AudioType.SFX4);
    }
    public void UIClickSFX()
    {
        AudioController.instance.PlayAudio(AudioType.SFX3);
    }
}