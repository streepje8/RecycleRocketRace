using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public class RocketDatabase : MonoBehaviour
{
    public List<Rocket> rockets = new List<Rocket>();
    public List<HeightLandMark> landMarks = new List<HeightLandMark>();
    private string filepath;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        filepath = Application.persistentDataPath + "/rocketdata.json";
        TryLoadRockets();
    }

    public void SaveRockets()
    {
        if (rockets != null && rockets.Count > 0)
        {
            File.WriteAllText(filepath, JsonConvert.SerializeObject(rockets, Formatting.Indented));
            Debug.Log("Saved rockets to: " + filepath);
        }
    }

    public void TryLoadRockets()
    {
        if (File.Exists(filepath))
        {
            string json = File.ReadAllText(filepath);
            rockets = JsonConvert.DeserializeObject<List<Rocket>>(json);
            if (rockets == null || rockets.Count < 1) rockets = new List<Rocket>();
        }
    }

    private void OnApplicationQuit()
    {
        SaveRockets();
    }

    [ContextMenu("Sort Landmarks by height")]
    public void SortLandMarks()
    {
        landMarks = landMarks.OrderBy(x => x.height).ToList();
    }

    // Wheatley 383300km
    // 9km from sun> Timber Hearth
    //
    //     (Satellites / Other stuff sent into space) (Honestly some of these are estimates/averages based on data/orbit)
    // Golden Disc 11000000000 km
    //     Luna I 149597871 km
    //     Explorer II 2000 km
    //     Explorer I 1454 km
    //     Sputnik III 1040 km
    //     Sputnik II (Laika) 982 km
    //     Vanguard I 654 km
    //     Sputnik I 577 km
    //     Tyazhely Sputnik VII 250 km
    //     Sputnik VI 215 km
    //
    //
    //     (Planets from earth)
    // Moon 384400 km
    //     Venus 41400000 km 
    //     Mars 78340000 km
    //     Mercury 91691000 km
    //     Sun 149597871 km
    //     Jupiter 628730000 km
    //     Saturn 1275000000 km
    //     Uranus 2723950000 km
    //     Neptune 4351400000 km
    //
    //     (Buildings from ground)
    // Burj Khalifa 829.8m
    // Tokyo Skytree 634m
    // CN Tower 553.33m
    // Willis Tower 442m
    // Empire State Building 381m
    // TV Tower Vinnytsia 354m
    // Eiffel Tower 312m
    // Star Tower 291m
    // H1 Tower 273.8m
    // Haliade-X Prototype 270m
    // Djamaa el Djazaïr 265m
    // Cat Hai – Phu Long cable car towers 214.8m
    // Gateway Arch 192m
    // Kuwait Towers 187m
    // Olympic Stadium 175m
    // San Jacinto Monument 173.7m
    // Washington Monument 169.29m
    // High Roller 167.6m
    // Ulmer Münster 162m
    // Vehicle Assembly Building 160m
    // Santa Cruz del Valle de los Caídos 152.4m
    // Great Pyramid of Giza 138.8m
    // Jetavanaramaya 122m
    // Joseph Chamberlain Memorial Clock Tower 100m
    // Avicii Arena 85m
    // Pyramid of Djoser 62.5m
    // Tower of Jericho 8.5m
    //
    //
    // (From sea level)
    // Mount Everest 8850m
    // Khumbu 8000m
    // Llullaillaco volcano 6739m
    // University of Tokyo Atacama Observatory 5640m
    // Daocheng Yading Airport 4411m
    // Yak golf course 3970m
    // La Paz 3650m
    // Quito 2850m
    // Whitehorse 640m
}