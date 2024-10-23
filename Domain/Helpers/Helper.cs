using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace Domain.Helpers;

public static class Helper
{
    private static readonly Dictionary<Type, PropertyInfo[]> _TypesWithWriteableProperties = new();

    public static string AddUploadPrefix(this string imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl)) return imageUrl;

        return $"/upload/{imageUrl.TrimStart('/')}";
    }

    public static string GenerateSlug(this string title)
    {
        if (string.IsNullOrEmpty(title))
            return string.Empty;

        // Türkçe karakterleri İngilizce karakterlere dönüştür
        title = title
            .Replace("ş", "s")
            .Replace("Ş", "S")
            .Replace("ç", "c")
            .Replace("Ç", "C")
            .Replace("ğ", "g")
            .Replace("Ğ", "G")
            .Replace("ü", "u")
            .Replace("Ü", "U")
            .Replace("ö", "o")
            .Replace("Ö", "O")
            .Replace("ı", "i")
            .Replace("İ", "I");

        // Küçük harfe dönüştür
        title = title.ToLower();

        // Özel karakterleri ve boşlukları temizle
        title = Regex.Replace(title, @"[^a-z0-9\s-]", "");

        // Tekrarlanan boşlukları ve tireleri temizle
        title = Regex.Replace(title, @"[\s-]+", " ").Trim();

        // Boşlukları tire ile değiştir
        title = Regex.Replace(title, @"\s", "-");

        // Zararlı kod içermediğinden emin ol
        title = Regex.Replace(title, @"<.*?>", string.Empty);

        return title;
    }

    public static void ApplyCustomOperation<T>(this JsonPatchDocument<T> patchDocument, T entity, string path,
        OperationType operationType, Action<T, string> applyAction) where T : class
    {
        var operation = patchDocument.Operations
            .FirstOrDefault(op =>
                op.path.Equals(path, StringComparison.OrdinalIgnoreCase) && op.OperationType == operationType);

        if (operation != null)
        {
            var value = operation.value.ToString();
            applyAction(entity, value);
        }
    }


    public static decimal GetKur(this string kur)
    {
        var bugun = "http://www.tcmb.gov.tr/kurlar/today.xml";
        var xmlDoc = new XmlDocument();
        xmlDoc.Load(bugun);

        if (kur == "€")
        {
            var EURO_Alis = xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteBuying").InnerXml;
            var EURO_Satis = xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteSelling").InnerXml;
            var result = EURO_Satis.ToDecimal(new CultureInfo("en-EN"));

            return result;
        }

        if (kur == "$")
        {
            var USD_Alis = xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteBuying").InnerXml;
            var USD_Satis = xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteSelling").InnerXml;
            var result = USD_Satis.ToDecimal(new CultureInfo("en-EN"));

            return result;
        }

        return 0;
    }

    public static decimal ToKur(this decimal? deger, string kur)
    {
        if (deger != null && deger > 0)
        {
            var bugun = "http://www.tcmb.gov.tr/kurlar/today.xml";
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(bugun);

            if (kur == "euro")
            {
                var EURO_Alis = xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteBuying").InnerXml;
                var EURO_Satis = xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteSelling").InnerXml;
                var result = EURO_Satis.ToDecimal(new CultureInfo("en-EN")) * deger;
                return result.Value;
            }

            if (kur == "dolar")
            {
                var USD_Alis = xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteBuying").InnerXml;
                var USD_Satis = xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteSelling").InnerXml;
                var result = USD_Satis.ToDecimal(new CultureInfo("en-EN")) * deger;
                return result.Value;
            }

            return deger.Value;
        }

        return 0;
    }


    public static string GetDisplayName(this Enum value)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));

        var attribute = value.GetType().GetField(value.ToString())
            .GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().FirstOrDefault();

        if (attribute == null) return value.ToString();

        var propValue = attribute.Name;
        return propValue;
    }


    public static string GetDisplayName(this PropertyInfo prop)
    {
        if (prop.CustomAttributes == null || prop.CustomAttributes.Count() == 0)
            return prop.Name;

        var displayNameAttribute = prop.CustomAttributes.Where(x => x.AttributeType == typeof(DisplayNameAttribute))
            .FirstOrDefault();

        if (displayNameAttribute == null || displayNameAttribute.ConstructorArguments == null ||
            displayNameAttribute.ConstructorArguments.Count == 0)
            return prop.Name;

        return displayNameAttribute.ConstructorArguments[0].Value.ToString() ?? prop.Name;
    }

    public static string strSqlColumn(this DataTable table)
    {
        var sql = "";
        foreach (DataColumn column in table.Columns)
        {
            if (sql.Length > 0)
                sql += ", ";
            sql += column.ColumnName;
        }

        return sql;
    }


    public static Dictionary<string, object> GetPropertyAttributes(PropertyInfo property)
    {
        var attribs = new Dictionary<string, object>();
        foreach (var attribData in property.GetCustomAttributesData())
        {
            var typeName = attribData.Constructor.DeclaringType.Name;
            if (typeName.EndsWith("Attribute"))
                typeName = typeName.Substring(0, typeName.Length - 9);

            if (typeName == "Required")
                attribs[typeName] = typeName.ToLower();
            else if (typeName == "DataType")
                attribs[typeName] = attribData.ConstructorArguments[0].Value;
            else
                attribs[typeName] = attribData.ConstructorArguments.Count > 0
                    ? attribData.ConstructorArguments[0].Value
                    : "";
        }

        return attribs;
    }

    public static void SetValueCustom(this object container, string propertyName, object value)
    {
        container.GetType()?.GetProperty(propertyName)?.SetValue(container, value, null);
    }

    public static object GetPropValue(this object obj, string fieldName)
    {
        //<-- fieldName = "Details.Name"
        object value = null;
        var nameParts = fieldName.Split('.');
        foreach (var part in nameParts)
        {
            if (obj == null) return "";

            var type = obj.GetType();
            var info = type.GetProperty(part);
            if (info == null) return "";

            if (info.PropertyType.Name.ToLower().Contains("collection"))
                value = info.GetValue(obj, null) as IList;
            //foreach (var item in list) //<-- this list should be the "Details" property
            //{
            //    value += "," + item;
            //}
            else
                value = info.GetValue(obj, null);
        }

        return value;
    }


    public static Expression<Func<T, bool>> filter<T>(IList<Expression<Func<T, bool>>> predicateExpressions,
        IList<Func<Expression, Expression, BinaryExpression>> logicalFunctions)
    {
        Expression<Func<T, bool>> filter = null;

        if (predicateExpressions.Count > 0)
        {
            var firstPredicate = predicateExpressions[0];
            var body = firstPredicate.Body;
            for (var i = 1; i < predicateExpressions.Count; i++)
                body = logicalFunctions[i - 1](body, predicateExpressions[i].Body);

            filter = Expression.Lambda<Func<T, bool>>(body, firstPredicate.Parameters);
        }

        return filter;
    }


    public static List<string> validControl<T>(this T table, string prop, string errorText)
    {
        var list = new List<string>();

        var p = prop.Split(',').ToList();
        var sourceProperties = table.GetType().GetProperties().ToList();

        p.ForEach(o =>
        {
            var item = sourceProperties.FirstOrDefault(i => i.Name == o);
            if (item != null)
            {
                var cont = item.GetValue(table, null).ToStr();
                if (cont == null || string.IsNullOrEmpty(cont) || cont == "-1")
                    list.Add(item.Name + "  : " + errorText);
            }
        });

        return list;
    }


    public static string ExQuote(this string value)
    {
        return "'" + value.ToStr() + "'";
    }

    public static int GetNumberDigits(int value)
    {
        if (value < 10) return 10;
        var deger = 1;
        var basamak = 1;
        do
        {
            value /= 10;
            basamak++;
        } while (value > 10);

        for (var i = 0; i < basamak; i++) deger *= 10;

        return deger;
    }

    public static string Base64Encode(this string plainText)
    {
        if (plainText == null)
            return plainText;

        var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(plainTextBytes);
    }

    public static string Base64Decode(this string base64EncodedData)
    {
        if (base64EncodedData == null)
            return base64EncodedData;

        var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
        return Encoding.UTF8.GetString(base64EncodedBytes);
    }

    public static DataTable ToDataTable<T>(this List<T> data)
    {
        var properties =
            TypeDescriptor.GetProperties(typeof(T));
        var table = new DataTable();
        foreach (PropertyDescriptor prop in properties)
            table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
        foreach (var item in data)
        {
            var row = table.NewRow();
            foreach (PropertyDescriptor prop in properties)
                row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
            table.Rows.Add(row);
        }

        return table;
    }

    public static DataTable ToDataTable<T>(this IList<T> data)
    {
        var properties =
            TypeDescriptor.GetProperties(typeof(T));
        var table = new DataTable();
        foreach (PropertyDescriptor prop in properties)
            table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
        foreach (var item in data)
        {
            var row = table.NewRow();
            foreach (PropertyDescriptor prop in properties)
                row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
            table.Rows.Add(row);
        }

        return table;
    }

    public static bool tccontrol1(this string kimlikno)
    {
        kimlikno = kimlikno.Trim();
        if (kimlikno.Length != 11) return false;

        var sayilar = new int[11];
        for (var i = 0; i < kimlikno.Length; i++) sayilar[i] = int.Parse(kimlikno[i].ToString());

        var toplam = 0;
        for (var i = 0; i < kimlikno.Length - 1; i++) toplam += sayilar[i];

        if ((toplam.ToString()[1].ToString() == sayilar[10].ToString()) & (sayilar[10] % 2 == 0))
            return true;
        return false;
    }

    public static bool tccontrol2(this string kimlikno)
    {
        var returnvalue = false;
        if (kimlikno.Length == 11)
        {
            long ATCNO, BTCNO, TcNo;
            long C1, C2, C3, C4, C5, C6, C7, C8, C9, Q1, Q2;

            TcNo = long.Parse(kimlikno);

            ATCNO = TcNo / 100;
            BTCNO = TcNo / 100;

            C1 = ATCNO % 10;
            ATCNO = ATCNO / 10;
            C2 = ATCNO % 10;
            ATCNO = ATCNO / 10;
            C3 = ATCNO % 10;
            ATCNO = ATCNO / 10;
            C4 = ATCNO % 10;
            ATCNO = ATCNO / 10;
            C5 = ATCNO % 10;
            ATCNO = ATCNO / 10;
            C6 = ATCNO % 10;
            ATCNO = ATCNO / 10;
            C7 = ATCNO % 10;
            ATCNO = ATCNO / 10;
            C8 = ATCNO % 10;
            ATCNO = ATCNO / 10;
            C9 = ATCNO % 10;
            ATCNO = ATCNO / 10;
            Q1 = (10 - ((C1 + C3 + C5 + C7 + C9) * 3 + C2 + C4 + C6 + C8) % 10) % 10;
            Q2 = (10 - ((C2 + C4 + C6 + C8 + Q1) * 3 + C1 + C3 + C5 + C7 + C9) % 10) % 10;

            returnvalue = BTCNO * 100 + Q1 * 10 + Q2 == TcNo;
        }

        return returnvalue;
    }


    /// <summary>
    ///     Add Quote to string value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ExToUpper(this string value)
    {
        var deger = string.Empty;
        for (var i = 0; i < value.Length; i++) deger += GetUpperChar(value[i]);

        return deger.ToUpper();
    }

    public static string ExToLower(this string value)
    {
        var deger = string.Empty;
        for (var i = 0; i < value.Length; i++) deger += GetLowerChar(value[i]);

        return deger;
    }

    private static string GetUpperChar(char value)
    {
        var deger = value.ToString();
        switch (value)
        {
            case 'i':
                deger = "I";
                break;
        }

        return deger;
    }

    private static string GetLowerChar(char value)
    {
        var deger = value.ToString();
        switch (value)
        {
            case 'I':
                deger = "i";
                break;
        }

        return deger.ToLower();
    }


    public static string ToStr(this object key)
    {
        return key == null ? string.Empty : key.ToString();
    }

    public static char ToChar(this object key)
    {
        return Convert.ToChar(key);
    }

    public static string ToStrDate(this DateTime key, string format)
    {
        return key == null ? string.Empty : key.ToString("dd" + format + "MM" + format + "yyyy");
    }

    public static bool ToBoolean(this object key)
    {
        var deger = false;
        if (key != null)
        {
            if (key.ToString().Contains("True") || key.ToString().Contains("False"))
                bool.TryParse(key.ToString(), out deger);
            else
                deger = Convert.ToBoolean(key.ToInt());
        }

        return deger;
    }

    public static int ToInt(this object key)
    {
        var value = 0;
        if (key != null)
            int.TryParse(key.ToString(), out value);
        return value;
    }

    public static long ToLong(this object key)
    {
        long value = 0;
        if (key != null)
            long.TryParse(key.ToString(), out value);
        return value;
    }

    public static int ToInt(this object key, int value)
    {
        var ret = value;
        if (key != null)
            if (!int.TryParse(key.ToString(), out value))
                value = ret;

        return value;
    }

    public static decimal ToDecimal(this object key, CultureInfo Culture)
    {
        decimal value = 0;
        if (key != null)
            decimal.TryParse(key.ToString(), NumberStyles.Any, Culture, out value);
        return value;
    }

    public static decimal ToDecimal(this object key)
    {
        decimal value = 0;
        if (key != null)
            decimal.TryParse(key.ToString(), NumberStyles.Any, new CultureInfo("tr-TR"), out value);
        return value;
    }

    public static decimal ToDecimal(this object key, decimal value, CultureInfo Culture)
    {
        var ret = value;
        if (key != null)
            if (!decimal.TryParse(key.ToString(), NumberStyles.Any, Culture, out value))
                value = ret;

        return value;
    }

    public static decimal ToDecimal(this object key, decimal value)
    {
        var ret = value;
        if (key != null)
            if (!decimal.TryParse(key.ToString(), NumberStyles.Any, new CultureInfo("tr-TR"), out value))
                value = ret;

        return value;
    }

    public static string toFixed(this double number, uint decimals)
    {
        return number.ToString("N" + decimals);
    }

    public static double ToDouble(this object key)
    {
        double value = 0;
        if (key != null)
            double.TryParse(key.ToString(), NumberStyles.Any, new CultureInfo("tr-TR"), out value);
        return value;
    }

    public static double ToDouble(this object key, CultureInfo Culture)
    {
        double value = 0;
        if (key != null)
            double.TryParse(key.ToString(), NumberStyles.Any, Culture, out value);
        return value;
    }

    public static double ToDouble(this object key, double value, CultureInfo Culture)
    {
        var ret = value;
        if (key != null)
            if (!double.TryParse(key.ToString(), NumberStyles.Any, Culture, out value))
                value = ret;

        return value;
    }

    public static DateTime? ToDateTime(this object key)
    {
        DateTime? deger = null;
        if (key != null)
            try
            {
                var date = new DateTime();
                if (DateTime.TryParse(key.ToString(), out date))
                    return date;
                return null;
            }
            catch
            {
                Console.WriteLine(@"AIop81F9y0ORvI5v98QT");
            }

        return deger;
    }

    public static string ToYMD(this DateTime theDate)
    {
        return theDate.ToString("yyyyMMdd");
    }

    public static string ToYMD(this DateTime? theDate)
    {
        return theDate.HasValue ? theDate.Value.ToYMD() : string.Empty;
    }

    public static int TryParseToInt(this string Deger, int value)
    {
        int.TryParse(Deger, out value);
        return value;
    }

    public static double TryParseToDouble(this string Deger, double value)
    {
        double.TryParse(Deger, out value);
        return value;
    }

    public static decimal TryParseToDecimal(this string deger, decimal value)
    {
        decimal.TryParse(deger, out value);
        return decimal.Round(value, 2);
    }

    public static decimal TryParseToDecimal(this string deger, decimal value, bool round)
    {
        decimal.TryParse(deger, out value);
        if (round) return decimal.Round(value, 2);
        return value;
    }

    public static string QuotedStr(this object columnValue)
    {
        if (columnValue == null) columnValue = "";
        switch (Type.GetTypeCode(columnValue.GetType()))
        {
            case TypeCode.String:
                return "'" + EscapeText(columnValue.ToString()) + "'";
            default:
                return EscapeText(columnValue.ToString());
        }
    }

    public static string EscapeText(string textToEscape)
    {
        var backslashesEscaped = textToEscape.Replace(@"\", @"\\");
        var backslashAndSingleQuoteEscaped = backslashesEscaped.Replace(@"'", @"\'");
        return backslashAndSingleQuoteEscaped;
    }

    public static int ExGetWeekIndex(this DayOfWeek week)
    {
        var value = -1;

        switch (week)
        {
            case DayOfWeek.Friday:
                value = 4;
                break;
            case DayOfWeek.Monday:
                value = 0;
                break;
            case DayOfWeek.Saturday:
                value = 5;
                break;
            case DayOfWeek.Sunday:
                value = 6;
                break;
            case DayOfWeek.Thursday:
                value = 3;
                break;
            case DayOfWeek.Tuesday:
                value = 1;
                break;
            case DayOfWeek.Wednesday:
                value = 2;
                break;
        }

        return value;
    }

    public static bool IsDate(string str)
    {
        var res = false;
        try
        {
            var dt = DateTime.Parse(str);
            res = true;
        }
        catch
        {
            Console.WriteLine(@"rKrZpQ7cKEdfARKM7swZ");
            // Not a date, handle appropriately
            res = false;
        }

        return res;
    }

    public static bool IsBetween<T>(this T value, T min, T max) where T : IComparable
    {
        //return Comparer<T>.Default.Compare(value, min) >= 0
        //    && Comparer<T>.Default.Compare(value, max) <= 0;
        return min.CompareTo(value) <= 0 && value.CompareTo(max) <= 0;
    }

    public static bool InRange<T>(this T value, params T[] values)
    {
        // Should be even number of items
        Debug.Assert(values.Length % 2 == 0);

        for (var i = 0; i < values.Length; i += 2)
            if (!value.InRange(values[i], values[i + 1]))
                return false;

        return true;
    }

    public static Task<List<T>> ToListAsync<T>(this IQueryable<T> list)
    {
        return Task.Run(() => list.ToList());
    }

    /// <summary>
    ///     Verilen doğum tarihine göre yaş bilgisi döner
    /// </summary>
    /// <param name="BirthDate"></param>
    /// <returns></returns>
    public static int ExGetAge(this DateTime? BirthDate)
    {
        if (BirthDate.HasValue)
            return DateTime.Now.Date.Subtract(BirthDate.Value.Date).TotalDays.ToInt() / 365;
        return 0;
    }

    public static int ExGetAge(this DateTime BirthDate)
    {
        return DateTime.Now.Date.Subtract(BirthDate.Date).TotalDays.ToInt() / 365;
    }

    public static int ExGetAge(this DateTime BirthDate, DateTime RegisterDate)
    {
        return RegisterDate.Date.Subtract(BirthDate.Date).TotalDays.ToInt() / 365;
    }

    public static List<Dictionary<string, object>> Read(DbDataReader reader)
    {
        var expandolist = new List<Dictionary<string, object>>();
        foreach (var item in reader)
        {
            IDictionary<string, object> expando = new ExpandoObject();
            foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(item))
            {
                var obj = propertyDescriptor.GetValue(item);
                expando.Add(propertyDescriptor.Name, obj);
            }

            expandolist.Add(new Dictionary<string, object>(expando));
        }

        return expandolist;
    }

    public static Dictionary<string, object> DictionaryFromType(this object atype)
    {
        if (atype == null) return new Dictionary<string, object>();
        var t = atype.GetType();
        var props = t.GetProperties();
        var dict = new Dictionary<string, object>();
        foreach (var prp in props)
        {
            var value = prp.GetValue(atype, new object[] { });
            dict.Add(prp.Name, value);
        }

        return dict;
    }

    public static string[] PropertiesFromType(this object atype)
    {
        if (atype == null) return new string[] { };
        var t = atype.GetType();
        var props = t.GetProperties();
        var propNames = new List<string>();
        foreach (var prp in props) propNames.Add(prp.Name);

        return propNames.ToArray();
    }


    public static string ToEn(this string text)
    {
        return string.Join("", text.Normalize(NormalizationForm.FormD)
            .Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark));
    }

    public static string toTr(this string str)
    {
        string once, sonra;
        once = str;
        sonra = once.Replace('ı', 'i');
        once = sonra.Replace('ö', 'o');
        sonra = once.Replace('ü', 'u');
        once = sonra.Replace('ş', 's');
        sonra = once.Replace('ğ', 'g');
        once = sonra.Replace('ç', 'c');
        sonra = once.Replace('İ', 'I');
        once = sonra.Replace('Ö', 'O');
        sonra = once.Replace('Ü', 'U');
        once = sonra.Replace('Ş', 'S');
        sonra = once.Replace('Ğ', 'G');
        once = sonra.Replace('Ç', 'C');
        str = once;
        return str;
    }

    public static string toCustomTr(this string str)
    {
        str = str.Trim().Replace(".", "").Replace(" ", "").ToLower(new CultureInfo("tr-TR", false)).toTr();
        return str;
    }

    public static void setUrl(this string url)
    {
        Process.Start(url);
    }

    public static string getStr(this string str, string start, string end)
    {
        var first = str.IndexOf(start);
        var last = str.LastIndexOf(end);
        var str2 = str.Substring(first + 1, last - first - 1);
        return str2;
    }

    public static double ToMaxMin(bool durum, params double[] args)
    {
        return durum ? args.ToList().Max() : args.ToList().Min();
    }

    public static List<KeyValuePair<string, string>> GetEnumValuesAndDescriptions<T>()
    {
        var enumType = typeof(T);

        if (enumType.BaseType != typeof(Enum))
            throw new ArgumentException("T is not System.Enum");

        var enumValList = new List<KeyValuePair<string, string>>();

        foreach (var e in Enum.GetValues(typeof(T)))
        {
            var fi = e.GetType().GetField(e.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            enumValList.Add(new KeyValuePair<string, string>(attributes.Length > 0 ? e.ToString() : e.ToString(),
                attributes[0].Description));
        }

        return enumValList;
    }

    public static List<string> GetEnumDescriptions<T>()
    {
        var enumType = typeof(T);

        if (enumType.BaseType != typeof(Enum))
            throw new ArgumentException("T is not System.Enum");

        var enumValList = new List<string>();

        foreach (var e in Enum.GetValues(typeof(T)))
        {
            var fi = e.GetType().GetField(e.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            enumValList.Add(attributes[0].Description);
        }

        return enumValList;
    }

    public static void noSetValueCustom<T>(this object container, string nopropertyName, object value, object total)
    {
        var prop = container.GetType().GetProperties().ToList();
        prop.ForEach(o =>
        {
            if (!nopropertyName.Contains(o.Name) && o.CanWrite)
            {
                var val = container.GetPropValue(o.Name);
                if (val != null)
                    container.SetValueCustom(o.Name,
                        (val.ToDouble() * value.ToDouble() / total.ToDouble()).ToString("0.##"));
            }
            else
            {
                var val = container.GetPropValue(o.Name);
                if (val != null)
                    container.SetValueCustom(o.Name, val);
            }
        });
    }

    public static void noSetValueCustom2<T>(this object container2, List<T> containerList, string nopropertyName,
        string propertyName)
    {
        container2.GetType().GetProperties().ToList().ForEach(o =>
        {
            if (!nopropertyName.Contains(o.Name) &&
                (!string.IsNullOrEmpty(propertyName) ? propertyName.Contains(o.Name) : true) && o.CanWrite)
            {
                var total = new List<double>();
                containerList.Where(l => l.GetType().GetProperty(o.Name).CanWrite).ToList().ForEach(oo =>
                {
                    var vals = oo.GetPropValue(o.Name).ToDouble();
                    total.Add(vals);
                });
                var val = container2.GetPropValue(o.Name).ToDouble();
                total.Add(val);
                if (total.Count > 0)
                    container2.SetValueCustom(o.Name, total.Average().ToString("0.##"));
            }
            else
            {
                containerList.ForEach(oo =>
                {
                    var vals = oo.GetPropValue(o.Name);
                    container2.SetValueCustom(o.Name, vals);
                });
            }
        });
    }


    public static string GetCleanText(this string text)
    {
        var outtext = text.ToLower(CultureInfo.GetCultureInfo("en"));
        outtext = ClearTurkish(outtext);
        var validchars = "abcdefghijklmnopqrstuvwxyz0123456789";
        var outvalidtext = "";
        for (var x = 0; x < outtext.Length; x++)
            if (validchars.IndexOf(outtext.Substring(x, 1)) != -1)
                outvalidtext += outtext.Substring(x, 1);
            else
                outvalidtext += "-";
        return outvalidtext.Replace("---", "-").Replace("--", "-").Replace("+", "_arti_");
    }

    public static string ClearTurkish(this string input)
    {
        return
            StripHtml(input)
                .Replace("Ç", "C")
                .Replace("ç", "c")
                .Replace("Ğ", "G")
                .Replace("ğ", "g")
                .Replace("İ", "I")
                .Replace("ı", "i")
                .Replace("Ö", "ö")
                .Replace("ö", "o")
                .Replace("Ş", "S")
                .Replace("ş", "s")
                .Replace("Ü", "U")
                .Replace("ü", "u")
                .Replace("â", "a")
                .Replace("?", "-")
                .Replace("=", "-")
                .Replace("!", "-")
                .Replace(" ", "-")
                .Replace("&", "-")
                .Replace("%", "-")
                .Replace("\\", "-")
                .Replace("(", "-")
                .Replace(")", "-")
                .Replace("<", "-")
                .Replace(">", "-")
                .Replace("'", "-")
                .Replace("\"", "-")
                .Replace("\n", "-")
                .Replace(".", "-")
                .Replace(",", "-")
                .Replace("@", "-")
                .Replace(":", "-")
                .Replace(";", "-")
                .Replace("“", "-")
                .Replace("”", "-")
                .Replace("/", "-")
                .Replace("+", "_arti_");
    }

    public static string GetCleanTextSearch(this string text)
    {
        var outtext = text.ToLower(CultureInfo.GetCultureInfo("en"));
        var validchars = "abcçdefgğhıijklmnoöpqrsştuüvwxyz0123456789 ";
        var outvalidtext = "";
        for (var x = 0; x < outtext.Length; x++)
            if (validchars.IndexOf(outtext.Substring(x, 1)) != -1)
                outvalidtext += text.Substring(x, 1);
            else
                outvalidtext += "";

        var removedless2chars = "";
        foreach (var holder in outvalidtext.Trim().Split(' '))
            if (holder.Trim().Length > 2)
            {
                removedless2chars += holder;
                removedless2chars += " ";
            }

        return removedless2chars.Trim();
    }

    public static string ReplaceIllegalCharacters(this string val, string replaceCharacter = "")
    {
        replaceCharacter = replaceCharacter ?? "";
        if (string.IsNullOrEmpty(val))
            return val;
        val = val.Replace(" ", "")
            .Replace("!", replaceCharacter)
            .Replace("'", replaceCharacter)
            .Replace("^", replaceCharacter)
            .Replace("%", replaceCharacter)
            .Replace("&", replaceCharacter)
            .Replace("/", replaceCharacter)
            .Replace("(", replaceCharacter)
            .Replace(")", replaceCharacter)
            .Replace("=", replaceCharacter)
            .Replace("?", replaceCharacter)
            .Replace("<", replaceCharacter)
            .Replace(">", replaceCharacter)
            .Replace("£", replaceCharacter)
            .Replace("#", replaceCharacter)
            .Replace("½", replaceCharacter)
            .Replace("{", replaceCharacter)
            .Replace("[", replaceCharacter)
            .Replace("]", replaceCharacter)
            .Replace("}", replaceCharacter)
            .Replace("\\", replaceCharacter)
            .Replace("|", replaceCharacter)
            .Replace("*", replaceCharacter)
            .Replace("é", replaceCharacter)
            .Replace("¨", replaceCharacter)
            .Replace("~", replaceCharacter)
            .Replace("`", replaceCharacter)
            .Replace(";", replaceCharacter)
            .Replace(":", replaceCharacter)
            .Replace(" ", replaceCharacter);
        return val;
    }

    public static string StripHtml(this string text)
    {
        return Regex.Replace(text, @"<(.|\n)*?>", string.Empty);
    }


    public static string ceoUrl(this string url)
    {
        if (string.IsNullOrEmpty(url)) return "";
        url = url.ToLower();
        url = url.Trim();
        //if (url.Length > 100)
        //{
        //    url = url.Substring(0, 100);
        //}
        url = url.Replace("İ", "I");
        url = url.Replace("ı", "i");
        url = url.Replace("ğ", "g");
        url = url.Replace("Ğ", "G");
        url = url.Replace("ç", "c");
        url = url.Replace("Ç", "C");
        url = url.Replace("ö", "o");
        url = url.Replace("Ö", "O");
        url = url.Replace("ş", "s");
        url = url.Replace("Ş", "S");
        url = url.Replace("ü", "u");
        url = url.Replace("Ü", "U");
        url = url.Replace("'", "");
        url = url.Replace("\"", "");
        var replacerList = @"$%#@!*?;:~`+=()[]{}|\'<>,/^&"".".ToCharArray();
        for (var i = 0; i < replacerList.Length; i++)
        {
            var strChr = replacerList[i].ToString();
            if (url.Contains(strChr)) url = url.Replace(strChr, string.Empty);
        }

        var r = new Regex("[^a-zA-Z0-9_-]");
        url = r.Replace(url, "-");
        while (url.IndexOf("--") > -1)
            url = url.Replace("--", "-");

        return url;
    }


    public static async Task ForEachAsync<T>(this List<T> list, Func<T, Task> func)
    {
        foreach (var value in list) await func(value);
    }


    public static string ToQueryString(this object request, string separator = ",")
    {
        if (request == null)
            throw new ArgumentNullException("request");

        // Get all properties on the object
        var properties = request.GetType().GetProperties()
            .Where(x => x.CanRead)
            .Where(x => x.GetValue(request, null) != null)
            .ToDictionary(x => x.Name, x => x.GetValue(request, null));

        // Get names for all IEnumerable properties (excl. string)
        var propertyNames = properties
            .Where(x => !(x.Value is string) && x.Value is IEnumerable)
            .Select(x => x.Key)
            .ToList();

        // Concat all IEnumerable properties into a comma separated string
        foreach (var key in propertyNames)
        {
            var valueType = properties[key].GetType();
            var valueElemType = valueType.IsGenericType
                ? valueType.GetGenericArguments()[0]
                : valueType.GetElementType();
            if (valueElemType.IsPrimitive || valueElemType == typeof(string))
            {
                var enumerable = properties[key] as IEnumerable;
                properties[key] = string.Join(separator, enumerable.Cast<object>());
            }
        }

        // Concat all key/value pairs into a string separated by ampersand
        return string.Join("&", properties
            .Select(x => string.Concat(
                Uri.EscapeDataString(x.Key), "=",
                Uri.EscapeDataString(x.Value.ToString()))));
    }


    public static bool isMail(this string email)
    {
        try
        {
            var addr = new MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public static string limit(this string source, int maxLength)
    {
        if (source.Length <= maxLength) return source;

        return source.Substring(0, maxLength);
    }

    #region ENUMS

    public static List<KeyValuePair<string, int>> GetEnumList<T>()
    {
        var list = new List<KeyValuePair<string, int>>();
        foreach (var e in Enum.GetValues(typeof(T))) list.Add(new KeyValuePair<string, int>(e.ToString(), (int)e));

        return list;
    }


    public static string ExGetEnumDescription(this string Text, Enum value)
    {
        if (value == null)
            Text = string.Empty;

        var fi = value.GetType().GetField(value.ToString());

        if (fi == null)
            Text = string.Empty;

        var attributes =
            (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

        if (attributes != null &&
            attributes.Length > 0)
            Text = attributes[0].Description;
        else
            Text = value.ToStr();

        return Text;
    }

    public static string ExGetDescription(this Enum value)
    {
        if (value == null)
            return string.Empty;

        var fi = value.GetType().GetField(value.ToString());

        if (fi == null)
            return string.Empty;

        var attributes =
            (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

        if (attributes != null &&
            attributes.Length > 0)
            return attributes[0].Description;
        return value.ToStr();
    }

    public static string ExGetDescription(this PropertyInfo property)
    {
        if (property == null)
            return string.Empty;

        var attributes =
            (DescriptionAttribute[])property.GetCustomAttributes(typeof(DescriptionAttribute), false);

        if (attributes != null &&
            attributes.Length > 0)
            return attributes[0].Description;
        return property.Name.ToStr();
    }

    public static string ExGetDescription(this Type property)
    {
        if (property == null)
            return string.Empty;

        var attributes =
            (DescriptionAttribute[])property.GetCustomAttributes(typeof(DescriptionAttribute), false);

        if (attributes != null &&
            attributes.Length > 0)
            return attributes[0].Description;
        return property.Name.ToStr();
    }

    #endregion

    #region Data Extension

    public static T NewClass<T>(this T source) where T : class, new()
    {
        var destination = new T();
        PropertyInfo[] properties;
        var type = typeof(T);
        lock (_TypesWithWriteableProperties)
        {
            if (!_TypesWithWriteableProperties.TryGetValue(type, out properties))
            {
                var props = new List<PropertyInfo>();
                var classProps = type.GetProperties();
                foreach (var prop in classProps)
                    if (prop.CanWrite)
                        props.Add(prop);

                properties = props.ToArray();
                _TypesWithWriteableProperties[type] = properties;
            }
        }

        foreach (var prop in properties)
        {
            var value = prop.GetValue(source);
            try
            {
                prop.SetValue(destination, value);
            }
            catch
            {
                Console.WriteLine(@"kNWvMLUwguPX9gSnB70n");
            }
        }

        return destination;
    }

    public static T Copy<T, U>(this U source) where T : class, new()
    {
        var destination = new T();
        var destinationProperties = destination.GetType().GetProperties().ToList();
        var sourceProperties = source.GetType().GetProperties().ToList();

        foreach (var destinationProperty in destinationProperties)
        {
            var sourceProperty = sourceProperties.Find(item => item.Name == destinationProperty.Name);

            if (sourceProperty != null && destinationProperty.CanWrite)
                try
                {
                    destinationProperty.SetValue(destination, sourceProperty.GetValue(source, null), null);
                }
                catch (Exception)
                {
                    Console.WriteLine(@"QAZlpQtSbuxuePR89Znk");
                }
        }

        return destination;
    }

    #endregion
}