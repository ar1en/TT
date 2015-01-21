﻿Shader "Cubik universal"
	{
	Properties 
	{
		_Color1 ("Main color", Color) = (1,1,1,1)
		_Power1 ("Power1", Range (0,2)) = 1
		_Power2 ("Power2", Range (0,2)) = 1
		_LerpSt ("Lerp Step", Range (0,1)) = 0.5
		_MainTex ("Base (RGB)", 2D) = "white"
		_LightMap ("Base (RGB)", 2D) = "white" {}	
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex, _LightMap;
		float4 _Color1, _ClrMnBld;
		float _Power1, _Power2, alpCtex, _LerpSt;
		  
		struct Input 
		{
			float2 uv_MainTex;
		};	
					
		void surf (Input IN, inout SurfaceOutput o) 
		{
			fixed2 scrolledUV = IN.uv_MainTex;
			fixed xscroll = scrolledUV.x;
			fixed yscroll = scrolledUV.y;
			
			half4 c = tex2D (_MainTex, half2(scrolledUV.x, scrolledUV.y));
			half4 d = tex2D (_LightMap, half2(scrolledUV.x, scrolledUV.y));
			alpCtex = c.a;
			_ClrMnBld = alpCtex * _Color1 * _Power1;
			
			if (scrolledUV.y > 0.5 && scrolledUV.x < 0.5) 
				{
				o.Emission = lerp(_ClrMnBld, d, _LerpSt);
				}
				else o.Albedo = lerp(c.rgb, d, _LerpSt) * _Power2;
			o.Alpha = c.a;
				}
		ENDCG
	} 
	FallBack "Diffuse"
}
