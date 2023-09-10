using CarModelAPI.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.Formats.Asn1;
using System.Globalization;

namespace CarModelAPI.Services
{
    public class CarMakeService
    {
        private readonly string _csvFilePath = "CarMake.csv";

        public List<CarMake> GetAllCarMakes()
        {
            using (var reader = new StreamReader(_csvFilePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                return csv.GetRecords<CarMake>().ToList();
            }
        }

        public int GetCarMakeIdByName(string makeName)
        {
            var carMakes = GetAllCarMakes();
            var carMake = carMakes.FirstOrDefault(x => x.make_name.ToUpper().Trim().Equals(makeName.ToUpper().Trim(), System.StringComparison.OrdinalIgnoreCase));
            return carMake?.make_id ?? 0;
        }
    }
}
