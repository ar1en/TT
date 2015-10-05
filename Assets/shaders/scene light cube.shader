Shader "Scene light Cubik"
{
	Properties
	{
		_Color1("Light", Color) = (1,1,1,1)
		_Power1("Power", Range(0,2)) = 1
		_MainTex("Base (RGB)", 2D) = "white" {}
	}
		SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
#pragma surface surf Lambert

		sampler2D _MainTex;
	float4 _Color1;
	float _Power1, wer;

	struct Input
	{
		float2 uv_MainTex;
	};

	void surf(Input IN, inout SurfaceOutput o)
	{
		fixed2 scrolledUV = IN.uv_MainTex;
		fixed xscroll = scrolledUV.x;
		fixed yscroll = scrolledUV.y;

		half4 c = tex2D(_MainTex, half2(scrolledUV.x, scrolledUV.y));
		wer = c.r;

		o.Emission = _Color1 * _Power1;
		//o.Albedo = c.rgb;
		o.Alpha = c.a;
	}
	ENDCG
	}
		FallBack "Diffuse"
}
