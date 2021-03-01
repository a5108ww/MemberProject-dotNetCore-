using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemberManager.Manager;
using MemberManager.Models.DbModels;
using MemberManager.Models.ViewSearchModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MemberManager.Controllers
{
    public class SendTypesController : Controller
    {
        private readonly SendTypesManager sendTypesManager;

        public SendTypesController(SendTypesManager _sendTypesManager)
        {
            sendTypesManager = _sendTypesManager;
        }

        public async Task<IActionResult> Index(SendTypesSearchModel parameters = null)
        {
            SendTypesManager.Criteria criteria = new SendTypesManager.Criteria();
            criteria.name = parameters.sendTypesName;

            List<SendTypes> sendTypeses = await sendTypesManager.ExcuteQuery(criteria);
            return View(sendTypeses);
        }

        public IActionResult Edit(Int64 id = 0)
        {
            SendTypes sendTypes = new SendTypes();
            if(id > 0)
                sendTypes = sendTypesManager.GetById(id);

            return View(sendTypes);
        }

        public async Task<string> Save(SendTypes sendTypes)
        {
            Dictionary<String, Object> valueObject = new Dictionary<string, object>();

            SendTypes editSendTypes= null;
            if (sendTypes.id > 0)
            {
                editSendTypes = sendTypesManager.GetById(sendTypes.id);
                if (editSendTypes != null)
                {
                    editSendTypes.name = sendTypes.name;
                    editSendTypes.sort = sendTypes.sort;
                }
            }
            else
                editSendTypes = sendTypes;

            try
            {
                if (editSendTypes != null)
                {
                    string result = await Verification(editSendTypes);
                    if (string.IsNullOrWhiteSpace(result))
                    {
                        sendTypesManager.Save(editSendTypes);

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

        public string Removed(Int64 id)
        {
            Dictionary<String, Object> valueObject = new Dictionary<string, object>();

            try
            {
                sendTypesManager.Removed(id);

                valueObject.Add("success", true);
                valueObject.Add("message", "");
            }
            catch (Exception ex)
            {
                valueObject.Add("success", false);
                valueObject.Add("message", "刪除失敗，錯誤訊息:" + ex.Message);
            }

            return JsonConvert.SerializeObject(valueObject);
        }

        public async Task<string> Verification(SendTypes sendTypes)
        {
            string result = "";

            SendTypesManager.Criteria criteria = new SendTypesManager.Criteria();
            criteria.name = sendTypes.name;

            List<SendTypes> tempSendTypeses = await sendTypesManager.ExcuteQuery(criteria);

            //先檢查名稱是否重複
            if (tempSendTypeses != null && tempSendTypeses.Count > 0)
            {
                SendTypes tempSendTypes = tempSendTypeses.FirstOrDefault();
                if ((sendTypes.id <= 0 && tempSendTypeses.Count >= 1) ||
                 (sendTypes.id > 0 && tempSendTypeses.Count > 1) ||
                 (tempSendTypeses.Count == 1 && tempSendTypes.id != sendTypes.id))
                {
                    result = "產品名稱重複";
                }
            }

            return result;
        }
    }
}
