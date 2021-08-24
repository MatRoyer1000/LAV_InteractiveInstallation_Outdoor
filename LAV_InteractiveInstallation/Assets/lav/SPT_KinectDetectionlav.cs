using UnityEngine;
using System.Collections;
using Windows.Kinect;


public class SPT_KinectDetectionlav : MonoBehaviour
{
    public GameObject DepthSourceManager;
    public Material Mat;
    private KinectSensor _Sensor;
    private CoordinateMapper _Mapper;
    private Vector3[] _Vertices;
    Texture2D texture;
    byte[] depthBitmapBuffer;
    /// Coordonnées Points 01
    public float PosZ1;
    public float PosX1;
    public float PosY1;
    private float PosX1b;
    private float PosY1b;
    private float PosX1c;
    private float PosY1c;
    /// Coordonnées Points 02
    public float PosZ2 ;
    public float PosX2 ;
    public float PosY2 ;
    public float H;
    public float W;
    //public float Dist;
    //public float DistPoint2;
    /// CoordonnéesValue condition trigger event
    private float t1 = 0;
    private float t2 = 0;
    public float scale = 1.0f;
    private const int _DownsampleSizex =16;
    private const int _DownsampleSizey =14;
    private const double _DepthScale = 0.1f;
    private const int _Speed = 50;
    private DepthSourceManager _DepthManager;
    /// Width & Height zone canvas
    public float lh;
    public float lb;
    public float lg;
    public float ld;
    public float maxDistance = 10;
    public float minDistance = 400;
    public float maxHauteur = 0;
    public float minHauteur = 0;
    //public bool Coucou;
    //public SPT_EventTrig KinectEvent;
    private Vector2 g;
    private float vtot;
    private float Lh;
    private float Lb;
    private float Lg;
    private float Ld;
    float XVelocity = 0.0f;
    float YVelocity = 0.0f;
    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }
    void Start()
    {
        // KinectEvent.Touch1();
        


        g = new Vector2(0.0f, 0.0f);
        vtot = 0.0f;
    _Sensor = KinectSensor.GetDefault();
        if (_Sensor != null)
        {
            _Mapper = _Sensor.CoordinateMapper;
            var frameDesc = _Sensor.DepthFrameSource.FrameDescription;
            H = frameDesc.Height / _DownsampleSizey;
            W = frameDesc.Width / _DownsampleSizex;
             Lh = lh * H; ;
             Lb = lb * H;
             Lg = lg * W;
             Ld = ld * W;
            // Downsample to lower resolution
            CreateMesh(frameDesc.Width / _DownsampleSizex, frameDesc.Height / _DownsampleSizey);
            depthBitmapBuffer = new byte[frameDesc.LengthInPixels * 3];
            texture = new Texture2D(frameDesc.Width, frameDesc.Height, TextureFormat.RGB24, false);
            if (!_Sensor.IsOpen)
            {
                _Sensor.Open();
            }
        }        
    }

    void CreateMesh(int width, int height)
    { 
        _Vertices = new Vector3[width * height];

        
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = (y * width) + x;

                _Vertices[index] = new Vector3(x, y, 0);               
            }
        }
    } 

    void Update()
    {

        if (_Sensor == null)
        {
            return;
        }
                         
            if (DepthSourceManager == null)
            {
                return;
            }            
            _DepthManager = DepthSourceManager.GetComponent<DepthSourceManager>();
            if (_DepthManager == null)
            {
                return;
            }
            
        RefreshData(_DepthManager.GetData());

        PosZ1 =1000.0f;
        
        g = new Vector2(0.0f, 0.0f);
        vtot = 0.0f;
        for (int i = 1; i < _Vertices.Length; i++)
       
        {
         
                //float nv = Mathf.Clamp01(map(_Vertices[i].z, maxDistance, minDistance, 0.0f, 1.0f));
                /*float nv2 = 1000;
                if (nv < 0.1f) { nv2 = 1000; }
                else { nv2 = nv; }  
                if (nv > 0.9f) { nv2 = 1000; }
                else { nv2 = nv; }       
                //float nv = _Vertices[i].z;   */
                float tv = 1000;
                if (_Vertices[i].z > minDistance && _Vertices[i].z < maxDistance && _Vertices[i].y<maxHauteur
                && _Vertices[i].y > minHauteur
                ) { tv = _Vertices[i].z; }

                //g += new Vector2(map(_Vertices[i].x, Ld, Lg, 1.0f, 0.0f), map(_Vertices[i].y, Lb, Lh, 1.0f, 0.0f)) * nv;
                //vtot += nv;
                if (tv < PosZ1)
                {
                    PosZ1 = _Vertices[i].z;
                    PosX1c = _Vertices[i].x;
                    PosY1c = _Vertices[i].y;
                }
                 
        }
        //PosX2 = g.x/vtot;
        //PosY2 = g.y/vtot;
        PosX1b = Mathf.SmoothDamp(PosX1b, PosX1c, ref XVelocity, 0.1f);
        PosY1b = Mathf.SmoothDamp(PosY1b, PosZ1, ref YVelocity, 0.1f);
        PosX1 = map(PosX1b, Ld, Lg, 0.0f, 1.0f);
        //PosY1 = map(PosY1b, Lb, Lh, 1.0f, 0.0f);
        //PosX1 = PosX1b;
        PosY1 = PosY1b;
       // PosZ2 = Vector2.Distance(new Vector2(PosX1, PosY1), new Vector2(PosX2, PosY2));
        
        //updateTexture();

         Mat.SetFloat("_p1", PosX1);
         Mat.SetFloat("_p2", PosY1);
         //Mat.SetFloat("_p3",  PosX2 );
         //Mat.SetFloat("_p4",  PosY2 );
         //Mat.SetFloat("_p5", PosZ2);
        //Mat.SetFloat("_lh", lh);
        //Mat.SetFloat("_lb", lb);
        //Mat.SetFloat("_ld", ld);
        //Mat.SetFloat("_lg", lg);
        //Mat.SetTexture("_texsec" , texture);  

        if (float.IsNaN(PosZ2))
        {
            PosZ2 = 0;
            PosY2 = 0;
            PosX2 = 0;
        }


    }
    
    private void RefreshData(ushort[] depthData)
    {
        var frameDesc = _Sensor.DepthFrameSource.FrameDescription;
    
        for (int y = 0; y < 420; y += _DownsampleSizey)
        {
            for (int x = 0; x < frameDesc.Width; x += _DownsampleSizex)
            {
                int indexX = x / _DownsampleSizex;
                int indexY = y / _DownsampleSizey;
                int v1 = indexY * (frameDesc.Width / _DownsampleSizex);
                if(v1 > 960 ) { v1 = 960; }
                int smallIndex = v1 + indexX;
               // double avg = GetAvg(depthData, x, y, frameDesc.Width, frameDesc.Height);
                double avg = depthData[(y * frameDesc.Width) + x];
                
                avg = avg * _DepthScale;
                
                _Vertices[smallIndex].z = (float )avg;
                // Update UV mapping with CDRP

            }
        }     
    }


    /*void updateTexture()
    {
        // get new depth data from DepthSourceManager.
        ushort[] rawdata = _DepthManager.GetData();

        // convert to byte data (
        for (int i = 0; i < rawdata.Length; i++)
        {

            byte value = (byte)(rawdata[i] * scale);
            // byte value2 = (byte)255;
            if (value < 0.05f) { value = 0; value = 255; }
            int colorindex = i * 3;
            depthBitmapBuffer[colorindex + 0] = value;
            depthBitmapBuffer[colorindex + 1] = value;
            depthBitmapBuffer[colorindex + 2] = value;
        }
        // make texture from byte array
        texture.LoadRawTextureData(depthBitmapBuffer);
        texture.Apply();
    }        */
    void OnApplicationQuit()
    {
        if (_Mapper != null)
        {
            _Mapper = null;
        }
        
        if (_Sensor != null)
        {
            if (_Sensor.IsOpen)
            {
                _Sensor.Close();
            }

            _Sensor = null;
        }
    }
}
