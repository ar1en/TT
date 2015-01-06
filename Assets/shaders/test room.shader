Shader "room"
	{
	Properties 
	{
		_Color1 ("Area", Color) = (1,1,1,1)
		_Color2 ("Signal", Color) = (1,1,1,1)
		_Color3 ("Diffuse", Color) = (1,1,1,1)
		_Delta1 ("Smeshenie", range (0,1)) = 0.5
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
		float i, _Delta1;
		
		
		
		  
		struct Input 
		{
			float2 uv_MainTex;
		};	
					
		void surf (Input IN, inout SurfaceOutput o) 
		{
			fixed2 coordUV = IN.uv_MainTex;
			fixed xcoord1 = coordUV.x;
			fixed ycoord1 = coordUV.y;
			fixed xcoord2 = coordUV.x;
			fixed ycoord2 = coordUV.y;
			xcoord1 += _Delta1;
			ycoord1 += 0.5;
			xcoord2 += 0.1;
			ycoord2 += 0.2;
			
			//if (coordUV.x < 0.7)
			//	{
			//	if (coordUV.x > 0.3 && coordUV.x < 0.7) 
			//	o.Albedo = tex2D (_MainTex, fixed2(xcoord1,ycoord1));
			//	else o.Albedo = tex2D (_MainTex, fixed2(xcoord2,ycoord2));
			//	}
			//	else o.Albedo = _Color2.rgb;
			o.Alpha = _Color3.a;
				}
		ENDCG
	} 
	FallBack "Diffuse"
}
