Shader "Ramka new test"
	{
	Properties 
	{
		_Color1 ("Area", Color) = (1,1,1,1)
		_Color2 ("Zona", Color) = (1,1,1,1)
		_Color3 ("VneshGrani", Color) = (1,1,1,1)
		_Coord ("Coord", float) = 1
		_Power2 ("Power2", float) = 1
		_MainTex ("Base (RGB)", 2D) = "white" {}	
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		float4 _Color1, _Color2, _Color3;
		float coefsetki, i, _Coord, _Power2;
		  
		struct Input 
		{
			float2 uv_MainTex;
		};	
									
		void surf (Input IN, inout SurfaceOutput o) 
		{
			fixed2 coordUV = IN.uv_MainTex;
			fixed xcoord = coordUV.x;
			fixed ycoord = coordUV.y;
			
		coefsetki = 0.036;
			
			float4 c = tex2D (_MainTex, fixed2(xcoord,ycoord));
		
		if (coordUV.x > 0.5)
		{
			if (coordUV.y > (0.1 + (_Coord * coefsetki)) && coordUV.y < (0.1 + (_Coord * coefsetki) + coefsetki))
				o.Emission = _Color2.rgb;
			else o.Emission = _Color1.rgb;
		}
		else o.Emission = _Color3.rgb * _Power2;
			 o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
