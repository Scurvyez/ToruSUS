using UnityEngine;

public class TorusMPBController : MonoBehaviour
{
    public GameObject[] torusObjects;

    // Start is called before the first frame update
    void Start()
    {
        SetRandomScrollSpeeds();
    }

    public void SetRandomScrollSpeeds()
    {
        MaterialPropertyBlock mPB = new MaterialPropertyBlock();

        foreach (var torusObject in torusObjects)
        {
            float newRandScrollSpeed = Random.Range(0.0f, 0.05f);
            float newRandSparkleIntensity = Random.Range(0.0f, 10.0f);
            float newRandPulseSpeed = Random.Range(0.0f, 2.0f);
            Color newRandBaseColor = new (Random.value, Random.value, Random.value);
            mPB.SetFloat("_ScrollSpeed", newRandScrollSpeed);
            mPB.SetFloat("_SparkleIntensity", newRandSparkleIntensity);
            mPB.SetFloat("_PulseSpeed", newRandPulseSpeed);
            mPB.SetColor("_Color", newRandBaseColor);
            MeshRenderer renderer = torusObject.GetComponent<MeshRenderer>();
            renderer.SetPropertyBlock(mPB);
        }
    }
}
