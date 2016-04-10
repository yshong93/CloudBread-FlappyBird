using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using AssemblyCSharp;
using Assets.Scripts.CloudBread.Data;

namespace Assets.Scripts.CloudBread
{
    class CloudBread
    {
        public static string ServerAddress = "https://cb2-auth-demo.azurewebsites.net/";

        private string auth = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdGFibGVfc2lkIjoic2lkOmNkMDIzZTcyMjgxZmE0ZTUzMGE0ZmMzYzgxODBlMjE4Iiwic3ViIjoic2lkOjgzMTZhZWU5NzY4ODk3NDg1Y2M3OGI5ZjY3NjYxZDJjIiwiaWRwIjoiZmFjZWJvb2siLCJ2ZXIiOiIzIiwiaXNzIjoiaHR0cHM6Ly9jYjItYXV0aC1kZW1vLmF6dXJld2Vic2l0ZXMubmV0LyIsImF1ZCI6Imh0dHBzOi8vY2IyLWF1dGgtZGVtby5henVyZXdlYnNpdGVzLm5ldC8iLCJleHAiOjE0NjUxMDQwOTAsIm5iZiI6MTQ1OTkzMTU0M30.wNgTUtXobPGGXYtLjZedyMrWI5WSbnhwN9Co6LdfgEg";

        private Action<string, WWW> _requestCallback = null;

        public void CBInsRegMember(Action<string, WWW> callback)
        {
            var ServerEndPoint = ServerAddress + "api/CBInsRegMember";

            //WWWHelper.Instance.OnHttpRequest += OnHttpRequest;
            WWWHelper helper = WWWHelper.Instance;
            helper.OnHttpRequest += OnHttpRequest;

            MemberData memberData = new MemberData {
                MemberID_Members = (string)PlayerPrefs.GetString("userId"),
                EmailAddress_Members = (string)PlayerPrefs.GetString("userId"),
                Name1_Members = (string)PlayerPrefs.GetString("nickName")
            };

            string jsonBody = JsonParser.Write(memberData);

            helper.POST("CBInsRegMember", ServerEndPoint, jsonBody);

            _requestCallback = callback;
        }

        public void CBComUdtMemberGameInfoes(Action<string, WWW> callback)
        {
            var ServerEndPoint = ServerAddress + "api/CBComUdtMemberGameInfoes";

            WWWHelper helper = WWWHelper.Instance;
            helper.OnHttpRequest += OnHttpRequest;

            MemberGameInfo gameinfoData = new MemberGameInfo
            {
                MemberID = (string)PlayerPrefs.GetString("userId"),
                Level = 2,
                Points = PlayerPrefs.GetInt("bestScore")
            };

            string jsonBody = JsonParser.Write(gameinfoData);

            helper.POST("CBComUdtMemberGameInfoes", ServerEndPoint, jsonBody);

            _requestCallback = callback;
        }

        public void CBRank(Action<string, WWW> callback)
        {
            var ServerEndPoint = ServerAddress + "api/CBRank";

            WWWHelper helper = WWWHelper.Instance;
            helper.OnHttpRequest += OnHttpRequest;

            Dictionary<string, string> bodyDic = new Dictionary<string, string>();
            bodyDic.Add("sid", (string)PlayerPrefs.GetString("nickName"));
            bodyDic.Add("point", (string)PlayerPrefs.GetInt("bestScore").ToString());

            helper.POST("CBRank", ServerEndPoint, bodyDic);

            _requestCallback = callback;
        }

        public void CBTopRanker(Action<string, WWW> callback)
        {
            var nickName = (string)PlayerPrefs.GetString("nickName");

            //var ServerEndPoint = ServerAddress + "api/topranker/" + CBUtils.CBEncoding(nickName) + "/30";
            var ServerEndPoint = ServerAddress + "api/topranker/ccc/30";
            Debug.Log(ServerEndPoint);

            WWWHelper helper = WWWHelper.Instance;
            helper.OnHttpRequest += OnHttpRequest;

            helper.get("CBTopRanker", ServerEndPoint);

            _requestCallback = callback;
        }

        private void HTTPRequestAuthSend()
        {
            var serverEndPoint = ServerAddress + "api/ping";

            Dictionary<string, string> Header = new Dictionary<string, string>();
            Header.Add("Accept", "application/json");
            Header.Add("X-ZUMO-VERSION", "ZUMO/2.0 (lang=Managed; os=Windows Store; os_version=--; arch=X86; version=2.0.31217.0)");
            Header.Add("X-ZUMO-FEATURES", "AJ");
            Header.Add("ZUMO-API-VERSION", "2.0.0");
            Header.Add("Content-Type", "application/json");
            if (AzureMobileAppRequestHelper.AuthToken != null)
                Header.Add("x-zumo-auth", AzureMobileAppRequestHelper.AuthToken);

            WWWHelper.Instance.get("1", serverEndPoint);

            //WWW www = new WWW(serverEndPoint, null, Header);
            //StartCoroutine(WaitForRequest(www));
        }

        public void OnHttpRequest(string id, WWW www)
        {
            WWWHelper helper = WWWHelper.Instance;
            helper.OnHttpRequest -= OnHttpRequest;

            if (www.error != null)
            {
                Debug.Log("[Error] " + www.error);
                _requestCallback("error", www);
            }
            else {
                Debug.Log(www.text);

                var RequestJsonString = www.text;

                _requestCallback(RequestJsonString, www);


            }
        }

    }
}
