using System;
using System.Collections.Generic;
using Siemens.Engineering;
using Siemens.Engineering.CustomIdentity;
using Siemens.Engineering.Hmi;
using Siemens.Engineering.HW;
using Siemens.Engineering.HW.Features;
using Siemens.Engineering.SW;
using TiaGenerator.Tia.Extensions;
using TiaGenerator.Tia.Models;

namespace TiaGenerator.Tia.Utils
{
	public static class DeviceUtils
	{
		/// <summary>
		/// Key that will be used for assigning a custom identifier for a Device/DeviceItem
		/// </summary>
		public const string DeviceIdentifierKey = "Identifier";
		
		/// <summary>
        /// Searches for a <see cref="Device"/> or <see cref="DeviceItem"/> with the defined identifier 
        /// </summary>
        /// <param name="project">The project to search in</param>
        /// <param name="identifier">The identifier to search for</param>
        /// <returns>The found device, or device item. But always only the found item</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static (Device?, DeviceItem?)? FindDeviceOrItemByAppId(Project project, string identifier)
        {
            try
            {
                if (project == null) throw new ArgumentNullException(nameof(project));
                
                if (string.IsNullOrWhiteSpace(identifier))
                    throw new ArgumentException("Value cannot be null or whitespace.", nameof(identifier));
                
                foreach (var device in project.Devices)
                {
                    var deviceIdentifier = device.Identifier();

                    if (deviceIdentifier != null && string.Equals(identifier, deviceIdentifier))
                        return (device, null);
                    
                    foreach (var deviceItem in device.DeviceItems)
                    {
                        var deviceItemIdentifier = deviceItem.Identifier();
                        
                        if (deviceItemIdentifier is null)
                            continue;

                        if (string.Equals(identifier, deviceItemIdentifier))
                            return (null, deviceItem);
                    }
                }

                return null;
            }
            catch (Exception e)
            {
                throw new TiaException($"There was an error while searching for a device or deviceItem with identifier '{identifier}'", e);
            }
        }

        /// <summary>
        /// Find a <see cref="Device"/> via a APP ID
        /// </summary>
        /// <param name="project">The project to search for</param>
        /// <param name="identifier">The identifier to search for</param>
        /// <returns>The device that was found or null</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Device? FindDeviceByAppId(Project project, string identifier)
        {
            try
            {
                if (project == null) throw new ArgumentNullException(nameof(project));
                
                if (string.IsNullOrWhiteSpace(identifier))
                    throw new ArgumentException("Value cannot be null or whitespace.", nameof(identifier));

                foreach (var device in project.Devices)
                {
                    var identityProvider = device.GetService<CustomIdentityProvider>();

                    var deviceIdentifier = identityProvider?.Get(DeviceIdentifierKey);
                
                    if (deviceIdentifier is null)
                        continue;

                    if (string.Equals(identifier, deviceIdentifier))
                        return device;
                }

                return null;
            }
            catch (Exception e)
            {
                throw new TiaException($"There was an error while searching for a device with ID '{identifier}'", e);
            }
        }
        
        /// <summary>
        /// Find a device item via a APP ID
        /// </summary>
        /// <param name="project">The project to search for</param>
        /// <param name="identifier">The identifier to search for</param>
        /// <returns>The device item that was found or null</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static DeviceItem? FindDeviceItemByAppId(Project project, string identifier)
        {
            try
            {
                if (project == null) throw new ArgumentNullException(nameof(project));
                
                if (string.IsNullOrWhiteSpace(identifier))
                    throw new ArgumentException("Value cannot be null or whitespace.", nameof(identifier));
                
                foreach (var device in project.Devices)
                {
                    foreach (var deviceItem in device.DeviceItems)
                    {
                        var identityProvider = deviceItem.GetService<CustomIdentityProvider>();

                        var deviceItemIdentifier = identityProvider?.Get(DeviceIdentifierKey);
                        
                        if (deviceItemIdentifier is null)
                            continue;

                        if (string.Equals(identifier, deviceItemIdentifier))
                            return deviceItem;
                    }
                }

                return null;
            }
            catch (Exception e)
            {
                throw new TiaException($"There was an error while searching for a device item with ID '{identifier}'", e);
            }
        }

        /// <summary>
        /// Find the first PLC device in a project
        /// </summary>
        /// <param name="project">The project to search</param>
        /// <returns>Null, or a tuple containing the required information</returns>
        /// <exception cref="TiaException"></exception>
        public static (Device device, DeviceItem deviceItem, PlcSoftware plcSoftware)? FindFirstPlcDevice(Project project)
        {
            try
            {
                if (project == null) throw new ArgumentNullException(nameof(project));

                foreach (var device in project.Devices)
                {
                    foreach (var deviceItem in device.DeviceItems)
                    {
                        var softwareContainer = deviceItem.GetService<SoftwareContainer>();

                        if (softwareContainer?.Software is PlcSoftware plcSoftware)
                        {
                            return (device, deviceItem, plcSoftware);
                        }
                    }
                }

                return null;
            }
            catch (Exception e)
            {
                throw new TiaException("There was an error while searching for the first PLC device", e);
            }
        }

