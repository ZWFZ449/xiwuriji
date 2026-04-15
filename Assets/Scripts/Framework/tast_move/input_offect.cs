using MVC;
using TMPro;
using UnityEngine.UI;

public class input_offect : Base_Mono
{
    private InputField inputField;

    private TMP_Text info, title;

    private Button close;

    private Button confirm;
    private void Awake()
    {
        inputField =Find<InputField>("bg/InputField");
        info = Find<TMP_Text>("bg/info");
        title = Find<TMP_Text>("bg/title");
        close = Find<Button>("close_button");
        close.onClick.AddListener(delegate {gameObject.SetActive(false); });
        confirm = Find<Button>("bg/confirm");
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="_title"></param>
    /// <param name="_info"></param>
    public void Init(object _title, string _info)
    {
        title.text= _title.ToString();
        info.text = _info;
    }
    /// <summary>
    /// 返回按键
    /// </summary>
    public Button GetConfirm { get { return confirm; } }
    /// <summary>
    /// 输入内容
    /// </summary>
    public string GetInput { get { return Tool_Battle.IsValidString( inputField.text)? inputField.text : "" ; } }

}
