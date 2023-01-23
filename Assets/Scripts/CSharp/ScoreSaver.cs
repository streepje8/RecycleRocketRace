using System;
using System.IO;
using UnityEngine;

public class ScoreSaver : MonoBehaviour
{
    public Stream textureStream;
    public Stream statsStream;
    public Stream scoreStream;
    public Stream nameStream;
    public Stream output;
    private Rocket r;

    private void Awake()
    {
        r = new Rocket();
        r.guid = Guid.NewGuid().ToString();
        nameStream.AddOutputStream<string>(name => r.name = name);
        scoreStream.AddOutputStream<float>(score =>
        {
            r.achievedHeight = score;
            GameController.Instance.rocketbase.rockets.Add(r);
            GameController.Instance.rocketbase.SaveRockets();
            output.StreamData(r);
        });
        statsStream.AddOutputStream<RocketStats>(stats => r.stats = stats);
        textureStream.AddOutputStream<Texture2D>(image =>
        {
            string rocketsFolder = Application.persistentDataPath + "/rockets";
            if (!Directory.Exists(rocketsFolder))
            {
                Directory.CreateDirectory(rocketsFolder);
            }

            string filename = rocketsFolder + "/" + r.guid + ".png";
            File.WriteAllBytes(filename, image.EncodeToPNG());
        });
    }
}