using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sis_Inmobiliaria.Core.Application.Dtos.PropertyType;
using Sis_Inmobiliaria.Core.Application.Interfaces;
using Sis_Inmobiliaria.Core.Application.ViewModels.PropertyType;
namespace Sis_Inmobiliaria.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PropertyTypeController(IPropertyTypeService propertyTypeService,  IMapper mapper) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var dtos = await propertyTypeService.GetAllWithInclude();

            var listEntityVms = mapper.Map<List<PropertyTypeViewModel>>(dtos);

            return View(listEntityVms);
        }
        public IActionResult Create()
        {
            return View("Save", new SavePropertyTypeViewModel() {Id = 0, Name = "" , Active = false, Description = ""});
        }

        [HttpPost]
        public async Task<IActionResult> Create(SavePropertyTypeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("Save", vm);
            }

            PropertyTypeDto dto = mapper.Map<PropertyTypeDto>(vm);
            await propertyTypeService.AddAsync(dto);
            return RedirectToRoute(new { controller = "PropertyType", action = "Index" });
        }
        public async Task<IActionResult> Edit(int id)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToRoute(new { controller = "PropertyType", action = "Index" });
            }

            ViewBag.EditMode = true;
            var dto = await propertyTypeService.GetById(id);
            if (dto == null)
            {
                return RedirectToRoute(new { controller = "PropertyType", action = "Index" });
            }
            SavePropertyTypeViewModel vm = mapper.Map<SavePropertyTypeViewModel>(dto);
            return View("Save", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SavePropertyTypeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.EditMode = true;
                return View("Save", vm);
            }

            PropertyTypeDto dto = mapper.Map<PropertyTypeDto>(vm);
            await propertyTypeService.UpdateAsync(dto, dto.Id);
            return RedirectToRoute(new { controller = "PropertyType", action = "Index" });
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToRoute(new { controller = "PropertyType", action = "Index" });
            }

            var dto = await propertyTypeService.GetById(id);
            if (dto == null)
            {
                return RedirectToRoute(new { controller = "PropertyType", action = "Index" });
            }
            DeletePropertyTypeViewModel vm = mapper.Map<DeletePropertyTypeViewModel>(dto);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeletePropertyTypeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            await propertyTypeService.DeleteAsync(vm.Id);
            return RedirectToRoute(new { controller = "PropertyType", action = "Index" });
        }

    }
}
