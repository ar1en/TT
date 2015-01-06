Shader "Ramka"
	{
	Properties 
	{
		_Color1 ("T", Color) = (1,1,1,1)
		_Color2 ("E", Color) = (1,1,1,1)
		_Color3 ("T", Color) = (1,1,1,1)
		_Color4 ("R", Color) = (1,1,1,1)
		_Color5 ("I", Color) = (1,1,1,1)
		_Color6 ("S", Color) = (1,1,1,1)
		_Color7 ("Diffise", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}	
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		float4 _Color1, _Color2, _Color3,_Color4, _Color5, _Color6, _Color7;
		  
		struct Input 
		{
			float2 uv_MainTex;
		};	
					
		void surf (Input IN, inout SurfaceOutput o) 
		{
			fixed2 scrolledUV = IN.uv_MainTex;
			fixed xscroll = scrolledUV.x;
			fixed yscroll = scrolledUV.y;
			
			half4 c = tex2D (_MainTex, fixed2(xscroll,yscroll));
							

			if (scrolledUV.x < 0.1) o.Emission = _Color1.rgb;
				else o.Albedo = _Color7.rgb;
			if (scrolledUV.x > 0.1 && scrolledUV.x < 0.2) o.Emission = _Color2.rgb;
				else o.Albedo = _Color7.rgb;
			if (scrolledUV.x > 0.2 && scrolledUV.x < 0.3) o.Emission = _Color3.rgb;
				else o.Albedo = _Color7.rgb;
			if (scrolledUV.x > 0.3 && scrolledUV.x < 0.4) o.Emission = _Color4.rgb;
				else o.Albedo = _Color7.rgb;
			if (scrolledUV.x > 0.4 && scrolledUV.x < 0.5) o.Emission = _Color5.rgb;
				else o.Albedo = _Color7.rgb;
			if (scrolledUV.x > 0.5 && scrolledUV.x < 0.6) o.Emission = _Color6.rgb;
				else o.Albedo = _Color7.rgb;
			o.Alpha = c.a;
				}
		ENDCG
	} 
	FallBack "Diffuse"
}
