using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Interon.Roadlab.Core.Enums;
using Interon.Roadlab.LIMS.DTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Interon.Roadlab.App.Core.Models
{
    [Table("ClientRequest")]
    public class ClientRequest

    {
        private TokenData _token;

        [PrimaryKey]
        [NotNull]

        public int Id { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public string StatusData { get; set; }
        [Ignore]
        public List<string> Status
        {
            get
            {
                try
                {
                   return  JsonConvert.DeserializeObject<List<string>>(StatusData);
                     
                }
                catch
                {
                    return new List<string>();
                }
            }
            set
            {
                StatusData = JsonConvert.SerializeObject(value);
            }
        }


        public string TokenData { get; set; }
        [Ignore]
        public TokenData Token
        {
            get
            {
                return JsonConvert.DeserializeObject<TokenData>(TokenData);
            }
            set
            {
                TokenData = JsonConvert.SerializeObject(value);
            }


        }

        [Ignore]
        public string LocalStatus => Status.FirstOrDefault().StatusContverter();
        [Ignore]
        public string SiteString
        {
            get
            {
                try
                {
                    var _site = (JObject)Token.Site;

                    try
                    {
                        return _site.ToObject<Site>().Name.ToString().Substring(1, 50) + "...";
                    }
                    catch
                    {
                        return _site.ToObject<Site>().Name;
                    }
                }
                catch (Exception e)
                {
                    try
                    {
                        return Token.Site.ToString().Substring(1, 50) + "...";
                    }
                    catch
                    {
                        return Token.Site.ToString();
                    }

                }
            }



        }
        [Ignore]
        public Site Site {
            get
            {
                try
                {
                    var _site = (JObject)Token.Site;

                    try
                    {
                        return _site.ToObject<Site>();
                    }
                    catch
                    {
                        return new Site()
                        {
                            Name = SiteString,
                            Id = 0
                        };
                    }
                }
                catch
                {
                    return new Site();

                }
            }
            
        }
        [Ignore]
        public string RequestDetails
        {
            get
            {
                try
                {
                    return Token.RequestedServiceDetails.ToString().Substring(1, 50) + "...";
                }
                catch
                {
                    return Token.RequestedServiceDetails.ToString();
                }
            }



        }
        private static string GetNumbers(string input)
        {
            return new string(input.Where(c => char.IsDigit(c)).ToArray());
        }
        [Ignore]
        public string QuotationTotal
        {
            get
            {
                try
                {
                    string temp = GetNumbers(Token.QuotationTotal);
                    if (string.IsNullOrWhiteSpace(temp))
                    {
                        return "R 0.00";
                    }
                    return $"{Convert.ToDouble(temp):C}";
                }
                catch
                {
                    return "R 0.00";
                }

            }



        }
    }
}