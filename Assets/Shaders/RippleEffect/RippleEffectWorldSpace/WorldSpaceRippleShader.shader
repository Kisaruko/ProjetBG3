Shader "Custom/WorldSpaceRippleShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}

        _RippleTex("RippleTex",2D) = "white"{}
	    _RippleOrigin("Ripple Origin", Vector) = (0,0,0,0)
		_RippleDistance("Ripple Distance", Float) = 0
		_RippleWidth("Ripple Width", Float) = 0.1
         
		_NormalMap ("Normal map", 2D) = "NormalMap" {}
		_Smoothness ("Smoothness", Range(0,1)) =0 
         _Metallic ("Metallic", 2D) = "white" {}
        _Emissive("Emissive", 2D) = "black" {}
    	_EmissiveColor("_EmissiveColor", Color) = (1,1,1,1)
    	_EmissiveIntensity("_EmissiveIntensity", Range(0,1000) ) = 0.5
		_StaticEmissive("StaticEmissive", 2D) ="black" {}
		_StaticEmissiveColor("_StaticEmissiveColor", Color) = (1,1,1,1)
    	_StaticEmissiveIntensity("_StaticEmissiveIntensity", Range(0,1000) ) = 0.5

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex,_RippleTex;
        sampler2D _Emissive;
	    float4 _EmissiveColor;
        float _EmissiveIntensity;

		sampler2D _StaticEmissive;
    	float4 _StaticEmissiveColor;
		float _StaticEmissiveIntensity;


        struct Input
        {
            float2 uv_MainTex;
			float2 uv_RippleTex;
			float3 worldPos;
            float2 uv_Emissive;
			float2 uv_BumpMap;
			float2 uv_StaticEmissive;
        };

        half _Glossiness;
        sampler2D _Metallic;
        fixed4 _Color;
		fixed4 _RippleOrigin;
		float _RippleDistance;
		float _RippleWidth;
	    sampler2D _BumpMap;
		half _Smoothness;
		
        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
	        fixed4 z = tex2D (_MainTex, IN.uv_MainTex) * _Color;
		    o.Albedo = z.rgb;
		 	o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
            fixed4 cSpec = tex2D(_Metallic, IN.uv_MainTex);
			o.Metallic = cSpec.rgb;
			o.Smoothness = _Smoothness * cSpec.a;
         //   o.Alpha = z.a;

		//RIPPLESTUFF
	    float distance = length( IN.worldPos.xyz - _RippleOrigin.xyz) - _RippleDistance * _RippleOrigin.w ;
	    float halfWidth = _RippleWidth *0.5;
	    float lowerDistance = distance - halfWidth;
		float upperDistance = distance + halfWidth;
        fixed4 c = tex2D (_RippleTex, IN.uv_RippleTex) * _Color;
		float ringStrength = pow(max(0, 1 - (abs(distance) / halfWidth)), 2.1) *(lowerDistance< 0 && upperDistance> 0);

		//EMISSIONSTUFF

        float4 Tex2D1=tex2D(_Emissive,(IN.uv_Emissive.xyxy).xy);
		float tex2D2 = tex2D(_StaticEmissive,(IN.uv_StaticEmissive.xyxy).xy);

   	    float4 Multiply0=Tex2D1 * _EmissiveColor * ringStrength;
   		float4 Multiply2=Multiply0 * _EmissiveIntensity.xxxx;

		float4 Multiply1 = tex2D2 *_StaticEmissiveColor;
		float4 Multiply3 = Multiply1*_StaticEmissiveIntensity;

  		o.Emission = Multiply3+ringStrength*Multiply2*(1- _RippleOrigin.w);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
