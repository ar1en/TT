﻿Shader "Ramka"
{
	Properties
	{
		_CurrentColor("CurrentColor", Color) = (1,1,1,1)
		_NextColor("NextColor", Color) = (1,1,1,1)
		_DiffColor("Diffuse", Color) = (1,1,1,1)
		_ColorChangeCounter("Counter", float) = 1
		_Power("FtClPw", float) = 1
//		_Power2("ScClPw", float) = 1
//		_Power3("ZonePW", float) = 1
		_Step("Lamp Step", float) = 1
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
	float coefsetki, alpTex1, _Power, _Step, _Coord, _ColorChangeCounter;

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

		alpTex1 = c.a;

		if (coordUV.x >= 0.5)
		{
			o.Albedo = _DiffColor.rgb;
		}
		else {
			o.Emission = lerp(_CurrentColor.rgb * pow(alpTex1, _Step) * _Power,
				_NextColor.rgb * pow(alpTex1, _Step) * _Power, _ColorChangeCounter);
		}

	}
	ENDCG
	}
		FallBack "Diffuse"
}
