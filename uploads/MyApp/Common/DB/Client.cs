using System.Text.RegularExpressions;
namespace MyApp.Common;

public class Client
{
    public int ClientID { get; set; }
    private const int FIOLen = 250;
    private const string FIOPattern = @"^[А-ЯЁ][а-яё-]+ [А-ЯЁ][а-яё-]+(?: [А-ЯЁ][а-яё]+)?$";
    private const string PhonePattern = @"^\d(11)$";
    private string _fio = string.Empty;
    private string _phone = string.Empty;
    public string FIO
    {
        get
        {
            return _fio;
        }
        set
        {
            if (value.Length > FIOLen)
            {
                throw new ArgumentException($"FIO must be shorten than {FIOLen}!");
            }
            else
            {
                if (Regex.IsMatch(value, FIOPattern))
                {
                    _fio = value;
                }
                else
                {
                    throw new ArgumentException($"FIO format error!");
                }
            }
        }
    }
    public string Phone
    {
        get
        {
            return _phone;
        }
        set
        {
            if (Regex.IsMatch(value, PhonePattern))
            {
                _phone = value;
            }
            else
            {
                throw new ArgumentException($"Phone format error!");
            }
        }
    }

}