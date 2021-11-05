using UnityEngine.UI;

public class RegisterManager : BoardManager
{
    public InputField _account;
    public InputField _password;
    public InputField _passwordCheck;
    public Toggle _passwordToggle;
    public Toggle _passwordCheckToggle;
    public Text notice;


    void Start()
    {
        base.Start();
        Initialize();
    }

    
    
    public void OnPasswordCheckToggleChange()
    {
        SwitchContext(_passwordCheck, _passwordCheckToggle);
    }

    
    
    public void OnPasswordToggleChange()
    {
        SwitchContext(_password, _passwordToggle);
    }


    /**
     * 初始化密码为密文状态
     */
    private void Initialize()
    {
        _password.contentType = InputField.ContentType.Password;
        _passwordCheck.contentType = InputField.ContentType.Password;
        notice.text = "";
    }

    /**
     * 通过toggle切换field为密文/明文
     */
    private static void SwitchContext(InputField field, Toggle toggle)
    {
        field.contentType = toggle.isOn? InputField.ContentType.Standard : InputField.ContentType.Password;
        field.ForceLabelUpdate();
    }

}