using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RocketSpawner : MonoBehaviour
{
    public float minTimeBetweenRockets = 10;
    public float maxTimeBetweenRockets = 20;
    public GameObject rocketPrefab;
    private List<Texture2D> rockets = new List<Texture2D>();

    void Start()
    {
        List<Rocket> toload = new List<Rocket>();
        for (int i = 0; i < Mathf.Min(GameController.Instance.rocketbase.rockets.Count,10); i++)
        {
            toload.Add(GameController.Instance.rocketbase.rockets[
                Random.Range(0, GameController.Instance.rocketbase.rockets.Count)]);
        }

        if (toload.Count > 0)
        {
            foreach (var rocket in toload)
            {
                if (File.Exists(Application.persistentDataPath + "/rockets/" + rocket.guid + ".png"))
                {
                    byte[] image =
                        File.ReadAllBytes(Application.persistentDataPath + "/rockets/" + rocket.guid + ".png");
                    Texture2D tex = new Texture2D(2, 2);
                    tex.LoadImage(image);
                    rockets.Add(tex);
                }
                else
                {
                    Debug.LogError("Tried to load an image for rocket \"" + rocket.name +
                                   "\" but the file with the name " + rocket.guid + ".png does not exist!");
                }
            }
        }
    }

    private float timer = 0f;

    void Update()
    {
        if (rockets.Count > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                MenuRocket mr = Instantiate(rocketPrefab, new Vector3(50, 50, 50), Quaternion.identity)
                    .GetComponent<MenuRocket>();
                mr.SetTexture(rockets[Random.Range(0, rockets.Count)]);
                timer = Random.Range(minTimeBetweenRockets, maxTimeBetweenRockets);
            }
        }
    }
}