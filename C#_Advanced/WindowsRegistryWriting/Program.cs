using Microsoft.Win32;

namespace WiindowsRegistryWriting
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");


            string keypath = @"HKEY_CURRENT_USER\SOFTWARE\YourSoftware"; // the access allowed just for the current user
            //string keypath = @"HKEY_LOCAL_MACHINE\SOFTWARE\YourSoftware"; // the access is denied on local machine and needs permision
            string valueName = "valueName";
            string valueData = "valueData";


            try
            {
                Registry.SetValue(keypath , valueName , valueData , RegistryValueKind.String);

                Console.WriteLine("The key has been successfully added to regisetery..");
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error has occured : {ex.Message}");
            }


            try
            {
                // Read the value from the Registry
                string value = Registry.GetValue(keypath, valueName, null) as string;


                if (value != null)
                {
                    Console.WriteLine($"The value of {valueName} is: {value}");
                }
                else
                {
                    Console.WriteLine($"Value {valueName} not found in the Registry.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
