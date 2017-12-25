腾讯云短信 C# SDK
===

## 腾讯短信服务

目前`腾讯云短信`为客户提供`国内短信`、`国内语音`和`海外短信`三大服务，腾讯云短信SDK支持以下操作：

### 国内短信

国内短信支持操作：

- 单发短信
- 指定模板单发短信
- 群发短信
- 指定模板群发短信
- 拉取短信回执和短信回复状态

### 海外短信

海外短信支持操作：

- 单发短信
- 指定模板单发短信
- 群发短信
- 指定模板群发短信
- 拉取短信回执和短信回复状态

> `Note` 海外短信和国内短信使用同一接口，只需替换相应的国家码与手机号码，每次请求群发接口手机号码需全部为国内或者海外手机号码。

### 语音通知

语音通知支持操作：

- 发送语音验证码
- 发送语音通知

## 开发

### 准备

在开始开发云短信应用之前，需要准备如下信息:

- [x] 获取SDK AppID和AppKey

云短信应用SDK `AppID`和`AppKey`可在[短信控制台](https://console.cloud.tencent.com/sms)的应用信息里获取，如您尚未添加应用，请到[短信控制台](https://console.cloud.tencent.com/sms)中添加应用。

- [x] 申请签名

一个完整的短信由短信`签名`和短信正文内容两部分组成，短信`签名`须申请和审核，`签名`可在[短信控制台](https://console.cloud.tencent.com/sms)的相应服务模块`内容配置`中进行申请。

- [x] 申请模板

同样短信或语音正文内容`模板`须申请和审核，`模板`可在[短信控制台](https://console.cloud.tencent.com/sms)的相应服务模块`内容配置`中进行申请。

### 安装

#### nuget

要使用qcloudsms_csharp功能，只需要在.nuspec文件中添加如下依赖：

```xml
<dependencies>
  <dependency id="qcloud.qcloudsms_csharp" version="0.1.0" />
</dependencies>
```

或者参考nuget官方网站进行安装: https://docs.microsoft.com/en-us/nuget/quickstart/use-a-package

### 用法

> 若您对接口存在疑问，可以查阅 [开发指南](https://cloud.tencent.com/document/product/382/5808) 和 [API文档](https://qcloudsms.github.io/qcloudsms_csharp/)。

- **准备必要参数**

```csharp
// 短信应用SDK AppID
int appid = 122333333;

// 短信应用SDK AppKey
string appkey = "9ff91d87c2cd7cd0ea762f141975d1df37481d48700d70ac37470aefc60f9bad";

// 需要发送短信的手机号码
string[] phoneNumbers = {"21212313123", "12345678902", "12345678903"};

// 短信模板ID，需要在短信应用中申请
int templateId = 7839; // NOTE: 这里的模板ID`7839`只是一个示例，真实的模板ID需要在短信控制台中申请
```

- **单发短信**

```csharp
using qcloudsms_csharp;
using qcloudsms_csharp.json;
using qcloudsms_csharp.httpclient;

using System;

try
{
    SmsSingleSender ssender = new SmsSingleSender(appid, appkey);
    var result = ssender.send(0, "86", phoneNumbers[0],
        "您的验证码是: 12345", null, null);
    Console.WriteLine(result);
}
catch (JSONException e)
{
    Console.WriteLine(e);
}
catch (HTTPException e)
{
    Console.WriteLine(e);
}
catch (Exception e)
{
    Console.WriteLine(e);
}
```

> `Note` 如需发送海外短信，同样可以使用此接口，只需将国家码 `86` 改写成对应国家码号。
> `Note` 无论单发/群发短信还是指定模板ID单发/群发短信都需要从控制台中申请模板并且模板已经审核通过，才可能下发成功，否则返回失败。


- **指定模板ID单发短信**

```csharp
using qcloudsms_csharp;
using qcloudsms_csharp.json;
using qcloudsms_csharp.httpclient;

using System;

try
{
    SmsSingleSender ssender = new SmsSingleSender(appid, appkey);
    var result = ssender.sendWithParam("86", phoneNumbers[0],
        templateId, new[] { "12345" }, null, null, null);
    Console.WriteLine(result);
}
catch (JSONException e)
{
    Console.WriteLine(e);
}
catch (HTTPException e)
{
    Console.WriteLine(e);
}
catch (Exception e)
{
    Console.WriteLine(e);
}
```

> `Note` 无论单发短信还是指定模板ID单发短信都需要从控制台中申请模板并且模板已经审核通过，才可能下发成功，否则返回失败。

- **群发**

```csharp
using qcloudsms_csharp;
using qcloudsms_csharp.json;
using qcloudsms_csharp.httpclient;

using System;

try
{
    SmsMultiSender msender = new SmsMultiSender(appid, appkey);
    var result = msender.send(0, "86", phoneNumbers,
        "您的验证码是: 67890", null, "");
    Console.WriteLine(result);
}
catch (JSONException e)
{
    Console.WriteLine(e);
}
catch (HTTPException e)
{
    Console.WriteLine(e);
}
catch (Exception e)
{
    Console.WriteLine(e);
}
```

> `Note` 无论单发/群发短信还是指定模板ID单发/群发短信都需要从控制台中申请模板并且模板已经审核通过，才可能下发成功，否则返回失败。

- **指定模板ID群发**

```csharp
using qcloudsms_csharp;
using qcloudsms_csharp.json;
using qcloudsms_csharp.httpclient;

using System;

try
{
    SmsMultiSender msender = new SmsMultiSender(appid, appkey);
    var sresult = msender.sendWithParam("86", phoneNumbers, templateId,
        new[]{"67890"}, null, null, null);
    Console.WriteLine(sresult);
catch (JSONException e)
{
    Console.WriteLine(e);
}
catch (HTTPException e)
{
    Console.WriteLine(e);
}
catch (Exception e)
{
    Console.WriteLine(e);
}
```

> `Note` 群发一次请求最多支持200个号码，如有对号码数量有特殊需求请联系腾讯云短信技术支持(QQ:3012203387)。
> `Note` 无论单发/群发短信还是指定模板ID单发/群发短信都需要从控制台中申请模板并且模板已经审核通过，才可能下发成功，否则返回失败。

- **发送语音验证码**

```csharp
using qcloudsms_csharp;
using qcloudsms_csharp.json;
using qcloudsms_csharp.httpclient;

using System;

try
{
    SmsVoiceVerifyCodeSender vvcsender = new SmsVoiceVerifyCodeSender(appid, appkey);
    var result = vvcsender.send("86", phoneNumbers[0], "09876", 2, "");
    Console.WriteLine(result);
}
catch (JSONException e)
{
    Console.WriteLine(e);
}
catch (HTTPException e)
{
    Console.WriteLine(e);
}
catch (Exception e)
{
    Console.WriteLine(e);
}
```

> `Note` 语音验证码发送只需提供验证码数字，例如在msg=“5678”，您收到的语音通知为“您的语音验证码是5678”，如需自定义内容，可以使用语音通知。

- **发送语音通知**

```csharp
using qcloudsms_csharp;
using qcloudsms_csharp.json;
using qcloudsms_csharp.httpclient;

using System;

try
{
    SmsVoicePromptSender vspsender = new SmsVoicePromptSender(appid, appkey);
    var result = vspsender.send("86", phoneNumbers[0], 2, 2, "您的验证码是: 54321", null);
    Console.WriteLine(result);
}
catch (JSONException e)
{
    Console.WriteLine(e);
}
catch (HTTPException e)
{
    Console.WriteLine(e);
}
catch (Exception e)
{
    Console.WriteLine(e);
}
```

- **拉取短信回执以及回复**

```csharp
using qcloudsms_csharp;
using qcloudsms_csharp.json;
using qcloudsms_csharp.httpclient;

using System;

try
{
    SmsStatusPuller spuller = new SmsStatusPuller(appid, appkey);
    var callbackResult = spuller.pullCallback(5);
    Console.WriteLine(callbackResult);

    var replyResult = spuller.pullReply(5);
    Console.WriteLine(replyResult);
}
catch (JSONException e)
{
    Console.WriteLine(e);
}
catch (HTTPException e)
{
    Console.WriteLine(e);
}
catch (Exception e)
{
    Console.WriteLine(e);
}
```

> `Note` 短信拉取功能需要联系腾讯云短信技术支持(QQ:3012203387)，量大客户可以使用此功能批量拉取，其他客户不建议使用。

- **拉取单个手机短信状态**

```csharp
using qcloudsms_csharp;
using qcloudsms_csharp.json;
using qcloudsms_csharp.httpclient;

using System;

try
{
    var callbackResult = mspuller.pullCallback("86", phoneNumbers[0], 1514022500, 1514022590, 5);
    Console.WriteLine(callbackResult);

    var replyResult = mspuller.pullReply("86", phoneNumbers[0], 1514022500, 1514022590, 5);
    Console.WriteLine(replyResult);
}
catch (JSONException e)
{
    Console.WriteLine(e);
}
catch (HTTPException e)
{
    Console.WriteLine(e);
}
catch (Exception e)
{
    Console.WriteLine(e);
}
```

- **发送国际短信**

国际短信发送可以参考单发短信。

#### 使用连接池

多个线程可以共用一个连接池发送API请求，多线程并发单发短信示例如下：

```csharp
using qcloudsms_csharp;
using qcloudsms_csharp.httpclient;
using qcloudsms_csharp.json;

using System;
using System.Threading;


public class SmsTest
{
    public class SmsArg
    {
        public SmsSingleSender sender;
        public string nationCode;
        public string phoneNumber;
        public string msg;

        public SmsArg(SmsSingleSender sender, string nationCode, string phoneNumber, string msg)
        {
            this.sender = sender;
            this.nationCode = nationCode;
            this.phoneNumber = phoneNumber;
            this.msg = msg;
        }
    }

    public static void SendSms(object data)
    {
        SmsArg arg = (SmsArg)data;
        try
        {
            var result = arg.sender.send(0, arg.nationCode, arg.phoneNumber, arg.msg, "", "");
            Console.WriteLine("{0}, {1}", result, arg.phoneNumber);
        }
        catch (JSONException e)
        {
            Console.WriteLine(e);
        }
        catch (HTTPException e)
        {
            Console.WriteLine(e);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    static void Main(string[] args)
    {
        int appid = 122333333;
        string appkey = "9ff91d87c2cd7cd0ea762f141975d1df37481d48700d70ac37470aefc60f9bad";
        string[] phoneNumbers = {
                "21212313123", "12345678902", "12345678903",
                "21212313124", "12345678903", "12345678904",
                "21212313125", "12345678904", "12345678905",
                "21212313126", "12345678905", "12345678906",
                "21212313127", "12345678906", "12345678907",
            };

        // 创建一个连接池httpclient
        PoolingHTTPClient httpclient = new PoolingHTTPClient();

        // 创建SmsSingleSender时传入连接池http client
        SmsSingleSender ssender = new SmsSingleSender(appid, appkey, httpclient);

        // 创建线程
        Thread[] threads = new Thread[phoneNumbers.Length];
        for (int i = 0; i < phoneNumbers.Length; i++)
        {
            threads[i] = new Thread(SmsTest.SendSms);
        }

        // 运行线程
        for (int i = 0; i < threads.Length; i++)
        {
            threads[i].Start(new SmsArg(ssender, "86", phoneNumbers[i], "您验证码是：5678"));
        }

        // join线程
        for (int i = 0; i < threads.Length; i++)
        {
            threads[i].Join();
        }

        // 关闭连接池httpclient
        httpclient.close();
    }
}
```

### 使用自定义HTTP client实现

如果需要使用自定义的HTTP client实现，只需实现`qcloudsms_csharp.httpclient.IHTTPClient`接口，并在构造API对象时传入自定义HTTP client即可，一个参考示例如下：

```csharp
using qcloudsms_csharp;
using qcloudsms_csharp.httpclient;

// using myhttp_namespace;

public class CustomHTTPClient : IHTTPClient
{
    public HTTPResponse fetch(HTTPRequest request)
    {
        // 1. 创建自定义HTTP request
        // MyHTTPrequest req = MyHTTPRequest.build(request)

        // 2. 创建自定义HTTP cleint
        // MyHTTPClient client = new MyHTTPClient();

        // 3. 使用自定义HTTP client获取HTTP响应
        // MyHTTPResponse response = client.fetch(req);

        // 4. 转换HTTP响应到HTTPResponse
        // HTTPResponse res = transformToHTTPResponse(response);

        // 5. 返回HTTPResponse实例
        // return res;
    }

    public void close()
    {
    }
}

// 创建自定义HTTP client
CustomHTTPClient httpclient = new CustomHTTPClient();
// 构造API对象时传入自定义HTTP client
SmsSingleSender ssender = new SmsSingleSender(appid, appkey, httpclient);
```

> `Note` 注意上面的这个示例代码只作参考，无法直接编译和运行，需要作相应修改。
