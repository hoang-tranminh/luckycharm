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
    /// From ketqua.net
    /// </summary>
    public class DataFetcher1 :DataFetcherBase
    {
        public DataFetcher1()
        {
            Special = new Regex(@"Đặc Biệt<\/h3><\/td>\s+<td class=""bor f2 db"" colspan=""12"">(\d+)<\/td>", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);

            First = new Regex(@"Giải Nhất<\/h3><\/td>\s+<td class=""bor f2"" colspan=""12"">(\d+)<\/td>", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);

            Second = new Regex(@"Giải Nhì<\/h3><\/td>\s+<td class=""bol f2"" colspan=""6"">(\d+)</td>\s+<td class=""bor f2"" colspan=""6"">(\d+)<\/td>", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);

            Third = new Regex(@"Giải Ba<\/h3><\/td>\s+<td class=""bol f2"" colspan=""4"">(\d+)<\/td>\s+<td class=""bol f2"" colspan=""4"">(\d+)<\/td>\s+<td class=""bor f2"" colspan=""4"">(\d+)<\/td><\/tr>\s+<tr><td class=""bol f2"" colspan=""4"">(\d+)<\/td>\s+<td class=""bol f2"" colspan=""4"">(\d+)<\/td>\s+<td class=""bor f2"" colspan=""4"">(\d+)<\/td>", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);

            Fourth = new Regex(@"Giải Tư<\/h3><\/td>\s+<td class=""bol f2"" colspan=""3"">(\d+)<\/td>\s+<td class=""bol f2"" colspan=""3"">(\d+)<\/td>\s+<td class=""bol f2"" colspan=""3"">(\d+)<\/td>\s+<td class=""bor f2"" colspan=""3"">(\d+)<\/td>", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);

            Fifth = new Regex(@"Giải Năm<\/h3><\/td>\s+<td class=""bol f2"" colspan=""4"">(\d+)<\/td>\s+<td class=""bol f2"" colspan=""4"">(\d+)<\/td>\s+<td class=""bor f2"" colspan=""4"">(\d+)<\/td><\/tr>\s+<tr><td class=""bol f2"" colspan=""4"">(\d+)<\/td>\s+<td class=""bol f2"" colspan=""4"">(\d+)<\/td>\s+<td class=""bor f2"" colspan=""4"">(\d+)<\/td>", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);

            Sixth = new Regex(@"Giải Sáu<\/h3><\/td>\s+<td class=""bol f2"" colspan=""4"">(\d+)<\/td>\s+<td class=""bol f2"" colspan=""4"">(\d+)<\/td>\s+<td class=""bor f2"" colspan=""4"">(\d+)<\/td>", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);

            Seventh = new Regex(@"Giải Bảy<\/h3><\/td>\s+<td class=""bol f2"" colspan=""3"">(\d+)<\/td>\s+<td class=""bol f2"" colspan=""3"">(\d+)<\/td>\s+<td class=""bol f2"" colspan=""3"">(\d+)<\/td>\s+<td class=""bor f2"" colspan=""3"">(\d+)<\/td>", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);


            DateFormat = "dd/MM/yyyy";

            URL = "http://ketqua.net/xo-so-truyen-thong.php?ngay={0}";
        }
    }
}