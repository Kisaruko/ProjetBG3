Shader "Custom/PostRendering/Pencil"

{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _GradThresh ("Gradiant threshold", range(0.000001, 0.2)) = 0.01 // base 0.03
        _ColorThreshold ("Color Threshold", range(0.0, 1)) = 0.5
        _OutLineTresh("Outline treshold",range(0.1,20.0)) = 2.0 // base 3.0
        _DrawingColor("Drawing Color",Color) = (1,1,1,1)
        _BackGroundColor("BackGroundColor",Color) = (0,0,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
		//Cull Off Zwrite Off ZTest Always
        Pass
        {

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 4.0
            #include "UnityCG.cginc"
 
            sampler2D _MainTex;
 
            float _GradThresh;
            float _ColorThreshold;
            float _Intensity;
            float _OutLineTresh;
            float4 _DrawingColor;
            float4 _BackGroundColor;

 
            struct v2f {
                float4 pos : SV_POSITION;
                float4 screenuv : TEXCOORD0;
            };
 
            v2f vert(appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.screenuv = ComputeScreenPos(o.pos);
                return o;
            }
 
            #define PI2 6.28318530717959
            #define STEP 2.0
            #define RANGE 4.0//16base
            #define ANGLENUM 4.0
            #define GRADTHRESH 0.01
            #define SENSITIVITY 10.0
 
            float4 getCol(float2 pos)
            {
                return tex2D(_MainTex, pos / _ScreenParams.xy);
            }
 
            float getVal(float2 pos)
            {
                float4 c = getCol(pos);
                return dot(c.xyz, float3(0.2126, 0.7152, 0.0722));
            }
 
            float2 getGrad(float2 pos, float delta)
            {
                float2 d = float2(delta, 0.0);
                return float2(getVal(pos + d.xy) - getVal(pos - d.xy),
                              getVal(pos + d.yx) - getVal(pos - d.yx)) / delta / 2.0;
            }
 
            void pR(inout float2 p, float a) {
                p =  cos(a) * p + sin(a) * float2(p.y, -p.x);
            }
 
            fixed4 frag(v2f i) : SV_Target
            {
                float2 screenuv = i.screenuv.xy / i.screenuv.w;
                float2 screenPos = float2(i.screenuv.x * _ScreenParams.x, i.screenuv.y * _ScreenParams.y);
                float weight = 1.0;
 				
                for(int j = 0; j < ANGLENUM; j++)
                {
                    float2 dir = float2(1.0, 0.0) ;
                    pR(dir, j * PI2 / (2.0 * ANGLENUM));
       
                    float2 grad = float2(-dir.y, dir.x);
       
                    for(int i = -RANGE; i <= RANGE; i += STEP)
                    {
                        float2 b = normalize(dir);
                        float2 pos2 = screenPos + float2(b.x, b.y) * i;
           
                        if (pos2.y < 0.0 || pos2.x < 0.0 || pos2.x > _ScreenParams.x || pos2.y > _ScreenParams.y)
                            continue;
           
                        float2 g = getGrad(pos2, 1.0);
 
                        if (sqrt(dot(g,g)) < _GradThresh)
                            continue;
           
                        weight -= pow(abs(dot(normalize(grad), normalize(g))), SENSITIVITY) / floor((_OutLineTresh * RANGE + 1.0) / STEP) / ANGLENUM;
                    }
                }

				
                float4 col = getCol(screenPos);

                float4 background = lerp(col, _BackGroundColor, _ColorThreshold);
 
                return lerp(_DrawingColor, background, weight);
            }
 
            ENDCG
        }
    }
}
