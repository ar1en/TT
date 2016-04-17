Shader "Ramka"
{
	Properties
	{
		_CurrentColor("CurrentColor", Color) = (1,1,1,1)
		_NextColor("NextColor", Color) = (1,1,1,1)
		_DiffColor("Diffuse", Color) = (1,1,1,1)
		_ColorChangeCounter("Counter", float) = 1
		_Power("Power", float) = 1
		_Power2("Pow2", float) = 1
		_Step1("Step1", float) = 1
		_Step2("Step2", float) = 1
		_MainTex("Base (RGB)", 2D) = "white"  {}
	}
		SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
#pragma surface surf Lambert


		sampler2D _MainTex, _SecTex;
	float3 _ColorLerp1, _ColorLerp2;
	float4 _CurrentColor, _NextColor, _DiffColor;
	float alpTex, _Power, _Power2,_Step1, _Step2, _Coord, _ColorChangeCounter;

	struct Input
	{
		float2 uv_MainTex;
	};

	void surf(Input IN, inout SurfaceOutput o)
	{
		float2 coordUV = IN.uv_MainTex;
		float xcoord = coordUV.x;
		float ycoord = coordUV.y;

		float4 c = tex2D(_MainTex, fixed2(xcoord,ycoord));

		alpTex = c.a;

		if (coordUV.x >= 0.5)
		{
			o.Albedo = _DiffColor.rgb;
		}
		else {
			o.Emission = lerp(_CurrentColor.rgb * pow(alpTex, _Step1) * _Power,
				_NextColor.rgb * pow(alpTex, _Step2) * _Power, _ColorChangeCounter);
		}

	}
	ENDCG
	}
		FallBack "Diffuse"
}
