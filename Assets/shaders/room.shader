Shader "room"
	{
	Properties 
	{
		_Color1 ("Diffuse", Color) = (1,1,1,1)
		_Color2 ("Signal", Color) = (1,1,1,1)
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
			fixed xcoord = coordUV.x;
			fixed ycoord = coordUV.y;
			
			float4 c = tex2D (_MainTex,coordUV);
			
			o.Albedo = c.rgb;
			o.Alpha = _Color1.a;
				}
		ENDCG
	} 
	FallBack "Diffuse"
}
