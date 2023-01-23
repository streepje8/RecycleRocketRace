using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardDisplay : MonoBehaviour
{
    public Rocket myRocket;
    public float minY;
    public float maxY;
    public AnimationCurve size;

    [Header("Thingies to modify")]
    public TMP_Text rocketName;
    public TMP_Text rocketHeight;
    public TMP_Text rankText;
    public RawImage icon;
    public float startY = 0;
    public float maxScrollY = 0;
    
    public void SetRocket(int rank,Rocket r)
    {
        myRocket = r;
        rocketName.text = r.name;
        rocketHeight.text = r.achievedHeight + "km flown";
        rankText.text = rank + ".";
        if (File.Exists(Application.persistentDataPath + "/rockets/" + r.guid + ".png"))
        {
            byte[] image = File.ReadAllBytes(Application.persistentDataPath + "/rockets/" + r.guid + ".png");
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(image);
            icon.texture = tex;
        }
    }

    public float scrollSpeed = 100f;
    private float y = 0;
    private float smoother = 0;
    
    void Update()
    {
        float yval = Mathf.Clamp(transform.localPosition.y, minY, maxY);
        float scale = size.Evaluate((yval - minY) / (float)(maxY - minY));
        y -= Input.mouseScrollDelta.y * scrollSpeed;
        y = Mathf.Clamp(y, 0, maxScrollY);
        smoother = Mathf.Lerp(smoother, y, 2f * Time.deltaTime);
        transform.localPosition = new Vector3(transform.position.x, startY + smoother, transform.position.z);
        transform.localScale = new Vector3(scale, scale, scale);
    }
}