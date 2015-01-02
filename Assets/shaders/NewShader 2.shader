Shader "Custom/NewShader" 
	{
	Properties 
	{
		_Color12 ("Supcolor", Color) = (1,1,1,1)
		_Color13 ("Areacolor", Color) = (1,1,1,1)
		_Color14 ("14", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}	
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		float4 _Color12, _Color13, _Color14;
		float i;
		  
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
						
			i = 4*frac(2*_Time); 	

			if (scrolledUV.x > 0.5 && scrolledUV.y < 0.5) 
						{
						if (scrolledUV.x > (0.58+i*0.1) && scrolledUV.x < (0.6+i*0.1)) 
							o.Albedo = pow(_Color12, 2);
							else o.Albedo = _Color13.rgb;
						}
				else o.Emission = pow(_Color14.rgb, 0.5);
			o.Alpha = c.a;
				}
		ENDCG
	} 
	FallBack "Diffuse"
}
