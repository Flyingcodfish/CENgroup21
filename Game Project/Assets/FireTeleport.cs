using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FireTeleport : MonoBehaviour
{

    public GameObject teleportAnim;
    public GameObject teleportSound;
    public GameObject warning;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!GameSaver.liveSave.firetutorialpoint)
            {
                warning.SetActive(true);
            }
            else
            {
                teleportSound.SetActive(true);
                teleportAnim.SetActive(true);
                StartCoroutine(LoadAfterAnim());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!GameSaver.liveSave.firetutorialpoint)
            {
                warning.SetActive(false);
            }
        }
    }

    public IEnumerator LoadAfterAnim()
    {
        yield return new WaitForSeconds(0.6f);
        float fadeTime = GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene("fire_dungeon");
    }
}


