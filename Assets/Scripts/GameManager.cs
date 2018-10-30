using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Riddle
{
    public string password;
    [TextArea]
    public string riddle;
}

public class GameManager : MonoBehaviour {
    public List<Riddle> riddles = new List<Riddle>();
    public int passwordLengthLimit = 6;
    public Text passwordText;
    public Button[] touches;
    public GameObject popup;
    public Text riddleText;
    public GameObject screamer;

    private string currentPassword = "";
    private AudioSource audioSource;

    public void WriteNumber(char str)
    {
        if (currentPassword.Length < passwordLengthLimit)
        {
            currentPassword += str;
            passwordText.text = currentPassword;
        }
    }

    public void Validate()
    {
        Riddle riddle = riddles.Find((Riddle tmp) => tmp.password == currentPassword);
        if (riddle != null)
        {
            riddleText.text = riddle.riddle;
            popup.SetActive(true);
        }
        else
        {
            screamer.SetActive(true);
            audioSource.Play();
            StartCoroutine(RemoveScreamer());
        }
        currentPassword = "";
        passwordText.text = currentPassword;
    }

    public void Cancel()
    {
        if (currentPassword.Length > 0)
        {
            currentPassword = currentPassword.Remove(currentPassword.Length - 1, 1);
            passwordText.text = currentPassword;
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        for (int i = 0; i < touches.Length; i++)
        {
            char letter = touches[i].name[touches[i].name.Length - 1];
            if (letter >= '0' && letter <= '9')
            {
                touches[i].onClick.AddListener(() => WriteNumber(letter));
            } 
            else if (letter == 'A')
            {
                touches[i].onClick.AddListener(() => Cancel());
            }
            else if (letter == 'V')
            {
                touches[i].onClick.AddListener(() => Validate());
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            popup.SetActive(false);
        }
    }

    private IEnumerator RemoveScreamer()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        screamer.SetActive(false);
    }
}
