using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Mvc.Html;
using System.Linq.Expressions;
using System.Web.Mvc.Ajax;

namespace Eurovision.Helpers
{
    public static class StringExtensionMethods
    {
        /// <summary>
        /// Extention method to allow a string comparison where you can supply the comparison type 
        /// (i.e. ignore case, etc).
        /// </summary>
        /// <param name="value">The compare string.</param>
        /// <param name="comparisionType">The comparison type - enum, use OrdinalIgnoreCase to ignore case.</param>
        /// <returns>Returns true if the string is present within the original string.    </returns>
        public static bool Contains(this string original, string value, StringComparison StringComparison = StringComparison.OrdinalIgnoreCase)
        {
            if (original == null)
            {
                return false;
            }
            else
            {
                bool result =  original.IndexOf(value, StringComparison) >= 0;
                return result;
            }
        }
    }
    public static class HtmlHelperExtensions
    {

        public enum MessageType
        {
            OK,
            Comment,
            Warning,
            Error
        }
        
        public static MvcHtmlString MailTo(this HtmlHelper htmlHelper, string mailaddress)
        {
            TagBuilder link = new TagBuilder("a");
            link.Attributes.Add("href", string.Format("mailto:{0}",mailaddress));
            link.SetInnerText(mailaddress);
            return MvcHtmlString.Create(link.ToString());
        }
        public static MvcHtmlString MailTo(this HtmlHelper htmlHelper, string DisplayText, string MailAddress, string Subject, string Body)
        {
            TagBuilder link = new TagBuilder("a");
            link.Attributes.Add("href", string.Format("mailto:{0}?Subject={1}&Body={2}",MailAddress,Subject,Body));
            link.SetInnerText(DisplayText);
            link.AddCssClass("createButton");
            return MvcHtmlString.Create(link.ToString());
        }


