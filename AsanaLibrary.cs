using RestSharp;

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

public class AsanaLibrary
{
    public List<string> Trap = new List<string>();
    internal INI Ini;
    internal string apiUrl = "https://app.asana.com/api/1.0";
    internal Dictionary<string, object> queryDict = new Dictionary<string, object>();
    internal Dictionary<string, object> paramDict = new Dictionary<string, object>();
    internal string token;

    public AsanaLibrary(string tkn)
    {
        token = tkn;
    }

    public AsanaLibrary(FileInfo fi)
    {
        Ini = new INI(fi.FullName);
        token = Ini.IniReadValue("Authorisation", "Token", string.Empty);
    }

    public string Do(string method, string param)
    {
        {
            if (Trap.FindIndex(e => e == MethodBase.GetCurrentMethod().Name) > -1)
            {
                System.Diagnostics.Debugger.Launch();
            }

            var answer = ReplaceFromDict(param, queryDict);
            var client = new RestClient(apiUrl);
            var request = new RestRequest(answer)
            {
                Method = (Method)Enum.Parse(typeof(Method), method)
            };
            request.AddHeader("Authorization", "Bearer " + token);

            foreach (var kvp in paramDict)
            {
                request.AddParameter(kvp.Key, kvp.Value, ParameterType.GetOrPost);
            }
            var response = client.Execute(request);
            return response.Content;
        }
    }
    public string Get(string param) => Do("GET", param);

    public string Put(string param) => Do("PUT", param);

    public string Post(string param) => Do("POST", param);

    public string Delete(string param) => Do("DELETE", param);

    public void SetQueryDict(string name, object value)
    {
        if (Trap.FindIndex(e => e == MethodBase.GetCurrentMethod().Name) > -1)
        {
            System.Diagnostics.Debugger.Launch();
        }

        queryDict[name] = value;
    }

    public void SetParamDict(string name, object value)
    {
        if (Trap.FindIndex(e => e == MethodBase.GetCurrentMethod().Name) > -1)
        {
            System.Diagnostics.Debugger.Launch();
        }

        paramDict[name] = value;
    }

    public void ClearParamDict() => paramDict.Clear();

    public void ClearQueryDict() => queryDict.Clear();

    public string ReplaceFromDict(string pattern, Dictionary<string, object> dso)
    {
        if (Trap.FindIndex(e => e == MethodBase.GetCurrentMethod().Name) > -1)
        {
            System.Diagnostics.Debugger.Launch();
        }

        string answer = pattern;
        foreach (var kvp in dso)
        {
            answer = answer.Replace("{" + kvp.Key + "}", kvp.Value.ToString());
        }
        return answer;
    }
}

