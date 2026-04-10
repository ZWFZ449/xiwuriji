using MVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class panl_particle : Base_Mono
{
    /// <summary>
    /// 粒子组件
    /// </summary>
    private ParticleSystem rainSystem;
    /// <summary>
    /// 控制粒子行为的基本参数
    /// </summary>
    private ParticleSystem.MainModule main;
    /// <summary>
    /// 控制粒子发射行为的模块
    /// </summary>
    private ParticleSystem.EmissionModule emission;
    /// <summary>
    /// 定义粒子发射器形状的模块
    /// </summary>
    private ParticleSystem.ShapeModule shape; 
    private void Awake()
    {
        rainSystem = Find<ParticleSystem>("Particle System");
        main = rainSystem.main;
        emission = rainSystem.emission;
        shape = rainSystem.shape;
        RainEffect();
    }


    public void RainEffect()
    {
        //初始速度
        main.startSpeed = 15f;
        //生命周期
        main.startLifetime = 100f;
        //初始大小
        main.startSize =10f;
        //重力影响
        main.gravityModifier = 0.1f;
        ///随时间发射速率
        emission.rateOverTime = 1000;
       //发射器形状类型：球体、半球体、圆锥体等
        shape.shapeType = ParticleSystemShapeType.Cone;
        //角度，用于圆锥形发射器
        shape.angle = 25f;
    }



}
