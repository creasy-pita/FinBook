using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using User.Identity.Dto;

namespace Contact.API.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Get_ReturnRightJsonConvertDeserializeObject_WithExpectedJsonStr()
        {
            string str = "{\"id\":1,\"name\":\"creasypita\",\"company\":\"google-xplay\",\"title\":null,\"avatar\":null}";
            //string str = "{\"id\":1,\"name\":\"creasypita\",\"company\":\"google-xplay\",\"title\":\"11\",\"avatar\":\"22\"}";
            //string str = "{\"Id\":1,\"Name\":\"creasypita\",\"Company\":\"google-xplay\",\"Title\":\"11\",\"Avatar\":\"22\"}";
            var obj = JsonConvert.DeserializeObject(str);
            var obj1 = JsonConvert.DeserializeObject(str) as BaseUserInfo;
            str = str.Replace("\"id\"", "\"userid\"");
            var obj2 = JsonConvert.DeserializeObject<BaseUserInfo>(str);
            try
            {
                Console.WriteLine("11"+ obj2.Company + "222121:" + obj2.UserId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("exception");
            }
           // Assert.Equal("creasypita", obj1.Name.ToString());
        }
    }
}
