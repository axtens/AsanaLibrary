using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

public class INI
{
    public string path;

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool WritePrivateProfileString(
        string lpAppName,
        string lpKeyName,
        string lpString,
        string lpFileName);

    // [DllImport("kernel32", CharSet = CharSet.Unicode)]
    // private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    static extern uint GetPrivateProfileString(
        string lpAppName,
        string lpKeyName,
        string lpDefault,
        StringBuilder lpReturnedString,
        uint nSize,
        string lpFileName);
    //[DllImport("kernel32", CharSet = CharSet.Unicode)]
    //private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

    /// <summary>
    /// INIFile Constructor.
    /// </summary>
    /// <param name="INIPath"></param>
    public INI(string INIPath) => path = INIPath;

    /// <summary>
    /// Write Data to the INI File
    /// </summary>
    /// <param name="Section"></param>
    /// Section name
    /// <param name="Key"></param>
    /// Key Name
    /// <param name="Value"></param>
    /// Value Name
    public void IniWriteValue(string Section, string Key, string Value)
    {
        WritePrivateProfileString(Section, Key, Value, this.path);
    }

    /// <summary>
    /// Read Data Value From the Ini File
    /// </summary>
    /// <param name="Section"></param>
    /// <param name="Key"></param>
    /// <param name="Path"></param>
    /// <returns></returns>
    public string IniReadValue(string Section, string Key, string DefaultValue)
    {
        StringBuilder temp = new StringBuilder(1024);
        uint i = GetPrivateProfileString(Section, Key, "", temp, 1024, this.path);
        if (i != 0)
            return temp.ToString();
        else
            return DefaultValue;

    }
}
