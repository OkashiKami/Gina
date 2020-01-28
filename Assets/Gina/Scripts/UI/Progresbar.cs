using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Progresbar : MonoBehaviour
{
    public Image fill;
    public TextMeshProUGUI lable;

    [Range(0, 1)] public float value = 1f;
    public AnimationCurve alpha = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.0001f, 1), new Keyframe(1, 1));
    public bool showLabel;

    public Gradient color = new Gradient()
    {
        alphaKeys = new GradientAlphaKey[]
        {
            new GradientAlphaKey(1, 0f),
            new GradientAlphaKey(1, 1f)
        },
        colorKeys = new GradientColorKey[]
        {
            new GradientColorKey(Color.red, 0),
            new GradientColorKey(new Color(1f, .5f, 0f, 1f), .25f),
            new GradientColorKey(new Color(1f, 1f, 0f), .75f),
            new GradientColorKey(Color.green, 1)
        }
    };

    public float[] Set
    {
        set
        {
            this.value = (value[0] / value[1]);
        }
    }


    void Update()
    {
        if (!fill) fill = transform.Find("fill").GetComponent<Image>();
        if (!lable) lable = transform.Find("lable").GetComponent<TextMeshProUGUI>();


        var sd = GetComponent<RectTransform>().sizeDelta;
        fill.rectTransform.sizeDelta = new Vector2(23 + (value * (sd.x - 23)), fill.rectTransform.sizeDelta.y);

        var color = this.color.Evaluate(value);
        fill.color = new Color(color.r, color.g, color.b, alpha.Evaluate(value));

        if (showLabel)
            lable.text = ((value / 1) * 100).ToString("n0") + "%";
        else
            lable.text = string.Empty;
    }
}
