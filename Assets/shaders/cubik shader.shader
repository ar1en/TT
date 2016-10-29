Shader "Cubik shader"
{
	Properties
	{
		_Color1("Main color", Color) = (1,1,1,1)
		_Color2("sec color", Color) = (1,1,1,1)
		_Color3("in faces", Color) = (1,1,1,1)
		_Color4("out faces", Color) = (1,1,1,1)
		_Power1("Pw main", float) = 1.6
		_Power2("Pw in faces", Float) = 1.5
		_Power3("Pw out faces", Float) = 1.7
		_Power4("Pw common", Float) = 8
		_1stTex("AlphaMap", 2D) = "white"
	}

		SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
#pragma surface surf Lambert

		sampler2D _1stTex;
	float4 _Color1, _Color2, _Color3, _Color4;
	float _Power1, _Power2, _Power3, _Power4, _AlphaPow;

	struct Input
	{
		float2 uv_1stTex;
	};

	//sampler2D _AlphaMap;

	void surf(Input IN, inout SurfaceOutput o)
	{
		fixed2 scrolledUV1 = IN.uv_1stTex;
		fixed xscroll1 = scrolledUV1.x;


		_AlphaPow = tex2D(_1stTex, IN.uv_1stTex).a;

		if (scrolledUV1.y < 0.5)
			o.Albedo = _Color4; //внешние грани
		else if (scrolledUV1.y > 0.5 && scrolledUV1.y < 0.7)
			o.Albedo = _Color3; // внутренние грани
		else if (scrolledUV1.y > 0.7)
			o.Emission = _Color1; // основна грань

	}
	ENDCG
	}
		FallBack "Diffuse"
}
