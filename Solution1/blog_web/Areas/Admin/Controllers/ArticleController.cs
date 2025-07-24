using AutoMapper;
using data.UnitOfWorks;
using entity.DTOs.Articles;
using entity.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using service.Services.Abstractions;
using service.Services.Concrete;

namespace blog_web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticleController : Controller
    {
        private readonly IArticleService articleService;
        private readonly IMapper mapper;
        private readonly ICategoryService categoryService;  

        public ArticleController(IArticleService articleService, ICategoryService categoryService,IMapper mapper)
        {
            this.articleService = articleService;
            this.mapper = mapper;
        }
        public async   Task < IActionResult> Index()
        {
            var articles=articleService.GetAllArticlesWithCategoryNonDeletedAsync();    
            return View(articles);
        }
        [HttpGet]
        public async Task<IActionResult> Add() {
            var categories = await categoryService.GetAllCategoriesNonDeleted();
            return View(new ArticleAddDTO { Categories =categories});
        }
        [HttpPost]
        public async Task<IActionResult> Add(ArticleAddDTO articleAddDTO)
        {
            await articleService.CreateArticleAsync(articleAddDTO);
            RedirectToAction("Index", "Article", new { area = "Admin" });
            var categories = await categoryService.GetAllCategoriesNonDeleted();
            return View(new ArticleAddDTO { Categories = categories });
        }
        [HttpGet]
        public async Task<IActionResult> Update(Guid articleId)
        {
            var articles = await articleService.GetArticlesWithCategoryNonDeletedAsync(articleId);
            var categories= await categoryService.GetAllCategoriesNonDeleted();
            var articleUpdateDto=mapper.Map<ArticleUpdateDTO>(articles);    

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Update(ArticleUpdateDTO articleUpdateDTO)
        {
          await articleService.UpdateArticleAsync(articleUpdateDTO);
            var categories = await categoryService.GetAllCategoriesNonDeleted();
        

            return View(articleUpdateDTO);
        }
    }
  
  
}


