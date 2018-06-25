using System.Collections.Generic;
using System.Linq;
using System.Text;
using Testura.Android.Device.Ui.Search;

namespace Testura.Android.Util
{
    internal static class WithErrorMessageBuilder
    {
        internal static string BuildWithErrorMessage(IList<With> withs)
        {
            if (withs == null || !withs.Any())
            {
                return "Could not find node";
            }

            var errorMessage = new StringBuilder();
            errorMessage.Append("Could not find node where ");
            if (withs.Count == 1)
            {
                errorMessage.Append(withs.First().ErrorMessage);
            }
            else
            {
                for (var n = 0; n < withs.Count; n++)
                {
                    errorMessage.Append($"{withs[n].ErrorMessage}");
                    if (n < withs.Count - 2)
                    {
                        errorMessage.Append(", ");
                    }
                    else if (n == withs.Count - 2)
                    {
                        errorMessage.Append(" and ");
                    }
                }
            }

            return errorMessage.ToString();
        }
    }
}
