using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using FinchAPI;
using HidSharp.ReportDescriptors.Parser;
using MarchantFinchMission3;

namespace MarchantFinchMission3
{

    // **************************************************
    //
    // Title: Marchant - Finch Talent Show
    // Description: Starter solution with the helper methods,
    //              opening and closing screens, and the menu
    // Application Type: Console
    // Author: Velis, John, edited by Marchant, James
    // Dated Created: 1/29/2020
    // Last Modified: 11/29/2020
    //
    // **************************************************

    public enum Command
    {
        NONE,
        MOVEFORWARD,
        MOVEBACKWARD,
        STOPMOTORS,
        WAIT,
        TURNRIGHT,
        TURNLEFT,
        LEDON,
        LEDOFF,
        GETTEMPERATURE,
        DONE
    }


    class Program
    {
        /// <summary>
        /// first method run when the app starts up
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            SetTheme();

            DisplayWelcomeScreen();
            DisplayMenuScreen();
            DisplayClosingScreen();
        }

        /// <summary>
        /// setup the console theme
        /// </summary>
        static void SetTheme()
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.BackgroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// *****************************************************************
        /// *                     Main Menu                                 *
        /// *****************************************************************
        /// </summary>
        static void DisplayMenuScreen()
        {
            Console.CursorVisible = true;

            bool quitApplication = false;
            string menuChoice;

            Finch finchRobot = new Finch();

            do
            {
                DisplayScreenHeader("Main Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Connect Finch Robot");
                Console.WriteLine("\tb) Talent Show");
                Console.WriteLine("\tc) Data Recorder");
                Console.WriteLine("\td) Alarm System");
                Console.WriteLine("\te) User Programming");
                Console.WriteLine("\tf) Disconnect Finch Robot");
                Console.WriteLine("\tq) Quit");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        DisplayConnectFinchRobot(finchRobot);
                        break;

                    case "b":
                        DisplayTalentShowMenuScreen(finchRobot);
                        break;

                    case "c":
                        DisplayDataRecorderMenuScreen(finchRobot);
                        break;

                    case "d":
                        DisplayAlarmSystemMenuScreen(finchRobot);
                        break;

                    case "e":
                        DisplayUserProgrammingMenuScreen(finchRobot);
                        break;

                    case "f":
                        DisplayDisconnectFinchRobot(finchRobot);
                        break;

                    case "q":
                        DisplayDisconnectFinchRobot(finchRobot);
                        quitApplication = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitApplication);
        }


        #region USER PROGRAMMING

        static void DisplayUserProgrammingMenuScreen(Finch finchRobot)
        {

            bool quitMenu = false;
            string menuChoice;


            (int motorSpeed, int ledBrightness, double waitSeconds) commandParameters;
            commandParameters = (0, 0, 0);
            List<Command> commands = new List<Command>();
            do
            {
                DisplayScreenHeader("User Programming Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Set Parameters");
                Console.WriteLine("\tb) Add Commands");
                Console.WriteLine("\tc) View Commands");
                Console.WriteLine("\td) Execute commands");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        commandParameters = UserProgrammingDisplayGetCommandParameters();
                        break;

                    case "b":
                        UserProgrammingDisplayGetCommands(commands);
                        break;

                    case "c":
                        UserProgrammingDisplayCommands(commands);
                        break;

                    case "d":
                        UserProgrammingDisplayExecuteCommands(finchRobot, commands, commandParameters);
                        break;

                    case "q":
                        quitMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitMenu);
        }

        static (int motorSpeed, int ledBrightness, double waitSeconds) UserProgrammingDisplayGetCommandParameters()
        {
            (int motorSpeed, int ledBrightness, double waitSeconds) commandParameters;
            commandParameters = (0, 0, 0);

            bool validResponse = false;
            DisplayScreenHeader("Command Parameters");

            //
            // set motor speed
            //
            Console.Write("Enter Motor Speed [1 - 255]");
            do
            {

                if (int.TryParse(Console.ReadLine(), out commandParameters.motorSpeed))
                {
                    validResponse = true;
                }
                else
                {
                    Console.WriteLine("Please enter motor speed.");
                }
            } while (!validResponse);

            //
            // set led brightness
            //
            Console.Write("Enter LED Brightness [1 - 255]");

            bool validResponse2 = false;
            do
            {

                if (int.TryParse(Console.ReadLine(), out commandParameters.ledBrightness))
                {
                    validResponse2 = true;
                }
                else
                {
                    Console.WriteLine("Please enter LED brightness.");
                }
            } while (!validResponse2);

            //
            // set wait time
            //
            Console.Write("Enter wait time [seconds]");
            bool validResponse3 = false;
            do
            {

                if (double.TryParse(Console.ReadLine(), out commandParameters.waitSeconds))
                {
                    validResponse3 = true;
                }
                else
                {
                    Console.WriteLine("Please enter wait time.");
                }
            } while (!validResponse3);


            DisplayMenuPrompt("User Programming");

            return commandParameters;
        }

