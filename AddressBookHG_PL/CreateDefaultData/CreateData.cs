using AddressBookHG_BL.EmailSenderProcess;
using AddressBookHG_BL.InterfacesOfManagers;
using AddressBookHG_DL.ContextInfo;
using AddressBookHG_EL.Entities;
using AddressBookHG_EL.IdentityModels;
using AddressBookHG_EL.ViewModels;
using AddressBookHG_UL;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Serilog.Core;

namespace AddressBookHG_PL.CreateDefaultData
{
    public class CreateData
    {
        private readonly Logger _logger;

        public CreateData(Logger logger)
        {
            _logger = logger;
        }

        public void CreateRoles(IServiceProvider serviceProvider)
            {

                var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
                var emailManager = serviceProvider.GetRequiredService<IEmailManager>();

                //var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
                var context = serviceProvider.GetService<AddressbookContext>();
                var configuration = serviceProvider.GetService<IConfiguration>();// appsettings.json dosyasına ulaşmak için

                string[] emails = configuration["ManagerEmails"].Split(',');
                var allRoles = Enum.GetNames(typeof(AllRoles));


                foreach (string role in allRoles)
                {
                    var result = roleManager.RoleExistsAsync(role).Result; //rolden var mı?
                    if (!result) //rolden yok!
                    {
                        AppRole r = new AppRole()
                        {
                            InsertedDate = DateTime.Now,
                            Name = role,
                            IsDeleted = false,
                            Description = $"Sistem tarafından oluşturuldu"
                        };
                        var roleResult = roleManager.CreateAsync(r).Result;

                        //roleresulta bakalım
                        if (roleResult.Succeeded)
                        {
                            foreach (var item in emails)
                            {
                                //emailManager.SendEmail(new EmailMessageModel()
                                //{
                                //    Body = $"Az önce sistem tarafından {r} rolü oluşturuldu",
                                //    Subject = "AddressBook Role Oluşturuldu",
                                //    To = item
                                //});
                            }


                        }
                        else
                        {
                            //log email
                        }
                    }

                }



            }

            public void CreateAllCity(IServiceProvider serviceProvider)
            {
                try
                {
                    //manager almak zorunda değilsiniz repoda alabilirdiniz...
                    var cityManager = serviceProvider.GetRequiredService<ICityManager>();

                    //var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ExcelFiles");
                    //var filename = "Cities.xlsx";
                    //var filePath=Path.Combine(path, filename);

                    var file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ExcelFiles", "Cities.xlsx");

                    using (var wbook = new XLWorkbook(file))
                    {
                        var ws1 = wbook.Worksheet(1);
                        var rows = ws1.RowsUsed();
                        foreach (var item in rows)
                        {
                            if (item.RowNumber() > 1) // başlık satırını atladık
                            {
                                var cityname = item.Cell(1).Value.ToString();
                                var platecode = item.Cell("B").Value.ToString();
                                CityDTO c = new CityDTO()
                                {
                                    InsertedDate = DateTime.Now,
                                    Name = cityname,
                                    PlateCode = platecode,
                                    IsDeleted = false
                                };
                                //eğer bu il veritabanında yoksa ekle!
                                if (cityManager.GetByCondition(x => x.Name.ToLower() == cityname.ToLower()).Data == null)
                                {
                                    cityManager.Add(c);

                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    //loglanabilir
                }
            }


            public void CreateAllDistrict(IDistrictManager districtManager, ICityManager cityManager)
            {
                try
                {
                    var file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ExcelFiles", "Districts.xlsx");

                    using (var wbook = new XLWorkbook(file))
                    {
                        var ws1 = wbook.Worksheet(1);
                        var rows = ws1.RowsUsed();
                        foreach (var item in rows)
                        {
                            if (item.RowNumber() > 1) // başlık satırını atladık
                            {
                                var districtname = item.Cell(1).Value.ToString();
                                var cityplatecode = item.Cell(2).Value.ToString();

                                var city = cityManager.GetByCondition(x => x.PlateCode == cityplatecode).Data;

                                DistrictDTO d = new DistrictDTO()
                                {
                                    InsertedDate = DateTime.Now,
                                    CityId = city.Id,
                                    Name = districtname,
                                    IsDeleted = false
                                };
                                if (districtManager.GetByCondition
                                    (x => x.Name.ToLower() == districtname.ToLower()
                                    && x.CityId == city.Id
                                    ).Data == null)
                                {
                                    districtManager.Add(d);

                                }


                            }
                        }
                    }


                }
                catch (Exception)
                {
                    //loglanabilir
                }
            }


            public void CreateAllNeighborhood(IServiceProvider serviceProvider)
            {

                try
                {
                    var context = serviceProvider.GetService<AddressbookContext>();
                    var file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ExcelFiles", "Neighborhood.xlsx");

                    using (var wbook = new XLWorkbook(file))
                    {
                        var ws1 = wbook.Worksheet(1);
                        var rows = ws1.RowsUsed();
                        foreach (var item in rows)
                        {
                            if (item.RowNumber() >1 ) // başlık satırını atladık
                            {
                                var cityname = item.Cell("A").Value.ToString().Trim(); //İStanbul
                                var districtname = item.Cell("B").Value.ToString().Trim(); //Kadıköy
                                var neigh = item.Cell("C").Value.ToString().Trim(); //hasanpaşa mah

                                var city = context.CityTable.Where(x => x.Name.ToLower() == cityname.ToLower()).FirstOrDefault();

                                var district = context.DistrictTable.Where(x =>
                                x.Name.ToLower() == districtname.ToLower()
                                && x.CityId == city.Id).FirstOrDefault();

                                Neighborhood n = new Neighborhood()
                                {
                                    InsertedDate = DateTime.Now,
                                    Name = neigh,
                                    DistrictId = district.Id,
                                    IsDeleted = false

                                };
                                //List<byte> istedigimIller = new List<byte>() { 34, 6, 35 };

                                if (context.NeighborhoodTable.Where(x => x.DistrictId == district.Id &&
                                x.Name.ToLower() == neigh.ToLower()).FirstOrDefault() == null
                                //&& istedigimIller.Contains(city.Id)
                                )
                                {
                                    context.NeighborhoodTable.Add(n);                                  
                                    context.SaveChanges();
                                _logger.Information($"{DateTime.Now.ToString()} -  CreateData/CreateAllNeighborhood  - BİLGİ: {neigh} mahallesi eklendi ");

                            }
                            }
                        }
                    }
                }
            catch (Exception ex)
            {
                _logger.Error($"{DateTime.Now.ToString()} -  CreateData/CreateAllNeighborhood  - HATA: {ex} ");

            }
        }
        }
    }
