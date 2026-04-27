using Common;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource audioSource;
    List<string> ClipList = new List<string>() { "攻击敌人", "被敌人攻击","敌人攻击", "敌人被攻击","回复血量","回城","使用物品",
            "购买物品","男角色死亡","女角色死亡","释放雷电术","风呼啸声","获得称号"};//单次音频文件名字
    static Dictionary<string, AudioClip> BGMdic = new Dictionary<string, AudioClip>();
    static Dictionary<string, AudioClip> Clipdic = new Dictionary<string, AudioClip>();
    public int index;//音频文件索引
    List<string> bgmName = new List<string>() { "暗域", "村子外", "伏妖塔", "古墓", "机关洞", "将军坟", "禁地", "开启", "矿洞", "逆魔古刹",
    "沙漠城","蛇洞","通天塔","妖神界","银杏村","中州城"}; //bgm名字; //bgm 地图名字、与音频文件名字相同，修改需同步修改
    private void Awake()
    {
        Instance = this.GetComponent<AudioManager>();
        if (Clipdic.Count == 0) StartCoroutine("GetClip");

    }
    void Start()
    {
        audioSource = this.gameObject.GetComponent<AudioSource>();
    }




    public IEnumerator GetClip()
    {
        foreach (var item in ClipList)
        {
            
            ResourceRequest re = Resources.LoadAsync<AudioClip>($"AudioClip/{item.ToString()}");
            Clipdic.Add(item.ToString(), re.asset as AudioClip);
        }
        return null;
    }
    public IEnumerator GetBGM()
    {
        if (File.Exists(Application.dataPath + "/Resources/AudioClip/BGM/" + BGMtempName + ".wav"))
        {
            ResourceRequest re = Resources.LoadAsync<AudioClip>($"AudioClip/BGM/{BGMtempName}");
            BGMdic.Add(BGMtempName, re.asset as AudioClip);
        }
        return null;
    }
    /// <summary>
    /// 播放音效（一次）
    /// </summary>
    /// <param name="s"></param>
    public void playAudio(ClipEnum clip)
    {
        if (SumSave.crt_setting.user_data_settings.Count >= 1 && SumSave.crt_setting.user_data_settings[0] == 0) return;
        if (Clipdic.ContainsKey(clip.ToString())) { audioSource.PlayOneShot(Clipdic[clip.ToString()]); return; }
        else StartCoroutine("GetClip");

    }
    string BGMtempName;//调用背景播放方法提供的参数
    /// <summary>
    /// 改变背景音乐
    /// </summary>
    /// <param name="clip"></param>
    public void ChangeBGM(BGMenum bg)
    {
        if (SumSave.crt_setting.user_data_settings.Count >= 1 && SumSave.crt_setting.user_data_settings[0] == 0) return;
        if (BGMdic.ContainsKey(bg.ToString())) { audioSource.clip = BGMdic[bg.ToString()]; Debug.Log("没有音效库"); return; }
        else
        {
            BGMtempName = bg.ToString();
            StartCoroutine("GetBGM");
        }

    }/// <summary>
     /// 背景音乐是否静音
     /// </summary>
     /// <param name="isPlay"></param>
    public void isMuteBGM(bool isPlay)
    {
        if (audioSource == null) return;
        audioSource.mute = isPlay;
    }
    /// <summary>
    /// 单次反转bgm静音状态
    /// </summary>
    /// <param name="isPlay"></param>
    public void isMuteBGMOne()
    {
        if (audioSource == null) return;
        audioSource.mute = !audioSource.mute;
    }
    /// <summary>
    /// 是否仅暂停背景音乐（可重新播放）
    /// </summary>
    /// <param name="isPlay"></param>
    public void isPlayBGM(bool isPlay)
    {
        if (audioSource == null) return;
        if (isPlay) audioSource.UnPause();
        else audioSource.Pause();
    }/// <summary>
     /// 是否播放背景音乐(结束或从头开始)
     /// </summary>
     /// <param name="isPlay"></param>
    public void isDieBGM(bool isPlay)
    {
        if (audioSource == null) return;
        if (isPlay) audioSource.Play();
        else audioSource.Stop();
    }
}
public enum BGMenum
{
    暗域, 村子外, 伏妖塔, 古墓, 机关洞, 将军坟,
    禁地, 开启, 矿洞, 逆魔古刹,
    沙漠城, 蛇洞, 通天塔, 妖神界, 银杏村, 中州城
}
public enum ClipEnum
{
    攻击敌人, 被敌人攻击, 敌人攻击, 回复血量, 回城, 使用物品, 购买物品, 男角色死亡, 女角色死亡, 释放雷电术, 风呼啸声, 获得称号
}