        static void UserProgrammingDisplayGetCommands(List<Command> commands)
        {
            DisplayScreenHeader("Commands");
            bool validResponse = false;
            bool isDoneAdding = false;
            string userResponse;
            Command command;

            //
            // have user enter commands
            //
            foreach (Command commandName in Enum.GetValues(typeof(Command)))
            {
                if (commandName != Command.NONE)
                {
                    Console.WriteLine($"\t{commandName}");
                }
            }
            do
            {
                Console.WriteLine("Enter Command:");
                userResponse = Console.ReadLine().ToUpper();
                validResponse = true;
                if (userResponse == "DONE")
                {
                    isDoneAdding = true;
                }
                else if (!Enum.TryParse(userResponse, out command))
                {
                    Console.WriteLine();
                    Console.WriteLine("Please enter a valid command");
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();
                    validResponse = false;
                }
                else
                {
                    commands.Add(command);
                }
            } while (!validResponse || !isDoneAdding);

            DisplayMenuPrompt("User Programming");
        }

        static void UserProgrammingDisplayCommands(List<Command> commands)
        {
            DisplayScreenHeader("Commands");

            //
            // display commands entered
            //
            foreach (Command command in commands)
            {
                Console.WriteLine("\t" + command);
            }

            DisplayMenuPrompt("User Programming");
        }

        static void UserProgrammingDisplayExecuteCommands(Finch finchRobot, List<Command> commands, (int motorSpeed, int ledBrightness, double waitSeconds) commandParameters)
        {
            string temperature;
            DisplayScreenHeader("Execute Commands");

            Console.WriteLine("\tThe Finch Robot is ready to execute the list of commands. Please ensure there is adequate space.");
            DisplayContinuePrompt();

            //
            // commence commands
            //
            foreach (Command command in commands)
            {
                switch (command)
                {
                    case Command.NONE:
                        Console.WriteLine("\t\tInvaild Command");
                        break;
                    case Command.MOVEFORWARD:
                        finchRobot.setMotors(commandParameters.motorSpeed, commandParameters.motorSpeed);
                        break;
                    case Command.MOVEBACKWARD:
                        finchRobot.setMotors(-commandParameters.motorSpeed, -commandParameters.motorSpeed);
                        break;
                    case Command.STOPMOTORS:
                        finchRobot.setMotors(0, 0);
                        break;
                    case Command.WAIT:
                        int waitMilliseconds;
                        waitMilliseconds = (int)(commandParameters.waitSeconds * 1000);
                        finchRobot.wait(waitMilliseconds);
                        break;
                    case Command.TURNRIGHT:
                        finchRobot.setMotors(commandParameters.motorSpeed, 0);
                        break;
                    case Command.TURNLEFT:
                        finchRobot.setMotors(0, commandParameters.motorSpeed);
                        break;
                    case Command.LEDON:
                        finchRobot.setLED(commandParameters.ledBrightness, commandParameters.ledBrightness, commandParameters.ledBrightness);
                        break;
                    case Command.LEDOFF:
                        finchRobot.setLED(0, 0, 0);
                        break;
                    case Command.GETTEMPERATURE:
                        temperature = finchRobot.getTemperature().ToString();
                        Console.WriteLine(temperature);
                        break;
                    case Command.DONE:
                        finchRobot.setLED(0, 0, 0);
                        finchRobot.setMotors(0, 0);
                        break;
                    default:
                        break;
                }
                Console.WriteLine("\t\t" + command);
            }
            finchRobot.setLED(0, 0, 0);
            finchRobot.setMotors(0, 0);
            DisplayMenuPrompt("User Programming");
        }

        #endregion

        #region ALARM SYSTEM
        //**********************************************************
        //                   Alarm System Menu
        //**********************************************************
        static void DisplayAlarmSystemMenuScreen(Finch finchRobot)
        {
            Console.CursorVisible = true;

            bool quitMenu = false;
            string menuChoice;

            string sensorsToMonitor = "";
            string rangeType = "";
            string rangeTypeTemperature = "";
            int minMaxThresholdValue = 0;
            int minMaxThresholdValueTemperature = 0;
            int timeToMonitor = 0;
            bool doesWantTemperature = false;




            do
            {
                DisplayScreenHeader("Alarm System Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Set Sensors to Monitor");
                Console.WriteLine("\tb) Set Range Type");
                Console.WriteLine("\tc) Set Min/Max Thresholds");
                Console.WriteLine("\td) Set Time to Monitor");
                Console.WriteLine("\te) Set Alarm");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        sensorsToMonitor = AlarmSystemDisplaySensorToMonitor();
                        doesWantTemperature = AlarmSystemDisplayTemperatureSelect();
                        break;

                    case "b":
                        rangeType = AlarmSystemDisplayRangeType();
                        if (doesWantTemperature == true)
                        {
                            rangeTypeTemperature = AlarmSystemDisplayRangeTypeTemperature();
                        }
                        break;

                    case "c":
                        minMaxThresholdValue = AlarmSystemDisplaySetMinMaxThreshold(finchRobot, rangeType);
                        if (doesWantTemperature == true)
                        {
                            minMaxThresholdValueTemperature = AlarmSystemDisplaySetMinMaxThresholdTemperature(finchRobot, rangeTypeTemperature);
                        }
                        break;

                    case "d":
                        timeToMonitor = AlarmSystemTimeToMonitor();
                        break;
                    case "e":
                        if (sensorsToMonitor == "" || rangeType == "" || minMaxThresholdValue == 0 || timeToMonitor == 0)
                        {
                            Console.WriteLine("Please enter all required values.");
                        }
                        else
                        {
                            AlarmSystemSetAlarm(finchRobot, sensorsToMonitor, rangeType, minMaxThresholdValue, timeToMonitor, rangeTypeTemperature, minMaxThresholdValueTemperature, doesWantTemperature);
                        }



                        break;
                    case "q":
                        quitMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitMenu);
        }

