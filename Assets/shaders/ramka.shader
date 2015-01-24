Shader "Ramka"
	{
	Properties 
	{
		_Color1 ("Area", Color) = (1,1,1,1)
		_Color2 ("Diffuse", Color) = (1,1,1,1)
		_Power1 ("Power", float) = 1
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
		float4 _Color1, _Color2;
		float i, _Power1, _Power2;
		  
		struct Input 
		{
			float2 uv_MainTex;
		};	
									
		void surf (Input IN, inout SurfaceOutput o) 
		{
			fixed2 coordUV = IN.uv_MainTex;
			fixed xcoord = coordUV.x;
			fixed ycoord = coordUV.y;
			//float i= 0.5 + 0.5*sin(_Time.y);
			
			float4 c = tex2D (_MainTex, fixed2(xcoord,ycoord));
		
		if (coordUV.x > 0.6)
			{
			o.Emission = _Color1.rgb * _Power1;
			}
		else o.Emission = _Color2.rgb * _Power2;
			 o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
