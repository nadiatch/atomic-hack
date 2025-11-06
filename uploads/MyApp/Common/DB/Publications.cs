using System.Text.RegularExpressions;
namespace MyApp.Common;

public class Publication
{
    public int PublicationID { get; set; }
    private const int NameLen = 100;
    private const string NamePattern = @"^[А-ЯЁ][а-яё]+$";
    private string _name = string.Empty;
    public string Name
    {
        get
        {
            return _name;
        }
        set
        {
            if (value.Length > NameLen)
            {
                throw new ArgumentException($"Name must be shorten than {NameLen}!");
            }
            else
            {
                if (Regex.IsMatch(value, NamePattern))
                {
                    _name = value;
                }
                else
                {
                    throw new ArgumentException($"Name format error!");
                }
            }
        }
    }
}