        static string AlarmSystemDisplayRangeTypeTemperature()
        {
            string rangeTypeTemperature;
            bool validResponse = false;
            DisplayScreenHeader("Range Type: Temperature");

            Console.Write("Enter Range Type for temperature [minimum, maximum]");

            //
            // user set range type
            //
            do
            {
                rangeTypeTemperature = Console.ReadLine().ToLower();
                if (rangeTypeTemperature == "minimum" || rangeTypeTemperature == "maximum")
                {
                    validResponse = true;
                }
                else
                {
                    Console.WriteLine("Please enter the display range you are using [minimum, maximum].");
                }
            } while (!validResponse);

            Console.WriteLine($"Range type {rangeTypeTemperature}.");
            Console.WriteLine();
            DisplayContinuePrompt();

            return rangeTypeTemperature;
        }

        static int AlarmSystemDisplaySetMinMaxThresholdTemperature(Finch finchRobot, string rangeTypeTemperature)
        {
            int minMaxThresholdValueThresholdTemperature = 0;
            bool validResponse = false;

            DisplayScreenHeader("Min/Max Threshold Value");

            Console.WriteLine($"Ambient temperature {finchRobot.getTemperature()}");


            Console.Write($"Enter the {rangeTypeTemperature} the threshold value:");

            //
            // user sets min or max threshold
            //
            do
            {
                int.TryParse(Console.ReadLine(), out minMaxThresholdValueThresholdTemperature);
                if (minMaxThresholdValueThresholdTemperature > 0)
                {
                    validResponse = true;
                }
                else
                {
                    Console.WriteLine("Please enter a minimum/maximum value greater than 0.");
                }
            } while (!validResponse);

            Console.WriteLine($"Minimum/Maximum value {minMaxThresholdValueThresholdTemperature}.");

            DisplayContinuePrompt();

            return minMaxThresholdValueThresholdTemperature;
        }

        static bool AlarmSystemDisplayTemperatureSelect()
        {
            bool doesWantTemperature = false;
            bool validResponse = false;
            string temperatureResponse;
            DisplayScreenHeader("Determine if measuring Temperature");

            Console.WriteLine("Do you wish to monitor temperature? [Yes/No]");
            
            
            do
            {
                temperatureResponse = Console.ReadLine().ToLower();
                if (temperatureResponse == "yes")
                {
                    doesWantTemperature = true;
                    validResponse = true;
                }
                else if(temperatureResponse == "no")
                    {
                    doesWantTemperature = false;
                    validResponse = true;
                }
                else
                {
                    Console.WriteLine("Please enter yes or no.");
                }
            } while (!validResponse);

            DisplayContinuePrompt();
            return doesWantTemperature;
        }

        static string AlarmSystemDisplaySensorToMonitor()
        {
            string sensorsToMonitor;
            bool validResponse = false;
            DisplayScreenHeader("Sensors to Monitor");

            Console.WriteLine("Enter Sensors to Monitor [left, right, both]:");
            //
            // have user enter which sensors are being used
            //
            do
            {
                sensorsToMonitor = Console.ReadLine().ToLower();
                if (sensorsToMonitor == "left" || sensorsToMonitor == "right" || sensorsToMonitor == "both")
                {
                    validResponse = true;
                }
                else
                {
                    Console.WriteLine("Please enter which sensors you are using [left, right, both].");
                }
            } while (!validResponse);

            Console.WriteLine($"Selected sensor {sensorsToMonitor}.");

            DisplayContinuePrompt();
            return sensorsToMonitor;
        }

