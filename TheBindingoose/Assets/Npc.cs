using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement; // Agrega esta directiva

public class Npc : MonoBehaviour
{
    [SerializeField] private GameObject dialogue;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private string[] dialog;
    private int index;
    [SerializeField] private GameObject contButton;
    public float wordSpeed;
    public bool playerIsClose = false;
    [SerializeField] private PlayerInput _playerInput;

    private void OnEnable()
    {
        _playerInput.actions["Interacts"].performed += ctx => Interact(ctx);
        _playerInput.actions["Interacts"].canceled += ctx => Interact(ctx);
    }

    void Interact(InputAction.CallbackContext ctx )
    {
        if (ctx.performed && playerIsClose == true)
        {
            if (dialogue.activeInHierarchy)
            {
                if (ctx.performed)
                {
                    dialogueText.text = "";

                    dialogue.SetActive(false);
                }
               
            }
            else
            {
                dialogue.SetActive(true);
                StartCoroutine(Typing());
            }
            Debug.Log("Interaccion");
        }
        if (dialogueText.text == dialog[index])
        {
            contButton.SetActive(true);
        }

       
    }

    
    public void ZeroText()
    {
        dialogueText.text = "";
        index = 0;
        dialogue.SetActive(false);
        SceneManager.LoadScene("Shop");

    }

    public void NextLine()
    {
        if (index < dialog.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            ZeroText();
        }
    }

    IEnumerator Typing()
    {
        foreach (char letter in dialog[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            ZeroText();
        }
    }
}