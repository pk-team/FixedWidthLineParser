using CSharp.Layout;

var partLineParser = new FixedWidthLineParser<PartLineLayout>();
var line = "A1245Computer KeyboardComm32";

var partNo = partLineParser.Parse(line, x => x.PartNo); // A1245
var partDescription = partLineParser.Parse(line, x => x.Description); // Computer Keyboard
var commodityCode = partLineParser.Parse(line, x => x.CommodityCode); // 001050

Console.WriteLine($"PartNo: {partNo}");
Console.WriteLine($"Description: {partDescription}");
Console.WriteLine($"CommodityCode: {commodityCode}");
