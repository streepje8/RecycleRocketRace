using System.Linq;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    public Vector3 startPos = new Vector3(0, 280, 0);
    public GameObject prefab;
    public Transform canvas;

    private void Start()
    {
        Vector3 curpos = startPos;
        int i = 1;
        foreach (var rocket in GameController.Instance.rocketbase.rockets.OrderByDescending(x => x.achievedHeight))
        {
            LeaderboardDisplay disp = Instantiate(prefab,canvas).GetComponent<LeaderboardDisplay>();
            disp.SetRocket(i,rocket);
            disp.transform.localPosition = curpos;
            disp.startY = curpos.y;
            disp.maxScrollY = Mathf.Max(200 * GameController.Instance.rocketbase.rockets.Count - 180 - 3*200,0);
            i++;
            curpos.y -= 200;
        }
    }
}