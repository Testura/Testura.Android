using System.Collections.Generic;
using System.Linq;
using System.Text;
using Testura.Android.Device.Ui.Search;

namespace Testura.Android.Util
{
    internal static class ByErrorMessageBuilder
    {
        internal static string BuildByErrorMessage(IList<By> bys)
        {
            if (bys == null || !bys.Any())
            {
                return "Could not map node (did not provide any \"bys\"?";
            }

            var errorMessage = new StringBuilder();
            errorMessage.Append("Could not find node where ");
            if (bys.Count == 1)
            {
                errorMessage.Append(bys.First().ErrorMessage);
            }
            else
            {
                for (var n = 0; n < bys.Count; n++)
                {
                    errorMessage.Append($"{bys[n].ErrorMessage}");
                    if (n < bys.Count - 2)
                    {
                        errorMessage.Append(", ");
                    }
                    else if (n == bys.Count - 2)
                    {
                        errorMessage.Append(" and ");
                    }
                }
            }

            return errorMessage.ToString();
        }
    }
}
