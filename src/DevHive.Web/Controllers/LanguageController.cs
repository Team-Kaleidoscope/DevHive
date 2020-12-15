using System;
using System.Threading.Tasks;
using AutoMapper;
using DevHive.Data.Repositories;
using DevHive.Services.Models.Language;
using DevHive.Services.Services;
using DevHive.Web.Models.Language;
using Microsoft.AspNetCore.Mvc;

namespace DevHive.Web.Controllers
{
	[ApiController]
	[Route("/api/[controller]")]
	public class LanguageController
	{
		private readonly LanguageService _languageService;
		private readonly IMapper _languageMapper;

		public LanguageController(DevHiveContext context, IMapper mapper)
		{
			this._languageService = new LanguageService(context, mapper);
			this._languageMapper = mapper;
		}

		[HttpPost]
		public Task<IActionResult> Create(CreateLanguageWebModel createLanguageWebModel)
		{
			LanguageServiceModel languageServiceModel = this._languageMapper.Map<LanguageServiceModel>(createLanguageWebModel); 
			
			return this._languageService.CreateLanguage(languageServiceModel);
		}

		[HttpGet]
		public Task<IActionResult> GetById(Guid id)
		{
			return this._languageService.GetLanguageById(id);
		}


		[HttpPut]
		public Task<IActionResult> Update(UpdateLanguageWebModel updateLanguageWebModel)
		{
			LanguageServiceModel languageServiceModel = this._languageMapper.Map<LanguageServiceModel>(updateLanguageWebModel);

			return this._languageService.UpdateLanguage(languageServiceModel);
		}
	}
}