        static string AlarmSystemDisplayRangeType()
        {
            string rangeType;
            bool validResponse = false;
            DisplayScreenHeader("Range Type");

            Console.Write("Enter Range Type [minimum, maximum]");

            //
            // user set range type
            //
            do
            {
                rangeType = Console.ReadLine().ToLower();
                if (rangeType == "minimum" || rangeType == "maximum")
                {
                    validResponse = true;
                }
                else
                {
                    Console.WriteLine("Please enter the display range you are using [minimum, maximum].");
                }
            } while (!validResponse);

            Console.WriteLine($"Range type {rangeType}.");
            Console.WriteLine();
            DisplayContinuePrompt();

            return rangeType;
        }

        static int AlarmSystemDisplaySetMinMaxThreshold(Finch finchRobot, string rangeType)
        {
            int minMaxThresholdValue = 0;
            bool validResponse = false;

            DisplayScreenHeader("Min/Max Threshold Value");

            Console.WriteLine($"Ambient Left Light Level: {finchRobot.getLeftLightSensor()}");
            Console.WriteLine($"Ambient Right Light Level: {finchRobot.getRightLightSensor()}");

            Console.Write($"Enter the {rangeType} the threshold value:");

            //
            // user sets min or max threshold
            //
            do
            {
                int.TryParse(Console.ReadLine(), out minMaxThresholdValue);
                if (minMaxThresholdValue > 0)
                {
                    validResponse = true;
                }
                else
                {
                    Console.WriteLine("Please enter a minimum/maximum value greater than 0.");
                }
            } while (!validResponse);

            Console.WriteLine($"Minimum/Maximum value {minMaxThresholdValue}.");

            DisplayContinuePrompt();

            return minMaxThresholdValue;
        }

        static int AlarmSystemTimeToMonitor()
        {
            int timeToMonitor = 0;
            bool validResponse = false;
            DisplayScreenHeader("Time to Monitor");

            Console.Write("Enter Time to Monitor:");

            //
            // user sets amount of time to monitor
            //
            do
            {
                int.TryParse(Console.ReadLine(), out timeToMonitor);
                if (timeToMonitor > 0)
                {
                    validResponse = true;
                }
                else
                {
                    Console.WriteLine("Please enter a time greater than 0.");
                }
            } while (!validResponse);

            Console.WriteLine();
            Console.WriteLine($"Time to Monitor: {timeToMonitor}");



            DisplayContinuePrompt();

            return timeToMonitor;
        }

