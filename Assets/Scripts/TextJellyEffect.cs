using UnityEngine;
using TMPro;

public class TextJellyEffect : MonoBehaviour
{
    public float amplitude = 0.2f;   
    public float frequency = 2f;     
    public float phaseOffset = 0.5f; 

    private TMP_Text textMesh;

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

            // Centro de la letra
            Vector3 charMid = (vertices[vertexIndex + 0] + vertices[vertexIndex + 2]) / 2;

            float wave = Mathf.Sin(Time.time * frequency + i * phaseOffset);

            float scaleY = 1 + wave * amplitude;
            float scaleX = 1 - wave * amplitude; 
            Matrix4x4 matrix = Matrix4x4.TRS(
                Vector3.zero,
                Quaternion.identity,
                new Vector3(scaleX, scaleY, 1)
            );

            for (int j = 0; j < 4; j++)
            {
                Vector3 offset = vertices[vertexIndex + j] - charMid;
                vertices[vertexIndex + j] = charMid + matrix.MultiplyPoint3x4(offset);
            }
        }

        // Aplicar cambios
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            textMesh.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }
}