        /// <summary>
        /// Find any PLC devices in a project
        /// </summary>
        /// <param name="project">The project to search</param>
        /// <returns>Empty collection, or List of tuples containing the required information's</returns>
        /// <exception cref="TiaException"></exception>
        public static List<(Device device, DeviceItem deviceItem, PlcSoftware plcSoftware)>? FindAnyPlcDevices(Project project)
        {
            try
            {
                if (project == null) throw new ArgumentNullException(nameof(project));

                var results = new List<(Device device, DeviceItem deviceItem, PlcSoftware plcSoftware)>();

                foreach (var device in project.Devices)
                {
                    foreach (var deviceItem in device.DeviceItems)
                    {
                        // TODO: Hier gibt es eine Exception
                        var softwareContainer = deviceItem.GetService<SoftwareContainer>();

                        if (softwareContainer?.Software is PlcSoftware plcSoftware)
                        {
                            results.Add((device, deviceItem, plcSoftware));
                        }
                    }
                }

                return results;
            }
            catch (EngineeringRuntimeException)
            {
                // Ignored
            }
            catch (Exception e)
            {
                throw new TiaException("There was an error while searching for multiple PLC devices", e);
            }

            return null;
        }

        /// <summary>
        /// Find the first HMI device in a project
        /// </summary>
        /// <param name="project">The project to search</param>
        /// <returns>Null, or a tuple containing the required information</returns>
        /// <exception cref="TiaException"></exception>
        public static HmiDevice? FindFirstHmiDevice(Project project)
        {
            try
            {
                if (project == null) throw new ArgumentNullException(nameof(project));

                foreach (var device in project.Devices)
                {
                    foreach (var deviceItem in device.DeviceItems)
                    {
                        var softwareContainer = deviceItem.GetService<SoftwareContainer>();

                        if (softwareContainer?.Software is HmiTarget hmiSoftware)
                        {
                            return new HmiDevice(device, deviceItem, hmiSoftware);
                        }
                    }
                }

                return null;
            }
            catch (Exception e)
            {
                throw new TiaException("There was an error while searching for the first HMI device", e);
            }
        }

        /// <summary>
        /// Find any HMI devices in a project
        /// </summary>
        /// <param name="project">The project to search</param>
        /// <returns>Empty collection, or List of tuples containing the required information's</returns>
        /// <exception cref="TiaException"></exception>
        public static List<HmiDevice> FindAnyHmiDevices(Project project)
        {
            try
            {
                if (project == null) throw new ArgumentNullException(nameof(project));

                var results = new List<HmiDevice>();
                
                foreach (var device in project.Devices)
                {
                    foreach (var deviceItem in device.DeviceItems)
                    {
                        var softwareContainer = deviceItem.GetService<SoftwareContainer>();

                        if (softwareContainer?.Software is HmiTarget hmiSoftware)
                        {
                            results.Add(new HmiDevice(device, deviceItem, hmiSoftware));
                        }
                    }
                }

                return results;
            }
            catch (Exception e)
            {
                throw new TiaException("There was an error while searching for multiple HMI devices", e);
            } 
        }

        /// <summary>
        /// Create a new device
        /// </summary>
        /// <param name="project">The project to create the device in</param>
        /// <param name="typeIdentifier">Defines the device that is to be created</param>
        /// <param name="deviceRoot">The root name for hardware configuration view</param>
        /// <param name="hierarchyName">The name that will be displayed in the project hierarchy</param>
        /// <returns>The newly created device</returns>
        /// <exception cref="TiaException"></exception>
        public static Device CreateDevice(Project? project, string typeIdentifier, string deviceRoot, string hierarchyName)
        {
            try
            {
                if (project == null) throw new ArgumentNullException(nameof(project));

                if (string.IsNullOrWhiteSpace(typeIdentifier))
                    throw new ArgumentException("Value cannot be null or whitespace.", nameof(typeIdentifier));
                
                if (string.IsNullOrWhiteSpace(deviceRoot))
                    throw new ArgumentException("Value cannot be null or whitespace.", nameof(deviceRoot));
                
                if (hierarchyName is null)
                    throw new ArgumentException("Value cannot be null", nameof(hierarchyName));

                // TODO: Validate typeIdentifier
                
                return project.Devices.CreateWithItem(typeIdentifier, hierarchyName, deviceRoot);
            }
            catch (Exception e)
            {
                throw new TiaException("There was an error while creating a device", e);
            }
        }
        
        // TODO: Work on the following methods
        //Enumerate devices in groups or sub-groups
        public static void EnumerateDevices(Project project)
        {
            Console.WriteLine("Devices:");
            EnumerateDeviceObjects(project.Devices);
            
            foreach (DeviceUserGroup deviceUserGroup in project.DeviceGroups)
            {
                EnumerateDeviceUserGroup(deviceUserGroup);
            }
        }
        private static void EnumerateDeviceUserGroup(DeviceUserGroup deviceUserGroup)
        {
            Console.WriteLine($"{Environment.NewLine}Group: {deviceUserGroup.Name}");
            
            EnumerateDeviceObjects(deviceUserGroup.Devices);
            
            foreach (DeviceUserGroup subDeviceUserGroup in deviceUserGroup.Groups)
            {
                // recursion
                EnumerateDeviceUserGroup(subDeviceUserGroup);
            }
        }
        private static void EnumerateDeviceObjects(DeviceComposition deviceComposition)
        {
            foreach (Device device in deviceComposition)
            {
                Console.WriteLine($"{Environment.NewLine}- {device.Name}, {device.TypeIdentifier}");
                
                foreach (var deviceItem in device.DeviceItems)
                {
                    Console.WriteLine($" + {deviceItem.Name}, {deviceItem.TypeIdentifier}");
                }
            }
        }
    }
}