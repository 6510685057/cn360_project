using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System.IO.Ports;   // ต้องมีบรรทัดนี้

public class SerialController : MonoBehaviour

{

    public string portName = "COM3";  // เปลี่ยนเป็น COM ของ Arduino

    public int baudRate = 9600;

    private SerialPort port;

    public ReactionManager reactionManager;

    void Start()

    {

        try

        {

            port = new SerialPort(portName, baudRate);

            port.ReadTimeout = 20;

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

        if (port == null || !port.IsOpen) return;

        try

        {

            string msg = port.ReadLine();

            if (!string.IsNullOrEmpty(msg))

            {

                msg = msg.Trim();

                if (msg == "PRESSED" && reactionManager != null)

                {

                    reactionManager.OnButtonPressed();

                }

            }

        }

        catch (System.TimeoutException)

        {

            // เงียบไป ไม่เป็นไร แค่ไม่มีข้อมูลเข้ามา

        }

        catch (System.Exception e)

        {

            Debug.LogWarning("Serial read error: " + e.Message);

        }

    }

    public void SendHit()

    {

        if (port != null && port.IsOpen)

        {

            port.WriteLine("HIT");

        }

    }

    public void SendMiss()

    {

        if (port != null && port.IsOpen)

        {

            port.WriteLine("MISS");

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
 