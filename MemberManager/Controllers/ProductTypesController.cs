using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MemberManager.Models.ViewSearchModel;
using MemberManager.Models.DbModels;
using MemberManager.Manager;
using Microsoft.AspNetCore.Http;
using MemberManager.Context;
using MemberManager.Extensions;
using Newtonsoft.Json;

namespace MemberManager.Controllers
{
    public class ProductTypesController : Controller
    {
        private readonly ISession session;
        private readonly ProductTypesManager productTypesManager;

        public ProductTypesController(
            ProductTypesManager _productTypesManager,

            IHttpContextAccessor _httpContextAccessor)
        {
            productTypesManager = _productTypesManager;

            session = _httpContextAccessor.HttpContext.Session;
        }

        public async Task<IActionResult> Index(ProductTypesSearchModel parameters = null)
        {
            UserContext userContext = session.GetObjectFromJson<UserContext>(UserContext.SESSION_NAME.ToString());

            List<ProductTypes> productTypes = null;
            if(parameters != null)
            {
                ProductTypesManager.Criteria criteria = new ProductTypesManager.Criteria();
                criteria.name = parameters.productTypeName;
                productTypes = await productTypesManager.ExcuteQuery(criteria);
            }
            else
            {
                productTypes = new List<ProductTypes>();
            }
            
            ViewData["searchParameters"] = parameters;
            return View(productTypes);
        }

        public IActionResult Edit(Int64 id = 0)
        {
            ProductTypes productTypes = new ProductTypes();
            if (id > 0)
                productTypes = productTypesManager.GetById(Convert.ToInt64(id));

            return View(productTypes);
        }

        public string Removed(Int64 id)
        {
            Dictionary<String, Object> valueObject = new Dictionary<string, object>();

            try
            {
                productTypesManager.Removed(id);

                valueObject.Add("success", true);
                valueObject.Add("message", "");
            }
            catch(Exception ex)
            {
                valueObject.Add("success", false);
                valueObject.Add("message", "刪除失敗，錯誤訊息:" + ex.Message);
            }

            return JsonConvert.SerializeObject(valueObject);
        }

        public async Task<string> Save(ProductTypes productTypes)
        {
            Dictionary<String, Object> valueObject = new Dictionary<string, object>();

            ProductTypes editProductTypes = null;
            if (productTypes.id > 0)
            {
                editProductTypes = productTypesManager.GetById(productTypes.id);
                if (editProductTypes != null)
                {
                    editProductTypes.name = productTypes.name;
                    editProductTypes.sort = productTypes.sort;
                }
            }
            else
                editProductTypes = productTypes;

            try
            {
                if (editProductTypes != null)
                {
                    string result = await Verification(editProductTypes);
                    if (string.IsNullOrWhiteSpace(result))
                    {
                        productTypesManager.Save(editProductTypes);

                        valueObject.Add("success", true);
                        valueObject.Add("message", "儲存成功");
                    }
                    else
                        throw new Exception(result);
                }
                else
                    throw new Exception("資料錯誤!");
            }
            catch (Exception ex)
            {
                valueObject.Add("success", false);
                valueObject.Add("message", "儲存失敗，錯誤訊息:" + ex.Message);
            }

            return JsonConvert.SerializeObject(valueObject);
        }

        public async Task<string> Verification(ProductTypes productTypes)
        {
            string result = "";

            ProductTypesManager.Criteria criteria = new ProductTypesManager.Criteria();
            criteria.name = productTypes.name;


            List<ProductTypes> tempProductTypeses = await productTypesManager.ExcuteQuery(criteria);
            if (tempProductTypeses != null && tempProductTypeses.Count > 0)
            {
                ProductTypes tempProductTypes = tempProductTypeses.FirstOrDefault();
                if ((productTypes.id <= 0 && tempProductTypeses.Count >= 1) ||
                    (productTypes.id > 0 && tempProductTypeses.Count > 1) ||
                    (tempProductTypeses.Count == 1 && tempProductTypes.id != productTypes.id))
                {
                    result = "產品名稱重複";
                }
            }

            return result;
        }
    }
}
