using UnityEngine;

public class SymbolInteraction : MonoBehaviour
{
    public GameObject relatedFlag;
    private AudioSource nationalAnthem;
    private bool isTriggered = false;

    void Start()
    {
        nationalAnthem = GetComponent<AudioSource>();

        // ÖNEMLİ: Sahne açılır açılmaz çalmasını engellemek için durdur
        if (nationalAnthem != null)
        {
            nationalAnthem.Stop();
        }

        if (relatedFlag != null) relatedFlag.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Sadece HandCursor çarptığında çalış ve sahne ilk açıldığında (ilk 1 saniye) çalışma
        if (Time.timeSinceLevelLoad > 1.0f && !isTriggered)
        {
            if (other.gameObject.name.Contains("Hand") || other.CompareTag("Player"))
            {
                ActivateSymbol();
            }
        }
    }

    void ActivateSymbol()
    {
        isTriggered = true;
        if (relatedFlag != null) relatedFlag.SetActive(true);
        if (nationalAnthem != null) nationalAnthem.Play();

        Debug.Log(gameObject.name + " başarıyla tetiklendi.");
    }
}
