namespace  API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IndexController : ControllerBase
{

    private readonly IConfiguration configuration;
    private readonly string WebRootPath;



    public IndexController(IConfiguration configuration)
    {
        this.configuration = configuration;
    }


    [HttpGet]
    [Route("/")]
    public async Task<ActionResult> index()
    {      
        return  Ok($"Welcome to iTMS Driver APP. Version:{this.configuration.GetSection("App_Version").Value.ToString()}. System Time: {DateTime.Now.ToString()}");
    }

 

}
