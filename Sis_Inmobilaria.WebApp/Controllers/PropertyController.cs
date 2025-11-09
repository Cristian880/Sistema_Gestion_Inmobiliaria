using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sis_Inmobiliaria.Core.Application.Dtos.Property;
using Sis_Inmobiliaria.Core.Application.Interfaces;
using Sis_Inmobiliaria.Core.Application.ViewModels.Property;
using System.Security.Claims;

namespace Sis_Inmobiliaria.WebApp.Controllers
{
    [Authorize]
    public class PropertyController(IPropertyService propertyService, IPropertyTypeService propertyTypeService, IMapper mapper) : Controller
    { 
    //public class PropertyController : Controller
    //{
    //    private readonly IPropertyService _PropertyService;
    //    private readonly IPropertyTypeService _PropertyTypeService;
    //    private readonly IMapper _mapper;

    //    public PropertyController(IPropertyService PropertyService, IPropertyTypeService PropertyTypeService, IMapper mapper)
    //    {
    //        _PropertyService = PropertyService;
    //        _PropertyTypeService = PropertyTypeService;
    //        _mapper = mapper;
    //    }
        public async Task<IActionResult> Index()
        {
            List<PropertyDto> dtos;
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (User.IsInRole("Admin"))
            {
                dtos = await propertyService.GetAllWithInclude();//dtos = await _PropertyService.GetAllWithInclude();

            }
            else if (!string.IsNullOrEmpty(userId))
            {
                dtos = await propertyService.GetAllByUserIdAsync(userId);
            }
            else 
            {
                dtos = [];
            }

            var listEntityVms = mapper.Map<List<PropertyViewModel>>(dtos);
            return View(listEntityVms);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.PropertyTypes = await propertyTypeService.GetAll();
            return View("Save", new SavePropertyViewModel()
            {
                Id = 0,
                Name = "",
                Direction = "",
                Description = "",
                PropertyPublicacionDate = DateTime.Now,
                Active = true,
                PropertyTypeId = null,
                Price = 0,
                Size = 0,
                PropertyImageFile = null,
                UserId = null
            });
        }
        [HttpPost]
        public async Task<IActionResult> Create(SavePropertyViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PropertyTypes = await propertyTypeService.GetAll();
                return View("Save", vm);
            }
            PropertyDto dto = mapper.Map<PropertyDto>(vm);
            dto.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await propertyService.AddAsync(dto);
            if (User.IsInRole("Admin"))
            {
                return RedirectToRoute(new { controller = "Property", action = "Index" });
            }
            else
            {
                return RedirectToRoute(new { controller = "GeneralPublic", action = "Index" });
            }
        }
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await propertyService.GetById(id);
            if (dto == null)
            {
                return RedirectToRoute(new { controller = "Property", action = "Index" });
            }
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!User.IsInRole("Admin") && dto.UserId != userId)
            {
                // ¡No autorizado!
                return RedirectToRoute(new { controller = "Home", action = "AccessDenied" });
            }
            ViewBag.EditMode = true;
            ViewBag.PropertyTypes = await propertyTypeService.GetAll();

            SavePropertyViewModel vm = mapper.Map<SavePropertyViewModel>(dto);
            return View("Save", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SavePropertyViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.EditMode = true;
                ViewBag.PropertyTypes = await propertyTypeService.GetAll();
                return View("Save", vm);
            }
            var existingDto = await propertyService.GetById(vm.Id);
            if (existingDto == null)
            {
                return RedirectToRoute(new { controller = "Property", action = "Index" });
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!User.IsInRole("Admin") && existingDto.UserId != userId)
            {
                return RedirectToRoute(new { controller = "Home", action = "AccessDenied" });
            }

            PropertyDto dto = mapper.Map<PropertyDto>(vm);
            dto.UserId = existingDto.UserId; 

            await propertyService.UpdateAsync(dto, dto.Id);
            return RedirectToRoute(new { controller = "Property", action = "Index" });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var dto = await propertyService.GetById(id);
            if (dto == null)
            {
                return RedirectToRoute(new { controller = "Property", action = "Index" });
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!User.IsInRole("Admin") && dto.UserId != userId)
            {
                return RedirectToRoute(new { controller = "Home", action = "AccessDenied" });
            }

            DeletePropertyViewModel vm = mapper.Map<DeletePropertyViewModel>(dto);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeletePropertyViewModel vm)
        {
            var existingDto = await propertyService.GetById(vm.Id);
            if (existingDto == null)
            {
                return RedirectToRoute(new { controller = "Property", action = "Index" });
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!User.IsInRole("Admin") && existingDto.UserId != userId)
            {
                return RedirectToRoute(new { controller = "Home", action = "AccessDenied" });
            }

            await propertyService.DeleteAsync(vm.Id);
            return RedirectToRoute(new { controller = "Property", action = "Index" });
        }
    }
}
