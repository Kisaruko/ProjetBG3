﻿Shader "Custom/LitDissolve"{
 Properties {
 _Color ("Color", Color) = (1,1,1,1)
 _MainTex ("Albedo (RGB)", 2D) = "white" {}
 _Glossiness ("Smoothness", Range(0,1)) = 0.5
 _Metallic ("Metallic",2D) = "black"{}
  _BumpMap ("Normal map", 2D) = "bump" {}
 //Dissolve properties
 _DissolveTexture("Dissolve Texture", 2D) = "white" {} 
 _Amount("Amount", Range(0,1)) = 0
 _BorderSize("Border size", Range(0,0.5)) = 0
 _EdgeColour("Edge Color", Color) = (1,1,1,1)
 }
 
 SubShader {
 Tags { "RenderType"="Opaque" }
 LOD 200
// Cull Off //Fast way to turn your material double-sided
 
 CGPROGRAM
 #pragma surface surf Standard fullforwardshadows
 
 #pragma target 3.0
 
 sampler2D _MainTex;
 
 struct Input {
		float2 uv_MainTex;
		float2 uv_BumpMap;
			  };
 
 half _Glossiness;
 sampler2D _Metallic;
 fixed4 _Color;
 
 //Dissolve properties
 sampler2D _DissolveTexture;
 half _Amount;
 float _BorderSize;
 float4 _EdgeColour;
  sampler2D _BumpMap;
 void surf (Input IN, inout SurfaceOutputStandard o) {
 
 //Dissolve function
 half dissolve_value = tex2D(_DissolveTexture, IN.uv_MainTex).r;
 clip(dissolve_value - _Amount);
  o.Emission =_EdgeColour * step( dissolve_value - _Amount, _BorderSize); //emits white color with 0.05 border size
 
 //Basic shader function
 fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color; 
 fixed4 cSpec = tex2D(_Metallic, IN.uv_MainTex);
 
 o.Albedo = c.rgb;
 o.Metallic = cSpec.rgb;
 o.Smoothness = _Glossiness;
 o.Alpha = c.a;
 o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
 }
 ENDCG
 }
 FallBack "Diffuse"
}
