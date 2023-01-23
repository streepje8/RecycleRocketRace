using UnityEngine;
using UnityEngine.UI;

public class FinalCamProcessor : MonoBehaviour
{
    public Stream processedStream;

    public float scanTime = 2f;
    public Stream webcam;
    public RawImage output;
    public Material webcammat;
    public Material blurmat;
    public Material secondaryProcessor;
    public ComputeShader maskProcessor;
    public Material finalProcessor;
    public int outlineSize = 2;

    public bool isProcessing = false;

    private WebCamTexture input;
    private float timer = 0f;
    private RenderTexture outputTexture = null;
    private RenderTexture blurredSobel = null;
    private RenderTexture processedSobel = null;
    private RenderTexture sobelWebcam = null;
    private RenderTexture webcamCopy = null;
    private static readonly int Width = Shader.PropertyToID("_Width");
    private static readonly int Height = Shader.PropertyToID("_Height");
    private int kernel = 0;
    private static readonly int SobelMode = Shader.PropertyToID("_SobelMode");
    private int outline = 2;

    private void Awake()
    {
        webcam.AddOutputStream<WebCamTexture>(t =>
        {
            input = t;
            blurmat.SetInt(Width, input.width);
            blurmat.SetInt(Height, input.height);
            secondaryProcessor.SetInt(Width, input.width);
            secondaryProcessor.SetInt(Height, input.height);
            maskProcessor.SetInt("_InputWidth", input.width);
            maskProcessor.SetInt("_InputHeight", input.height);
        });
        kernel = maskProcessor.FindKernel("CSMain");
    }

    public void Process()
    {
        if (webcamCopy == null) webcamCopy = new RenderTexture(input.width, input.height, 1);
        Graphics.CopyTexture(input, webcamCopy);
        if (sobelWebcam == null) sobelWebcam = new RenderTexture(input.width, input.height, 1);
        webcammat.SetInt(SobelMode, 1);
        Graphics.Blit(input, sobelWebcam, webcammat);
        webcammat.SetInt(SobelMode, 0);
        if (blurredSobel == null) blurredSobel = new RenderTexture(input.width, input.height, 1);
        Graphics.Blit(sobelWebcam, blurredSobel, blurmat);
        if (processedSobel == null) processedSobel = new RenderTexture(input.width, input.height, 1);
        Graphics.Blit(blurredSobel, processedSobel, secondaryProcessor);
        if (outputTexture == null)
        {
            outputTexture = new RenderTexture(input.width, input.height, 1);
            outputTexture.enableRandomWrite = true;
        }

        Graphics.CopyTexture(processedSobel, outputTexture);
        output.texture = outputTexture;
        maskProcessor.SetTexture(kernel, "_Texture", outputTexture);
        outline = outlineSize;
        isProcessing = true;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > scanTime && input != null)
        {
            timer = 0f;
            if (!isProcessing)
                Process();
            else Export();

            #region failed attempts

            //Graphics.CopyTexture(rendtext, 0, 0, 0, 0, rendtext.width, rendtext.height, inputTex, 0, 0, 0, 0);
            /*
             private Texture2D rendered;
            RenderTexture.active = rendtext;
            inputTex.ReadPixels(new Rect(0,0,rendtext.width,rendtext.height),0,0);
            inputTex.Apply();
            RenderTexture.active = null;
            if (lastInputTex != null)
            {
                var meh = inputTex.GetPixels();
                FloodFill(lastInputTex, outputTex, 0, 0, Color.magenta);
                FloodFill(lastInputTex, outputTex, 0, inputTex.height, Color.magenta);
                FloodFill(lastInputTex, outputTex, inputTex.width, 0, Color.magenta);
                FloodFill(lastInputTex, outputTex, inputTex.width, inputTex.height, Color.magenta);
                outputTex.Apply();
            }
            */

            /*
            if(tex == null) tex = new Texture2D(input.width / downscalefactor, input.height / downscalefactor);
            if(rendtex == null)rendtex = new RenderTexture(input.width, input.height, 1);
            Graphics.Blit(input,rendtex,webcammat);
            if(rendered == null) rendered = new Texture2D(rendtex.width, rendtex.height);
            RenderTexture.active = rendtex;
            rendered.ReadPixels(new Rect(0,0,rendtex.width,rendtex.height),0,0);
            rendered.Apply();
            timer = 0f;
            int blacks = 999;
            bool state = false;
            int hits = 0;
            for (int y = 0; y < input.height; y += 10)
            {
                for (int x = 0; x < input.width; x += 10)
                {
                    float sobelvalue = Mathf.Pow(rendered.GetPixel(x, y).r, 2);
                    if (sobelvalue > _ActivationValue)
                    {
                        hits++;
                        if (hits > _MinActivationHits)
                        {
                            if (blacks > 4)
                            {
                                state = !state;
                            }
                        }
                        
                    }
                    else
                    {
                        blacks++;
                        if (hits > 2) blacks = 0;
                        hits = 0;

                    }
                    tex.SetPixel(x/downscalefactor,y/downscalefactor,!state ? Color.white : Color.black);
                }
                state = false;
            }
            tex.Apply();
            output.texture = tex;
            */

            #endregion
        }

