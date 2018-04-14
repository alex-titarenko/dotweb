using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Linq;
using TAlex.AspNetCore.Mvc.Sorting;

namespace TAlex.AspNetCore.Mvc.UI.Grid
{
    /// <summary>
	/// Renders a grid as an HTML table.
	/// </summary>
	public class HtmlTableGridRenderer<T> : GridRenderer<T> where T : class
    {
        private const string DefaultCssClass = "grid";

        public HtmlTableGridRenderer()
        {
        }

        protected override void RenderHeaderCellEnd()
        {
            RenderText("</th>");
        }

        protected virtual void RenderEmptyHeaderCellStart()
        {
            RenderText("<th>");
        }

        protected override void RenderHeaderCellStart(GridColumn<T> column)
        {
            var attributes = new Dictionary<string, object>(column.HeaderAttributes);

            if (IsSortingEnabled && column.Sortable)
            {
                bool isSortedByThisColumn = (GridModel.SortOptions.Column == GenerateSortColumnName(column));

                if (isSortedByThisColumn)
                {
                    string sortClass = GridModel.SortOptions.Direction == SortDirection.Ascending ? "sort_asc" : "sort_desc";

                    if (attributes.ContainsKey("class") && attributes["class"] != null)
                    {
                        sortClass = string.Join(" ", new[] { attributes["class"].ToString(), sortClass });
                    }

                    attributes["class"] = sortClass;
                }
            }

            string attrs = BuildHtmlAttributes(attributes);

            if (attrs.Length > 0)
                attrs = " " + attrs;

            RenderText(string.Format("<th{0}>", attrs));
        }


        protected override void RenderHeaderText(GridColumn<T> column)
        {
            if (IsSortingEnabled && column.Sortable)
            {
                string sortColumnName = GenerateSortColumnName(column);

                bool isSortedByThisColumn = GridModel.SortOptions.Column == sortColumnName;

                var sortOptions = new GridSortOptions
                {
                    Column = sortColumnName
                };

                if (isSortedByThisColumn)
                {
                    sortOptions.Direction = (GridModel.SortOptions.Direction == SortDirection.Ascending)
                        ? SortDirection.Descending
                        : SortDirection.Ascending;
                }
                else //default sort order
                {
                    sortOptions.Direction = column.InitialDirection ?? GridModel.SortOptions.Direction;
                }

                var routeValues = this.CreateRouteValuesForSortOptions(sortOptions, GridModel.SortPrefix);

                //Re-add existing querystring
                foreach (var key in Context.HttpContext.Request.Query.Keys.Where(key => key != null))
                {
                    if (!routeValues.ContainsKey(key))
                    {
                        routeValues[key] = Context.HttpContext.Request.Query[key];
                    }
                }

                var queryPart = QueryString.Create(routeValues);                
                var path = this.Context.HttpContext.Request.Path.ToUriComponent();

                var link = '/' + path.TrimStart('/') + queryPart.ToUriComponent();
                RenderText(link);
            }
            else
            {
                base.RenderHeaderText(column);
            }
        }

        private IDictionary<string, StringValues> CreateRouteValuesForSortOptions(GridSortOptions sortOptions, string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
            {
                return new Dictionary<string, StringValues>
                {
                    { nameof(GridSortOptions.Column), sortOptions.Column },
                    { nameof(GridSortOptions.Direction), sortOptions.Direction.ToString() }
                };
            }

            //There must be a nice way to do this...
            return new Dictionary<string, StringValues>
            {
                { $"{prefix}.{nameof(GridSortOptions.Column)}", sortOptions.Column },
                { $"{prefix}.{nameof(GridSortOptions.Direction)}", sortOptions.Direction.ToString() }
            };
        }

        protected virtual string GenerateSortColumnName(GridColumn<T> column)
        {
            //Use the explicit sort column name if specified. If not possible, fall back to the property name.
            //If the property name cannot be inferred (ie the expression is not a MemberExpression) then try the display name instead.
            return column.SortColumnName ?? column.Name ?? column.DisplayName;
        }

        protected override void RenderRowStart(GridRowViewData<T> rowData)
        {
            var attributes = GridModel.Sections.Row.Attributes(rowData);

            if (!attributes.ContainsKey("class"))
            {
                attributes["class"] = rowData.IsAlternate ? "gridrow_alternate" : "gridrow";
            }

            string attributeString = BuildHtmlAttributes(attributes);

            if (attributeString.Length > 0)
            {
                attributeString = " " + attributeString;
            }

            RenderText(string.Format("<tr{0}>", attributeString));
        }

        protected override void RenderRowEnd()
        {
            RenderText("</tr>");
        }

        protected override void RenderEndCell()
        {
            RenderText("</td>");
        }

        protected override void RenderStartCell(GridColumn<T> column, GridRowViewData<T> rowData)
        {
            string attrs = BuildHtmlAttributes(column.Attributes(rowData));
            if (attrs.Length > 0)
                attrs = " " + attrs;

            RenderText(string.Format("<td{0}>", attrs));
        }

        protected override void RenderHeadStart()
        {
            string attributes = BuildHtmlAttributes(GridModel.Sections.HeaderRow.Attributes(new GridRowViewData<T>(null, false)));
            if (attributes.Length > 0)
            {
                attributes = " " + attributes;
            }


            RenderText(string.Format("<thead><tr{0}>", attributes));
        }

        protected override void RenderHeadEnd()
        {
            RenderText("</tr></thead>");
        }

        protected override void RenderGridStart()
        {
            if (!GridModel.Attributes.ContainsKey("class"))
            {
                GridModel.Attributes["class"] = DefaultCssClass;
            }

            string attrs = BuildHtmlAttributes(GridModel.Attributes);

            if (attrs.Length > 0)
                attrs = " " + attrs;

            RenderText(string.Format("<table{0}>", attrs));
        }

        protected override void RenderGridEnd(bool isEmpty)
        {
            RenderText("</table>");
        }

        protected override void RenderEmpty()
        {
            RenderHeadStart();
            RenderEmptyHeaderCellStart();
            RenderHeaderCellEnd();
            RenderHeadEnd();
            RenderBodyStart();
            RenderText("<tr><td>" + GridModel.EmptyText + "</td></tr>");
            RenderBodyEnd();
        }

        protected override void RenderBodyStart()
        {
            RenderText("<tbody>");
        }

        protected override void RenderBodyEnd()
        {
            RenderText("</tbody>");
        }

        /// <summary>
        /// Converts the specified attributes dictionary of key-value pairs into a string of HTML attributes. 
        /// </summary>
        /// <returns></returns>
        protected string BuildHtmlAttributes(IDictionary<string, object> attributes)
        {
            return DictionaryExtensions.ToHtmlAttributes(attributes);
        }
    }
}
