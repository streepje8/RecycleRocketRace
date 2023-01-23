using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public struct KompasEntry
{
    public float height;
    public bool isLandmark;
    public HeightLandMark landmark;
    public Rocket rocket;
}

public class Kompas : MonoBehaviour
{
    public Stream heightStream;
    public Stream nameStream;
    public TMP_Text text;
    public TMP_Text heightText;
    public TMP_Text nextLandmarkText;
    public float currentHeight = 0f;
    public AnimationCurve sizeCurve;
    public AnimationCurve alphaCurve;
    public Transform movingTextParent;
    public GameObject textPrefab;
    public float positionalMinMax = 1000;

    private string rname = "UNSET";
    private List<KompasEntry> entries = new List<KompasEntry>();
    private List<KompasText> activeTexts = new List<KompasText>();
    private List<KompasText> inActiveTexts = new List<KompasText>();
    private int currentIndex = 0;

    private void Awake()
    {
        nameStream.AddOutputStream<string>(rocketname => rname = rocketname);
    }

    private void Start()
    {
        GameController.Instance.rocketbase.rockets.ForEach(r => entries.Add(new KompasEntry()
        {
            isLandmark = false,
            rocket = r,
            height = r.achievedHeight * 100f
        }));
        GameController.Instance.rocketbase.landMarks.ForEach(lm => entries.Add(new KompasEntry()
        {
            isLandmark = true,
            landmark = lm,
            height = lm.height
        }));
        entries = entries.OrderBy(x => x.height).ToList();
        heightStream.AddOutputStream<float>(UpdateHeight);
    }

    private void UpdateHeight(float currentHeightIn)
    {
        text.text = rname;
        heightText.text = "" + Mathf.Round(currentHeightIn / 100f) / 10f + "km";
        currentHeight = currentHeightIn;
        float currentHeightCM = currentHeight * 100;
        float visionRangeCM = currentHeightCM * 100;
        if (entries.Count > currentIndex)
        {
            if (currentHeightCM + visionRangeCM > entries[currentIndex].height)
            {
                KompasText toUse;
                if (inActiveTexts.Count < 1)
                {
                    toUse = Instantiate(textPrefab, movingTextParent).GetComponent<KompasText>();
                }
                else
                {
                    toUse = inActiveTexts[0];
                    toUse.gameObject.SetActive(true);
                    inActiveTexts.Remove(toUse);
                }

                KompasEntry entry = entries[currentIndex];
                toUse.height = entry.height;
                if (entry.isLandmark)
                    toUse.text = "[" + (Mathf.Round(toUse.height / 100f / 100f) / 10f) + "km] " + entry.landmark.name;
                else toUse.text = "[" + (Mathf.Round(toUse.height / 100f / 100f) / 10f) + "km] " + entry.rocket.name;
                activeTexts.Add(toUse);
                currentIndex++;
            }
        }
        else
        {
            nextLandmarkText.text = "You are the highest right now 0-0!";
        }

        if (activeTexts.Count > 0 && activeTexts[0].height < currentHeightCM - visionRangeCM)
        {
            KompasText toInactivate = activeTexts[0];
            activeTexts.Remove(toInactivate);
            toInactivate.gameObject.SetActive(false);
            inActiveTexts.Add(toInactivate);
        }

        KompasText nextmark = null;
        float playerdst = Mathf.Infinity;
        activeTexts.ForEach(x =>
        {
            float zeroOneValue = Mathf.Clamp01(currentHeightCM / (x.height * 2f));
            x.transform.localScale = Vector3.one * sizeCurve.Evaluate(zeroOneValue);
            x.tmpText.color = new Color(1, 1, 1, alphaCurve.Evaluate(zeroOneValue));
            x.transform.localPosition = new Vector3(0, positionalMinMax, 0) -
                                        new Vector3(0, positionalMinMax * 2f * zeroOneValue - positionalMinMax, 0) -
                                        new Vector3(0, positionalMinMax, 0);
            if ((x.height / 100f) > currentHeightIn)
            {
                float pdst = (x.height / 100f) - currentHeightIn;
                if (pdst < playerdst)
                {
                    nextmark = x;
                    playerdst = pdst;
                }
            }
        });
        nextLandmarkText.text = nextmark?.text ??
                                (entries.Count > currentIndex ? "Getting ready for liftoff!" : "You are the highest!");
    }
}