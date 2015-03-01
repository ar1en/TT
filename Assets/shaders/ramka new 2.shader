﻿Shader "Ramka new 2"
	{
	Properties 
	{
		_Color1 ("FtColor", Color) = (1,1,1,1)
		_Color2 ("ScColor", Color) = (1,1,1,1)
		_Color3 ("Diffuse", Color) = (1,1,1,1)
		_Coord ("Coord", float) = 1
		_coordSc ("coordSc", float) = 1
		_Counter ("Counter", float) = 1
		_Power1 ("FtClPw", float) = 1
		_Power2 ("ScClPw", float) = 1
		_Power3 ("ZonePW", float) = 1
		_Step ("Step", float) = 1
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
		int flag;
		float3 _ColorLerp1, _ColorLerp2;
		float4 _Color1, _Color2, _Color3;
		float coefsetki, alpTex1, alpTex2,_Power1, _Power2, _Power3, 
			  _Step, _Counter, _Coord, _coordSc, _coordTh;
		  
		struct Input 
		{
			float2 uv_MainTex;
			float2 uv_SecTex;
		};	
									
		void surf (Input IN, inout SurfaceOutput o) 
		{
			float2 coordUV = IN.uv_MainTex;
			float xcoord = coordUV.x;
			float ycoord = coordUV.y;
			
			float2 coordUV2 = IN.uv_SecTex;
			float xcoord2 = coordUV2.x;
			float ycoord2 = coordUV2.y;
			
			coefsetki = 0.045;
	
			if (_coordTh == floor(_Coord)-1)
			{
				flag = 1;
			}
			else 
			{
				flag = 0;
			}
			
			if (flag == 0)
			{
				ycoord2 -= (coefsetki*_Coord);
			}
			else 
			{
				ycoord2 -= (coefsetki*_coordSc);
			}																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																								
																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																												
			float4 c = tex2D (_MainTex, fixed2(xcoord,ycoord));
			float4 d = tex2D (_SecTex, fixed2(xcoord2,ycoord2));
			
			alpTex1 = c.a;
			alpTex2 = d.a;
			
			if (coordUV.x < 0.2)
			{				
				o.Albedo = _Color3.rgb;
			}
			
			if (coordUV.x > 0.2 && coordUV.x < 0.5)
			{				
				o.Emission = lerp(_Color1.rgb * _Power3, _Color2.rgb * _Power3, _Counter * 0.0083);
			}
			
			if (coordUV.x > 0.5 && coordUV.x < 0.8)
			{				
				o.Emission = lerp(_Color1.rgb * pow(alpTex1, _Step) * _Power3, 
				_Color2.rgb * pow(alpTex1, _Step) * _Power3, _Counter * 0.0083);
			}
			
			if (coordUV.x > 0.8)
			{				
				o.Emission =  lerp(_Color1.rgb * (alpTex1+alpTex2) * _Power1, _Color2.rgb * (alpTex1+alpTex2) * _Power1, _Counter * 0.0083);
			}
			
			_coordTh = _Coord;
			
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
