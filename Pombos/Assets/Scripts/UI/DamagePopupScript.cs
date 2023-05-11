using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopupScript : MonoBehaviour
{
    [SerializeField] float moveYSpeed = 20f;
    [SerializeField] float disappearTimer = 1f;
    [SerializeField] float disappearSpeed = 3f;
    [SerializeField] Color normalHitColor;
    [SerializeField] Color criticalHitColor;
    [SerializeField] float criticalHitfontsize = 1f;
    [SerializeField] float normalHitfontsize = 0.7f;

    [SerializeField] TextMeshProUGUI textMesh;
    public static DamagePopupScript Create(Vector3 position, int damageAmount, bool isCritical)
    {
        Transform damagePopupTransform = Instantiate(GameAssets.instance.damagePopup, position, Quaternion.identity, GameManager.instance.popupCanvasTransform).transform;

        DamagePopupScript damagePopup = damagePopupTransform.GetComponent<DamagePopupScript>();
        damagePopup.Setup(damageAmount, isCritical);

        return damagePopup;
    }

    private void Setup(int damageAmount, bool isCritical)
    {
        textMesh.SetText(damageAmount.ToString());
        if (isCritical)
        {
            textMesh.color = criticalHitColor;
            textMesh.fontSize = criticalHitfontsize;
        }
        else
        {
            textMesh.color = normalHitColor;
            textMesh.fontSize = normalHitfontsize;
        }
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
