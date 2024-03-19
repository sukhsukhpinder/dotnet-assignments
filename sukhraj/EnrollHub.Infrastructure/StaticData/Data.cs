namespace EnrollHub.Infrastructure.StaticData
{
    public static class Data
    {
        public static readonly Dictionary<int, string> states = new Dictionary<int, string>
    {
        { 1, "Andhra Pradesh" },
        { 2, "Arunachal Pradesh" },
        { 3, "Punjab" },
        { 4, "Chandigarh" },
        { 5, "Haryana" },
        { 6, "Himachal Pradesh" },
        { 7, "Delhi" }
    };

        public static readonly Dictionary<int, string> courses = new Dictionary<int, string>
    {
        { 1, "BA" },
        { 2, "BCA" },
        { 3, "MBA" },
        { 4, "BTech" },
    };

        public static int FindKeyByValue(Dictionary<int, string> dictionary, string searchValue)
        {
            foreach (var pair in dictionary)
            {
                if (EqualityComparer<string>.Default.Equals(pair.Value, searchValue))
                {
                    return pair.Key;
                }
            }
            return -1;
        }
    }
}
