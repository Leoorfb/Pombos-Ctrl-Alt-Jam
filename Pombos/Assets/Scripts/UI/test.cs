using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class test : MonoBehaviour
{
    [SerializeField] float moveYSpeed = 20f;
    [SerializeField] float disappearTimer = 1f;
    [SerializeField] float disappearSpeed = 3f;
    [SerializeField] Color normalHitColor;
    [SerializeField] Color criticalHitColor;

    private TextMeshPro textMesh;

    public static test Create(Vector3 position, int damageAmount)
    {
        Transform damagePopupTransform = Instantiate(GameAssets.instance.damagePopup, position, Quaternion.identity).transform;

        test damagePopup = damagePopupTransform.GetComponent<test>();
        damagePopup.Setup(damagePopup);

        return damagePopup;
    }

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        textMesh.color = normalHitColor;
    }

    private void Setup(test damagePopup)
    {
        textMesh.SetText(damagePopup.ToString());
    }

    private void Update()
    {
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            textMesh.alpha -= disappearSpeed * Time.deltaTime;

            if (textMesh.alpha < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
