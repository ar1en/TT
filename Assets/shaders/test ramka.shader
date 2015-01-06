Shader "Ramka"
	{
	Properties 
	{
		_Color1 ("Area", Color) = (1,1,1,1)
		_Color2 ("Diffuse", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}	
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert
		#pragma target 3.0

		sampler2D _MainTex;
		float4 _Color1, _Color2;
		  
		struct Input 
		{
			float2 uv_MainTex;
		};	
									
		void surf (Input IN, inout SurfaceOutput o) 
		{
			fixed2 coordUV = IN.uv_MainTex;
			fixed xcoord = coordUV.x;
			fixed ycoord = coordUV.y;
		//	float aa, ycordscrl;
		//	int mass[10],i,cc;
		//	for (i = 1; i != 10; i++ ) mass[i] = i/10;
		//	aa = 0.01 + frac(_Time);
		//	ycordscrl = 0.5*frac(_Time);
			
		//	half4 c = tex2D (_MainTex, fixed2(xcoord,ycoord));
		//	for (cc = 1; cc != 100; cc++)
		//	{
		//	if (coordUV.x > 0.6 && coordUV.y < cc*0.01 + 0.01 && coordUV.y > cc*0.01) 
		//		{
		//		o.Emission = _Color1.rgb*cc*0.1;
		//		}
		//	else o.Albedo = _Color2.rgb;
		//	}
		//	cc = 0;
		//	o.Alpha = c.a;
				}
		ENDCG
	} 
	FallBack "Diffuse"
}