        static void AlarmSystemSetAlarm(Finch finchRobot, string sensorsToMonitor, string rangeType,  int minMaxThresholdValue, int timeToMonitor, string rangeTypeTemperature, int minMaxThresholdValueTemperature, bool doesWantTemperature)
        {
            DisplayScreenHeader("Set Alarm");

            Console.WriteLine("\tAlarm Settings");
            Console.WriteLine($"\t\tSensor to Monitor: {sensorsToMonitor}");
            Console.WriteLine($"\t\tRange Type: {rangeType}");
            Console.WriteLine($"\t\tMin/Max Threshold: {minMaxThresholdValue}");
            Console.WriteLine($"\t\tTime to Monitor: {timeToMonitor}");
            if (doesWantTemperature == true)
            {
                Console.WriteLine($"\t\tRange Type: {rangeTypeTemperature}");
                Console.WriteLine($"\t\tMin/Max Threshold: {minMaxThresholdValueTemperature}");
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to start the alarm system");
            Console.CursorVisible = false;
            Console.ReadKey();
            Console.CursorVisible = true;

            int second = 1;
            while ((!AlarmSystemThresholdExceeded(finchRobot, sensorsToMonitor, rangeType, minMaxThresholdValue, rangeTypeTemperature, minMaxThresholdValueTemperature, doesWantTemperature) && second <= timeToMonitor))
            {
                Console.SetCursorPosition(10, 12);
                Console.WriteLine(($"\t\tTime: {second++}"));
                finchRobot.wait(1000);
            }

            //
            // display status
            //
            if (second > timeToMonitor)
            {
                Console.WriteLine();
                Console.WriteLine("\tThreshold Not Exceeded");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("\tThreshold Exceeded");
            }

            DisplayContinuePrompt();
        }

        static bool AlarmSystemThresholdExceeded(Finch finchRobot, string sensorsToMonitor, string rangeType, int minMaxThresholdValue, string rangeTypeTemperature, int minMaxThresholdValueTemperature, bool doesWantTemperature)
        {
            //
            //get current light value
            //
            int currentLeftLightSensorValue = finchRobot.getLeftLightSensor();
            int currentRightLightSensorValue = finchRobot.getRightLightSensor();
            double currentTemperature = finchRobot.getTemperature();
            bool thresholdExceeded = false;
            thresholdExceeded = false;
            if (doesWantTemperature == true)
            {
                switch (sensorsToMonitor)
                {
                    case "left":
                        if (rangeType == "minimum")
                        {
                            if (currentLeftLightSensorValue < minMaxThresholdValue)
                            {
                                thresholdExceeded = true;
                            }
                            else if (rangeTypeTemperature == "minimum")
                            {
                                thresholdExceeded = currentTemperature < minMaxThresholdValueTemperature;
                            }
                            else
                            {
                                thresholdExceeded = currentTemperature > minMaxThresholdValueTemperature;
                            }
                        }
                        else
                        {
                            if (currentLeftLightSensorValue > minMaxThresholdValue)
                            {
                                thresholdExceeded = true;
                            }
                            else if (rangeTypeTemperature == "minimum")
                            {
                                thresholdExceeded = currentTemperature < minMaxThresholdValueTemperature;
                            }
                            else
                            {
                                thresholdExceeded = currentTemperature > minMaxThresholdValueTemperature;
                            }
                        }
                        break;

                    case "right":
                        if (rangeType == "minimum")
                        {
                            thresholdExceeded = currentRightLightSensorValue < minMaxThresholdValue;
                            if (rangeTypeTemperature == "minimum")
                            {
                                thresholdExceeded = currentTemperature < minMaxThresholdValueTemperature;
                            }
                            else
                            {
                                thresholdExceeded = currentTemperature > minMaxThresholdValueTemperature;
                            }
                        }
                        else
                        {
                            thresholdExceeded = currentRightLightSensorValue > minMaxThresholdValue;
                            if (rangeTypeTemperature == "minimum")
                            {
                                thresholdExceeded = currentTemperature < minMaxThresholdValueTemperature;
                            }
                            else
                            {
                                thresholdExceeded = currentTemperature > minMaxThresholdValueTemperature;
                            }
                        }
                        break;

                    case "both":
                        if (rangeType == "minimum")
                        {
                            thresholdExceeded = (currentRightLightSensorValue < minMaxThresholdValue) || (currentLeftLightSensorValue < minMaxThresholdValue);
                            if (rangeTypeTemperature == "minimum")
                            {
                                thresholdExceeded = currentTemperature < minMaxThresholdValueTemperature;
                            }
                            else
                            {
                                thresholdExceeded = currentTemperature > minMaxThresholdValueTemperature;
                            }
                        }
                        else
                        {
                            thresholdExceeded = (currentRightLightSensorValue > minMaxThresholdValue) || (currentLeftLightSensorValue > minMaxThresholdValue);
                            if (rangeTypeTemperature == "minimum")
                            {
                                thresholdExceeded = currentTemperature < minMaxThresholdValueTemperature;
                            }
                            else
                            {
                                thresholdExceeded = currentTemperature > minMaxThresholdValueTemperature;
                            }
                        }
                        break;
                    default:
                        Console.WriteLine("Sensor not set");
                        break;
                }
            }
            else
            {
                switch (sensorsToMonitor)
                {
                    case "left":
                        if (rangeType == "minimum")
                        {
                            thresholdExceeded = currentLeftLightSensorValue < minMaxThresholdValue;
                        }
                        else
                        {
                            thresholdExceeded = currentLeftLightSensorValue > minMaxThresholdValue;
                        }
                        break;

                    case "right":
                        if (rangeType == "minimum")
                        {
                            thresholdExceeded = currentRightLightSensorValue < minMaxThresholdValue;
                        }
                        else
                        {
                            thresholdExceeded = currentRightLightSensorValue > minMaxThresholdValue;
                        }
                        break;

                    case "both":
                        if (rangeType == "minimum")
                        {
                            thresholdExceeded = (currentRightLightSensorValue < minMaxThresholdValue) || (currentLeftLightSensorValue < minMaxThresholdValue);
                        }
                        else
                        {
                            thresholdExceeded = (currentRightLightSensorValue > minMaxThresholdValue) || (currentLeftLightSensorValue > minMaxThresholdValue);
                        }
                        break;
                    default:
                        Console.WriteLine("Sensor not set");
                        break;
                }
            }
            

            return thresholdExceeded;
        }
        #endregion

        #region DATA RECORDER

        static void DisplayDataRecorderMenuScreen(Finch finchRobot)
        {
            Console.CursorVisible = true;

            bool quitMenu = false;
            string menuChoice;
            int numberOfDataPoints = 0;
            double frequencyOfDataPoints = 0;
            double[] temperatures = new double[0];
            double[] temperaturesFahrenheit = new double[0];

            do
            {
                DisplayScreenHeader("Data Recorder Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Get number of data points");
                Console.WriteLine("\tb) Get frequency of data points");
                Console.WriteLine("\tc) Get Data Set");
                Console.WriteLine("\td) Get Data Set in Fahrenheit");
                Console.WriteLine("\te) Display Data Set");
                Console.WriteLine("\tf) Display Data Set in Fahrenheit");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        numberOfDataPoints = DataRecorderDisplayGetNumberOfDataPoints();
                        break;

                    case "b":
                        frequencyOfDataPoints = DataRecorderDisplayFrequencyOfDataPoints();
                        break;

                    case "c":
                        if (numberOfDataPoints == 0 || frequencyOfDataPoints == 0)
                        {
                            Console.WriteLine();
                            Console.WriteLine("\t\tPlease indicate the number of data points and frequency.");
                            Console.WriteLine();
                            DisplayContinuePrompt();
                        }
                        else

                        {
                            temperatures = DataRecorderDisplayGetData(numberOfDataPoints, frequencyOfDataPoints, finchRobot);
                        }
                        break;

                    case "d":
                        if (numberOfDataPoints == 0 || frequencyOfDataPoints == 0)
                        {
                            Console.WriteLine();
                            Console.WriteLine("\t\tPlease indicate the number of data points and frequency.");
                            Console.WriteLine();
                            DisplayContinuePrompt();
                        }
                        else

                        {
                            temperaturesFahrenheit = DataRecorderDisplayGetDataFahrenheit(numberOfDataPoints, frequencyOfDataPoints, finchRobot);
                        }
                        break;

                    case "e":
                        DataRecorderDisplayData(temperatures);
                        break;

                    case "f":
                        DataRecorderDisplayDataFahrenheit(temperaturesFahrenheit);
                        break;

                    case "q":
                        quitMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitMenu);
        }

        static double[] DataRecorderDisplayGetDataFahrenheit(int numberOfDataPoints, double frequencyOfDataPoints, Finch finchRobot)
        {
            double[] temperaturesFahrenheit = new double[numberOfDataPoints];

            DisplayScreenHeader("Get Data Points");
            Console.WriteLine($"Number of Data Points: {numberOfDataPoints}.");
            Console.WriteLine($"Frequency of Data Points: {frequencyOfDataPoints}.");
            Console.WriteLine();

            Console.WriteLine("\t The Finch robot is ready to record temperatures. Press any key to begin.");
            Console.ReadKey();
            Console.WriteLine();

            double temperature;
            int milliseconds;
            //
            // get temperatures
            //
            for (int index = 0; index < numberOfDataPoints; index++)
            {
                temperature = ((finchRobot.getTemperature() * 1.8) + 32);
                Console.WriteLine($"\t\tTemperature Reading {index + 1}: {temperature}");
                temperaturesFahrenheit[index] = temperature;
                milliseconds = ((int)(frequencyOfDataPoints) * 1000);
                finchRobot.wait(milliseconds);
            }
            Console.WriteLine();
            Console.WriteLine("\t The data recording is complete.");

            DisplayContinuePrompt();
            return temperaturesFahrenheit;
        }

        static void DataRecorderDisplayDataFahrenheit(double[] temperaturesFahrenheit)
        {
            DisplayScreenHeader("Data Set");

            DataRecorderDisplayTableFahrenheit(temperaturesFahrenheit);

            DisplayContinuePrompt();
        }

        static void DataRecorderDisplayTableFahrenheit(double[] temperaturesFahrenheit)
        {
            Console.WriteLine("Recording".PadLeft(15) +
                 "Temperature in Fahrenheit".PadLeft(15)
                 );
            //
            // display temperatues
            //
            for (int index = 0; index < temperaturesFahrenheit.Length; index++)
            {
                Console.WriteLine(
                    (index + 1).ToString().PadLeft(15) +
                temperaturesFahrenheit[index].ToString("n2").PadLeft(15)
                );
            }
        }

        static void DataRecorderDisplayTable(double[] temperatures)
        {
            Console.WriteLine("Recording".PadLeft(15) +
                "Temperature".PadLeft(15)
                );
            //
            // display temperatues
            //
            for (int index = 0; index < temperatures.Length; index++)
            {
                Console.WriteLine(
                    (index + 1).ToString().PadLeft(15) +
                temperatures[index].ToString("n2").PadLeft(15)
                );
            }
        }
        static void DataRecorderDisplayData(double[] temperatures)
        {
            DisplayScreenHeader("Data Set");

            DataRecorderDisplayTable(temperatures);

            DisplayContinuePrompt();
        }

        //**********************************************************
        //                    Number of Data Points
        //**********************************************************
        static int DataRecorderDisplayGetNumberOfDataPoints()
        {
            int numberOfDataPoints;

            DisplayScreenHeader("Number of Data Points");
            Console.WriteLine("Enter the number of Data Points:");
            bool validResponse;
            validResponse = false;

            //
            // have user enter number of data points
            //
            do
            {
                Console.WriteLine();
                Console.Write("\tEnter number of data points:");
                if (int.TryParse(Console.ReadLine(), out numberOfDataPoints) && numberOfDataPoints > 0)
                {
                    validResponse = true;
                }
                else
                {
                    Console.WriteLine("\tPlease enter a valid number of data points.");
                    Console.WriteLine();
                }
            } while (!validResponse);

            Console.WriteLine();
            Console.WriteLine($"Number of Data Points: {numberOfDataPoints}");

            return numberOfDataPoints;
        }
        //*******************************************************
        //             Frequency of Data Points
        //*******************************************************

        static double DataRecorderDisplayFrequencyOfDataPoints()
        {
            double frequencyOfDataPoints;

            DisplayScreenHeader("Frequency of Data Points");
            //validate input
            Console.WriteLine("Enter the frequency of Data Points:");
            bool validResponse;
            validResponse = false;

            //
            // have user enter frequency of data points
            //
            do
            {
                Console.WriteLine();
                Console.Write("\tEnter frequency of data points:");
                if (double.TryParse(Console.ReadLine(), out frequencyOfDataPoints) && frequencyOfDataPoints > 0)
                {
                    validResponse = true;
                }
                else
                {
                    Console.WriteLine("\tPlease enter a valid frequency of data points.");
                    Console.WriteLine();
                }
            } while (!validResponse);

            Console.WriteLine();
            Console.WriteLine($"Number of Data Points: {frequencyOfDataPoints}");

            return frequencyOfDataPoints;
        }

        //**********************************************************
        //                   Display Data
        //**********************************************************
        static double[] DataRecorderDisplayGetData(int numberOfDataPoints, double frequencyOfDataPoints, Finch finchRobot)
        {
            double[] temperatures = new double[numberOfDataPoints];

            DisplayScreenHeader("Get Data Points");
            Console.WriteLine($"Number of Data Points: {numberOfDataPoints}.");
            Console.WriteLine($"Frequency of Data Points: {frequencyOfDataPoints}.");
            Console.WriteLine();

            Console.WriteLine("\t The Finch robot is ready to record temperatures. Press any key to begin.");
            Console.ReadKey();
            Console.WriteLine();

            double temperature;
            int milliseconds;
            //
            // get temperatures
            //
            for (int index = 0; index < numberOfDataPoints; index++)
            {
                temperature = finchRobot.getTemperature();
                Console.WriteLine($"\t\tTemperature Reading {index + 1}: {temperature}");
                temperatures[index] = temperature;
                milliseconds = ((int)(frequencyOfDataPoints) * 1000);
                finchRobot.wait(milliseconds);
            }
            Console.WriteLine();
            Console.WriteLine("\t The data recording is complete.");

            DisplayContinuePrompt();
            return temperatures;
        }
        #endregion

        #region TALENT SHOW

        /// <summary>
        /// *****************************************************************
        /// *                     Talent Show Menu                          *
        /// *****************************************************************
        /// </summary>
        static void DisplayTalentShowMenuScreen(Finch finchRobot)
        {
            Console.CursorVisible = true;

            bool quitTalentShowMenu = false;
            string menuChoice;

            do
            {
                DisplayScreenHeader("Talent Show Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Light");
                Console.WriteLine("\tb) Sound");
                Console.WriteLine("\tc) Dance");
                Console.WriteLine("\td) Grand Finale");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        DisplayLight(finchRobot);
                        break;

                    case "b":
                        DisplaySound(finchRobot);
                        break;

                    case "c":
                        DisplayDance(finchRobot);
                        break;

                    case "d":
                        DisplayGrandFinale(finchRobot);
                        break;

                    case "q":
                        quitTalentShowMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitTalentShowMenu);
        }

        /// <summary>
        /// *****************************************************************
        /// *                       Talent Show > Light                     *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void DisplayLight(Finch finchRobot)
        {
            int red = 0;
            int blue = 0;
            int green = 0;
            Console.CursorVisible = false;

            DisplayScreenHeader("Light");

            Console.WriteLine("\tThe Finch robot will now show off its glowing talent!");
            DisplayContinuePrompt();
            //
            // activate led
            //
            do
            {
                finchRobot.setLED(red, 0, 0);
                finchRobot.wait(50);
                red = red += 10;
            } while (red <= 255);
            do
            {
                finchRobot.setLED(red, blue, 0);
                finchRobot.wait(50);
                red = red -= 10;
                blue = blue += 10;
            } while (blue <= 255);
            do
            {
                finchRobot.setLED(0, blue, green);
                finchRobot.wait(50);
                green = green += 10;
                blue = blue -= 10;
            } while (green <= 255);
            //
            // reset led
            //
            finchRobot.setLED(0, 0, 0);


            DisplayMenuPrompt("Talent Show Menu");
        }

        /// <summary>
        /// *****************************************************************
        /// *                       Talent Show > Sound                     *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void DisplaySound(Finch finchRobot)
        {
            int pitch = 0;
            Console.CursorVisible = false;

            DisplayScreenHeader("Sound");

            Console.WriteLine("\tThe Finch robot will now show off its singing talent!");
            DisplayContinuePrompt();

            //
            // activate buzzer
            //
            do
            {
                finchRobot.noteOn(pitch);
                finchRobot.wait(100);
                finchRobot.noteOff();
                pitch = pitch += 100;
            } while (pitch <= 1500);

            //
            // reset buzzer
            //
            finchRobot.noteOff();
            DisplayMenuPrompt("Talent Show Menu");
        }

        /// <summary>
        /// *****************************************************************
        /// *                       Talent Show > Dance                     *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void DisplayDance(Finch finchRobot)
        {
            int rep = 0;
            Console.CursorVisible = false;

            DisplayScreenHeader("Dance");

            Console.WriteLine("\tThe Finch robot will now show off its dancing talent!");
            Console.WriteLine();
            Console.WriteLine("Do be warned, the finch robot shall move, please move the finch to an appropriate spot");
            DisplayContinuePrompt();
            //
            //activate motors
            //
            do
            {
                finchRobot.setMotors(50, -50);
                finchRobot.wait(500);
                finchRobot.setMotors(-50, 50);
                finchRobot.wait(500);
                rep = rep + 1;
            } while (rep <= 10);

            //
            // reset motors
            //
            finchRobot.setMotors(0, 0);

            DisplayMenuPrompt("Talent Show Menu");
        }

        /// <summary>
        /// *****************************************************************
        /// *                 Talent Show > Grand Finale                    *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void DisplayGrandFinale(Finch finchRobot)
        {
            int red = 0;
            int blue = 0;
            int green = 0;
            int pitch = 0;
            int rep = 0;
            Console.CursorVisible = false;

            DisplayScreenHeader("Grand Finale");

            Console.WriteLine("\tAnd now for the grand finale!");
            Console.WriteLine();
            Console.WriteLine("Do be warned, the finch robot shall move, please move the finch to an appropriate spot");
            DisplayContinuePrompt();
            //
            // activate motors, buzzer, and led
            //
            do
            {
                finchRobot.setMotors(50, -50);
                finchRobot.setLED(red, blue, green);
                finchRobot.noteOn(pitch);
                finchRobot.wait(500);
                finchRobot.setMotors(-50, 50);
                finchRobot.wait(500);
                red = red += 25;
                blue = blue += 15;
                green = green += 5;
                pitch = pitch += 100;
                rep = rep + 1;
            } while (rep <= 10);

            //
            // reset motors, buzzer, and led
            //
            finchRobot.setMotors(0, 0);
            finchRobot.noteOff();
            finchRobot.setLED(0, 0, 0);

            DisplayMenuPrompt("Talent Show Menu");
        }
        #endregion

        #region FINCH ROBOT MANAGEMENT

        /// <summary>
        /// *****************************************************************
        /// *               Disconnect the Finch Robot                      *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void DisplayDisconnectFinchRobot(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Disconnect Finch Robot");

            Console.WriteLine("\tAbout to disconnect from the Finch robot.");
            DisplayContinuePrompt();

            finchRobot.disConnect();

            Console.WriteLine("\tThe Finch robot is now disconnect.");

            DisplayMenuPrompt("Main Menu");
        }

