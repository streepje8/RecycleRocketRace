[System.Serializable]
public struct Rocket
{
    public string name;
    public float achievedHeight;
    public RocketStats stats;
    public string guid;

    public Rocket(string name)
    {
        this.name = name;
        achievedHeight = 0;
        guid = System.Guid.NewGuid().ToString();
        stats = new RocketStats();
    }
}