        /// <summary>
        /// Given a Model's property, a controller, and a method that belongs to that controller, 
        /// this function will create an input html element with a data-autocomplete-url property
        /// with the method the autocomplete will need to call the method. A HiddenFor will be
        /// created for the Model's property passed in, so the HiddenFor will be validated 
        /// and the html input will not.
        /// </summary>
        /// <returns></returns>
        public static MvcHtmlString AutocompleteWithHiddenFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, string controllerName, string actionName, object value = null)
        {
            // Create the URL of the Autocomplete function
            string autocompleteUrl = UrlHelper.GenerateUrl(null, actionName,
                                                         controllerName,
                                                         null,
                                                         html.RouteCollection,
                                                         html.ViewContext.RequestContext,
                                                         includeImplicitMvcValues: true);

            // Create the input[type='text'] html element, that does 
            // not need to be aware of the model

            String textbox = "<input type='text' ID='"+ expression.Body.ToString().Replace(".","_") + "' data-autocomplete-url='" + autocompleteUrl + "'";

            // However, it might need to be have a value already populated
            if (value != null)
            {
                textbox += "value='" + value.ToString() + "'";
            }

            // close out the tag
            textbox += " />";

            // A validation message that will fire depending on any 
            // attributes placed on the property
            MvcHtmlString valid = html.ValidationMessageFor(expression);

            // The HiddenFor that will bind to the ID needed rather than 
            // the text received from the Autocomplete
            MvcHtmlString hidden = html.HiddenFor(expression);

            string both = textbox + " " + hidden + " " + valid;
            return MvcHtmlString.Create(both);
        }
        public static MvcHtmlString DropDownListWithSubmit(this HtmlHelper htmlHelper, string name, string optionLabel)
        {
            MvcHtmlString DDL = htmlHelper.DropDownList(name, optionLabel);
            return MvcHtmlString.Create(DDL.ToString().Replace("id=", "onchange=\"$(this.form).submit();\" id="));
        }
        public static MvcHtmlString DropDownListForWithSubmit<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionLabel, string defaultValue)
        {
            MvcHtmlString DDLF = htmlHelper.DropDownListFor(expression, selectList, optionLabel, null);
            return MvcHtmlString.Create(DDLF.ToString().Replace("id=", "onchange=\"$(this.form).submit();\" id=").Replace("alue=\"\"", string.Format("alue=\"{0}\"", defaultValue)));
        }
        public static MvcHtmlString Timeago(this HtmlHelper helper, DateTime? dateTime)
        {
            if (dateTime == null)
            {
                var tag = new TagBuilder("span");
                tag.SetInnerText("Never");
                return MvcHtmlString.Create(tag.ToString());
            }
            else
            {
                DateTime output = ((DateTime)dateTime);
                var tag = new TagBuilder("abbr");
                tag.AddCssClass("timeago");
                tag.Attributes.Add("title", ((DateTime)dateTime).ToString("s"));
                tag.SetInnerText(output.ToString("f"));
                return MvcHtmlString.Create(tag.ToString());
            }
        }
        public static MvcHtmlString DisplayMessage(this HtmlHelper helper, MessageType MessageType, string header, string text)
        {
            TagBuilder msg = new TagBuilder("div");
            msg.MergeAttribute("style", "margin:20px");
            msg.AddCssClass(string.Format("message {0}", MessageType.ToString().ToLower()));

            msg.InnerHtml += "<span></span>";
            msg.InnerHtml += string.Format("<h6>{0}</h6>", header);
            msg.InnerHtml += string.Format("<p>{0}</p>", text);

            return MvcHtmlString.Create(msg.ToString());
        }
        /// <summary>
        /// Set Focus on a web page control at page load
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="helper"></param>
        /// <param name="Control">Control to set focus on at page load</param>
        /// <returns></returns>
        public static MvcHtmlString SetFocus<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> Control)
        {
            var propertyName = ExpressionHelper.GetExpressionText(Control);
            return MvcHtmlString.Create(string.Format("<script type=\"text/javascript\">document.getElementById('{0}').focus()</script>", propertyName.Replace('.','_')));
        }

        /// <summary>
        /// Set Focus on one of two controls, used primarily for logon and password.
        /// If the primary object has a value, focus is set on the secondary object
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="helper"></param>
        /// <param name="primary">First object to focus on</param>
        /// <param name="secondary">Object to focus on if primary control has data</param>
        /// <returns></returns>
        public static MvcHtmlString SetFocus<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> primary, Expression<Func<TModel, TProperty>> secondary)
        {
            var value = ModelMetadata.FromLambdaExpression(primary, helper.ViewData).Model;

            var propertyName = "";
            if (value == null)
            {
                propertyName = ExpressionHelper.GetExpressionText(primary);
            }
            else
            {
                propertyName = ExpressionHelper.GetExpressionText(secondary);
            }

            //var propertyName = ExpressionHelper.GetExpressionText(primary);
            return MvcHtmlString.Create(string.Format("<script type=\"text/javascript\">document.getElementById('{0}').focus()</script>", propertyName));
        }
        #region CBHelpers
        public static MvcHtmlString DropDownList(this HtmlHelper helper,
            string name, Dictionary<int, string> dictionary)
        {
            var selectListItems = new SelectList(dictionary, "Key", "Value");
            return helper.DropDownList(name, selectListItems);
        }

        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression, Dictionary<int, string> dictionary)
        {
            var selectListItems = new SelectList(dictionary, "Key", "Value");
            return helper.DropDownListFor(expression, selectListItems);
        }
        /// <summary>
        /// Adds red star to end of field name to indicate mandatory field if TestForRequired is set to true
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="helper"></param>
        /// <param name="expression"></param>
        /// <param name="TestForRequired"></param>
        /// <returns></returns>
        public static MvcHtmlString LabelFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, bool TestForRequired)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);

            string resolvedLabelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            if (String.IsNullOrEmpty(resolvedLabelText))
            {
                return MvcHtmlString.Empty;
            }

            if (TestForRequired && metadata.IsRequired)
            {
                var spanTag = new TagBuilder("span");
                spanTag.Attributes.Add("alt", "Mandatory field");
                spanTag.AddCssClass("required-star");
                spanTag.InnerHtml = "*";

                resolvedLabelText = string.Concat(resolvedLabelText, spanTag.ToString(TagRenderMode.Normal));
            }

            var tag = new TagBuilder("label");
            tag.Attributes.Add("for", TagBuilder.CreateSanitizedId(helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName)));
            tag.InnerHtml = resolvedLabelText;
            tag.AddCssClass("text_label");

            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        public static string GetMaxLength<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            return metadata.AdditionalValues["maxLength"].ToString();
        }

        /// <summary>
        /// HiddenFor 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="helper"></param>
        /// <param name="expression">Something</param>
        /// <param name="value">Default value to be added</param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString HiddenFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, object value, object htmlAttributes)
        {
            var propertyName = ExpressionHelper.GetExpressionText(expression);

            var input = new TagBuilder("input");
            input.MergeAttribute("id", helper.AttributeEncode(helper.ViewData.TemplateInfo.GetFullHtmlFieldId(propertyName)));
            input.MergeAttribute("name", helper.AttributeEncode(helper.ViewData.TemplateInfo.GetFullHtmlFieldName(propertyName)));
            input.MergeAttribute("value", value.ToString());
            input.MergeAttribute("type", "hidden");
            input.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            return MvcHtmlString.Create(input.ToString());
        }

        public static MvcHtmlString DropDownList(this HtmlHelper htmlHelper, string name, string optionLabel, string defaultValue)
        {
            MvcHtmlString DDL = htmlHelper.DropDownList(name, optionLabel);
            string find = string.Format("=\"{0}\"", defaultValue);
            return MvcHtmlString.Create(DDL.ToString().Replace(find, string.Format("{0} selected", find)));
        }

        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttributes, string defaultValue)
        {
            MvcHtmlString DDLF = htmlHelper.DropDownListFor(expression, selectList, optionLabel, htmlAttributes);
            string find = string.Format("=\"{0}\"", defaultValue);
            return MvcHtmlString.Create(DDLF.ToString().Replace(find, string.Format("{0} selected", find)));
        }

        public static MvcHtmlString ImageAndTextLink(this HtmlHelper htmlHelper, string imgSrc, string displayText, string alt, string actionName
                                                        , string controllerName, object routeValues, object htmlAttributes, object imgHtmlAttributes, string cssClass)
        {
            UrlHelper urlHelper = ((Controller)htmlHelper.ViewContext.Controller).Url;
            string imgtag = htmlHelper.Image(imgSrc, alt, imgHtmlAttributes);
            string url = urlHelper.Action(actionName, controllerName, routeValues);

            TagBuilder imglink = new TagBuilder("a");
            if (cssClass != null) { imglink.AddCssClass(cssClass.ToString()); }
            imglink.MergeAttribute("href", url);
            imglink.InnerHtml = imgtag + "&nbsp;" + displayText;
            imglink.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);

            string result = imglink.ToString();
            return MvcHtmlString.Create(result);
        }
        public static MvcHtmlString ImageActionLink(this AjaxHelper helper, string imageUrl, string titleText, string actionName, string controller, object routeValues
                                            , AjaxOptions ajaxOptions, object htmlAttributes)
        {
            var imgTag = new TagBuilder("img");
            imgTag.MergeAttribute("src", imageUrl);
            imgTag.MergeAttribute("title", titleText);
            var link = helper.ActionLink("[replaceme] " + titleText, actionName, controller, routeValues, ajaxOptions, htmlAttributes).ToHtmlString();   //update

            return new MvcHtmlString(link.Replace("[replaceme]", imgTag.ToString(TagRenderMode.SelfClosing)));
        }

        public static MvcHtmlString ImageLink(this HtmlHelper htmlHelper, string imgSrc, string alt, string actionName, string controllerName
                                                , object routeValues, object htmlAttributes, object imgHtmlAttributes)
        {
            UrlHelper urlHelper = ((Controller)htmlHelper.ViewContext.Controller).Url;
            string imgtag = htmlHelper.Image(imgSrc, alt, imgHtmlAttributes);
            string url = urlHelper.Action(actionName, controllerName, routeValues);

            TagBuilder imglink = new TagBuilder("a");
            imglink.MergeAttribute("href", url);
            imglink.InnerHtml = imgtag;
            imglink.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);

            string result = imglink.ToString();
            return MvcHtmlString.Create(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="imgSrc"></param>
        /// <param name="alt"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="imgHtmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString ImageLink(this HtmlHelper htmlHelper, string imgSrc, string alt, string actionName, string controllerName
                                                , RouteValueDictionary routeValues, object htmlAttributes, object imgHtmlAttributes)
        {
            UrlHelper urlHelper = ((Controller)htmlHelper.ViewContext.Controller).Url;
            string imgtag = htmlHelper.Image(imgSrc, alt, imgHtmlAttributes);
            string url = urlHelper.Action(actionName, controllerName, routeValues);

            TagBuilder imglink = new TagBuilder("a");
            imglink.MergeAttribute("href", url);
            imglink.InnerHtml = imgtag;
            imglink.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);

            string result = imglink.ToString();
            return MvcHtmlString.Create(result);
        }
        public static string Image(this HtmlHelper helper, string url, string altText, object htmlAttributes)
        {
            TagBuilder builder = new TagBuilder("img");
            builder.Attributes.Add("src", url);
            builder.Attributes.Add("alt", altText);
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return builder.ToString(TagRenderMode.SelfClosing);
        }
        #endregion
    }
}

