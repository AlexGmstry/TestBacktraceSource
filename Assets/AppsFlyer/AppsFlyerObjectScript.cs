using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using AppsFlyerSDK;
using TMPro;

// This class is intended to be used the the AppsFlyerObject.prefab

public class AppsFlyerObjectScript : MonoBehaviour , IAppsFlyerConversionData
{

    // These fields are set from the editor so do not modify!
    //******************************//
    public string devKey;
    public string appID;
    public string UWPAppID;
    public string macOSAppID;
    public bool isDebug;
    public bool getConversionData;

    public TextMeshProUGUI text;

    private StringBuilder deepLinkMessages = new StringBuilder();
    //******************************//


    void Start()
    {
        // These fields are set from the editor so do not modify!
        //******************************//
        AppsFlyer.setIsDebug(isDebug);
#if UNITY_WSA_10_0 && !UNITY_EDITOR
        AppsFlyer.initSDK(devKey, UWPAppID, getConversionData ? this : null);
#elif UNITY_STANDALONE_OSX && !UNITY_EDITOR
    AppsFlyer.initSDK(devKey, macOSAppID, getConversionData ? this : null);
#else
        AppsFlyer.initSDK(devKey, appID, getConversionData ? this : null);
#endif
        //******************************/
        AppsFlyer.OnDeepLinkReceived += OnDeepLink;

        AppsFlyer.startSDK();
        
        deepLinkMessages.Append("AppsFlyer.startSDK()" + "\n\n"); 
        text.text = deepLinkMessages.ToString();
    }
    
    private void OnDeepLink(object sender, EventArgs args)
    {
        deepLinkMessages.Append("AppsFlyer OnDeepLink: method works." + "\n\n"); 
        text.text = deepLinkMessages.ToString();
        if (args is DeepLinkEventsArgs deepLinkEventArgs) 
        { 
            StringBuilder tmp = new StringBuilder(); 
            switch (deepLinkEventArgs.status) 
            { 
                case DeepLinkStatus.FOUND: 
                    deepLinkMessages.Append("AppsFlyer OnDeepLink: Deep found" + "\n\n"); 
                    text.text = deepLinkMessages.ToString();
                    
                    if (deepLinkEventArgs.isDeferred()) 
                    {
                            AppsFlyer.AFLog("OnDeepLink", "This is a deferred deep link");
                            deepLinkMessages.Append("AppsFlyer OnDeepLink: This is a deferred deep link" + "\n\n");
                            text.text = deepLinkMessages.ToString();
                    }
                    else
                    {
                        AppsFlyer.AFLog("OnDeepLink", "This is a direct deep link");
                        deepLinkMessages.Append("AppsFlyer OnDeepLink: This is a direct deep link" + "\n\n");
                        text.text = deepLinkMessages.ToString();
                    }

                    // deepLinkParamsDictionary contains all the deep link parameters as keys
                    Dictionary<string, object> deepLinkParamsDictionary = null;
#if UNITY_IOS && !UNITY_EDITOR
                        foreach (var keyValuePair in deepLinkEventArgs.deepLink)
                        {
                            tmp.Append(keyValuePair + "\n");
                        }
                    
                        deepLinkMessages.Append("AppsFlyer deepLink: " + "\n" + tmp + "\n\n");
                        text.text = deepLinkMessages.ToString();
                        
                          if (deepLinkEventArgs.deepLink.ContainsKey("click_event") && deepLinkEventArgs.deepLink["click_event"] != null)
                          {
                              deepLinkParamsDictionary = deepLinkEventArgs.deepLink["click_event"] as Dictionary<string, object>;

                              if (deepLinkParamsDictionary != null)
                              {
                                  foreach (var keyValuePair in deepLinkParamsDictionary)
                                  {
                                      tmp.Append(keyValuePair + "\n");
                                  }
                                  
                                  deepLinkMessages.Append("AppsFlyer deepLinkParamsDictionary: " + "\n" + tmp + "\n\n");
                                  text.text = deepLinkMessages.ToString();
                              }
                          }
#elif UNITY_ANDROID && !UNITY_EDITOR
                        deepLinkParamsDictionary = deepLinkEventArgs.deepLink;
#endif

                    break;
                    case DeepLinkStatus.NOT_FOUND:
                        AppsFlyer.AFLog("OnDeepLink", "Deep link not found");
                        deepLinkMessages.Append("AppsFlyer OnDeepLink: Deep link not found" + "\n");
                        text.text = deepLinkMessages.ToString();
                        break;
                    default:
                        AppsFlyer.AFLog("OnDeepLink", "Deep link error");
                        deepLinkMessages.Append("AppsFlyer OnDeepLink: Deep link error" + "\n");
                        text.text = deepLinkMessages.ToString();
                        break;
                }
        }
        else
        {
            deepLinkMessages.Append("AppsFlyer OnDeepLink: args is not deepLinkEventArgs" + "\n");
            text.text = deepLinkMessages.ToString();
        }
    }

    void Update()
    {

    }

    // Mark AppsFlyer CallBacks
    public void onConversionDataSuccess(string conversionData)
    {
        AppsFlyer.AFLog("didReceiveConversionData", conversionData);
        Dictionary<string, object> conversionDataDictionary = AppsFlyer.CallbackStringToDictionary(conversionData);
        // add deferred deeplink logic here
    }

    public void onConversionDataFail(string error)
    {
        AppsFlyer.AFLog("didReceiveConversionDataWithError", error);
    }

    public void onAppOpenAttribution(string attributionData)
    {
        AppsFlyer.AFLog("onAppOpenAttribution", attributionData);
        Dictionary<string, object> attributionDataDictionary = AppsFlyer.CallbackStringToDictionary(attributionData);
        // add direct deeplink logic here
    }

    public void onAppOpenAttributionFailure(string error)
    {
        AppsFlyer.AFLog("onAppOpenAttributionFailure", error);
    }

}
