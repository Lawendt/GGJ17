Shader "Wave/VertexWave"
{
	Properties
	{
		_BaseColor ("BaseColor", Color) = (0,0,0,1)
		_TopColor ("TopColor", Color) = (1,1,1,1)
		_WaveMagnitude ("WaveMagnitude", float) = 1.0
		_WaveFrquency ("WaveFrquency", float) = 1.0
		_WaveSpeed("WaveSpeed", float) = 1.0
		_MaxDistance ("WaveMaxDistance", float) = 100.0
	}
	SubShader
	{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }

		Lighting Off
		Fog{ Mode Off }
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag alpha
			
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
				float4 color : COLOR0;
			};

			float4 _MainTex_ST;
			float4 _BaseColor;
			float4 _TopColor;
			float _WaveMagnitude;
			float _WaveFrquency;
			float _MaxDistance;
			float _WaveSpeed;
			
			v2f vert (appdata v)
			{
				v2f o;
				float dist = distance(float4(0, 0, 0, 0), v.vertex);
				float height = sin(dist/_MaxDistance*_WaveFrquency -_Time.w*_WaveSpeed)*_WaveMagnitude;
				o.vertex = UnityObjectToClipPos(v.vertex+float3(0,height,0));
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				float factor = (height / _WaveMagnitude)*0.5 + 1.0;
				o.color = (_BaseColor*(1-factor) + _TopColor*(factor))*(1-(dist/_MaxDistance));
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = i.color;
				return col;
			}
			ENDCG
		}
	}
}
