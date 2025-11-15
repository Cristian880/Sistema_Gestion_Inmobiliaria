using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sis_Inmobiliaria.Core.Application.Dtos.Property;
using Sis_Inmobiliaria.Core.Application.Interfaces;
using Sis_Inmobiliaria.Core.Application.ViewModels.Property;
using Sis_Inmobiliaria.Core.Application.ViewModels.PropertyType;
using System.Security.Claims;

namespace Sis_Inmobiliaria.WebApp.Controllers
{
    [Authorize(Roles = "GeneralPublic")]
    public class GeneralPublicController(IPropertyService propertyService, IPropertyTypeService propertyTypeService, IMapper mapper) : Controller
    {
        // GET: /GeneralPublic/Index
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
    }
}