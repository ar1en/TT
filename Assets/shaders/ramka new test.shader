Shader "Ramka new test"
	{
	Properties 
	{
		_Color1 ("Area", Color) = (1,1,1,1)
		_Color2 ("Area 2", Color) = (1,1,1,1)
		_Color3 ("VneshGrani", Color) = (1,1,1,1)
		_Color4 ("Sled Cubik", Color) = (1,1,1,1)
		_Coord ("Coord", float) = 1
		_Counter ("_Counter", float) = 1
		_fallspcount ("_fallspcount", float) = 1
		_Power1 ("Main", float) = 1
		_Power2 ("Cube", float) = 1
	//	_Power3 ("VneshGrani Pw", float) = 1
	//	_Power4 ("Power4", float) = 1
	//	_Raznica ("Raznica", float) = 1
	//	_LerpStep ("LerpStep", float) = 1
		_MainTex ("Base (RGB)", 2D) = "white" 
	//	_SecTex ("Base (RGB)", 2D) = "white" {}	
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex, _SecTex;
		float4 _Color1, _Color2, _Color3, _Color4;
		float coefsetki, alpTex1,_Power1, _Power2,a, _Counter, fallspcount;
		  
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
			o.Emission =  lerp((_Color4.rgb * alpTex1 * _Power1), (_Color1.rgb * alpTex1 * _Power1), (_Counter * 0.0083));
		}

		}
		ENDCG
	} 
	FallBack "Diffuse"
}
