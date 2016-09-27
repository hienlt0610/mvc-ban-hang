using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;

namespace WebBanHang.Utils
{
    public static class HtmlExtension
    {
        public static String FormatCurrency(this HtmlHelper helper, long? price)
        {
            return price.GetValueOrDefault(0).ToString("#,##0");
        }

        public static String FormatCurrency(this HtmlHelper helper, long price)
        {
            return price.ToString("#,##0");
        }

        public static String FormatCurrency(this HtmlHelper helper, int price)
        {
            return price.ToString("#,##0");
        }

        public static String FormatCurrency(this HtmlHelper helper, int? price)
        {
            return price.GetValueOrDefault(0).ToString("#,##0");
        }

        // ValidationLabel
        public static MvcHtmlString ValidationLabel(this HtmlHelper htmlHelper,
                                                    string modelName,
                                                    string labelText,
                                                    IDictionary<string, object> htmlAttributes)
        {
            if (modelName == null) { throw new ArgumentNullException("modelName"); }
            ModelState modelState = htmlHelper.ViewData.ModelState[modelName];
            ModelErrorCollection modelErrors = (modelState == null) ? null : modelState.Errors;
            ModelError modelError = ((modelErrors == null) || (modelErrors.Count == 0)) ? null : modelErrors[0];
            // If there is no error, we want to show a label.  If there is an error,
            // we want to show the error message.
            string tagText = labelText;
            string tagClass = "form_field_label_normal";
            if ((modelState != null) && (modelError != null))
            {
                tagText = modelError.ErrorMessage;
                tagClass = "form_field_label_error";
            }
            // Build out the tag
            TagBuilder builder = new TagBuilder("label");
            builder.MergeAttributes(htmlAttributes);
            builder.MergeAttribute("class", tagClass);
            builder.MergeAttribute("validationlabelfor", modelName);
            builder.SetInnerText(tagText);
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString ValidationLabel(this HtmlHelper htmlHelper,string modelName, string labelClass =  null)
        {
            Dictionary<string, object> attr = null;
            if (labelClass != null)
                attr = new Dictionary<String, object> { { "class", labelClass } };
            return HtmlExtension.ValidationLabel(htmlHelper, modelName, null, attr);
        }
    }
}