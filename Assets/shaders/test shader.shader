Shader "test shader"
{
	Properties
	{
		_MainColorCube("Main color", Color) = (1,1,1,1) // Основной цвет куба, зависящий от типа фигуры
		_InCubeColor("In Face", Color) = (1,1,1,1) // Цвет внутренних граней куба
		_OutCubeFaceColor("Out Faces", Color) = (1,1,1,1) // Цвет внешних граней куба
		_MainColorCubePw("Pw main", float) = 1 // коэффициент умножения для основного цвета куба
		_InCubeFaceColorPw("Pw in faces", Float) = 1 // коэффициент умножения для цвета внутренних граней
		_OutCubeFaceColorPw("Pw out faces", Float) = 1 // коэффициент умножения для цвета внешний граней
		_CubePower1("Cmn pow 1", Float) = 1 // свободный коэффициент 1 только для шейдера куба
		_CubePower2("Cmn pow 2", Float) = 1 // свободный коэффициент 2 только для шейдера куба
		_CubeColorMap("Cube Color Map", 2D) = "white" // текстура цветовой карты для шейдера кубика
		_CubeAlphaMap("Cube Alpha Map", 2D) = "white" // карта альфа канала для шейдера кубика
	}

		SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
#pragma surface surf Lambert

		sampler2D _CubeColorMap, _CubeAlphaMap;
	float4 _MainColorCube, _InCubeColor, _OutCubeFaceColor, _ColorfromMap;
	float _MainColorCubePw, _OutCubeFaceColorPw, _InCubeFaceColorPw, _CubePower1, _AlphaPow;

	struct Input
	{
		float2 uv_CubeAlphaMap;
	};

	void surf(Input IN, inout SurfaceOutput o)
	{
		fixed2 scrolledUV1 = IN.uv_CubeAlphaMap;
		fixed xscroll1 = scrolledUV1.x;


		_ColorfromMap = tex2D(_CubeColorMap, IN.uv_CubeAlphaMap);
		_AlphaPow = tex2D(_CubeAlphaMap, IN.uv_CubeAlphaMap).a;

		o.Albedo = _ColorfromMap * _OutCubeFaceColorPw; //внешние грани

	}
	ENDCG
	}
		FallBack "Diffuse"
}