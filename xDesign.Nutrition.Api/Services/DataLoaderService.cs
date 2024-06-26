﻿using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using xDesign.Nutrition.Api.Model;
using xDesign.Nutrition.Api.Util;
using System.Text.Json;
using System.Xml;

namespace xDesign.Nutrition.Api.Services
{
    public class DataLoaderService : IDataLoaderService
    {
        public IEnumerable<Food> LoadFoodsFromCsvFile(string _csvFileName)
        {
            using var readStream = File.OpenRead(_csvFileName);
            using var reader = new StreamReader(readStream);
            using var csvReader = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));

            var foods = new List<Food>();

            csvReader.Read();
            csvReader.ReadHeader();
            while (csvReader.Read())
            {
                var name = csvReader.GetField(CsvColumnHeadings.NameField);
                var calories = csvReader.GetField(CsvColumnHeadings.CaloriesField);
                var totalFat = csvReader.GetField(CsvColumnHeadings.TotalFatField);
                var caffeine = csvReader.GetField(CsvColumnHeadings.CaffeineField);
                var servingSize = csvReader.GetField(CsvColumnHeadings.ServingSizeField);

                if (IsValidFood(name, calories, totalFat, caffeine, servingSize))
                {
                    foods.Add(new Food
                    {
                        Name = name!,
                        Calories = int.Parse(calories!),
                        TotalFat = double.Parse(totalFat![..totalFat.IndexOf('g')]),
                        Caffeine = caffeine!
                    });
                }
            }

            return foods;
        }

        private static bool IsValidFood(string? name, string? calories, string? totalFat, string? caffeine, string? servingSize)
        {
            var requiredFields = new[] { name, calories, totalFat, caffeine, servingSize };
            if (requiredFields.Any(string.IsNullOrWhiteSpace))
            {
                return false;
            }

            return servingSize == "100 g";
        }

        public IEnumerable<Food> LoadFoodsFromJsonFile(string fileName)
        {
            // TODO add logic here 
            using var jsonFileStream = File.OpenRead(fileName);
            List<Food> data = JsonSerializer.Deserialize<List<Food>>(jsonFileStream);
            throw new NotImplementedException();

        }

        public IEnumerable<Food> LoadFoodsFromXmlFile(string fileName)
        {
            // TODO add logic here 
            using var xmlReader = XmlReader.Create(fileName);
            throw new NotImplementedException();
        }
    }
}
