using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class NPCWindowUI : MonoBehaviour
{
    [SerializeField] private NPC npc;
    private TextMeshProUGUI title;

    public void SHOW(NPC npc) { this.npc = npc;  StartCoroutine(Show()); }
    public void HIDE() { StartCoroutine(Hide()); this.npc = null; }

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

    private void Update()
    {
        if (!title) title = transform.Find("Header/Text (TMP)").GetComponent<TextMeshProUGUI>();

        if(npc)
        {

        }
    }

}
