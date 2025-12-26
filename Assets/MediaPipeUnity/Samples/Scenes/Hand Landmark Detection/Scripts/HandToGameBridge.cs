using UnityEngine;

public class HandToGameBridge : MonoBehaviour
{
    [Header("Sanal El Ayarý")]
    public Transform myHandCursor;
    public Camera mainCamera;

    [Header("Derinlik ve Hareket")]
    public float baseDistance = 10.0f; // Balonlarýn olduðu temel mesafe
    public float zSensitivity = 50.0f; // Derinlik hassasiyeti (Artýrýp/Azaltarak ayarla)

    // Takip edilecek nokta
    private Transform targetTransform;
    private float searchTimer = 0f;

    void Update()
    {
        // Hedef yoksa ara
        if (targetTransform == null || !targetTransform.gameObject.activeInHierarchy)
        {
            searchTimer -= Time.deltaTime;
            if (searchTimer <= 0)
            {
                FindIndexFingerSmart();
                searchTimer = 0.5f;
            }
            return;
        }

        if (targetTransform != null && mainCamera != null)
        {
            // 1. Ekrandaki X,Y pozisyonunu al (Piksel olarak)
            Vector3 screenPos = mainCamera.WorldToScreenPoint(targetTransform.position);

            // 2. Derinliði (Z) Hesapla
            // MediaPipe'in verdiði Z deðerini hassasiyetle çarpýp temel mesafeye ekliyoruz.
            // targetTransform.localPosition.z genelde çok küçüktür, bu yüzden büyük bir sayýyla (zSensitivity) çarpmak gerekir.
            float depthChange = targetTransform.localPosition.z * zSensitivity;
            float currentDepth = baseDistance + depthChange;

            // 3. Hesaplanan bu derinliði kullanarak Dünyaya Iþýnla
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, currentDepth));

            // 4. Pozisyonu uygula
            myHandCursor.position = worldPos;
        }
    }

    void FindIndexFingerSmart()
    {
        if (mainCamera == null) mainCamera = Camera.main;

        GameObject mainAnnotation = GameObject.Find("Multi HandLandmarkList Annotation");
        if (mainAnnotation != null)
        {
            Transform pointContainer = FindContainerWith21Children(mainAnnotation.transform);
            if (pointContainer != null)
            {
                targetTransform = pointContainer.GetChild(8); // Ýþaret Parmaðý
            }
        }
    }

    Transform FindContainerWith21Children(Transform parent)
    {
        if (parent.childCount == 21) return parent;
        foreach (Transform child in parent)
        {
            Transform result = FindContainerWith21Children(child);
            if (result != null) return result;
        }
        return null;
    }
}