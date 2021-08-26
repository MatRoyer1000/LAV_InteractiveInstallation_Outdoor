Shader "Unlit/test01"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
	_color ("_color", COLOR) = (1,1,1,1)
	_p1("_p1", Float) = 0
		_p2("_p2", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float _p1;
			float _p2;
			float4 _color;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
			float2 mod(float2 x, float2 y)
			{
				return x - y * floor(x / y);
			}
			float map(float value, float min1, float max1, float min2, float max2) {
				return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
			}
			float2x2 rot(float t) { float c = cos(t); float s = sin(t); return float2x2(c, -s, s, c); }
			float3 map(float2 p) {
				float mox = clamp(map(_p1, 0.1, 0.7, 0., 1.), 0., 1.);
				float moy = clamp(map(_p2, 100., 280., 0., 1.), 0., 1.);
				float pi = 3.14159265358979;
				float mx = (mox - 0.5)*2.;
				float lx = length(mx);
				float my = length((moy - 0.5)*2.);
				float sy = step(moy, 0.5);
				float ty = floor(my*lerp(3., 11., sy)*(1. - lx));
				float md = 0.35 + lerp(0.09, 0.5, sy)*my;
				p.y += lerp(md, 0., lx);


				float br = 0.5;
				float tr = pi * -0.25 + 0.25*(pow(distance(br, 0.5)*2., 3.))*sign(br - 0.5)*(1. - lx);
				p = mul(p, rot(tr));
				float d3f = 1.;
				float d4 = 1.;
				for (int i = 0; i < 4 + ty; i++) {
					p = mul(p, rot(-tr));
					p.y -= lerp(md, 0., lx);
					p = abs(p);
					if (p.x > p.y)p.xy = p.yx;
					float fd3 = length(p.x - i * 0.02);
					float d3 = smoothstep(0.015, 0.025, fd3);
					float d3b = step(length(p.y), 0.024 + i * 0.049);
					float d3c = max(d3, d3b);
					d3f = lerp(d3c, d3f, d3);
					d4 = min(d4, fd3);
				}
				float2 rg = float2(0.15,0.15);
				float2 pg = mod(p, rg) - 0.5*rg;
				float tl = 0.0015;
				float d1 = smoothstep(0.0, 0.002, min(length(pg.x), length(pg.y)));
				float df = min(d1, d3f);
				return float3(smoothstep(0.1, 0.3, d3f), d1, d4);
			}

            fixed4 frag (v2f i) : SV_Target
            {
			 float2 uv = -1. + 2. * i.uv;
	float2 ut = i.uv;
	uv.x *=16. / 9.;
	float r1 = 0.;

	float3 r2 = map(uv);
	float r4 = step(0.029, r2.z);
	float r5 = step(r2.z, 0.026);
	float r6 = max(r4, r5);
	float r3 = min(r2.x, max(r2.y, 1. - r4))*r6;
	return _color*r3;
            }
            ENDCG
        }
    }
}
