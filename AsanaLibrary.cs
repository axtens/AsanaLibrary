using Newtonsoft.Json;

using RestSharp;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

public class AsanaLibrary
{
    public List<string> Trap = new List<string>();
    internal INI Ini;
    internal string apiUrl = "https://app.asana.com/api/1.0";
    internal Dictionary<string, object> queryDict = new Dictionary<string, object>();
    internal Dictionary<string, object> paramDict = new Dictionary<string, object>();
    internal string token;
    public string RestResponse;
    public string RestRequest;

    public AsanaLibrary(string tkn)
    {
        token = tkn;
    }

    public AsanaLibrary(FileInfo fi)
    {
        Ini = new INI(fi.FullName);
        token = Ini.IniReadValue("Authorisation", "Token", string.Empty);
    }

    public string Do(string method, string param, bool debug = false)
    {
        {
            if (Trap.FindIndex(e => e == MethodBase.GetCurrentMethod().Name) > -1 || debug)
            {
                System.Diagnostics.Debugger.Launch();
            }

            var answer = ReplaceFromItems(param, queryDict);
            var client = new RestClient(apiUrl);
            var request = new RestRequest(answer, (Method)Enum.Parse(typeof(Method), method));

            request.AddHeader("Authorization", "Bearer " + token);
            request.AddHeader("Accept", "application/json");
            if (method == "PUT" || method == "POST")
                request.AddHeader("Content-Type", "application/json");
            //request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.RequestFormat = DataFormat.Json;

            if (method == "POST" || method == "PUT")
            {
                request.AddJsonBody(paramDict["body"]);
            }
            else
            {
                foreach (var kvp in paramDict)
                {
                    request.AddParameter(kvp.Key, kvp.Value, ParameterType.GetOrPost);
                }
            }

            RestRequest = JsonConvert.SerializeObject(request);

            var response = client.Execute(request);

            RestResponse = JsonConvert.SerializeObject(response);

            return response.Content;
        }
    }
    public string Get(string param, bool debug = false) => Do("GET", param, debug);

    public string Put(string param, bool debug = false) => Do("PUT", param, debug);

    public string Post(string param, bool debug = false) => Do("POST", param, debug);

    public string Delete(string param, bool debug = false) => Do("DELETE", param, debug);

    public void AddQueryItem(string name, object value)
    {
        if (Trap.FindIndex(e => e == MethodBase.GetCurrentMethod().Name) > -1)
        {
            System.Diagnostics.Debugger.Launch();
        }

        queryDict[name] = value;
    }

    public void AddParamItem(string name, object value)
    {
        if (Trap.FindIndex(e => e == MethodBase.GetCurrentMethod().Name) > -1)
        {
            System.Diagnostics.Debugger.Launch();
        }

        paramDict[name] = value;
    }

    public string GetQueryItem(string name)
    {
        if (Trap.FindIndex(e => e == MethodBase.GetCurrentMethod().Name) > -1)
        {
            System.Diagnostics.Debugger.Launch();
        }

        return queryDict.TryGetValue(name, out object value) ? value.ToString() : null;
    }

    public string GetParamItem(string name)
    {
        if (Trap.FindIndex(e => e == MethodBase.GetCurrentMethod().Name) > -1)
        {
            System.Diagnostics.Debugger.Launch();
        }

        return paramDict.TryGetValue(name, out object value) ? value.ToString() : null;
    }

    public void ClearParamItems() => paramDict.Clear();

    public void ClearQueryItems() => queryDict.Clear();

    public string ReplaceFromItems(string pattern, Dictionary<string, object> dso)
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

    public string DumpQueryItems()
    {
        return string.Join("\r\n", (from kv in queryDict.Keys select (kv + " -> " + queryDict[kv])).ToList());
    }

    public string DumpParamItems()
    {
        return string.Join("\r\n", (from kv in paramDict.Keys select (kv + " -> " + paramDict[kv])).ToList());
    }
}