
namespace NationalDisplay.Models;

public class PlanListModel{
    List<PlanInfo> planInfoList = new List<PlanInfo>();
    public PlanModel LoadPlanImage(string id)
    {
        Console.WriteLine("Load plan : " + id);

        PlanModel model = new PlanModel();

        // 플랫폼에서 통신으로 도면 정보, 도면 ID와 일치하는 센서 가져오기
        // 센서에는 도면 ID가 포함되어 있다.

        return model;
    }

    public List<PlanInfo> LoadPlanListInfo()
    {
        planInfoList.Clear();

        PipeClient.Start();
        // 도면 개수 확인하기
        // 도면 개수 만큼 planInfoList.Add(LoadPlanInfo)

        return planInfoList;
    }

    public PlanInfo LoadPlanInfo()
    {
        PlanInfo info = new PlanInfo();

        // 플랫폼에 요청 후 수신하기

        return info;
    }
}

public class PlanInfo{
    string id;
    string address;
    string buildingName;
    string dong;
    string floor;
    string ho;
}