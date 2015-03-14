﻿Shader "Ramka new 2"
	{
	Properties 
	{
		_CurrentColor ("CurrentColor", Color) = (1,1,1,1)
		_NextColor ("NextColor", Color) = (1,1,1,1)
		_DiffColor ("Diffuse", Color) = (1,1,1,1)
		_Coord ("Coord", float) = 1
		_ColorChangeCounter ("Counter", float) = 1
		_Power1 ("FtClPw", float) = 1
		_Power2 ("ScClPw", float) = 1
		_Power3 ("ZonePW", float) = 1
		_Step ("Lamp Step", float) = 1
		_Step2 ("Ramka Step", float) = 1
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
		float3 _ColorLerp1, _ColorLerp2;
		float4 _CurrColor, _CurrentColor, _NxColor, _NextColor, _DiffColor;
		float coefsetki, alpTex1, alpTex2, _Power1, _Power2, _Power3,
		_Step, _Step2, _Coord, _ColorChangeCounter;
		  
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
			
			ycoord2 -= _Coord * 0.045;
																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																															
			float4 c = tex2D (_MainTex, fixed2(xcoord,ycoord));
			float4 d = tex2D (_SecTex, fixed2(xcoord2,ycoord2));
			
			alpTex1 = c.a;
			alpTex2 = d.a;
			
			if (alpTex2 < 0.1)
			{
			_CurrColor = _CurrentColor;
			_NxColor = _NextColor;
			}
			else 
			{
			_CurrColor = lerp(_CurrentColor, (0.9,0.9,0.9,1), pow(alpTex2, 2));
			_NxColor = lerp(_NextColor, (0.9,0.9,0.9,1), pow(alpTex2, 2));
			}
			
			
			if (coordUV.x < 0.2)
			{				
				o.Albedo = _DiffColor.rgb;
			}
			
			if (coordUV.x > 0.2 && coordUV.x < 0.5)
			{				
				o.Emission = lerp(_CurrentColor.rgb * _Power3, _NextColor.rgb * _Power3, _ColorChangeCounter);
			}
			
			if (coordUV.x > 0.5 && coordUV.x < 0.8)
			{				
				o.Emission = lerp(_CurrColor.rgb * pow(alpTex1+alpTex2,_Step) * _Power1, 
				_NxColor.rgb * pow(alpTex1+alpTex2,_Step) * _Power1, _ColorChangeCounter);
			}
			
			if (coordUV.x > 0.8)
			{				
				o.Emission =  lerp(_CurrColor.rgb * pow(alpTex1+alpTex2,_Step2) * _Power2, 
				_NxColor.rgb * pow(alpTex1+alpTex2,_Step2) * _Power2, _ColorChangeCounter);
			}
			
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