        /// <summary>
        /// *****************************************************************
        /// *                  Connect the Finch Robot                      *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        /// <returns>notify if the robot is connected</returns>
        static bool DisplayConnectFinchRobot(Finch finchRobot)
        {
            Console.CursorVisible = false;

            bool robotConnected;

            DisplayScreenHeader("Connect Finch Robot");

            Console.WriteLine("\tAbout to connect to Finch robot. Please be sure the USB cable is connected to the robot and computer now.");
            DisplayContinuePrompt();

            robotConnected = finchRobot.connect();

            //  test connection and provide user feedback - text, lights, sounds
            if (robotConnected)
            {
                Console.WriteLine();
                Console.WriteLine("\t The finch will light up and make a sound to confirm connection.");
                finchRobot.setLED(50, 100, 50);
                finchRobot.noteOn(500);
                finchRobot.wait(1500);
                finchRobot.noteOff();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("\tFinch did not connect.");
            }

            DisplayMenuPrompt("Main Menu");

            //
            // reset finch robot
            //
            finchRobot.setLED(0, 0, 0);
            finchRobot.noteOff();

            return robotConnected;
        }

        #endregion

        #region USER INTERFACE

        // <summary>
        // *****************************************************************
        // *                     Welcome Screen                            *
        // *****************************************************************
        // </summary>
        static void DisplayWelcomeScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tFinch Control");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        // <summary>
        //*****************************************************************
        // *                     Closing Screen                            *
        // *****************************************************************
        // </summary>
        static void DisplayClosingScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tThank you for using Finch Control!");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        // <summary>
        // display continue prompt
        // </summary>
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("\tPress any key to continue.");
            Console.ReadKey();
        }

        // <summary>
        // display menu prompt
        // </summary>
        static void DisplayMenuPrompt(string menuName)
        {
            Console.WriteLine();
            Console.WriteLine($"\tPress any key to return to the {menuName} Menu.");
            Console.ReadKey();
        }

        // <summary>
        // display screen header
        // </summary>
        static void DisplayScreenHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + headerText);
            Console.WriteLine();
        }

        #endregion
    }
}
