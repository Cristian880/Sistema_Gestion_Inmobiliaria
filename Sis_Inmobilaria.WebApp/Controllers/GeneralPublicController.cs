using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sis_Inmobiliaria.Core.Application.Dtos.Property;
using Sis_Inmobiliaria.Core.Application.Interfaces;
using Sis_Inmobiliaria.Core.Application.ViewModels.Property;


namespace Sis_Inmobiliaria.WebApp.Controllers
{
    [Authorize(Roles = "GeneralPublic")]
    public class GeneralPublicController(IPropertyService propertyService, IPropertyTypeService propertyTypeService, IMapper mapper) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var dtos = await propertyService.GetAllWithInclude();

            var activeDtos = dtos.Where(pt => pt.Active).ToList();

            var listEntityVms = mapper.Map<List<PropertyViewModel>>(activeDtos);
            return View(listEntityVms);
        }
    }
}