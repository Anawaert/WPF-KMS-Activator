using System;
using Serilog;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Activator
{
    public partial class Utility
    {
        public static async Task<bool> CheckUpdateAvailable()
        {
            string targetUrl = "https://github.com/Anawaert/WPF-KMS-Activator/releases";
            string matchSpanRegexPattern = @"<span(.*?)</span>";
            string matchHyperlinkRegexPattern = @"<a(.*?)</a>";
            string matchVersionNumberRegexPattern = @"(\d+\.\d+\.\d+\.\d+)";
            string latestReleaseVersion = "3.0.0.0";

            try
            {
                HttpClient newClient = new HttpClient();
                HttpResponseMessage httpResponse = await newClient.GetAsync(targetUrl);  // 连接至GitHub上的主页
                httpResponse.EnsureSuccessStatusCode();  // 确保Http正确相响应
                string responseBody = await httpResponse.Content.ReadAsStringAsync();  // 将相应返回的主页内容从字节码转为字符串

                Regex getSpanTagsRegex = new Regex(matchSpanRegexPattern);  // 匹配<span>与</span>标签之间的全部内容,但考虑到<span>标签可能包含其他的样式，故仅匹配<span到</span>之间
                Regex getHyperlinkRegex = new Regex(matchHyperlinkRegexPattern);  // 匹配<a>与</a>标签之间的内容
                Regex getVersionNumsRegex = new Regex(matchVersionNumberRegexPattern);  // 匹配<span>与</span>标签之间以x.x.x.x为格式的版本号

                // 先将HTML字符串中所有<span></span>标签之间的内容捕获，然后合并成一个新的大字符串
                MatchCollection spanTagsRegexCollections = getSpanTagsRegex.Matches(responseBody);
                StringBuilder spanTagsStringBuilder = new StringBuilder();
                foreach (Match spanTag in spanTagsRegexCollections.Cast<Match>())
                {
                    spanTagsStringBuilder.Append(spanTag.Value);
                }

                // 然后将<span></span>组成的大字符串中包含<a></a>的内容捕获，Release页每个发行版的标题就在<a></a>里面
                MatchCollection hyperlinkRegexCollections = getHyperlinkRegex.Matches(spanTagsStringBuilder.ToString());
                StringBuilder hyperlinkStringBuilder = new StringBuilder();
                foreach (Match hyperlink in hyperlinkRegexCollections.Cast<Match>())
                {
                    hyperlinkStringBuilder.Append(hyperlink.Value);
                }

                // 再从<a></a>组成的“小”字符串中匹配版本号，第一个匹配就是最新版本的版本号
                Match versionNumsMatch = getVersionNumsRegex.Match(hyperlinkStringBuilder.ToString());

                return (versionNumsMatch.Value != latestReleaseVersion) && (versionNumsMatch.Success);
            }
            catch (Exception checkUpdateException)
            {

                return false;
            }
            finally
            {
                
            }
        }
    }
}
