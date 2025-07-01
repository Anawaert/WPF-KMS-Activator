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
        /// <summary>
        /// <para>检测是否有新版本可用</para>
        /// <para>Check if there is a new version available</para>
        /// </summary>
        /// <returns>
        /// <para>由 <see cref="Task"/> 包装的 <see langword="bool"/> 类型，在更新可用的时候返回 <see langword="true"/></para>
        /// <para><see langword="bool"/> type wrapped by <see cref="Task"/>, return <see langword="true"/> when an update is available</para>
        /// </returns>
        public static async Task<bool> CheckUpdateAvailable()
        {
            // 硬编码了 GitHub 上的 Release 页面地址，这里的正则表达式匹配模式是为了匹配版本号
            // 如果 GitHub 上的 Release 页面结构或版本号结构发生变化，这里的正则表达式也需要相应的调整
            // Hard-coded the GitHub Release page address, the regular expression matching pattern here is to match the version number
            // If the structure of the Release page on GitHub or the version number structure changes, the regular expression here also needs to be adjusted accordingly
            string targetUrl = "https://github.com/Anawaert/WPF-KMS-Activator/releases";
            string matchSpanRegexPattern = @"<span(.*?)</span>";
            string matchHyperlinkRegexPattern = @"<a(.*?)</a>";
            string matchVersionNumberRegexPattern = @"(\d+\.\d+\.\d+\.\d+)";
            string latestReleaseVersion = "3.0.0.0";

            try
            {
                // 连接至GitHub上的主页，并将相应返回的主页内容从字节码转为字符串
                // Connect to the homepage on GitHub and convert the corresponding returned homepage content from bytecode to a string
                HttpClient newClient = new HttpClient();
                HttpResponseMessage httpResponse = await newClient.GetAsync(targetUrl);
                httpResponse.EnsureSuccessStatusCode();  // 确保Http正确相响应
                string responseBody = await httpResponse.Content.ReadAsStringAsync();

                // 匹配 <span> 与 </span> 标签之间的全部内容，但考虑到 <span> 标签可能包含其他的样式，故仅匹配 <span> 到 </span> 之间
                // Match all content between <span> and </span> tags, but considering that the <span> tag may contain other styles, only match between <span> and </span>
                Regex getSpanTagsRegex = new Regex(matchSpanRegexPattern);
                // 匹配 <a> 与 </a> 标签之间的内容
                // Match the content between <a> and </a> tags
                Regex getHyperlinkRegex = new Regex(matchHyperlinkRegexPattern);
                // 匹配 <span> 与 </span> 标签之间以 x.x.x.x 为格式的版本号
                // Match the version number in the format of x.x.x.x between <span> and </span> tags
                Regex getVersionNumsRegex = new Regex(matchVersionNumberRegexPattern);

                // 先将 HTML 字符串中所有 <span></span> 标签之间的内容捕获，然后合并成一个新的大字符串
                // First capture all the content between the <span></span> tags in the HTML string, and then merge them into a new large string
                MatchCollection spanTagsRegexCollections = getSpanTagsRegex.Matches(responseBody);
                StringBuilder spanTagsStringBuilder = new StringBuilder();
                foreach (Match spanTag in spanTagsRegexCollections.Cast<Match>())
                {
                    spanTagsStringBuilder.Append(spanTag.Value);
                }

                // 然后将 <span></span> 组成的大字符串中包含 <a></a> 的内容捕获，Release 页每个发行版的标题就在 <a></a> 里面
                // Then capture the content containing <a></a> in the large string composed of <span></span>, the title of each release on the Release page is in <a></a>
                MatchCollection hyperlinkRegexCollections = getHyperlinkRegex.Matches(spanTagsStringBuilder.ToString());
                StringBuilder hyperlinkStringBuilder = new StringBuilder();
                foreach (Match hyperlink in hyperlinkRegexCollections.Cast<Match>())
                {
                    hyperlinkStringBuilder.Append(hyperlink.Value);
                }

                // 再从 <a></a> 组成的“小”字符串中匹配版本号，第一个匹配就是最新版本的版本号
                // Then match the version number from the "small" string composed of <a></a>, the first match is the version number of the latest version
                Match versionNumsMatch = getVersionNumsRegex.Match(hyperlinkStringBuilder.ToString());

                bool isUpdateAvailable = (versionNumsMatch.Value != latestReleaseVersion) && (versionNumsMatch.Success);

                Log.Logger.Information("Successfully checked for updates, the latest version is: {version} , and update available: {uptavl}", versionNumsMatch.Value, isUpdateAvailable);

                return isUpdateAvailable;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Failed to check for updates");
                return false;
            }
        }
    }
}
