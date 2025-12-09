// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;


// using System.IO.Ports;   // ต้องมีบรรทัดนี้

// public class SerialController : MonoBehaviour

// {

//     public string portName = "COM3";  // เปลี่ยนเป็น COM ของ Arduino

//     public int baudRate = 9600;

//     private SerialPort port;

//     public ReactionManager reactionManager;

//     void Start()

//     {

//         try

//         {

//             port = new SerialPort(portName, baudRate);

//             port.ReadTimeout = 20;

//             port.Open();

//             Debug.Log("Serial opened on " + portName);

//         }

//         catch (System.Exception e)

//         {

//             Debug.LogError("Cannot open serial port: " + e.Message);

//         }

//     }

//     void Update()

//     {

//         if (port == null || !port.IsOpen) return;

//         try

//         {

//             string msg = port.ReadLine();

//             if (!string.IsNullOrEmpty(msg))

//             {

//                 msg = msg.Trim();

//                 if (msg == "PRESSED" && reactionManager != null)

//                 {

//                     reactionManager.OnButtonPressed();

//                 }

//             }

//         }

//         catch (System.TimeoutException)

//         {

//             // เงียบไป ไม่เป็นไร แค่ไม่มีข้อมูลเข้ามา

//         }

//         catch (System.Exception e)

//         {

//             Debug.LogWarning("Serial read error: " + e.Message);

//         }

//     }

//     public void SendHit()

//     {

//         if (port != null && port.IsOpen)

//         {

//             port.WriteLine("HIT");

//         }

//     }

//     public void SendMiss()

//     {

//         if (port != null && port.IsOpen)

//         {

//             port.WriteLine("MISS");

//         }

//     }

//     void OnApplicationQuit()

//     {

//         if (port != null && port.IsOpen)

//         {

//             port.Close();

//         }

//     }

// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class SerialController : MonoBehaviour
{
    [Header("Serial Port Settings")]
    public string portName = "COM3";   // บน Windows เช่น "COM3"
                                       // บน Mac อาจเป็น "/dev/tty.usbmodemXXXX"
    public int baudRate = 9600;

    private SerialPort port;

    void Start()
    {
        try
        {
            port = new SerialPort(portName, baudRate);
            port.ReadTimeout = 20;
            port.NewLine = "\n";
            port.Open();
            Debug.Log("Serial opened on " + portName);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Cannot open serial port: " + e.Message);
        }
    }

    void Update()
    {
        // ตอนนี้เราไม่อ่านข้อมูลจาก Arduino แล้ว
        // ปล่อยว่างไว้ได้เลย
    }

    public void SendHit()
    {
        WriteLineSafe("HIT");
    }

    public void SendMiss()
    {
        WriteLineSafe("MISS");
    }

    public void SendPressure()
    {
        WriteLineSafe("PRESSURE");
    }

    private void WriteLineSafe(string msg)
    {
        if (port == null) return;
        if (!port.IsOpen) return;

        try
        {
            port.WriteLine(msg);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Serial write error: " + e.Message);
        }
    }

    void OnApplicationQuit()
    {
        if (port != null && port.IsOpen)
        {
            port.Close();
        }
    }
}
