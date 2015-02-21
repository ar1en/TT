﻿Shader "Ramka new test"
	{
	Properties 
	{
		_Color1 ("FtColor", Color) = (1,1,1,1)
		_Color2 ("ScColor", Color) = (1,1,1,1)
		_Color3 ("Zona", Color) = (1,1,1,1)
		_Color4 ("Diffuse", Color) = (1,1,1,1)
		_Coord ("Coord", float) = 1
		_Counter ("_Counter", float) = 1
		_Power1 ("FtClPw", float) = 1
		_Power2 ("ScClPw", float) = 1
		_MainTex ("Base (RGB)", 2D) = "white" 
		_SecTex ("Base (RGB)", 2D) = "white" {}	
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex, _SecTex;
		float4 _Color1, _Color2, _Color3, _Color4;
		float coefsetki, alpTex1,_Power1, _Power2, a, _Counter, _Coord;
		  
		struct Input 
		{
			float2 uv_MainTex;
			float uv_SecTex;
		};	
									
		void surf (Input IN, inout SurfaceOutput o) 
		{
			float2 coordUV = IN.uv_MainTex;
			float xcoord = coordUV.x;
			float ycoord = coordUV.y;
			
			coefsetki = 0.05;
			ycoord -= (coefsetki*_Coord);																		
																																																									
			float4 c = tex2D (_MainTex, fixed2(xcoord,ycoord));

			alpTex1 = c.a;
			
			if (coordUV.x < 0.6)
			{				
				o.Albedo = _Color3.rgb;
			}
			
			if (coordUV.x > 0.6 && coordUV.x < 0.7)
			{				
				o.Emission = _Color2.rgb * 1.5;
			}
			
			if (coordUV.x > 0.7)
			{				
				o.Emission =  lerp((_Color1.rgb * alpTex1 * _Power1), (_Color2.rgb * alpTex1 * _Power1), (_Counter * 0.0083));
			}

		}
		ENDCG
	} 
	FallBack "Diffuse"
}
