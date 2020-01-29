using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CharacterUI : MonoBehaviour
{
    public CharacterSlot[] slots;

    private void Reset()
    {
        slots = GetComponentsInChildren<CharacterSlot>();
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].name = $"Char Slot {i + 1} + [{slots[i].requireType}]";
            slots[i].Set();
            slots[i].Awake();
            slots[i].Update();
        }
    }

    private void Awake()
    {
        if (slots == null || slots.Length <= 0) Reset();
        StartCoroutine(Connect());
        FindObjectOfType<InputController>().onCharacter += () =>
        {
            var cg = GetComponent<CanvasGroup>();
            if (cg.alpha >= 1)
                StartCoroutine(Hide());
            if (cg.alpha <= 0)
                StartCoroutine(Show());

        };
    }

    private IEnumerator Connect()
    {
        PlayerInfo player = null;
        Debug.Log("Waiting for player");
        yield return new WaitUntil(() =>
        {
            player = FindObjectOfType<PlayerInfo>();
            return player;
        });
        Debug.Log("Player Found!");
        OnChanged(player.data.character);
        player.data.onCharacterChanged += OnChanged;
    }

    private IEnumerator Show()
    {
        var cg = GetComponent<CanvasGroup>();
    a:
        cg.alpha += 3f * Time.deltaTime;
        yield return new WaitForSeconds(0f);
        if (cg.alpha < 1)
            goto a;
        cg.blocksRaycasts = true;
    }
    private IEnumerator Hide()
    {
        var cg = GetComponent<CanvasGroup>();
    a:
        cg.alpha -= 3f * Time.deltaTime;
        yield return new WaitForSeconds(0f);
        if (cg.alpha > 0)
            goto a;
        cg.blocksRaycasts = false;
    }

    private void OnChanged(ItemData[] values)
    {
        for (int i = 0; i < values.Length; i++)
        {
            var slot = slots[i];
            var value = values[i];

            if (value != null && value.IsValid)
                slot.item = value;
            else
                slot.item = null;
        }
    }
}
