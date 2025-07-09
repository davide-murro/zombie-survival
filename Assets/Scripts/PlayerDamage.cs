using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField] Canvas bloodSplattersCanvas;
    [SerializeField] List<GameObject> bloodSplatterImages;
    [SerializeField] float effectDuration = 0.3f;

    void Awake()
    {
        DisableImages();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DisableImages()
    {
        foreach (GameObject image in bloodSplatterImages)
        {
            image.SetActive(false);
        }
    }

    public void ShowDamageImpact()
    {
        StartCoroutine(ShowBloodSplatters());
    }

    IEnumerator ShowBloodSplatters()
    {
        int randomIndex = Random.Range(0, bloodSplatterImages.Count);
        GameObject selectedImage = bloodSplatterImages[randomIndex];

        selectedImage.SetActive(true);
        yield return new WaitForSeconds(effectDuration);
        selectedImage.SetActive(false);
    }
}
