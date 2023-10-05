using AddressBookHG_BL.InterfacesOfManagers;
using AddressBookHG_DL.ImplementationsOfRepos;
using AddressBookHG_DL.InterfaceOfRepos;
using AddressBookHG_EL.Entities;
using AddressBookHG_EL.IdentityModels;
using AddressBookHG_EL.ViewModels;
using AddressBookHG_PL.Models;
using AddressBookHG_PL.PostalCodeAPIModels;
using AddressBookHG_UL;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;

namespace AddressBookHG_PL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICityRepo _cityRepo; // illeri repodan aldık :D
        private readonly IDistrictManager districtManager;
        private readonly INeighborhoodManager _neighborhoodManager;
        private readonly IUserAddressManager _userAddressManager;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

            public HomeController(ILogger<HomeController> logger, ICityRepo cityRepo, IDistrictManager districtManager, INeighborhoodManager neighborhoodManager, IUserAddressManager userAddressManager, IMapper mapper, UserManager<AppUser> userManager)
            {
                _logger = logger; // görüntü oalrak daha güzel
                _cityRepo = cityRepo; // görüntü oalrak daha güzel
                this.districtManager = districtManager;
                _neighborhoodManager = neighborhoodManager; // görüntü oalrak daha güzel
                _userAddressManager = userAddressManager; // görüntü oalrak daha güzel
                _mapper = mapper;
                _userManager = userManager;
            }

            public IActionResult Index()
            {
                return View();
            }

            [Authorize]
            public IActionResult Indexim()
            {
                return View();
            }

            [Authorize]
            public IActionResult Address()
            {
                try
                {
                    //Tüm iller sayfaya gitsin
                    var city = _cityRepo.GetAll();
                    ViewBag.AllCity = _mapper.Map<IQueryable<City>, List<CityDTO>>(city)
                        .OrderBy(x => x.Name).ToList();

                    //eğer repo kullanırsak dönüşümü kendimiz yapacağoz
                    //eğer manager kullanırsak bize zaten dönüşümlü

                    //useridyi sayfaya gönderelim böylece adres eklemede useridyi metoda aktarabiliriz
                    var username = User.Identity?.Name;
                    var user = _userManager.FindByNameAsync(username).Result;
                    ViewBag.UserId = user.Id;
                    return View();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "HATA: Home/Address");
                    ViewBag.AllCity = new List<CityDTO>();

                    return View();

                }
            }

            [HttpGet]
            public JsonResult GetDistrictsofCity(int id)
            {
                try
                {
                    var data = districtManager.GetAll(x => x.CityId == id && !x.IsDeleted).Data.OrderBy(x => x.Name).ToList();

                    if (data == null)
                    {
                        return Json(new { issucces = false, message = "İlçeler bulunamadı!" });
                    }

                    return Json(new { issucces = true, message = "İlçeler bulundu!", data });


                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"HATA: Home/GetDistrictsofCity cityId:{id}");
                    return Json(new { issucces = false, message = ex.Message });
                }
            }



            [HttpGet]
            public JsonResult GetNeighborhoodsofDistrict(int id)
            {
                try
                {
                    var data = _neighborhoodManager.GetAll(x => x.DistrictId == id && !x.IsDeleted).
                        Data.OrderBy(x => x.Name).ToList(); ;

                    if (data == null)
                    {
                        return Json(new { issucces = false, message = "Mahalle bulunamadı!" });
                    }

                    return Json(new { issucces = true, message = "Mahalle bulundu!", data });


                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"HATA: Home/GetNeighborhoodsofDistrict district:{id}");
                    return Json(new { issucces = false, message = ex.Message });
                }
            }


        [HttpPost]
        [Authorize]
        public JsonResult AddAddress([FromBody] UserAddressDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError($"HATA: Home/AddAddress ModelState is not valid model:{model}");
                    return Json(new { issucces = false, message = $"Eksik ya da hatalı bilgi girişi nedeniyle adres kaydedilemedi!" });

                }
                model.InsertedDate = DateTime.Now;
                model.IsDeleted = false;

                //Acaba yeni eklenilmesi istenen adres "isdefaultadres" olarak mı işaretlendi?
                if (model.IsDefaultAddress)
                {
                    // kullanıcının isdefault olan başka adresi var mı?
                    var defaultadress = _userAddressManager.GetAll(x => x.UserId == model.UserId
                    && x.IsDefaultAddress).Data;

                    foreach (var item in defaultadress)
                    {
                        item.IsDefaultAddress = false;
                        _userAddressManager.Update(item);
                    }
                }


                var result = _userAddressManager.Add(model);
                if (!result.IsSuccess)
                {
                    _logger.LogError($"HATA: Home/AddAddress adress eklenemedi! result.IsSuccess model:{model}");
                    return Json(new { issucces = false, message = $"Beklenmedik bir sorun nedeniyle adres eklenemedi!" });
                }
                _logger.LogInformation($"BİLGİ: Home/AddAddress yeni adress eklendi. model:{model}");
                return Json(new { issucces = true, message = $"Yeni adres eklendi!" });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"HATA: Home/AddAddress model:{model}");
                return Json(new { issucces = false, message = $"Beklenmedik bir hata oluştu!" });
            }
        }



        [HttpGet]
        [Authorize]
        public JsonResult GetAllAddress()
        {
            try
            {
                var username = User.Identity?.Name;
                var user = _userManager.FindByNameAsync(username).Result;
                var address = _userAddressManager.GetAll(x => x.UserId == user.Id && !x.IsDeleted).Data;
                foreach (var item in address)
                {
                    //adresin bağlı olduğu ilçe ve il bilgisini istiyoruz
                    //İSTANBUL - KADIKÖY Çiçek mah. Gül sok. No:1
                    var district = districtManager.GetByCondition(x => x.Id == item.Neighborhood.DistrictId).Data;

                    item.FullAddress = $"{item.Neighborhood.Name} {item.Details}\n{district.City.Name.ToUpper()} - {district.Name.ToUpper()} ";
                    item.AddressDetail = $"{item.Neighborhood.Name} {item.Details}";
                    item.CityandDistrict = $"{district.City.Name.ToUpper()} - {district.Name.ToUpper()} ";
                }

                if (address == null)
                {
                    return Json(new { issucces = false, message = "Adresler bulunamadı!" });
                }

                return Json(new { issucces = true, message = "Adresler bulundu!", address });


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"HATA: Home/GetAllAddress");
                return Json(new { issucces = false, message = $"Beklenmedik bir hata oluştu!" });
            }
        }


        [Authorize]
        public JsonResult DeleteAddress(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogError($"HATA: Home/DeleteAddress {id} değeri sıfırdan küçük olamaz!");
                    return Json(new { issuccess = false, message = $"id değeri sıfırdan küçük olamaz!" });
                }
                var address = _userAddressManager.GetbyId(id).Data;
                if (address == null)
                {
                    _logger.LogError($"HATA: Home/DeleteAddress {id} değeri sıfırdan küçük olamaz!");
                    return Json(new { issuccess = false, message = $"adres bulunamadı!" });
                }
                //  var result = _userAddressManager.Delete(address);  // hard delete
                address.IsDeleted = true;
                address.IsDefaultAddress = false;
                var result = _userAddressManager.Update(address);  // soft delete

                if (!result.IsSuccess)
                {
                    _logger.LogError($"HATA: Home/DeleteAddress Silme işlemi başarısız!");
                    return Json(new { issuccess = false, message = $"İşlem başarılı değil!" });
                }
                _logger.LogInformation($"Home/DeleteAddress adres silindi!");
                return Json(new { issuccess = true, message = $"Adres silindi!" });


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"HATA: Home/DeleteAddress");
                return Json(new { issuccess = false, message = $"Beklenmedik bir hata oluştu!" });
            }
        }


        public JsonResult GetPostalCode(byte cityid, int districtid, int neighid)
        {
            try
            {

                var district = districtManager.GetbyId(districtid).Data;
                var neigh = _neighborhoodManager.GetbyId(neighid).Data;

                if (district == null || neigh == null || cityid <= 0 || cityid > 81)
                {
                    _logger.LogError($"HATA: Home/GetPostalCode Posta kodu bulunamadı!");
                    return Json(new { issuccess = false, message = $"il-ilçe-mahalle bilgilerini eksiksiz girmelisiniz!" });
                }


                using (WebClient client = new WebClient())
                {
                    #region GET YAPILIYOR
                    string url = $"{Utilities.PostalCodeAPIURL}{cityid}";
                    //appsettings json dosyasından postakoduapi urli alalım


                    var response = client.DownloadString(url);
                    #endregion

                    #region GET NETİCESİNDE ÇIKTI ALINIYOR
                    //Elimdeki json'i objelere aktarmaliyim.
                    var result = JsonConvert.DeserializeObject<PostalCodeAPIModel1>(response);



                    var postalcodeResult = result.postakodu.FirstOrDefault(x => x.ilce == district.Name.ToUpper() && x.mahalle == neigh.Name.ToUpper());


                    if (postalcodeResult == null)
                    {
                        _logger.LogError($"HATA: Home/GetPostalCode Posta kodu bulunamadı! districtid={district.Id} name={district.Name.ToUpper()} neighid={neigh.Id} name={neigh.Name.ToUpper()}");
                        return Json(new { issuccess = false, message = $"Posta kodu bulunamadı!" });
                    }

                    _logger.LogInformation($"Home/GetPostalCode Posta kodu geldi. districtid={district.Id} name={district.Name.ToUpper()} neighid={neigh.Id} name={neigh.Name.ToUpper()}");

                    return Json(new { issuccess = true, message = $"Posta kodu geldi!", postalcode = postalcodeResult.pk });


                    #endregion

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"HATA: Home/GetPostalCode");
                return Json(new { issuccess = false, message = $"Beklenmedik bir hata oluştu!" });
            }
        }
    }
}
    