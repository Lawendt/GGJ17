Shader "Wave/SurfaceWave"
{
	Properties
	{
		_SonarBaseColor("Base Color",  Color) = (0.1, 0.1, 0.1, 0)
		_SonarWaveColor("Wave Color",  Color) = (1.0, 0.1, 0.1, 0)
		_SonarWaveParams("Wave Params", Vector) = (1, 20, 20, 10)
		_SonarWaveVector("Wave Vector", Vector) = (0, 0, 0, 0)
		_SonarAddColor("Add Color",   Color) = (0, 0, 0, 0)
	}

	SubShader
	{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
		LOD 200

		CGPROGRAM

		#pragma surface surf Lambert alpha:fade

		struct Input
		{
			float3 worldPos;
		};

		float3 _SonarBaseColor;
		float3 _SonarWaveColor;
		float4 _SonarWaveParams; // Amp, Exp, Interval, Speed
		float3 _SonarWaveVector;
		float3 _SonarAddColor;

		void surf(Input IN, inout SurfaceOutput o)
		{
			float w = length(IN.worldPos - _SonarWaveVector);

			// Moving wave.
			w -= _Time.y * _SonarWaveParams.w;

			// Get modulo (w % params.z / params.z)
			w /= _SonarWaveParams.z;
			w = w - floor(w);

			float alfa = pow(w,2);

			// Make the gradient steeper.
			float p = _SonarWaveParams.y;
			w = (pow(w, p) + pow(1 - w, p * 4)) * 0.5;

			float grad = w;

			// Amplify.
			w *= _SonarWaveParams.x;


			// Apply to the surface.
			o.Albedo = _SonarBaseColor;
			o.Alpha = alfa;
			o.Emission = _SonarWaveColor * w + _SonarAddColor;
		}

		ENDCG
	}
	
	Fallback "Diffuse"
}