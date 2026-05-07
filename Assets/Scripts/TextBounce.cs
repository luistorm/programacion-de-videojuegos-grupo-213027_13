using UnityEngine;
using TMPro;

public class TextBounce : MonoBehaviour
{
    public float amplitude = 10f;   // Qué tanto sube y baja
    public float frequency = 2f;    // Qué tan rápido rebota

    private TMP_Text textMesh;
    private Vector3[] originalVertices;

    void Start()
    {
        textMesh = GetComponent<TMP_Text>();
    }

    void Update()
    {
        textMesh.ForceMeshUpdate();

        var textInfo = textMesh.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible)
                continue;

            int vertexIndex = textInfo.characterInfo[i].vertexIndex;
            int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

            Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;

            // Offset con onda
            float offsetY = Mathf.Sin(Time.time * frequency + i * 0.5f) * amplitude;

            Vector3 offset = new Vector3(0, offsetY, 0);

            vertices[vertexIndex + 0] += offset;
            vertices[vertexIndex + 1] += offset;
            vertices[vertexIndex + 2] += offset;
            vertices[vertexIndex + 3] += offset;
        }

        // Aplicar cambios
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            textMesh.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }
}