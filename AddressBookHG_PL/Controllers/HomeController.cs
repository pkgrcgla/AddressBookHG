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


    }
}