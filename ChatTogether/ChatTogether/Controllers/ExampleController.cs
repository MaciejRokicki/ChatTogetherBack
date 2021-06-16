using AutoMapper;
using ChatTogether.Commons.EmailSender;
using ChatTogether.Commons.EmailSender.Models.Templates;
using ChatTogether.Commons.RandomStringGenerator;
using ChatTogether.Dal.Dbos;
using ChatTogether.FluentValidator;
using ChatTogether.Logic.Interfaces;
using ChatTogether.ViewModels;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatTogether.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExampleController : ControllerBase
    {
        private readonly ExampleValidator exampleValidator;
        private readonly IMapper mapper;
        private readonly IExampleService exampleService;
        private readonly IRandomStringGenerator randomStringGenerator;
        private readonly IEmailSender emailSender;

        public ExampleController(ExampleValidator exampleValidator, IMapper mapper, IExampleService exampleService, IRandomStringGenerator randomStringGenerator, IEmailSender emailSender)
        {
            this.exampleValidator = exampleValidator;
            this.mapper = mapper;
            this.exampleService = exampleService;
            this.randomStringGenerator = randomStringGenerator;
            this.emailSender = emailSender;     
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<ExampleViewModel>> Get(int id)
        {
            ExampleDbo exampleDbo = await exampleService.GetAsync(id);
            ExampleViewModel exampleViewModel = mapper.Map<ExampleViewModel>(exampleDbo);

            return Ok(exampleViewModel);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<ExampleViewModel>> GetMany()
        {
            IEnumerable<ExampleDbo> exampleDbos = await exampleService.GetManyAsync();

            IEnumerable<ExampleViewModel> exampleViewModels = mapper.Map<IEnumerable<ExampleViewModel>>(exampleDbos);

            return Ok(exampleViewModels);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<ExampleViewModel>> Post([FromBody] ExampleViewModel exampleViewModel)
        {
            ValidationResult validationResult = await exampleValidator.ValidateAsync(exampleViewModel);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            ExampleDbo exampleDbo = mapper.Map<ExampleDbo>(exampleViewModel);
            exampleDbo = await exampleService.CreateAsync(exampleDbo);
            exampleViewModel = mapper.Map<ExampleViewModel>(exampleDbo);

            return Ok(exampleViewModel);
        }

        [HttpPut("[action]")]
        public async Task<ActionResult<ExampleViewModel>> Update(int id, [FromBody] ExampleViewModel exampleViewModel)
        {
            ValidationResult validationResult = await exampleValidator.ValidateAsync(exampleViewModel);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            ExampleDbo exampleDbo = mapper.Map<ExampleDbo>(exampleViewModel);
            exampleDbo = await exampleService.UpdateAsync(id, exampleDbo);
            exampleViewModel = mapper.Map<ExampleViewModel>(exampleDbo);

            return Ok(exampleViewModel);
        }

        [HttpDelete("[action]")]
        public async Task<ActionResult<ExampleViewModel>> Delete(int id)
        {
            await exampleService.DeleteAsync(id);

            return Ok();
        }

        [HttpGet("[action]")]
        public async Task EmailTest(string recipientEmail, string subject)
        {
            string link = randomStringGenerator.Generate();
            await emailSender.Send(recipientEmail, subject, new ChangeEmailRequestTemplate(recipientEmail, "test@test.com", link));
        }
    }
}
