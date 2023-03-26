using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using P230_Pronia.DAL;
using P230_Pronia.Entities;

namespace P230_Pronia.Controllers
{
    public class PlantController : Controller
    {
        private readonly ProniaDbContext _context;

        public PlantController(ProniaDbContext context)
        {
            _context = context;
        }
        public IActionResult Detail(int id)
        {
            if (id == 0) return NotFound();
            Plant? plant = _context.Plants
                                .Include(p => p.PlantImages)
                                    .Include(p => p.PlantDeliveryInformation)
                                        .Include(p => p.PlantTags)
                                            .ThenInclude(pt => pt.Tag)
                                                .Include(p => p.PlantCategories)
                                                    .ThenInclude(pc => pc.Category).FirstOrDefault(p => p.Id == id);



            List<Plant> modified = new List<Plant>();
            if (plant is null) return NotFound();

            foreach (var item in _context.Plants.Include(p => p.PlantImages).Include(x => x.PlantCategories).ThenInclude(x => x.Category))
            {
                foreach (PlantCategory plantCategory in item.PlantCategories)
                {
                    foreach (var cat in plant.PlantCategories)
                    {
                        if (plantCategory.Category.Id == cat.Category.Id && !modified.Contains(item))
                        {

                            modified.Add(item);

                        }
                    }
                }
            }

            ViewBag.RelatedPlant = modified.Where(x => x.Id != id).Take(4).ToList();

            return View(plant);

        }

    }
}
