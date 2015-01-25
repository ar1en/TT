Shader "Ramka new test"
	{
	Properties 
	{
		_Color1 ("Area", Color) = (1,1,1,1)
		_Color2 ("Cube zone", Color) = (1,1,1,1)
		_Color3 ("VneshGrani", Color) = (1,1,1,1)
		_Color4 ("Sled Cubik", Color) = (1,1,1,1)
		_Coord ("Coord", float) = 1
		_Power1 ("Area", float) = 1
		_Power2 ("Cube", float) = 1
		_Power3 ("VneshGrani Pw", float) = 1
		_Power4 ("Power4", float) = 1
		_Raznica ("Raznica", float) = 1
		_LerpStep ("LerpStep", float) = 1
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
		float coefsetki, alpTex1, alpTex2, _Coord,ti,
			  _Power1, _Power2, _Power3, _Power4, _LerpStep, _Raznica;
		  
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
			
		//	float2 coordUVSc = IN.uv_SecTex;
		//	float xcoordSc = coordUVSc.x;
		//	float ycoordSc = coordUVSc.y;
			
			
			
		coefsetki = 0.038;
			
		//	ti = frac (_Time);
			
			
			//ycoordSc += 0.5;
			ycoord += 0.5;
			//ycoordSc -= (coefsetki * _Coord);
			ycoord -= (coefsetki * _Coord);
						
			float4 c = tex2D (_MainTex, fixed2(xcoord,ycoord));
			float4 d = tex2D (_SecTex, fixed2(xcoord,ycoord));
			
			alpTex1 = c.a;
			alpTex2 = d.a;
			
		if (coordUV.x > 0.55 && coordUV.y > 0.05)
		{				
			if (coordUV.y > 0.85)  o.Emission = lerp (_Color2.rgb, _Color4.rgb, 0.5);
			else o.Emission = _Color2.rgb * 2 * 1/exp(_Power4*alpTex2);
		}
		else o.Albedo = _Color3.rgb * _Power3;
			 o.Alpha = c.a;
			 
		//			if (coordUV.x > 0.55 && coordUV.y > 0.05)
	//	{
	///		if ((alpTex1 - 0.8*alpTex2) > abs(_Raznica))
	//			o.Emission = lerp ((_Color2.rgb * _Power4 * alpTex1), (_Color1.rgb * _Power1 * alpTex2), 1/(1*exp(alpTex1 - alpTex2)+0.001));
	//		else
			//	o.Emission = lerp ((_Color2.rgb * _Power4 * alpTex1), (_Color1.rgb * _Power1 * alpTex2), 0.7);	
	//		o.Emission = _Color1.rgb * _Power1 * alpTex2;
	//	}
	//	else o.Albedo = _Color3.rgb * _Power3;
    //o.Alpha = c.a; 
			 
		//if (coordUV.y > (0.1 + (_Coord * coefsetki)) && coordUV.y < (0.1 + (_Coord * coefsetki) + coefsetki))
		//		o.Emission = lerp ((_Color2 * _Power1), _Color3, _LerpStep);
		
			//else o.Albedo = _Color1.rgb * _Power3; 
		//	if ((alpTex1 - alpTex2) > _Raznica)
		//		o.Emission = lerp ((_Color2.rgb * _Power4 * alpTex1), (_Color1.rgb * _Power1 * alpTex2), frac (()));
		//	else
		//		o.Emission = lerp ((_Color2.rgb * _Power2 * alpTex1), (_Color1.rgb * _Power1 * alpTex2), _LerpStep);
			
			
			
			
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
