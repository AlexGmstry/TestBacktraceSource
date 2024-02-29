using Backtrace.Unity;
using Backtrace.Unity.Model;
using System;
using System.Collections.Generic;
using UnityEngine;

public class StartupScript : MonoBehaviour
{
    // Backtrace client instance
    public BacktraceClient _backtraceClient;
    void Start()
    {
        var serverUrl = "https://submit.backtrace.io/oleksiiyermakgmstry/7bc9ca0c0cb276c104fb832e5cc4028d4aec5a03b0b9d903b74c09cd9bc38f08/json"; ;
        var databasePath =  "${Application.persistentDataPath}/sample/backtrace/path";
        var attributes = new Dictionary<string, string>() { {"my-super-cool-attribute-name", "attribute-value"} };
        

        // or initialize Backtrace integration directly in your source code
        _backtraceClient = BacktraceClient.Initialize(
            url: serverUrl,
            databasePath: databasePath ,
            gameObjectName: name,
            attributes: attributes);
    }
    // Update is called once per frame
    void Update()
    {
        try
        {
            throw new NullReferenceException("this is test");
        }
        catch (Exception exception)
        {
            var report = new BacktraceReport(
                exception: exception,
                attachmentPaths: new List<string>() { @"file_path_1", @"file_path_2" }
            );
            _backtraceClient.Send(report);
        }
    }
}
