
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class SerialController : MonoBehaviour
{
    [Header("Serial Port Settings")]
    public string portName = "/dev/cu.usbmodem11301";   // บน Windows เช่น "COM3"
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
