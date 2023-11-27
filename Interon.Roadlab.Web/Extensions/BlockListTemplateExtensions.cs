using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Umbraco.Core.Models.Blocks;
using Umbraco.Core.Models.PublishedContent;

public static class BlockListTemplateExtensions
{
    public const string DefaultFolder = "blocklist/";
    public const string DefaultTemplate = "default";
    public static MvcHtmlString GetFirstBlockList(this HtmlHelper html, dynamic oModel, string template = DefaultTemplate)
    {
        BlockListModel model = null;
        //check if oModel is of type IPublishedContent or of type BlockListItem
        if (oModel is IPublishedContent)
        {
            IPublishedContent contentItem = (IPublishedContent)oModel;

            IPublishedProperty property = contentItem.Properties.FirstOrDefault(x => x.PropertyType.EditorAlias == "Umbraco.BlockList");
            var test = property.GetValue();
            model = property.GetValue() as BlockListModel;
        }
        else
        {
            if (oModel is BlockListItem)
            {
                BlockListItem blockListItem = (BlockListItem)oModel;


                IPublishedProperty property = blockListItem.Content.Properties.FirstOrDefault(x => x.PropertyType.EditorAlias == "Umbraco.BlockList");

                model = property.GetValue() as BlockListModel;
            }
        }

        if (model?.Count == 0)
        {
            return new MvcHtmlString(string.Empty);
        }

        return html.Partial(DefaultFolderTemplate(template), model);
    }

    private static string DefaultFolderTemplate(string template) => $"{DefaultFolder}{template}";
}