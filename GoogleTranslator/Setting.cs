using System;
using System.Drawing;
using System.IO;

public class Setting
{
    public String FromLanguage { get; set; }
    public String ToLanguage { get; set; }
    public Point StartupLocationPoint { get; set; }

    public Setting()
    {
        FromLanguage = "tr";
        ToLanguage = "fa";
        StartupLocationPoint = new Point();
    }

    public Setting(String Data)
    {
        var lines = Data.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
        foreach (var line in lines)
        {
            var left = line.Split(':')[0];
            var right = line.Split(':')[1];
            Process(left, right);
        }
    }

    private void Process(string left, string right)
    {
        switch (left.ToLower())
        {
            case "position":
                SetPosition(right);
                break;
            case "tolanguage":
                SetToLanguagae(right);
                break;
            case "fromlanguage":
                SetFromLanguagae(right);
                break;
        }
    }

    public static Setting LoadLocalSetting()
    {
        try
        {
            var setting = File.ReadAllText("Setting.txt");
            return new Setting(setting);
        }
        catch
        {
            return new Setting();
        }
    }

    public static void SaveLocalSetting(Setting setting)
    {
        File.WriteAllText("Setting.txt", setting);
    }

    private void SetFromLanguagae(string right)
    {
        FromLanguage = right.Trim();
    }

    private void SetToLanguagae(string right)
    {
        ToLanguage = right.Trim();
    }

    private void SetPosition(string right)
    {
        StartupLocationPoint = new Point(int.Parse(right.Split(',')[0]), int.Parse(right.Split(',')[1]));
    }

    public override string ToString()
    {
        return $"Position:{StartupLocationPoint.X},{StartupLocationPoint.Y}\r\nToLanguage:{ToLanguage}\r\nFromLanguage:{FromLanguage}";
    }

    public static implicit operator string(Setting setting)
    {
        return setting.ToString();
    }
}