        if (outputTexture != null && isProcessing)
            for (int i = 0; i < 10; i++)
            {
                maskProcessor.SetInt("_OutlineSize", outline);
                maskProcessor.Dispatch(0, input.width, input.height, 1);
                outline--;
            }
    }

    private void Export()
    {
        isProcessing = false;
        RenderTexture colorProcessed = new RenderTexture(input.width, input.height, 1);
        Graphics.Blit(webcamCopy, colorProcessed, webcammat);
        finalProcessor.SetTexture("_WebcamTex", webcamCopy);
        finalProcessor.SetTexture("_ColorTex", colorProcessed);
        finalProcessor.SetTexture("_SobelTex", outputTexture);
        RenderTexture finalRocket = new RenderTexture(input.width, input.height, 1);
        Graphics.Blit(webcamCopy, finalRocket, finalProcessor);
        output.texture = finalRocket;
        Texture2D finalRocketTex = new Texture2D(input.width, input.height);
        RenderTexture.active = finalRocket;
        finalRocketTex.ReadPixels(new Rect(0, 0, input.width, input.height), 0, 0);
        finalRocketTex.Apply();
        RenderTexture.active = null;
        processedStream.StreamData(finalRocketTex);
        GameController.Instance.NextScreen();
    }

    private void OnDisable()
    {
        //Prevent a big boi memory leak
        outputTexture?.Release();
        blurredSobel?.Release();
        processedSobel?.Release();
        sobelWebcam?.Release();
        webcamCopy?.Release();
        outputTexture = null;
        blurredSobel = null;
        processedSobel = null;
        sobelWebcam = null;
        webcamCopy = null;
    }

    #region even more failed attempts

    /*
public float _ActivationValue = 0.6f;
public int _MinActivationHits = 3;
public float power = 2;
public int downscalefactor = 2;
*/
    /*
    private void FloodFill(Texture2D inputTex, Texture2D outputTex, int startX, int startY, Color fillCol)
    {
        List<Vector2Int> toProcess = new List<Vector2Int>();
        toProcess.Add(new Vector2Int(startX,startY));
        while (toProcess.Count > 0)
        {
            Vector2Int currentPixel = toProcess[0];
            toProcess.RemoveAt(0);
            foreach (var vector2Int in FloodFillStep(inputTex, outputTex, currentPixel.x, currentPixel.y, fillCol))
            {
                toProcess.Add(vector2Int);
            }
        }
    }

    private List<Vector2Int> FloodFillStep(Texture2D inputTex, Texture2D outputTex, int startX, int startY, Color fillcol)
    {
        Color c = inputTex.GetPixel(startX, startY);
        if (c.r < 1)
        {
            outputTex.SetPixel(startX,startY,fillcol);
            List<Vector2Int> toScan = new List<Vector2Int>();
            PossiblyAddPixel(inputTex,startX + 1, startY,ref toScan);
            PossiblyAddPixel(inputTex,startX - 1, startY,ref toScan);
            PossiblyAddPixel(inputTex,startX, startY + 1,ref toScan);
            PossiblyAddPixel(inputTex,startX, startY - 1,ref toScan);
        }
        return new List<Vector2Int>();
    }

    public void PossiblyAddPixel(Texture2D inputTex, int x, int y, ref List<Vector2Int> list)
    {
        if(x > 0 && y > 0 && x < inputTex.width && y < inputTex.height) list.Add(new Vector2Int(x,y));
    }
    */

    #endregion
}