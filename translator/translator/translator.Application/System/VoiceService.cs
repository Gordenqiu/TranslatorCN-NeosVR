using EFCore.BulkExtensions;
using Furion;
using Furion.DatabaseAccessor;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Tci.V20190318.Models;
using TencentCloud.Tmt;
using TencentCloud.Tmt.V20180321;
using TencentCloud.Tmt.V20180321.Models;
using translator.Application.System.Dtos;
using translator.Core;

namespace translator.Application
{
    /// <summary>
    /// 语音处理接口
    /// </summary>
    public class VoiceService : IDynamicApiController
    {
        private readonly IRepository<voiceText> _voiceTextRepository;
        public VoiceService(IRepository<voiceText> voiceTextRepository)
        {
            _voiceTextRepository = voiceTextRepository;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(201, Type = typeof(string))]
        public string Get()
        {
            return "hello";
        }
        /// <summary>
        /// 存入数据库
        /// </summary>
        /// <returns></returns>
        public bool PostVoiceText(VoiceRequest request)
        {
            var voice = new voiceText();
            voice.Text = request.val;
            voice.Name = request.name;
            _voiceTextRepository.InsertNow(voice);

            return true;
        }
        /// <summary>
        /// neos调用
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(201, Type = typeof(string))]
        [ProducesResponseType(500, Type = typeof(string))]
        public string PostNeosReturn(string Name)
        {
            try 
            {
                string voice = "";
                var voices = _voiceTextRepository.Where(u => u.Name == Name).OrderByDescending(u => u.Id).ToList();
                
                if (voices.Count > 0)
                {
                    voice =  voices.FirstOrDefault().Text;
                    _voiceTextRepository.Context.BulkDelete(voices);
                }
                return voice;  
            }
            catch
            {
                return "NeosReturnError";
            } 
        }

        /// <summary>
        /// 获取翻译后内容
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(201, Type = typeof(string))]
        public string GetTranslateWord(string key,string source, string sourceText, string target)
        {
            // 实例化一个认证对象，入参需要传入腾讯云账户secretId，secretKey,此处还需注意密钥对的保密
            // 密钥可前往https://console.cloud.tencent.com/cam/capi网站进行获取
           
            var secert = key.Split(",");
            if (secert.Length != 2)
            {
                return "密钥错误，翻译失败！";
            }
            if (sourceText == "NeosReturnError" || string.IsNullOrEmpty(sourceText))
            {
                return string.Empty;
            }
            Credential cred = new Credential
            {
                SecretId = "",
                SecretKey = ""
            };
            if (secert[0]=="test"&& secert[1] == "test")
            {
                cred.SecretId = App.Configuration["TXSecret:Id"];
                cred.SecretKey = App.Configuration["TXSecret:Key"];
            }
            else
            {
                cred.SecretId = secert[0];
                cred.SecretKey = secert[1];
            }
            // 实例化一个client选项，可选的，没有特殊需求可以跳过
            ClientProfile clientProfile = new ClientProfile();
            // 实例化一个http选项，可选的，没有特殊需求可以跳过
            HttpProfile httpProfile = new HttpProfile();
            httpProfile.Endpoint = ("tmt.tencentcloudapi.com");
            clientProfile.HttpProfile = httpProfile;

            // 实例化要请求产品的client对象,clientProfile是可选的
            TmtClient client = new TmtClient(cred, "ap-shanghai", clientProfile);
            // 实例化一个请求对象,每个接口都会对应一个request对象
            TextTranslateRequest req = new TextTranslateRequest();
            req.Source = source;
            req.SourceText = sourceText;
            req.Target = target;
            req.ProjectId = 0;

            // 返回的resp是一个TextTranslateResponse的实例，与请求对象对应
            TextTranslateResponse resp = client.TextTranslateSync(req);
            // 输出json格式的字符串回包
            Console.WriteLine(AbstractModel.ToJsonString(resp));

            return resp.TargetText;
        }
    }

}