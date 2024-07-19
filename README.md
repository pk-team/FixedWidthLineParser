# Fixed Width Line Parser

Assume a layout file with the following shape.

```cs
public class PartLineLayout {
    public int PartNo = 5;
    public int Description = 17;
    public int CommodityCode = 6;
}

var partLineParser = new FixedWidthLineParser<PartLineLayout>();

var line = "A1245Computer KeyboardComm32";
var partNo = partLineParser.Parse(line, x => x.PartNo); // A1245
var partDescription = partLineParser.Parse(line, x => x.Description); // Computer Keyboard
var commodityCode = partLineParser.Parse(line, x => x.CommodityCode); // 001050

```

