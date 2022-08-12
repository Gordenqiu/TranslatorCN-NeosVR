using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Threading.Tasks;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Tmt;
using TencentCloud.Tmt.V20180321;
using TencentCloud.Tmt.V20180321.Models;
using translator.Application.System.Dtos;

namespace translator.Application
{
    /// <summary>
    /// 语音处理接口
    /// </summary>
    public class VoiceService : IDynamicApiController
    {
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
        /// 
        /// </summary>
        /// <returns></returns>
        public bool PostVoiceText(VoiceRequest request)
        {
            var e = request;
            return true;
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
            Credential cred = new Credential
            {
                SecretId = secert[0],
                SecretKey = secert[1]
            };
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