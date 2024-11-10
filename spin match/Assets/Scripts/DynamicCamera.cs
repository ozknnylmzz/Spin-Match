using UnityEngine;

public class DynamicCamera : MonoBehaviour
{
    [SerializeField] private float targetAspectRatio = 1125f / 2436f; // Hedef en-boy oranı (1125x2436)
    [SerializeField] private float orthographicSizeAtTargetAspect = 10f; // Hedef oran için ortografik boyut
    [SerializeField] private bool maintainViewport = true; // Gerektiğinde Viewport'u ayarlamak için

    private void Start()
    {
        AdjustCamera();
    }

    private void AdjustCamera()
    {
        float screenAspectRatio = (float)Screen.width / Screen.height;

        if (Mathf.Approximately(screenAspectRatio, targetAspectRatio))
        {
            // Hedef orana yakınsa, doğrudan hedef boyutları kullan
            Camera.main.orthographicSize = orthographicSizeAtTargetAspect;
            Camera.main.rect = new Rect(0, 0, 1, 1); // Tüm ekranı kullan
        }
        else
        {
            if (screenAspectRatio > targetAspectRatio)
            {
                // Geniş ekranlarda: dikey sınırlar eklenecek
                float inset = 1.0f - targetAspectRatio / screenAspectRatio;
                Camera.main.orthographicSize = orthographicSizeAtTargetAspect;
                if (maintainViewport)
                {
                    Camera.main.rect = new Rect(inset / 2, 0, 1 - inset, 1);
                }
            }
            else
            {
                // Dar ekranlarda: yatay sınırlar eklenecek
                float inset = 1.0f - screenAspectRatio / targetAspectRatio;
                Camera.main.orthographicSize = orthographicSizeAtTargetAspect;
                if (maintainViewport)
                {
                    Camera.main.rect = new Rect(0, inset / 2, 1, 1 - inset);
                }
            }
        }
    }
}