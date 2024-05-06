using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSetup : MonoBehaviour
{
    public GameObject quadPrefab;

    void Start()
    {
        gameObject.transform.position = new Vector3(0, 0, -80);
        // Cr�er le sol de la bo�te
        CreateQuad(new Vector2(0, -40), 80, 1);
        // Cr�er le plafond de la bo�te
        CreateQuad(new Vector2(0, 40.0f), 80, 1);

        // Murs de la bo�te
        CreateQuad(new Vector2(-40.5f, 0), 1, 80); // Mur gauche
        CreateQuad(new Vector2(40.5f, 0), 1, 80);  // Mur droit

        // Plateforme inclin�e
        GameObject platform = CreateQuad(new Vector2(0, -5), 40, 0.5f);
        platform.transform.rotation = Quaternion.Euler(0, 0, 45 - (45 / 2)); // Rotation de 45 degr�s
    }

    GameObject CreateQuad(Vector2 position, float width, float height)
    {
        GameObject quad = Instantiate(quadPrefab, position, Quaternion.identity);
        quad.transform.localScale = new Vector3(width, height, 1);
        quad.AddComponent<BoxCollider2D>();
        return quad;
    }
}
