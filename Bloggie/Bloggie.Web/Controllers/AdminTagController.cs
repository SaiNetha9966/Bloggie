using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text.Json.Serialization;

namespace Bloggie.Web.Controllers
{
    public class AdminTagController : Controller
    {
        private readonly BloggieDbContext bloggieDbContext;

        public AdminTagController(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }


        // add tags controller and add.cshtm view start
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(AddTagRequest addTagRequest)
        {
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName,
            };

            bloggieDbContext.Tags.Add(tag);
            bloggieDbContext.SaveChanges();
            return RedirectToAction("List");
        }

        // add tags controller and add.cshtm view end

        // dispaly tags controller and list.cshtml list start
        [HttpGet]

        public IActionResult List()
        {
            var tags = bloggieDbContext.Tags.ToList();

            return View(tags);
        }

        [HttpGet]
        // dispaly tags controller and list.cshtml list end


        // Edit tags controller and edit.cshtml list start
        public IActionResult Edit(Guid id)
        {
            var tag = bloggieDbContext.Tags.FirstOrDefault(x => x.Id == id);

            if (tag != null)
            {
                var editTag = new EditTagModel
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName,
                };
                return View(editTag);
            }
            else
            {
                return View();
            }

        }

        [HttpPost]

        public IActionResult Edit(EditTagModel editTagModel)
        {
            var tag = new Tag
            {
                Id = editTagModel.Id,
                Name = editTagModel.Name,
                DisplayName = editTagModel.DisplayName,
            };
           var existingTag = bloggieDbContext.Tags.Find(tag.Id);
            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                bloggieDbContext.SaveChanges();

                return RedirectToAction("Edit", new { id = editTagModel.Id });
            }
            else
            {
                return RedirectToAction("Edit" , new {id = editTagModel.Id});
            }
        }


        [HttpPost]
        public IActionResult Delete(EditTagModel EditTagModel)
        {
            var tag = bloggieDbContext.Tags.Find(EditTagModel.Id);
            if (tag != null)
            {
                bloggieDbContext.Tags.Remove(tag);

                bloggieDbContext.SaveChanges();

                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("List");
            }
        }


    }
}
