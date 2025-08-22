using DataAccessLayer;
using System;
using System.Data;

namespace BusinessLayer
{
    public class clsCountry
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }

        public clsCountry()
        {
            this.CountryID = -1;
            this.CountryName = "";
        }
        private clsCountry(int countryID, string countryName)
        {
            CountryID = countryID;
            CountryName = countryName;
        }

        public static clsCountry FindCountry(int _CountryID)
        {
            string countryName = "";

            bool isFound = clsCountriesData.GetCountryInfoByID(_CountryID, ref countryName);

            if (isFound)
            {
                return new clsCountry(_CountryID, countryName);
            }
            else
            {
                return null;
            }
        }

        public static clsCountry FindCountry(string _CountryName)
        {
            int countryID = -1;

            bool isFound = clsCountriesData.GetCountryInfoByName(ref countryID, _CountryName);

            if (isFound)
            {
                return new clsCountry(countryID, _CountryName);
            }
            else
            {
                return null;
            }
        }

        public static DataTable GetAllCountries()
        {
            return DataAccessLayer.clsCountriesData.GetAllCountries();
        }
    }
}
