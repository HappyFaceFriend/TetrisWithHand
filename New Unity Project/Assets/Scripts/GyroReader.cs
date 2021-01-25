using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroReader : MonoBehaviour
{
    public SerialController serialController;
    public HandControler hand;

    bool firstTimeReading = true;
    public Vector3 offset;
    public Vector3 error;
    Vector3 lastInput;
    // Start is called before the first frame update
    void Awake()
    {
    }
    // Update is called once per frame
    void Update()
    {
        ReadMessage();
        if(Input.GetKeyDown(KeyCode.R))
        {
            firstTimeReading = true;
        }
    }
    void ReadMessage()
    {
        string message = serialController.ReadSerialMessage();
        if (message == null)
            return;

        if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_CONNECTED))
            Debug.Log("Connection established");
        else if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_DISCONNECTED))
            Debug.Log("Connection attempt failed or disconnection detected");
        else
        {
            if (message[0] == '#')
                DecryptMessage(message);
            else
                Debug.Log("System message : " + message);
        }
    }
    void DecryptMessage(string message)
    {

        string[] s = message.Substring(1).Split('/');
        Vector3 inputVector = new Vector3(-float.Parse(s[2]), float.Parse(s[0]), float.Parse(s[1]));

        if (firstTimeReading)
        {
            offset = inputVector;
            lastInput = inputVector;
            firstTimeReading = false;
            error = Vector3.zero;
        }
        if (Mathf.Abs(inputVector.y - lastInput.y) <= 0.1f)
            error.y += inputVector.y - lastInput.y;
        hand.FeedVector(inputVector - offset - error);
        lastInput = inputVector;
    }
}
