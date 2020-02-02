Shader "Unlit/cellShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ShadowStrength ("Shadow Strength", Range(0, 1)) = 0.5
        _OutlineWidth ("Outline Width", Range(0, 0.1)) = 0.01
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
            "LightMode" = "ForwardBase"
            "PassFlags" = "OnlyDirectional"
        }
        LOD 100

        Pass
        {
            Cull Off
            Stencil
            {
                Ref 1
                Comp Always
                Pass Replace
            }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldNormal : NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _ShadowStrength;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Нормализуем вектор нормали, чтобы его длина равнялась 1
                float3 normal = normalize(i.worldNormal);
    
                // Считаем Dot Product для нормали и направления к источнику света
                // _WorldSpaceLightPos0 - встроенная переменная Unity
                float NdotL = dot(_WorldSpaceLightPos0, normal);
    
                // Cчитаем интенсивность света на поверхности
                // Если поверхность повернута к источнику света (NdotL > 0), 
                // то она полностью освещена.
                // В противном случае учитываем Shadow Strength для затенения
                float lightIntensity = NdotL > 0 ? 1 : _ShadowStrength;

                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                // Применяем затенение
                col *= lightIntensity;
                return col;
            }
            ENDCG
        }
        Pass
        {	
            // Скрываем полигоны, повернутые к камере
            Cull Off

            Stencil 
            {
                Ref 1
                Comp Greater
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            // Объявляем переменные
            half _OutlineWidth;
            static const half4 OUTLINE_COLOR = half4(0,0,0,0);

            v2f vert (appdata v)
            {
                // Конвертируем положение и нормаль вертекса в clip space
                float4 clipPosition = UnityObjectToClipPos(v.vertex);
                float3 clipNormal = mul((float3x3) UNITY_MATRIX_VP, mul((float3x3) UNITY_MATRIX_M, v.normal));
                            
                // Считаем смещение вершины по направлению нормали.
                // Также учитываем перспективное искажение и домножаем на компонент W,
                // чтобы сделать смещение постоянным,
                // вне зависимости от расстояния до камеры
                float2 offset = normalize(clipNormal.xy) * _OutlineWidth * clipPosition.w;
                            
                // Т.к. рассчет теперь ведется в пространстве экрана, 
                // надо учитывать соотношение сторон
                // и сделать толщину контура постоянной при любом aspect ratio.
                // _ScreenParams - встроенная переменная Unity
                float aspect = _ScreenParams.x / _ScreenParams.y;
                offset.y *= aspect;
                            
                // Применяем смещение
                clipPosition.xy += offset;
                            
                v2f o;
                o.vertex = clipPosition;
                            
                return o;
            }

            fixed4 frag () : SV_Target
            {
                // Все пиксели контура имеют один и тот же цвет
                return OUTLINE_COLOR;
            }
            ENDCG
        }
    }
}
