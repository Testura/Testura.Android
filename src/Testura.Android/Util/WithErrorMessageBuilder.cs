using System.Text;
using Testura.Android.Device.Ui.Search;

namespace Testura.Android.Util
{
    internal static class WithErrorMessageBuilder
    {
        internal static string BuildByErrorMessage(IList<Where> wheres)
        {
            if (wheres == null || !wheres.Any())
            {
                return "Could not map node (did not provide any \"wheres\"?";
            }

            var errorMessage = new StringBuilder();
            errorMessage.Append("Could not find node where ");
            if (wheres.Count == 1)
            {
                errorMessage.Append(wheres.First().ErrorMessage);
            }
            else
            {
                for (var n = 0; n < wheres.Count; n++)
                {
                    errorMessage.Append($"{wheres[n].ErrorMessage}");
                    if (n < wheres.Count - 2)
                    {
                        errorMessage.Append(", ");
                    }
                    else if (n == wheres.Count - 2)
                    {
                        errorMessage.Append(" and ");
                    }
                }
            }

            return errorMessage.ToString();
        }
    }
}
