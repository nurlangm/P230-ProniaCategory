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





            PlantCategory plantCategory1 = null;
            foreach (var item in plant.PlantCategories)
            {
                plantCategory1 = item;
                break;

            }

            ViewBag.RelatedPlant = _context.Plants.Where(x => x.Id != id).Include(p => p.PlantImages).Include(x => x.PlantCategories).Take(4).ToList();

            //if (plant is null) return NotFound();
            return View(plant);
        }

    }
}
