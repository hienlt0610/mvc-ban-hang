using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Core;

namespace WebBanHang.Controllers
{
    public class AppApiController : BaseController
    {
        //
        // GET: /Api/
        public ActionResult Index()
        {
            return View();
        }

        
        public ActionResult Province()
        {
            //Init Result
            var provices = Repository.Province.FetchAll().OrderBy(i => i.Type + " " + i.ProvinceName).ToList();
            var provinceList = new List<object>();
            dynamic result = new ExpandoObject();
            result.status = "ok";
            result.message = "";
            result.count = provices.Count;
            result.provices = provinceList;

            //Result content
            foreach(var item in provices){
                provinceList.Add(new {
                    province_id = item.ProvinceID,
                    province_name = item.ProvinceName,
                    province_type = item.Type
                });
            }
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }

        public ActionResult District_In_Province(int? id)
        {
            //Init Result
            var districtList = new List<object>();
            dynamic result = new ExpandoObject();
            result.status = "";
            result.message = "";

            //Result content
            if (id == null)
            {
                result.status = "error";
                result.message = "ProvinceID is empty";
            }
            else
            {
                var province = Repository.Province.FindById(id);
                if(province == null){
                    result.status = "error";
                    result.message = "Province does not exist in database";
                }
                else
                {
                    result.status = "ok";
                    result.message = "";
                    result.province = new
                    {
                        province_id = province.ProvinceID,
                        province_name = province.ProvinceName,
                        province_type = province.Type
                    };
                    result.count = 0;
                    result.districts = districtList;
                    var districts = province.Districts.OrderBy(i=> i.Type+" "+i.DistrictName ).ToList();
                    foreach (var item in districts)
                    {
                        districtList.Add(new
                        {
                            district_id = item.DistrictID,
                            district_name = item.DistrictName,
                            district_type = item.Type
                        });
                    }
                    result.count = districtList.Count;
                }
            }
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }

        public ActionResult Ward_In_District(int? id)
        {
            //Init Result
            var wardList = new List<object>();
            dynamic result = new ExpandoObject();
            result.status = "";
            result.message = "";

            //Result content
            if (id == null)
            {
                result.status = "error";
                result.message = "DistrictID is empty";
            }
            else
            {
                var dictrict = Repository.District.FindById(id);
                if (dictrict == null)
                {
                    result.status = "error";
                    result.message = "District does not exist in database";
                }
                else
                {
                    result.status = "ok";
                    result.message = "";
                    result.district = new
                    {
                        district_id = dictrict.DistrictID,
                        district_name = dictrict.DistrictName,
                        district_type = dictrict.Type,
                        province = new
                        {
                            province_id = dictrict.Province.ProvinceID,
                            province_name = dictrict.Province.ProvinceName,
                            province_type = dictrict.Province.Type
                        }
                    };
                    result.count = 0;
                    result.wards = wardList;
                    var wards = dictrict.Wards.OrderBy(i=>i.Type+" "+i.WardName).ToList();
                    foreach (var item in wards)
                    {
                        wardList.Add(new
                        {
                            ward_id = item.WardId,
                            ward_name = item.WardName,
                            ward_type = item.Type,
                            district = new {
                                district_id = item.District.DistrictID,
                                district_name = item.District.DistrictName,
                                district_type = item.District.Type,
                                province = new
                                {
                                    province_id = item.District.Province.ProvinceID,
                                    province_name = item.District.Province.ProvinceName,
                                    province_type = item.District.Province.Type
                                }
                            }
                        });
                    }
                }
            }
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }

        public ActionResult TotalCart()
        {
            var total = Cart.GetCount();
            dynamic result = new ExpandoObject();
            result.count = total;
            return Content(JsonConvert.SerializeObject(result), "application/json");
        }

	}
}