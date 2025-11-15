using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sis_Inmobiliaria.Core.Application.Dtos.Property;
using Sis_Inmobiliaria.Core.Application.Interfaces;
using Sis_Inmobiliaria.Core.Application.ViewModels.Property;
using Sis_Inmobiliaria.Core.Application.ViewModels.PropertyType;
using Sis_Inmobiliaria.WebApp.Helpers;
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
            var activeDtos = dtos.Where(p => p.Active).ToList();

            var listEntityVms = mapper.Map<List<PropertyViewModel>>(activeDtos);
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
            
            var created = await propertyService.AddAsync(dto);

            if (created != null)
            {
                if (vm.PropertyImageFile != null)
                {
                    var fileName = FileManager.Upload(vm.PropertyImageFile, created.Id.ToString(), "Properties");
                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        created.PropertyImage = fileName;
                        await propertyService.UpdateAsync(created, created.Id);
                    }
                }
            }

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
            
            var currentImage = existingDto.PropertyImage ?? string.Empty;
            dto.PropertyImage = FileManager.Upload(vm.PropertyImageFile, dto.Id.ToString(), "Properties", true, currentImage);

            await propertyService.UpdateAsync(dto, dto.Id);
            return RedirectToRoute(new { controller = "Property", action = "Index" });
        }

        public async Task<IActionResult> Details(int id)
        {
            var dto = await propertyService.GetById(id);
            if (dto == null)
            {
                return RedirectToRoute(new { controller = "Property", action = "Index" });
            }

            PropertyViewModel vm = mapper.Map<PropertyViewModel>(dto);
            return View(vm);
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

            existingDto.Active = false;
            await propertyService.UpdateAsync(existingDto, existingDto.Id);
            //FileManager.Delete(vm.Id.ToString(), "Properties");
            //await propertyService.DeleteAsync(vm.Id);
            return RedirectToRoute(new { controller = "Property", action = "Index" });
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> InactiveProperties()
        {
            var dtos = await propertyService.GetAllWithInclude();
            var inactiveDtos = dtos.Where(p => !p.Active).ToList();
            var listEntityVms = mapper.Map<List<PropertyViewModel>>(inactiveDtos);
            return View("Index", listEntityVms);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Reactivate(int id)
        {
            var existingDto = await propertyService.GetById(id);
            if (existingDto == null)
            {
                return RedirectToRoute(new { controller = "Property", action = "Index" });
            }

            existingDto.Active = true;
            await propertyService.UpdateAsync(existingDto, existingDto.Id);
            return RedirectToRoute(new { controller = "Property", action = "InactiveProperties" });
        }
    }
}
