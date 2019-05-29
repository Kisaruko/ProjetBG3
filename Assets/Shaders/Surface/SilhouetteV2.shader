Shader "Custom/SilhouetteV2" {
    Properties {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Dots("Dots",2D) = "white"{}
        _Size("Size",Vector) = (1,1,1,1)
		_BumpMap ("Normal map", 2D) = "bump" {}
        _Smoothness ("Smoothness", Range(0,1)) =0 
        _Metallic ("Metallic", 2D) = "white" {}
        _Emissive("Emissive", 2D) = "black" {}
    	_EmissiveColor("_EmissiveColor", Color) = (1,1,1,1)
    	_EmissiveIntensity("_EmissiveIntensity", Range(0,1000) ) = 0.5
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200

        GrabPass
        {
            "_BackgroundTexture"
        }

        ZWrite Off
        ZTest Greater


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
                float4 grabPos : TEXCOORD1;
            };

            sampler2D _MainTex,_Dots;
            float4 _MainTex_ST;
            float4 _Size;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 screen = i.vertex.xy/_ScreenParams.xy;
                float4 tex = tex2D(_Dots, screen /_Size);
                return tex;
            }
            ENDCG
        }
        ZWrite On
        ZTest LEqual 

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
	    #pragma shader_feature _METALLICGLOSSMAP

        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _Emissive;
        float4 _EmissiveColor;
        float _EmissiveIntensity;

        struct Input 
		{
            float2 uv_MainTex;
            float2 uv_Emissive;
			float2 uv_BumpMap;
        };

        sampler2D _Metallic;
        fixed4 _Color;
	    sampler2D _BumpMap;
		half _Smoothness;

        void surf (Input IN, inout SurfaceOutputStandard o) 
		{
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
			o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
            fixed4 cSpec = tex2D(_Metallic, IN.uv_MainTex);
			o.Metallic = cSpec.rgb;
			o.Smoothness = _Smoothness * cSpec.a;
            o.Alpha = c.a;

           float4 Tex2D1=tex2D(_Emissive,(IN.uv_Emissive.xyxy).xy);
   		   float4 Multiply0=Tex2D1 * _EmissiveColor;
   		   float4 Multiply2=Multiply0 * _EmissiveIntensity.xxxx;
   
  		  o.Emission = Multiply2;
        }
        ENDCG
    }
    FallBack "Diffuse"
}