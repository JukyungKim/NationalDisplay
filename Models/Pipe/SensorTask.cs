

namespace NationalDisplay.Models;

public class SensorTask
{
    static public string planId = "";
    public static async void Start()
    {
        await RequestSensorInfo();
    }
    public static Task RequestSensorInfo()
    {
        Action act = () =>
        {
            Console.WriteLine("Start request sensor info task");

            while(true)
            {
                Thread.Sleep(5000);
                

                if(planId != ""){
                    Console.WriteLine("Request sensor info");
                    PipeClient.command = 2;
                    PipeClient.Start();
                }                
            
                // 10초마다 센서 정보를 파이프 통해 요청하기
                // 도면 화면일 경우에만 실행                
            }
        };
        Task task = new Task(act);
        task.Start();

        return task;
    }
}