using AddressBookHG_BL.InterfacesOfManagers;
using AddressBookHG_DL.ImplementationsOfRepos;
using AddressBookHG_DL.InterfaceOfRepos;
using AddressBookHG_EL.Entities;
using AddressBookHG_EL.ViewModels;
using AddressBookHG_PL.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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

        public HomeController(ILogger<HomeController> logger, ICityRepo cityRepo, IDistrictManager districtManager, INeighborhoodManager neighborhoodManager, IUserAddressManager userAddressManager, IMapper mapper)
        {
            _logger = logger;
            _cityRepo = cityRepo;
            this.districtManager = districtManager;
            _neighborhoodManager = neighborhoodManager;
            _userAddressManager = userAddressManager;
            _mapper = mapper;
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
                ViewBag.AllCity = _mapper.Map<IQueryable<City>, List<CityDTO>>(city).OrderBy(x => x.Name).ToList();

                //eğer repo kullanırsak dönüşümü kendimiz yapacağoz
                //eğer manager kullanırsak bize zaten dönüşümlü

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
                var data = _neighborhoodManager.GetAll(x => x.DistrictId == id && !x.IsDeleted).Data.OrderBy(x => x.Name).ToList(); ;

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


    }
}