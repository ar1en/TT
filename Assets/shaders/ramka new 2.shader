Shader "Ramka new 2"
	{
	Properties 
	{
		_CurrentColor ("CurrentColor", Color) = (1,1,1,1)
		_NextColor ("NextColor", Color) = (1,1,1,1)
		_Color3 ("Diffuse", Color) = (1,1,1,1)
		_Coord ("Coord", float) = 1
		_coordSc ("coordSc", float) = 1
		_ColorChangeCounter ("Counter", float) = 1
		_Power1 ("FtClPw", float) = 1
		_Power2 ("ScClPw", float) = 1
		_Power3 ("ZonePW", float) = 1
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
		float4 _CurrentColor, _NextColor, _Color3;
		float coefsetki, alpTex1, alpTex2,_Power1, _Power2, _Power3, _Step, _Coord, _coordSc, _coordTh, _ColorChangeCounter;
		  
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
				o.Emission = lerp(_CurrentColor.rgb * _Power3, _NextColor.rgb * _Power3, _ColorChangeCounter);
			}
			
			if (coordUV.x > 0.5 && coordUV.x < 0.8)
			{				
				o.Emission = lerp(_CurrentColor.rgb * pow(alpTex1, _Step) * _Power3, 
				_NextColor.rgb * pow(alpTex1, _Step) * _Power3, _ColorChangeCounter);
			}
			
			if (coordUV.x > 0.8)
			{				
				o.Emission =  lerp(_CurrentColor.rgb * alpTex1 * _Power1, _NextColor.rgb * alpTex1 * _Power1, _ColorChangeCounter);
			}
			
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
