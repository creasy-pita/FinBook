using Contact.API.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace User.API.UnitTests
{
    public class JsonConvertDeserializeObject
    {
        [Fact]
        public void Get_ReturnRightJsonConvertDeserializeObject_WithExpectedJsonStr()
        {
            string str = "{\"id\":1,\"name\":\"creasypita\",\"company\":\"google-xplay\",\"title\":null,\"avatar\":null}";
            var obj = JsonConvert.DeserializeObject(str);
            var obj1 = JsonConvert.DeserializeObject(str) as BaseUserInfo;
            Assert.Equal("creasypita", obj1.Name.ToString());
        }


    }
}
