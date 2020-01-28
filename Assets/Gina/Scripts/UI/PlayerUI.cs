using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Image player_icon;
    [SerializeField] private TextMeshProUGUI nameLable;
    [SerializeField] private TextMeshProUGUI levelLable; 
    [SerializeField] private Progresbar healthbar;
    [SerializeField] private Progresbar staminabar;
    [SerializeField] private Progresbar mamabar;
    private void Reset()
    {
        if (!player_icon) player_icon = transform.Find("cirlce-mask/image").GetComponent<Image>();
        if (!nameLable) nameLable = transform.Find("name-bar").GetComponentInChildren<TextMeshProUGUI>();
        if (!levelLable) levelLable = transform.Find("level-bar").GetComponentInChildren<TextMeshProUGUI>();
        if (!healthbar) healthbar = transform.Find("health-bar").GetComponent<Progresbar>();
        if (!staminabar) staminabar = transform.Find("stamina-bar").GetComponent<Progresbar>();
        if (!mamabar) mamabar = transform.Find("mana-bar").GetComponent<Progresbar>();
    }

    private void Awake()
    {
        Reset();
        StartCoroutine(Connect());
    }

    private IEnumerator Connect()
    {
        Player player = null;
        Debug.Log("Waiting for player");
        yield return new WaitUntil(() =>
        {
            player = FindObjectOfType<Player>();
            return player;
        });
        Debug.Log("Player Found!");

        player_icon.sprite = null;
        player_icon.enabled = false;

        nameLable.text = player.data.player.Name;
        levelLable.text = $"Lv{player.data.player.Level}";
        healthbar.Set = player.data.player.Health;
        staminabar.Set = player.data.player.Stamina;
        mamabar.Set = player.data.player.Mana;

        player.data.player.onNameChaged += (n) => nameLable.text = n;
        player.data.player.onLevelChaged += (l) => levelLable.text = $"Lv{l}";
        player.data.player.onHealthChagned += (h) => healthbar.Set = h;
        player.data.player.onStaminaChagned += (s) => staminabar.Set = s;
        player.data.player.onManaChagned += (m) => mamabar.Set = m;
    }
}
