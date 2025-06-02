using System.Collections.Generic;
using System.Linq;

namespace Tweb_lavender_paradise.Helpers
{
    public static class CartHelper
    {
        // Преобразует строку "{1:2},{3:1}" в словарь {1:2, 3:1}
        public static Dictionary<int, int> Parse(string goods)
        {
            var result = new Dictionary<int, int>();
            if (string.IsNullOrWhiteSpace(goods)) return result;

            var items = goods.Split(',');
            foreach (var item in items)
            {
                var trimmed = item.Trim('{', '}');
                var parts = trimmed.Split(':');
                if (parts.Length == 2 &&
                    int.TryParse(parts[0], out int id) &&
                    int.TryParse(parts[1], out int qty))
                {
                    result[id] = qty;
                }
            }

            return result;
        }

        // Преобразует словарь {1:2, 3:1} в строку "{1:2},{3:1}"
        public static string Serialize(Dictionary<int, int> goods)
        {
            return string.Join(",", goods.Select(kvp => $"{{{kvp.Key}:{kvp.Value}}}"));
        }
    }
}
