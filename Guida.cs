namespace net_il_mio_fotoalbum
{
    /*
     * - creare in Models le classi necessarie a mappare le tabelle nel DB
     * 
     * - installare pacchetto nuGet Microsoft.EntityFrameworkCore
     *                              Microsoft.EntityFrameworkCore.SqlServer
     *                              Microsoft.EntityFrameworkCore.Design
     *                              dotnet tool install --global dotnet-ef (1 volta sola basta dopo aver installato VS)
     * 

       - aggiungiamo pacchetti per auth Microsoft.AspNetCore.Identity.EntityFrameworkCore 
                                         Microsoft.AspNetCore.Identity.UI
                                         Microsoft.VisualStudio.Web.CodeGeneration.Design
        
        -Eseguiamo il comando per installare il generatore di codice per lo scaffolding per le identity e le viste

                    dotnet tool install -g dotnet-aspnet-codegenerator (1 volta sola basta dopo aver installato VS)




       - creare un context che erediti DBContext con dentro tutte i modelli da mappare nel DB :

        
                                  public class SchoolContext : IdentityDbContext<IdentityUser>

                                    {
                                         public DbSet<Student> Students { get; set; }
                                         public DbSet<Course> Courses { get; set; }
                                         public DbSet<CourseImage> CourseImages { get; set; }
                                         public DbSet<Review> Reviews { get; set; }
                                         //private string sqlString = "Server=GAMMA;Database=Pizza;TrustServerCertificate=True";
                                         private string sqlString = "Server=DESKTOP-1DGOME6;Database=Pizza;Trusted_Connection=True;TrustServerCertificate=True";


                                         protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(sqlString);
    
                                     }


                    dotnet-aspnet-codegenerator identity --dbContext FotoContext --files "Account.Login;Account.Logout;Account.Register" -tfm "net60"



        - fare le migrations e aggiornare : dotnet ef migrations add InitialCreate             dotnet ef database update

        - Modifichiamo Program.cs con :     app.UseAuthentication(); dopo il routing

                                            modifichiamo questa riga da così:

                                            builder.Services.AddDbContext<FotoContext>(options => options.UseSqlServer(connectionString));
                                            a cosi                  
    
                                            builder.Services.AddDbContext<FotoContext>();
                                            
                                            // rimuoviamo la seguente riga:   var connectionString = builder.Configuration.GetConnectionString(...

                                            // dopo app.MapControllerRoute, aggiungiamo la seguente riga: app.MapRazorPages();


        - nel layout principale, che usano le viste come punto di partenza, con @RenderBody() si indica un segnaposto in cui verrà visulazziata la vista in questione
                                                                    , è possibile richiamare altri blocchi di pagina utilizzando il metodo @Html.Partial()
                    se nelle viste si specifica
                    @{
                        Layout = "_mylayout"; 
                    }                               si prenderà come layout base il nostro custom, altrimenti se non specifico nulla, prende il layout base

                inoltre:    nella vista si puo aggiungere una section
                        @section MySectionName {
                                                    <script>Contenuto della mia sezione.</script>
                                                }
            all'interno del tuo layout  utilizzare @RenderSection per visualizzare il contenuto della sezione: @RenderSection("MySectionName", required: false)


        -nel layout che stiamo utilizzando inseriamo il seguente codice

                                            <div class="collapse navbar-collapse" id="navbarNav">
                                                 <ul class="navbar-nav">
                                                     <li class="nav-item">
                                                     <a class="nav-link" aria-current="page" href="@Url.Action("Index", "User")">Admin</a>
                                                     </li>
                                                     <!-- code omitted -->
                                                    <partial name="_LoginPartial" />
                                                 </ul>
                                           </div>  


        -  Esempio di Crud              
                                    [Authorize(Roles = "User,Admin")]
                                    public class PizzaController : Controller
                                    {
                                        private ICustomLog _myLogger;
                                        private PizzaContext _db;


                                        public PizzaController(ICustomLog log, PizzaContext db)
                                        {
                                            _myLogger = log;
                                            _db = db;

                                        }
                                         [HttpGet]
                                            public IActionResult Index()
                                            {
                                                _myLogger.WriteLog($"L'utente è arrivato nel admins index");
                                                List<Pizza> pizzas = _db.Pizza.ToList();
                                                return View(pizzas);
                                            }


        -   Esempio di come dovra venire fuori il Program.cs modificato anche con le Dipendence Injection e con i ruoi di autorizzazione
        
                
                                builder.Services.AddDbContext<PizzaContext>();
                                
                                builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                                    .AddRoles<IdentityRole>()
                                    .AddEntityFrameworkStores<PizzaContext>();
                                
                                // Add services to the container.
                                builder.Services.AddControllersWithViews();
                                
                                //codice per json in modo che ignori le dipendenza cicliche di eventuali relazioni N:N o 1:N
                                builder.Services.AddControllers().AddJsonOptions(x =>
                                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
                                
                                // implementazione Dipendency injection del DB + Custom FileLogger
                                builder.Services.AddScoped<ICustomLog, CustomFileLogger>();
                                builder.Services.AddScoped<PizzaContext, PizzaContext>();
                                builder.Services.AddScoped<IRepositoryPizza, RepositoryPizza>();
                                
                                
                                var app = builder.Build();
                                
                                // Configure the HTTP request pipeline.
                                if (!app.Environment.IsDevelopment())
                                {
                                    app.UseExceptionHandler("/Home/Error");
                                    app.UseHsts();
                                }
                                
                                app.UseHttpsRedirection();
                                app.UseStaticFiles();
                                
                                app.UseRouting();
                                app.UseAuthentication();
                                app.UseAuthorization();
                                
                                app.UseCors(options => options // per i CORS
                                    .WithOrigins("http://127.0.0.1:5500") 
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
                                
                                
                                app.MapControllerRoute(
                                    name: "default",
                                    pattern: "{controller=Home}/{action=Index}/{id?}");
                                
                                app.MapRazorPages();
                                
                                app.Run();



        - Come gestire relazioni 1:N e N:N

                    public class Pizza
                    {
                        public long Id { get; set; }

                        [Required(ErrorMessage = "Il nome è obbligatorio")]
                        [StringLength(15, ErrorMessage = "Il nome massimo 15 caratteri dai su")]
                        public string Name { get; set; }

                        [Required(ErrorMessage = "la descrizione è obbligatoria")]
                        [DescriptionValidation]
                        [StringLength(1000, ErrorMessage = "massimo 1000 caratteri")]
                        public string Description { get; set; }

                        [Url]
                        [Required(ErrorMessage = "L'URL è obbligatorio")]
                        public string Photo { get; set; }

                        [PriceValidation]
                        [Required]
                        public double Price { get; set; }


                        //relazione 1:N con category
                        public long? CategoryId { get; set; }
                        public Category? Category { get; set; }


                        //relazione N:N
                        public List<Ingredient>? Ingredients { get; set; }



                        public Pizza() { }

                        public Pizza(string name, string description, string photo, double price)
                        {
                            Name = name;
                            Description = description;
                            Photo = photo;
                            Price = price;
                        }
                    }

                      public class Category (1:N)
                        {
                            public long Id { get; set; }

                            [Required(ErrorMessage = "Il titolo è obbligatorio")]
                            [StringLength(50, ErrorMessage = "Il titolo non puo superare i 50 caratteri")]
                            public string Name { get; set; }

                            // per relazione 1:N e per default di MVC rendiamoli nullable
                            public List<Pizza>? Pizzas { get; set; }

                            public Category() { }
                        }

                         public class Ingredient(N:N)
                            {
                                public long Id { get; set; }
                                public string Name { get; set; }
                                public List<Pizza>? Pizzas { get; set; }

                                public Ingredient() { }
                            }

            e si crea un nuovo model per gestire i form inserimento dati
                                   public class PizzaFormModel
                                    {
                                        public Pizza? Pizza { get; set; }
                                        public List<Category>? Categories { get; set; }

                                        public List<SelectListItem>? Ingredients { get; set; }

                                        public List<string>? SelectedIngredients { get; set; }
                                    }


        - PER LE AUTH : Per proteggere tutte le action di un controller.Ci basta aggiungere l'attribute [Authorize] sopra la definizione di classe e/o sulle singole action
                        Per specificare i ruoli necessari per tutte le action di un controller.Ci basta aggiungere Roles seguito dal ruolo o i ruoli
                    [Authorize(Roles = "ADMIN,USER")]
                    public class ProfileController : Controller
                    {

                    [Authorize(Roles = "ADMIN")]
                    [HttpGet]
                    public IActionResult Details(int id)
                    {


        - PER LE API: aggiungere api controller dentro cartella api

                namespace NetCore.Controllers.Api
                 {
                  [Route("api/Profiles/[action]")]
                  [ApiController]
                  public class ProfilesController : ControllerBase
                  {
                  }
                 }

                                delle response HTTP con i relativi dati da restituire e il relativo codice HTTP di stato.
                                    Ok(): restituisce un codice di stato 200 senza nessun body come risposta.
                                    Ok(object): restituisce 200 con annesso anche un body
                                    Created(string, object): restituisce 201 informando che la risorsa è stata salvata
                                    correttamente
                                    NotFound(): restituisce 404 informando che la risorsa non è stata trovata
                                    UnprocessableEntity(): restituisce 422 informando che ad esempio i dati di un
                                    modello non sono validi

                        [HttpGet]
                        public IActionResult GetPizzas()
                        {
                          List<Pizza> pizzas =_db.Pizza
                                        .Include(p => p.Ingredients)
                                        .Include(P => P.Category)
                                        .ToList();
                               return pizzas;
                        }

            PER LE POST/PUT OCCORRE UN ATTRIBUTO IN PIU 
                                [HttpPost]
                                    public IActionResult Create([FromBody]Pizza pizza)

     */
}
