using LuckyCharm.DataAccess;
using LuckyCharm.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace LuckyCharm.Busisness
{
    /// <summary>
    /// From xoso.wap
    /// </summary>
    public class DataFetcher2 : DataFetcherBase
    {

        public DataFetcher2()
        {
            Special = new Regex(@"Đặc Biệt<\/td><td class=""web_XS_2 chukq"" colspan=""12""><strong class=""do"">(\d+)<\/strong>", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);

            First = new Regex(@"Giải Nhất<\/td><td class=""web_XS_2 chukq"" colspan=""12"">(\d+)<\/td>", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);

            Second = new Regex(@"Giải Nhì<\/td><td class=""web_XS_2 chukq"" colspan=""6"">(\d+)<\/td><td class=""web_XS_2 chukq"" colspan=""6"">(\d+)<\/td>", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);

            Third = new Regex(@"Giải Ba<\/td><td class=""web_XS_2 chukq"" colspan=""4"">(\d+)<\/td><td class=""web_XS_2 chukq"" colspan=""4"">(\d+)<\/td><td class=""web_XS_2 chukq"" colspan=""4"">(\d+)<\/td><\/tr><tr class=""web_bg_Trang""><td class=""web_XS_2 chukq"" colspan=""4"">(\d+)<\/td><td class=""web_XS_2 chukq"" colspan=""4"">(\d+)<\/td><td class=""web_XS_2 chukq"" colspan=""4"">(\d+)<\/td>", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);

            Fourth = new Regex(@"Giải Tư<\/td><td class=""web_XS_2 chukq"" colspan=""3"">(\d+)<\/td><td class=""web_XS_2 chukq"" colspan=""3"">(\d+)<\/td><td class=""web_XS_2 chukq"" colspan=""3"">(\d+)<\/td><td class=""web_XS_2 chukq"" colspan=""3"">(\d+)<\/td>", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);

            Fifth = new Regex(@"Giải Năm<\/td><td class=""web_XS_2 chukq"" colspan=""4"">(\d+)<\/td><td class=""web_XS_2 chukq"" colspan=""4"">(\d+)<\/td><td class=""web_XS_2 chukq"" colspan=""4"">(\d+)<\/td><\/tr><tr class=""web_bg_Trang""><td class=""web_XS_2 chukq"" colspan=""4"">(\d+)<\/td><td class=""web_XS_2 chukq"" colspan=""4"">(\d+)<\/td><td class=""web_XS_2 chukq"" colspan=""4"">(\d+)<\/td>", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);

            Sixth = new Regex(@"Giải Sáu<\/td><td class=""web_XS_2 chukq"" colspan=""4"">(\d+)<\/td><td class=""web_XS_2 chukq"" colspan=""4"">(\d+)<\/td><td class=""web_XS_2 chukq"" colspan=""4"">(\d+)<\/td>", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);

            Seventh = new Regex(@"Giải Bảy<\/td><td class=""web_XS_2 chukq"" colspan=""3"">(\d+)<\/td><td class=""web_XS_2 chukq"" colspan=""3"">(\d+)<\/td><td class=""web_XS_2 chukq"" colspan=""3"">(\d+)<\/td><td class=""web_XS_2 chukq"" colspan=""3"">(\d+)<\/td>", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);

            DateFormat = "dd-MM-yyyy";

            URL = "http://xoso.wap.vn/ket-qua-xo-so-ngay-{0}.html";
        }
    }
}