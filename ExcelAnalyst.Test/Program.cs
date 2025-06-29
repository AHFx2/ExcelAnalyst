//using ClosedXML.Excel;

//string path = @"F:\coding\Projects\ExcelAnalyst\file.xlsx";
//var result = AnalyzeFuelConsumption(path);

//PrintAnalysis(result);



//record CarAnalysis(double TotalConsumption, int ActiveDays);
//record AnalysisResult(
//    int TotalCars,
//    int ActiveCars,
//    double TotalConsumption,
//    double AvgConsumption,
//    double ActiveRatio,
//    CarAnalysis? MaxCar,
//    CarAnalysis? MinCar
//);

//// AnalysisResult AnalyzeFuelConsumption(string filePath)
////{
//    using var workbook = new XLWorkbook(filePath);
//    var sheet = workbook.Worksheet(1);

//    var lastRow = sheet.LastRowUsed().RowNumber();
//    int startRow = lastRow - 11;

//    List<CarAnalysis> activeCars = new();
//    int totalCars = 0;

//    for (int row = startRow; row <= lastRow; row++)
//    {
//        totalCars++;
//        double total = 0;
//        int activeDays = 0;

//        for (int col = 10; col <= 49; col++) // الأعمدة من J إلى AW
//        {
//            var cell = sheet.Cell(row, col);
//            if (double.TryParse(cell.GetValue<string>(), out double value) && value > 0)
//            {
//                total += value;
//                activeDays++;
//            }
//        }

//        if (activeDays >= 20)
//        {
//            activeCars.Add(new CarAnalysis(total, activeDays));
//        }
//    }

//    double sum = activeCars.Sum(x => x.TotalConsumption);
//    double avg = activeCars.Count > 0 ? sum / activeCars.Count : 0;
//    double ratio = totalCars > 0 ? (double)activeCars.Count / totalCars * 100 : 0;

//    var maxCar = activeCars.OrderByDescending(x => x.TotalConsumption).FirstOrDefault();
//    var minCar = activeCars.OrderBy(x => x.TotalConsumption).FirstOrDefault();

//    return new AnalysisResult(totalCars, activeCars.Count, sum, avg, ratio, maxCar, minCar);
////}


//// 👇 الطباعة والتحليل للمستخدم
//static void PrintAnalysis(AnalysisResult result)
//{
//    Console.WriteLine("📊 تحليل استهلاك البنزين");
//    Console.WriteLine(new string('-', 40));

//    Console.WriteLine($"🚗 عدد السيارات الإجمالي: {result.TotalCars}");
//    Console.WriteLine($"🔧 السيارات النشطة (استخدمت > 20 يوم): {result.ActiveCars}");
//    Console.WriteLine($"🛑 السيارات غير النشطة: {result.TotalCars - result.ActiveCars}");

//    if (result.MaxCar != null && result.MinCar != null)
//    {
//        Console.WriteLine($"🔥 أعلى استهلاك: {result.MaxCar.TotalConsumption:F2} لتر");
//        Console.WriteLine($"🐢 أقل استهلاك (ضمن النشطين): {result.MinCar.TotalConsumption:F2} لتر");
//    }

//    Console.WriteLine($"📈 نسبة السيارات النشطة: {result.ActiveRatio:F1}%");

//    if (result.ActiveCars > 0)
//        Console.WriteLine($"✅ معدل استهلاك النشطين: {result.AvgConsumption:F2} لتر");
//    else
//        Console.WriteLine("❌ لا توجد سيارات نشطة حسب الشروط.");
//}
