Shader "UI/CircularFill"
{
    Properties
    {
        // 基础纹理，这里放你的圆环图片
        _MainTex("Texture", 2D) = "white" {}
    // 填充比例，范围0到1，将由脚本控制
    _FillAmount("Fill Amount", Range(0, 1)) = 1
        // 血条/进度条颜色
        _Color("Color", Color) = (1, 1, 1, 1)
        // (可选) 背景颜色，当图片部分透明时，可以看到这个颜色
        _BackgroundColor("Background Color", Color) = (0.5, 0.5, 0.5, 1)
        // (可选) 起始角度偏移（例如让进度从顶部开始）
        _StartAngle("Start Angle", Range(0, 360)) = 0
    }
        SubShader
    {
        // UI Shader常用的Tags
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }

        // 渲染透明物体需要开启混合
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            fixed4 _BackgroundColor;
            float _FillAmount;
            float _StartAngle;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // 将UV坐标从(0,1)转换到中心点(-1,1)的坐标系
                float2 center = float2(0.5, 0.5);
                float2 pos = i.uv - center;

                // 计算当前片段相对于中心点的角度（0到360度）
                // atan2(y, x) 返回弧度值，需要转换为角度
                float angle = atan2(pos.y, pos.x) * (180 / 3.14159265359);
                // 将角度从(-180, 180]映射到(0, 360]
                angle = (angle < 0) ? angle + 360 : angle;

                // 应用起始角度偏移
                angle = fmod(angle + 360 - _StartAngle, 360);

                // 核心逻辑：将角度转换为比例（0到1），并与_FillAmount比较
                // 如果当前角度比例大于填充量，则显示背景色（或透明）
                float angleFraction = angle / 360.0;
                if (angleFraction > _FillAmount)
                {
                    // 显示背景色
                    fixed4 bgCol = _BackgroundColor;
                    // 同时采样纹理的alpha通道，让背景也受原图透明通道影响
                    bgCol.a *= tex2D(_MainTex, i.uv).a;
                    return bgCol;
                }
                else
                {
                    // 显示前景色（血条颜色）
                    fixed4 col = tex2D(_MainTex, i.uv);
                    col *= _Color; // 乘以颜色属性，可以方便地改变血条颜色
                    return col;
                }
            }
            ENDCG
        }
    }
        // 如果上面Shader不支持，则使用回退Shader（可选）
                Fallback "UI/Default"
}