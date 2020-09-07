using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class DialogueSystem : MonoBehaviour
{

    public static DialogueSystem instance;
    public Sprite sprite1;
    [SerializeField]
    private SpriteRenderer image;
    [SerializeField]
    private TextMeshPro line;
    [SerializeField]
    private Transform box;

    private WaitForSecondsRealtime delay = new WaitForSecondsRealtime(2);
    private bool isDisplaying;
    private WaitUntil displayProcess;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        displayProcess = new WaitUntil(() => !isDisplaying);
        line.GetComponent<MeshRenderer>().sortingLayerName = "Front";
        line.GetComponent<MeshRenderer>().sortingOrder = 1;
    }
    public IEnumerator DisplayMultiDialogue(List<IEnumerator> dialogues)
    {
        foreach (var dialogue in dialogues)
        {

            yield return dialogue;
        }
    }

    public IEnumerator DisplayDialogue(Vector3 position, Transform parent, string text = "", float time = 2)
    {
        yield return displayProcess;
        isDisplaying = true;

        box.parent = parent;
        box.position = position;
        box.gameObject.SetActive(true);

        if (text != "")
        {
            StartCoroutine(DisplayText(text, time));
        }
    }

    public IEnumerator DisplayDialogue(Vector3 position, Transform parent, Sprite sprite = null, float time = 2)
    {
        yield return displayProcess;
        isDisplaying = true;

        box.parent = parent;
        box.position = position;
        box.gameObject.SetActive(true);

        if (sprite != null)
        {
            StartCoroutine(DisplayImage(sprite, time));
        }
    }

    private IEnumerator DisplayImage(Sprite sprite, float delayTime)
    {

        image.enabled = true;
        line.enabled = false;

        image.sprite = sprite;

        delay.waitTime = delayTime;
        yield return delay;

        box.gameObject.SetActive(false);
        isDisplaying = false;
    }

    private IEnumerator DisplayText(string text, float delayTime)
    {
        image.enabled = false;
        line.enabled = true;

        line.text = text;

        delay.waitTime = delayTime;
        yield return delay;

        box.gameObject.SetActive(false);
        isDisplaying = false;
    }

}