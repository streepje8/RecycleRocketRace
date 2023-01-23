using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct RocketColors
{
    private Dictionary<Color, float> percentages;

    public RocketColors(Dictionary<Color, float> percentages)
    {
        this.percentages = percentages;
    }

    public List<Color> GetScannedColors() => percentages.Keys.ToList();
    public float GetColorPercent(Color c) => percentages[c];
}

[Serializable]
public struct RocketStats
{
    public int height;
    public float weight;
    public RocketColors rocketColors;
}

public class StatCalculator : MonoBehaviour
{
    public Stream imageStream;
    public Stream statsStream;
    public int hitsRequiredBeforeALineIsClassifiedAsARocket = 0;

    void Awake()
    {
        imageStream.AddOutputStream<Texture2D>(ProcessStats);
    }

    public void ProcessStats(Texture2D tex)
    {
        RocketStats stats = new RocketStats();
        Color[] pixels = tex.GetPixels(0);
        stats.height = GetHeightSlow(pixels, tex.width, tex.height);
        stats.weight = GetWeightSlow(pixels, tex.width, tex.height);
        stats.rocketColors = GetRocketColors(pixels, tex.width, tex.height);
        statsStream.StreamData(stats);
    }

    private RocketColors GetRocketColors(Color[] pixels, int texWidth, int texHeight)
    {
        List<Color> colorsToScanFor = new List<Color>()
            { Color.red, Color.blue, Color.green, Color.cyan, Color.magenta, Color.yellow };
        Dictionary<Color, float> percentages = new Dictionary<Color, float>();
        colorsToScanFor.ForEach(x => percentages.Add(x, 0));
        pixels.AsParallel().ForAll((Color col) =>
        {
            int closest = ClosestColor3(colorsToScanFor, col);
            Color closestColor = colorsToScanFor[closest];
            percentages[closestColor] += 1;
        });
        percentages.ToList().ForEach(x => percentages[x.Key] = x.Value / pixels.Length);
        RocketColors colors = new RocketColors(percentages);
        return colors;
    }

    private float GetWeightSlow(Color[] pixels, int texWidth, int texHeight)
    {
        int filledPixels = 0;
        pixels.ToList().ForEach((col) =>
        {
            if (col.a > 0.5) filledPixels++;
        });
        return filledPixels / (float)pixels.Length;
    }

    private int GetWidthSlow(Color[] pixels, int width, int height)
    {
        int totalyes = 0;
        for (int y = height - 1; y >= 0; y--)
        {
            int hits = 0;
            for (int x = 0; x < width; x++)
            {
                Color pixel = pixels[y * width + x];
                if (pixel.a > 0.5) hits++;
            }

            if (hits > hitsRequiredBeforeALineIsClassifiedAsARocket) totalyes++;
            if (totalyes > 3) return y;
        }

        return 0;
    }

    private int GetHeightSlow(Color[] pixels, int width, int height)
    {
        int totalyes = 0;
        for (int x = width - 1; x >= 0; x--)
        {
            int hits = 0;
            for (int y = 0; y < height; y++)
            {
                Color pixel = pixels[y * width + x];
                if (pixel.a > 0.5) hits++;
            }

            if (hits > hitsRequiredBeforeALineIsClassifiedAsARocket) totalyes++;
            if (totalyes > 3) return x;
        }

        return 0;
    }

    private int GetHeightBinarySearch(Color[] pixels, int width, int height)
    {
        int offset = height / 2;
        int currenty = offset;
        int loops = 0;
        while (offset > 1 && loops < 10000)
        {
            offset /= 2;
            int hits = ScanLine(currenty, pixels, width);
            currenty += hits > hitsRequiredBeforeALineIsClassifiedAsARocket
                ? offset
                : -offset;
            loops++; //to prevent an infinite loop
        }

        return currenty;
    }

    private int ScanLine(int lineY, Color[] pixels, int width)
    {
        int hits = 0;
        for (int i = 0; i < width; i++)
        {
            Color pixel = pixels[lineY * width + i];
            hits += pixel.a > 0.5f ? 1 : 0;
        }

        return hits;
    }

    #region Helper Functions

    private int ClosestColor3(List<Color> colors, Color target)
    {
        Color.RGBToHSV(target, out float hue, out float sat, out float brightness);
        float hue1 = hue;
        var num1 = ColorNum(target, sat);
        var diffs = colors.Select(n =>
        {
            Color.RGBToHSV(n, out float huetwo, out float sattwo, out float brightnesstwo);
            return Math.Abs(ColorNum(n, sattwo) - num1) +
                   getHueDistance(hue, hue1);
        });
        var diffMin = diffs.Min(x => x);
        return diffs.ToList().FindIndex(n => Math.Abs(n - diffMin) < 0.2f);
    }

    private float getBrightness(Color c)
    {
        return (c.r * 0.299f + c.g * 0.587f + c.b * 0.114f) / 256f;
    }

    private float getHueDistance(float hue1, float hue2)
    {
        float d = Math.Abs(hue1 - hue2);
        return d > 180 ? 360 - d : d;
    }

    private float ColorNum(Color c, float saturation)
    {
        return saturation * 0.6f +
               getBrightness(c) * 0.4f;
    }


    private int ColorDiff(Color c1, Color c2)
    {
        return (int)Math.Sqrt((c1.r - c2.r) * (c1.r - c2.r)
                              + (c1.g - c2.g) * (c1.g - c2.g)
                              + (c1.b - c2.b) * (c1.b - c2.b));
    }

    #endregion
}