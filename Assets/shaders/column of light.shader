Shader "column of light"  
{
	Properties{
		_MainColor11("Color", color) = (1,1,1,0)
		_Trans("Transparent", Range(0,1)) = 0.5
		_ColumnAlphaMap("Cube Alpha Map", 2D) = "white" // карта альфа канала для шейдера кубика
		_ColumnAlphaAnimMap("Cube Alpha Map", 2D) = "white" // карта альфа канала для шейдера кубика
	}
		SubShader{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }

		LOD 200

		CGPROGRAM
#pragma surface surf Lambert alpha

	sampler2D _ColumnAlphaMap, _ColumnAlphaAnimMap;
	float4 _MainColor11;
	float _Trans, _AlphaCorrection, _AlphaAnimCorrection;


	struct Input
	{
		float2 uv_ColumnAlphaMap;
		float2 uv_ColumnAlphaAnimMap;
	};

	void surf(Input IN, inout SurfaceOutput o)
	{

		fixed2 scrolledUV1 = IN.uv_ColumnAlphaAnimMap;
		fixed yscroll = scrolledUV1.y * _SinTime;
		
		_AlphaCorrection = tex2D(_ColumnAlphaMap, IN.uv_ColumnAlphaMap).a;
		_AlphaAnimCorrection = tex2D(_ColumnAlphaAnimMap, IN.uv_ColumnAlphaAnimMap * frac(_Time/10)).a;
		o.Emission = _MainColor11.rgb;
		o.Alpha = _Trans * _AlphaCorrection * (1/(_AlphaAnimCorrection + 0.9));
	}
	ENDCG
	}
		FallBack "Diffuse"
}