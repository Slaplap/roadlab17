using System.Web.Mvc;
using Interon.Roadlab.Web.Core.ContentModels;
using Our.Umbraco.FullTextSearch.Interfaces;
using Our.Umbraco.FullTextSearch.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace Interon.Roadlab.Web.Core.MvcControllers
{
    public class SearchRenderModel : PublishedContentWrapped
    {
        public SearchRenderModel(IPublishedContent content) : base(content)
        {
            Model = content as BlogSearchResults;
        }

        public BlogSearchResults Model { get; set; }
        public IFullTextSearchResult FullTextSearchResult { get; set; }
    }

    public class BlogSearchResultsController : RenderMvcController
    {
        private readonly ISearchService _searchService;
        private readonly IFullTextSearchConfig _config;

        public BlogSearchResultsController(ISearchService searchService, IFullTextSearchConfig config)
        {
            _searchService = searchService;
            _config = config;
        }

        public override ActionResult Index(ContentModel model)
        {
            var searchModel = new SearchRenderModel(model.Content);


            if (Request["q"] != null)
            {
                int.TryParse(Request["p"], out var currentPage);
                currentPage = currentPage < 1 ? 1 : currentPage;

                var search = new Search(Request["q"])
                    .EnableHighlighting()
                    .AddTitleProperty("blogHeading")
                    .AddSummaryProperty("articleIntersectDescription")
                    .SetSummaryLength(300)
                    .SetPageLength(50)
                    .SetCulture(model.Content.GetCultureFromDomains());

                searchModel.FullTextSearchResult = _searchService.Search(search, currentPage);
            }
            else
            {
                searchModel.FullTextSearchResult = new FullTextSearchResult();
            }

            return CurrentTemplate(searchModel);
        }
    }
}