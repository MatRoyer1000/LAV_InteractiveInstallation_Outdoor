using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Variables : MonoBehaviour
{
    [SerializeField, Range(0, 1)]
    public float PositionX;
    public float PositionY;
    public float Rotation;
    public Text OBJ_Text;
    public float DisplayXValue;
    public float XValue;

    public Vector3 UserPositionX;
    public float MaxUserPositionX;
    public float MinUserPositionX;
    public float KinectValue;
    // LINE
    public GameObject VLine;
    public Vector2 LinePosX;
    // CANVAS
    public float FixedCanvasSize;
    public float DynamicCanvas;
    [SerializeField, Range(0, 2140)]
    public float CanvasHeight;
    public float Margin_Line;
    public float MinScaleCanvas;

    public Camera cam;
    public GameObject Background;
    public float MinKinect;
    public float MaxKinect;
    public float t2;
    public float duration;
    public Color color1;
    public Color color2;

    public SPT_KinectDetectionlav KinectTracking;

    //KINECT
    //public Kinect_Data SPT_Kinect;
    //public float KinectXPos;

    float map(float Val, float minInit, float MaxInit, float MinFinal, float MaxFinal)
    {
        return MinFinal + (Val - minInit) * (MaxFinal - MinFinal) / (MaxInit - minInit);
    }

    void Start()
    {
        //cam = GetComponent<Camera>();
        cam.clearFlags = CameraClearFlags.SolidColor;
    }

    void Update()
    {
        // KINECT USER X POSITION & ASSOCIATE WITH Canvas
        //UserPositionX.x = map(SPT_Kinect.Hip.transform.position.x, 0, 1, -0.85f, 0.5f);
        /////// OLD SYSTEM ::::: UserPositionX.x = map(SPT_Kinect.Hip.transform.position.x,MinKinect, MaxKinect, 0, 1);
        UserPositionX.x = map(KinectTracking.PosX1, MinKinect, MaxKinect, 0, 1);
        PositionX = UserPositionX.x;

        //UserPositionX.x = map(SPT_Kinect.HipXValue.x, 0, 1, -0.6f, 0.6f);
        UserPositionX.x = PositionX;
        // NORMALIZE TEXT CANVAS
        XValue = map(Mathf.Clamp(PositionX,0,1), 0, 1, -850f, FixedCanvasSize -Margin_Line );
        //XValue = map(Mathf.Clamp(PositionX, 0, 1), 0, 1, 0, FixedCanvasSize - 940);
        // DISPLAY TEXT
        OBJ_Text.text = "<b></b><size=45>void Update(){" +
            "UserPositionX.x = map(KinectTracking.PosX1, MinKinect, MaxKinect, 0, 1);" +
            "PositionX = UserPositionX.x; "+
            "UserPositionX = map(KinectValue, 0, 1, MinUserPositionX, MaxUserPositionX);" +
            "UserPositionX = PositionX;" +
            "XValue = map(Mathf.Clamp(PositionX,0,1), 0, 1, 0, FixedCanvasSize - 940); " +
            "RectTransform rectTransform;" +
            "rectTransform = text.GetComponent<RectTransform>();" +
            "rectTransform.localPosition = new Vector3(XValue, 0, 0);" +
            "DynamicCanvas = Mathf.Lerp(FixedCanvasSize, 200, PositionX);" +
            "rectTransform.sizeDelta = new Vector2(DynamicCanvas - 100, CanvasHeight);" +
            "<b><i>DisplayXValue = </i></b></size>" +  XValue +
            "<size=45>VLine.transform.position = new Vector3(Mathf.Lerp(LinePosX.x, LinePosX.y, PositionX), 0, 0);</size>";
        //"float fd = abs(dot(dot(mod(cos(uv.x * 5.+ time * .5), dot(uv.y, uv.x * 7)), mod(sin(uv.x * 8.26 * +uv.y * 23.23), uv.x + uv.y)), mod(uv.x * 81.3, uv.y * 78.)));"

        RectTransform rectTransform;
        rectTransform = OBJ_Text.GetComponent<RectTransform>();
        //rectTransform.localPosition = new Vector3( PositionX,0,0);
        rectTransform.localPosition = new Vector3(XValue,0,0);
        DynamicCanvas = Mathf.Lerp(FixedCanvasSize, MinScaleCanvas, PositionX);
        rectTransform.sizeDelta = new Vector2(DynamicCanvas, CanvasHeight);
        // DISPLAY POSITION X
        DisplayXValue = XValue;
        // LINE POS
        VLine.transform.position = new Vector3(Mathf.Lerp(LinePosX.x, LinePosX.y, PositionX),0 , 88);
        // KINECT
        if (UserPositionX.x ==0)
        {
           // PositionX = Mathf.Lerp()
        }

        if (UserPositionX.x < MaxKinect-0.5f )
        {
            //PositionX = Mathf.Lerp(MaxKinect, 0, t2 += 0.05f * Time.deltaTime);
        }

        if (UserPositionX.x > MinKinect+0.5f)
        {
            //float t = Mathf.PingPong(Time.time, duration) / duration;
            //cam.backgroundColor = Color.Lerp(color1, color2, t);
            //PositionX = Mathf.Lerp(MaxKinect, 0, t2 += 0.1f * Time.deltaTime);
        }

        if (Input.GetKeyDown("r"))
        {
            Application.LoadLevel(Application.loadedLevel);
        }

    }


}
