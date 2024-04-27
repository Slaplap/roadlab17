using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Interon.Roadlab.Web.v13.Extensions;

public static class BlockListTemplateExtensions
{
    public const string DefaultFolder = "blocklist/";
    public const string DefaultTemplate = "default";
    public static IHtmlContent GetFirstBlockList(this IHtmlHelper<dynamic> html, dynamic oModel, string template = DefaultTemplate)
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
            return  new HtmlContentBuilder ().SetContent("") ;
        }

        return html.Partial(DefaultFolderTemplate(template), model);
    }

    private static string DefaultFolderTemplate(string template) => $"{DefaultFolder}{template}";
}