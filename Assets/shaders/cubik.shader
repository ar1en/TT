Shader "Cubik"
	{
	Properties 
	{
		_Color1 ("Area", Color) = (1,1,1,1)
		_Color2 ("Signal", Color) = (1,1,1,1)
		_Color3 ("Diffuse", Color) = (1,1,1,1)
		//_Power1 ("Power", Float) = 1
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
		//int _Power1;
		  
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
							

			if (scrolledUV.x > 0.3) 
				{
				o.Albedo = _Color1.rgb;
				}
				else o.Albedo = _Color3.rgb;
			o.Alpha = c.a;
				}
		ENDCG
	} 
	FallBack "Diffuse"
}
