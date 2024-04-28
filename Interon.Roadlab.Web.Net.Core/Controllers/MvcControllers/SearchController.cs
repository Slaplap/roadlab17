using Interon.Roadlab.Web.Net.Core.Models.ContentModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Our.Umbraco.FullTextSearch.Interfaces;
using Our.Umbraco.FullTextSearch.Models;
using Our.Umbraco.FullTextSearch.Options;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Extensions;
using Our.Umbraco.FullTextSearch.Interfaces;
using Search = Our.Umbraco.FullTextSearch.Models.Search;
using Microsoft.AspNetCore.Http;
namespace Interon.Roadlab.Web.Net.Core.Models.ContentModels
{
    public partial class BlogSearchResults
    {

        public BlogSearchResults Model { get; set; } 
        public IFullTextSearchResult FullTextSearchResult { get; set; }
    }
}
namespace Interon.Roadlab.Web.Net.Core.Controllers.MvcControllers
{

    public class SearchViewModel : PublishedContentWrapped
    {
        public SearchViewModel(IPublishedContent content, IPublishedValueFallback publishedValueFallback)
            : base(content, publishedValueFallback)
        {
            Model = content as BlogSearchResults;
        }

        public BlogSearchResults Model { get; set; }
        public IFullTextSearchResult FullTextSearchResult { get; set; }
    }
    public class BlogSearchResultsController(
        
        IOptions<FullTextSearchOptions> options,
        ILogger<RenderController> logger,
        ICompositeViewEngine compositeViewEngine,
        IUmbracoContextAccessor umbracoContextAccessor, IPublishedValueFallback publishedValueFallback,
        ISearchService searchService)
        : RenderController(logger, compositeViewEngine, umbracoContextAccessor)
    {
        private readonly FullTextSearchOptions _options = options.Value;


        public override IActionResult Index()
        {
            

            var contentModel = new ContentModel(CurrentPage);
            SearchViewModel searchViewModel = new(CurrentPage!, publishedValueFallback);

            if (HttpContext.Request.Query.ContainsKey("q"))
            {
                var query = HttpContext.Request.Query["q"].ToString();
                int currentPage = 1;
                if (HttpContext.Request.Query.ContainsKey("p"))
                {
                    int.TryParse(HttpContext.Request.Query["p"], out currentPage);
                    currentPage = currentPage < 1 ? 1 : currentPage;
                }
                SearchViewModel viewModel = new(CurrentPage!, publishedValueFallback);
                
                var search = new Search(query)
                    .EnableHighlighting()
                    .AddTitleProperty("blogHeading")

                    .AddSummaryProperties("articleIntersectBodyText", "heroSmallHeading", "heroLargeHeading")
                    .SetSummaryLength(300)
                    .SetPageLength(50)
                    
                    .SetCulture(contentModel.Content.GetCultureFromDomains());
                search.AddWildcard = true;
                searchViewModel.FullTextSearchResult = searchService.Search(search, currentPage);
            }
            else
            {
                searchViewModel.FullTextSearchResult = new FullTextSearchResult(); // Ensure this is implemented or handled appropriately
            }

            return CurrentTemplate(searchViewModel);
        }
    }
}
