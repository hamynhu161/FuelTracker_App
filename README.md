# FuelTracker_App
This is a simple .NET MAUI mobile application that allows users to track fuel consumption and costs for their vehicle. The app is built with C# and XAML, and it does not use a database — instead, it stores all data in a local JSON file on the device.

**1. Features**<br>
  Add a new fuel entry (date, kilometers, liters, total cost).<br>
  View the list of all fuel entries.<br>
  Calculate and display average fuel consumption.<br>

**2. Data Storage (No Database)**<br>
  This app does not use a database such as SQLite or cloud storage. Instead, fuel data is stored in a simple JSON file located in the device's local application data folder.<br>
  File name: tankkausmuistio.json.<br>
  Location: Environment.SpecialFolder.LocalApplicationData.<br>
  Format: A list of fuel entries is serialized and deserialized using System.Text.Json.<br>
  
**3. Technologies Used**<br>
    .NET MAUI.<br>
    C#.<br>
    XAML.<br>
    System.Text.Json for serialization.<br>

**4. How It Works**<br>
  When the app starts, it checks if the JSON file exists.<br>
  If the file exists, it loads and deserializes the fuel data.<br>
  When the user adds a new entry, it's added to the list and saved by serializing the entire list back to JSON.<br>

