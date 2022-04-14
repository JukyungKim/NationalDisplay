using Microsoft.AspNetCore.Mvc;

namespace NationalDisplay.Models;

public class SensorData{
    public ulong id;
    public int smoke;
    public int temp;
    public int gas;
}