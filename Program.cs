using CTRE.Phoenix;
using CTRE.Phoenix.Controller;
using CTRE.Phoenix.MotorControl;
using CTRE.Phoenix.MotorControl.CAN;
using Microsoft.SPOT;
using System;
using System.Text;
using System.Threading;

namespace mail
{
    public class Program
    {
        // Initialize Motors
        static TalonSRX rightSlave = new TalonSRX(4);
        static TalonSRX right = new TalonSRX(3);
        static TalonSRX leftSlave = new TalonSRX(1);
        static TalonSRX left = new TalonSRX(2);

        //Drivebase Control Variables
        static double leftSpeed;
        static double rightSpeed;
        static bool disable = false;

        //Initialize the Logger and the Gamepad
        static StringBuilder stringBuilder = new StringBuilder();
        static CTRE.Phoenix.Controller.Xbox360Gamepad _gamepad = null;


        //Main Code
        public static void Main()
        {

            //This function loops forever
            while (true)
            {
                if (null == _gamepad)
                    _gamepad = new Xbox360Gamepad(UsbHostDevice.GetInstance(), 0);

                //Initialize disable buttons
                bool disableButtons = (_gamepad.GetButton(1) && _gamepad.GetButton(2) && _gamepad.GetButton(3) && _gamepad.GetButton(4));
                if (disableButtons)
                {
                    disable = true;
                }
                if (disable == false)
                {
                    //Run whatever code is in the Drive(); function
                    Drive();
                    //Print to console whatever is inside the logger
                    Debug.Print(stringBuilder.ToString());
                    stringBuilder.Clear();
                    Debug.Print("Enabled");
                    //Feed watchdog to keep Talons Enabled
                    CTRE.Phoenix.Watchdog.Feed();
                    //Run every 20ms
                    Thread.Sleep(40);

                }
                else
                {
                    //If disable == true, run the killDriveMotors() function
                    Debug.Print("Disabled");
                    killDriveMotors();
                    Thread.Sleep(200);
                }

            }
        }
        /**
         * If value is within 10% of center, clear it.
         * @param value [out] floating point value to deadband.
         */

        static void Deadband(ref float value)
        {
            if (value < -0.10)
            {
                /* outside of deadband */
            }
            else if (value > +0.10)
            {
                /* outside of deadband */
            }
            else
            {
                /* within 10% so zero it */
                value = 0;
            }
        }
        static void Drive()
        {

            //Initialize Gamepad
            if (null == _gamepad)
                _gamepad = new Xbox360Gamepad(UsbHostDevice.GetInstance(), 0);
            float x = _gamepad.GetAxis(0);
            float y = _gamepad.GetAxis(1);
            float twist = _gamepad.GetAxis(2);
            bool speedToggle = (_gamepad.GetButton(8) || _gamepad.GetButton(7));
            double speedMultiplier;
            if (speedToggle)
            {
                speedMultiplier = 1;
            }
            else
            {
                speedMultiplier = 0.5;
            }

            //Define throttle axes
            Deadband(ref x);
            Deadband(ref y);
            Deadband(ref twist);
            float leftThrot = y + twist;
            float rightThrot = y - twist;

            //set leftSpeed to whatever value is returned by requestRamp(), based on given inputs
            leftSpeed = requestRamp(leftThrot, leftSpeed);
            rightSpeed = requestRamp(rightThrot, rightSpeed);



            // Set motors to a certain velocity based on leftSpeed
            left.Set(ControlMode.PercentOutput, leftSpeed * speedMultiplier);
            leftSlave.Set(ControlMode.PercentOutput, leftSpeed * speedMultiplier);
            right.Set(ControlMode.PercentOutput, -rightSpeed * speedMultiplier);
            rightSlave.Set(ControlMode.PercentOutput, -rightSpeed * speedMultiplier);


            //Append joystick values to the logger output
            stringBuilder.Append("\t");
            stringBuilder.Append(x);
            stringBuilder.Append("\t");
            stringBuilder.Append(y);
            stringBuilder.Append("\t");
            stringBuilder.Append(twist);
        }


        //Returns a double when called based on the target speed in relation to the current speed
        static double requestRamp(double target, double current)
        {
            double rampRate = 0.08; // change per loop (tune this!)


            if (target > 0.2 || target <-0.2)
            {
                if (current < target)
                {
                    current += rampRate;
                    if (current > target)
                        current = target;
                }
                else if (current > target)
                {
                    current -= rampRate;
                    if (current < target)
                        current = target;
                }

                return current;

            }

            else return target;
        }

        
        //Sets all the drive motors to 0%
        static void killDriveMotors()
        {
            left.Set(ControlMode.PercentOutput, 0);
            leftSlave.Set(ControlMode.PercentOutput, 0);
            right.Set(ControlMode.PercentOutput, 0);
            rightSlave.Set(ControlMode.PercentOutput, 0);
            CTRE.Phoenix.Watchdog.Feed();
        }
    }
}


//fromage, Cheddar: nouns
