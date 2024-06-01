using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

public static class Converter
{
    public static float ConvertFromInchToMillimeter( float value ) => value * 25.4f;

    public static float ConvertFromMillimeterToInch( float value ) => value / 25.4f;

    public static float ConvertFromCelsiusToFahrenheit( float value ) => (value * 9f / 5f) + 32;

    public static float ConvertFromFahrenheitToCelsius( float value ) => (value - 32) * 5f / 9f;
}

class Program
{
    static void Main( string[] args )
    {
        string inputFilePath = @"C:\Users\gts\Downloads\test_4.csv";
        string directory = Path.GetDirectoryName(inputFilePath);
        string fileName = Path.GetFileName(inputFilePath);
        string outputFilePath = Path.Combine(directory, "converted_" + fileName);

        var convertedValues = new List<string>();

        using (var reader = new StreamReader(inputFilePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var parts = line.Split(',');
                if (parts.Length != 2)
                {
                    Console.WriteLine($"Invalid line format: {line}");
                    continue;
                }

                if (!float.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out float value)) 
                {
                    Console.WriteLine($"Invalid value: {parts[0]}");
                    continue;
                }

                string unit = parts[1].Trim().ToLower();
                float convertedValue;
                string newUnit;

                switch (unit)
                {
                    case "inch":
                        convertedValue = Converter.ConvertFromInchToMillimeter(value);
                        newUnit = "MM";
                        break;
                    case "mm":
                        convertedValue = Converter.ConvertFromMillimeterToInch(value);
                        newUnit = "inch";
                        break;
                    case "c":
                        convertedValue = Converter.ConvertFromCelsiusToFahrenheit(value);
                        newUnit = "F";
                        break;
                    case "f":
                        convertedValue = Converter.ConvertFromFahrenheitToCelsius(value);
                        newUnit = "C";
                        break;
                    default:
                        Console.WriteLine($"Unsupported unit: {unit}");
                        continue;
                }

                convertedValues.Add($"{convertedValue},{newUnit}");
            }
        }

        using (var writer = new StreamWriter(outputFilePath))
        {
            foreach (var convertedValue in convertedValues)
            {
                writer.WriteLine(convertedValue);
            }
        }

        Console.WriteLine("Conversion complete. Check the output.csv file.");
    }
}
