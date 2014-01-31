using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using JetBrains.Annotations;

namespace MvcUtilities
{
    public static class MvcHelpers
    {
        /// <summary>
        /// Returns a SelectList with staticly-typed parameters
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="enumerable"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="selectedValue"></param>
        /// <param name="valueFormatter"></param>
        /// <returns></returns>
        public static SelectList SelectList<T>(this HtmlHelper helper, IEnumerable<T> enumerable, Func<T, object> key,
            Func<T, object> value, object selectedValue = null, Func<object, object> valueFormatter = null)
        {
            var data = new Dictionary<string, string>();

            if (enumerable != null)
                foreach (var e in enumerable)
                {
                    string valueResult = (valueFormatter != null ? valueFormatter(value(e)) : value(e)).ToString();
                    data.Add(key(e).ToString(), valueResult);
                }

            SelectList list = selectedValue == null ? 
                                  new SelectList(data, "Key", "Value") : 
                                  new SelectList(data, "Key", "Value", selectedValue);

            return list;
        }

        public static MvcHtmlString Button(this HtmlHelper helper, string value, object htmlAttributes = null)
        {
            return Button(helper, value, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString Button(this HtmlHelper helper, string value, IDictionary<string, object> htmlAttributes)
        {
            var builder = new TagBuilder("input");
            builder.Attributes.Add("type", "submit");
            builder.Attributes.Add("value", value);
            builder.MergeAttributes(htmlAttributes);
            return MvcHtmlString.Create(builder.ToString());
        }

        /// <summary>
        /// If the given as expression model property is not null, output HttpPut and HiddenFor given expression.
        /// If it is null, stay on the HttpPost (creation) mode
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="helper"></param>
        /// <param name="formIdentifier">ID of this form</param>
        /// <param name="httpVerb">Override HttpMode, especially for HttpDelete mode</param>
        /// <returns></returns>
        public static MvcHtmlString ViewModeFor<TModel, TProperty>(this HtmlHelper<TModel> helper, 
            Expression<Func<TModel, TProperty>> formIdentifier, HttpVerbs? httpVerb = null)
        {
            var sb = new StringBuilder(Environment.NewLine);

            TProperty identifier = formIdentifier.Compile().Invoke(helper.ViewData.Model);

            if (httpVerb.HasValue)
                sb.AppendLine(helper.HttpMethodOverride(httpVerb.Value).ToHtmlString());

            if (!Equals(identifier, default(TProperty)))
            {
                if (!httpVerb.HasValue)
                    sb.AppendLine(helper.HttpMethodOverride(HttpVerbs.Put).ToHtmlString());

                sb.AppendLine(helper.HiddenFor(formIdentifier).ToHtmlString());
            }

            return MvcHtmlString.Create(sb.ToString());
        }




        /// <summary>
        /// Appends a javascript file into the Layout
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="script">path of the javascript file, i.e. /Scripts/script.js</param>
        public static string AppendScript(this HtmlHelper helper, [PathReference("~/")]string script)
        {
            var vd = helper.ViewContext.Controller.ViewData;

            if (vd["_Scripts"] == null)
                vd["_Scripts"] = new List<string>();

            var scripts = (List<string>)vd["_Scripts"];

            if (!scripts.Contains(script))
                scripts.Add(script);

            return null;
        }

        /// <summary>
        /// Returns appended scripts with script tags
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static IHtmlString GetAppendedScripts(this HtmlHelper helper)
        {
            var vd = helper.ViewContext.Controller.ViewData;

            if (vd["_Scripts"] == null)
                vd["_Scripts"] = new List<string>();

            var scripts = (List<string>)vd["_Scripts"];

            var sb = new StringBuilder();

            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            foreach (var script in scripts)
            {
                var path = urlHelper.Content("~/" + script);
                sb.Append("<script src=\"").Append(path).Append("\"></script>").Append(Environment.NewLine);
            }

            var mvcString = new MvcHtmlString(sb.ToString());

            return mvcString;
        }
    }
}