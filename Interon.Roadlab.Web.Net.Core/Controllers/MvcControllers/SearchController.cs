using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Extensions;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Interon.Roadlab.Web.Net.Core.Models.ContentModels;
using Umbraco.Cms.Core.Models;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using Examine;
using Umbraco.Cms.Web.Common;

namespace Interon.Roadlab.Web.Net.Core.Controllers
{
    public interface ISearchService
    {
        IEnumerable<IPublishedContent> SearchContentNames(string query);
    }
    public class SearchService : ISearchService
    {
        private readonly IExamineManager _examineManager;
        private readonly UmbracoHelper _umbracoHelper;

        public SearchService(IExamineManager examineManager, UmbracoHelper umbracoHelper)
        {
            _examineManager = examineManager;
            _umbracoHelper = umbracoHelper;
        }

        public IEnumerable<IPublishedContent> SearchContentNames(string query)
        {
             IEnumerable<string> ids = Array.Empty<string>();
            if (!string.IsNullOrEmpty(query) && _examineManager.TryGetIndex("ExternalIndex", out IIndex? index))
            {
                ids = index
                    .Searcher
                    .CreateQuery("content")
                    .NodeTypeAlias(BlogArticle.ModelTypeAlias)
                    .And()
                    .Field("blogHeading", query).Or()
                    .Field("articleIntersectDescription", query)
                    .Execute()
                    .Select(x => x.Id);
            }

            foreach (var id in ids)
            {
                yield return _umbracoHelper.Content(id);
            }
        }
    }

    public class SearchViewModel(IPublishedContent content, IPublishedValueFallback publishedValueFallback)
        : PublishedContentWrapped(content, publishedValueFallback)
    {
        public IEnumerable<IPublishedContent> SearchResults { get; set; } = Enumerable.Empty<IPublishedContent>();
        public bool HasSearched { get; set; }
    }







    //public class SearchRenderModel : PublishedContentWrapped
    //{
    //    public SearchRenderModel(IPublishedContent content) : base(content)
    //    {
    //        Model = content as BlogSearchResults; // Ensure your custom model implements IPublishedContent
    //    }

    //    public BlogSearchResults Model { get; set; }
    //    //public IFullTextSearchResult FullTextSearchResult { get; set; }
    //}



    public class BlogSearchResultsController(
        ILogger<RenderController> logger,
        ICompositeViewEngine compositeViewEngine,
        IUmbracoContextAccessor umbracoContextAccessor,
        IPublishedValueFallback publishedValueFallback,
        ISearchService searchService)
        : RenderController(logger,
            compositeViewEngine,
            umbracoContextAccessor)
    {
        public override IActionResult Index()
        {
            // Get the queryString from the request
            string queryString = HttpContext.Request.Query["q"];

            // Create the view model and pass it to the view
            SearchViewModel viewModel = new(CurrentPage!, publishedValueFallback)
            {
                SearchResults = searchService.SearchContentNames(queryString),
                HasSearched = !string.IsNullOrEmpty(queryString),
            };

            return CurrentTemplate(viewModel);
        }
    }





    //public class BlogSearchResultsController_old : RenderController
    //{
    //    private readonly ISearchService _searchService;



    //    public override IActionResult Index()
    //    {
    //        var contentModel = new ContentModel(CurrentPage);
    //        var searchModel = new SearchRenderModel(contentModel.Content);

    //        if (HttpContext.Request.Query.ContainsKey("q"))
    //        {
    //            var query = HttpContext.Request.Query["q"].ToString();
    //            int currentPage = 1;
    //            if (HttpContext.Request.Query.ContainsKey("p"))
    //            {
    //                int.TryParse(HttpContext.Request.Query["p"], out currentPage);
    //                currentPage = currentPage < 1 ? 1 : currentPage;
    //            }

    //            var search = new Search(query)
    //                .EnableHighlighting()
    //                .AddTitleProperty("blogHeading")
    //                .AddSummaryProperty("articleIntersectDescription")
    //                .SetSummaryLength(300)
    //                .SetPageLength(50)
    //                .SetCulture(contentModel.Content.GetCultureFromDomains());

    //            searchModel.FullTextSearchResult = _searchService.Search(search, currentPage);
    //        }
    //        else
    //        {
    //            searchModel.FullTextSearchResult = new FullTextSearchResult(); // Ensure this is implemented or handled appropriately
    //        }

    //        return CurrentTemplate(searchModel);
    //    }
    //}
}
