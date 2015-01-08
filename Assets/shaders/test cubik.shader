Shader "Cubik test"
	{
	Properties 
	{
		_Color1 ("Area", Color) = (1,1,1,1)
		_Color2 ("Signal", Color) = (1,1,1,1)
		_Color3 ("Diffuse", Color) = (1,1,1,1)
		//_Power1 ("Power", Float) = 1
		_Trans ("Transparent", Range(0,1)) = 0.5
		_MainTex ("Base (RGB)", 2D) = "white" {}	
	}
	SubShader 
	{
		Tags { "Queue"="Transparent" "RenderType"="Transparent"}
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert alpha

		sampler2D _MainTex;
		float4 _Color1, _Color2, _Color3;
		float _Trans;
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
					o.Alpha = 1;
					}
				else {
					  o.Albedo = _Color3.rgb;
					  o.Alpha = _Trans;
					  }
			//o.Alpha = c.a;
				}
		ENDCG
	} 
	FallBack "Diffuse"
}
