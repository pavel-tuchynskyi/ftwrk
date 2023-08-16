namespace FTWRK.Application.Common.Helpers
{
    public static class EmailMessageHelper
    {
        public static string ReplacePlaceholders(string template, params string[] placeholers)
        {
            foreach (var placeholer in placeholers)
            {
                template = String.Format(template, placeholer);
            }

            return template;
        }
    }
}
