
public class PipeProtocol
{
    enum EProtocol
    {
        SENSOR_INFO = 1, SENSOR_DATA, PLAN_IMAGE, PLAN_INFO
    };
    public static void selectProtocol(int protocol, List<byte> data)
    {
        switch ((EProtocol)protocol)
        {
            case EProtocol.SENSOR_INFO:
                sendSensorInfo(protocol, data);
                break;
            case EProtocol.SENSOR_DATA:
                sendSensorData(protocol, data);
                break;
            case EProtocol.PLAN_IMAGE:
                sendPlanImage(protocol, data);
                break;
            default:
                break;
        }
    }
    public static void receivedData(byte[] data)
    {
        int command = data[0];

        switch ((EProtocol)command)
        {
            case EProtocol.SENSOR_INFO:
                
                break;
            case EProtocol.SENSOR_DATA:
                break;
            case EProtocol.PLAN_IMAGE:
                break;
            default:
                break;
        }
    }

    public static void sendSensorInfo(int planId, List<byte> data)
    {
        data.Add((byte)EProtocol.SENSOR_INFO);
        data.Add((byte)planId);
    }
    public static void sendSensorData(int sensorId, List<byte> data)
    {
        data.Add((byte)EProtocol.SENSOR_DATA);
        data.Add((byte)sensorId);
    }
    public static void sendPlanImage(int planId, List<byte> data)
    {
        data.Add((byte)EProtocol.PLAN_IMAGE);
        data.Add((byte)planId);
    }

    public static void sendPlanInfo(List<byte> data)
    {
        data.Add((byte)EProtocol.PLAN_INFO);
    }
}