using System;
using UnityEngine;

namespace ColaFramework.Foundation
{
// 设备档次
// 按照TDR性能基线进行划分
// Android:
// 1档: RAM-4G,CPU-4核/8核,1.5GHZ以上
// 2档: RAM-3G,CPU-4核/8核,1.5GHZ以上~RAM-4G,CPU-4核/8核,1.5GH以下
// 3档: RAM-2G,CPU-8核,1.5GHZ以上~RAM-3G,CPU-4核/8核,1.5GHZ以下
// iOS:
// 1档: RAM-2G/3G,CPU-苹果A10+M10协处理器以上
// 2档: RAM-2G,CPU-苹果A9+M9协处理器以上
// 3档: RAM-1G,CPU-苹果A7+M7协处理器和苹果A8+M8协处理器
// 
// TODO:旧版数据，有待更新
//
    public enum DeviceLevel
    {
        UNKNOWN = 0, // 未知
        LEVEL_1 = 1, // 1档
        LEVEL_2 = 2, // 2档
        LEVEL_3 = 3, // 3档
        LEVEL_L = 4,
    }

    public static class MobileHelper
    {
        static DeviceLevel device_level = DeviceLevel.UNKNOWN;

        public static DeviceLevel GetDeviceLevel()
        {
            return device_level;
        }

        static MobileHelper()
        {
            InitDeviceLevelByGPU();
        }

        // 根据GPU平台判定机型档次
        static void InitDeviceLevelByGPU()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                // 高通骁龙
                if (SystemInfo.graphicsDeviceVendor == "Qualcomm")
                {
                    if (SystemInfo.graphicsDeviceName.StartsWith("Adreno"))
                    {
                        // 高通骁龙
                        if (SystemInfo.graphicsDeviceName.CompareTo("Adreno (TM) 530") >= 0)
                        {
                            // 基线骁龙820
                            device_level = DeviceLevel.LEVEL_1;
                        }
                        else if (SystemInfo.graphicsDeviceName.CompareTo("Adreno (TM) 506") >= 0)
                        {
                            // 基线骁龙625
                            device_level = DeviceLevel.LEVEL_2;
                        }
                        else
                        {
                            device_level = DeviceLevel.LEVEL_3;
                        }
                    }
                }

                // ARM Mali
                if (SystemInfo.graphicsDeviceVendor == "ARM")
                {
                    if (SystemInfo.graphicsDeviceName.StartsWith("Mali-G"))
                    {
                        // G系列
                        if (SystemInfo.graphicsDeviceName.CompareTo("Mali-G71") >= 0)
                        {
                            // 基线海思960
                            device_level = DeviceLevel.LEVEL_1;
                        }
                        else
                        {
                            device_level = DeviceLevel.LEVEL_2;
                        }
                    }
                    else if (SystemInfo.graphicsDeviceName.StartsWith("Mali-T"))
                    {
                        // T系列
                        if (SystemInfo.graphicsDeviceName.CompareTo("Mali-T830") >= 0)
                        {
                            // 基线海思650
                            device_level = DeviceLevel.LEVEL_2;
                        }
                        else
                        {
                            device_level = DeviceLevel.LEVEL_3;
                        }
                    }
                    else
                    {
                        device_level = DeviceLevel.LEVEL_3;
                    }
                }

                // IMG PowerVR
                if (SystemInfo.graphicsDeviceVendor == "IMG")
                {
                    if (SystemInfo.graphicsDeviceName.StartsWith("PowerVR"))
                    {
                        if (SystemInfo.graphicsDeviceName == "PowerVR 7XTP")
                        {
                            //基线联发科x30
                            device_level = DeviceLevel.LEVEL_1;
                        }

                        if (SystemInfo.graphicsDeviceName == "PowerVR G6200" ||
                            SystemInfo.graphicsDeviceName == "PowerVR GE8320")
                        {
                            device_level = DeviceLevel.LEVEL_2;
                        }
                    }
                }
            }

            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                // 苹果设备根据处理器型号进行机型划分, A10以上为1档，A9以上为2档，A8以上为3档（为覆盖更多以A7敢归类为3档）
                if (SystemInfo.deviceModel.StartsWith("iPhone"))
                {
                    if (SystemInfo.deviceModel.CompareTo("iPhone9") >= 0)
                    {
                        // 基线iPhone7以上
                        device_level = DeviceLevel.LEVEL_1;
                    }
                    else if (SystemInfo.deviceModel.CompareTo("iPhone8") >= 0)
                    {
                        //基线iPhone6s/Plus以上
                        device_level = DeviceLevel.LEVEL_2;
                    }
                    else if (SystemInfo.deviceModel.CompareTo("iPhone6") >= 0)
                    {
                        // 基线iPhone5s以上
                        device_level = DeviceLevel.LEVEL_3;
                    }
                }

                if (SystemInfo.deviceModel.StartsWith("iPad"))
                {
                    if (SystemInfo.deviceModel.CompareTo("iPad6,7") >= 0)
                    {
                        // iPad Pro 12.9
                        device_level = DeviceLevel.LEVEL_1;
                    }
                    else if (SystemInfo.deviceModel.CompareTo("iPad6") >= 0)
                    {
                        // iPad Pro 9.7
                        device_level = DeviceLevel.LEVEL_2;
                    }
                    else if (SystemInfo.deviceModel.CompareTo("iPad4") >= 0)
                    {
                        device_level = DeviceLevel.LEVEL_3;
                    }
                }
            }
        }

        // 根据内存和CPU判断机型档次
        static void InitDeviceLevelByCPURAM()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                if (Math.Round(SystemInfo.systemMemorySize / 1000f) >= 4 && SystemInfo.processorCount >= 4 &&
                    SystemInfo.processorFrequency >= 1500)
                {
                    device_level = DeviceLevel.LEVEL_1;
                }
                else if ((Math.Round(SystemInfo.systemMemorySize / 1000f) >= 3 && SystemInfo.processorCount >= 4 &&
                          SystemInfo.processorFrequency >= 1500) &&
                         (Math.Round(SystemInfo.systemMemorySize / 1000f) <= 4 && SystemInfo.processorCount <= 8 &&
                          SystemInfo.processorFrequency >= 1500))
                {
                    device_level = DeviceLevel.LEVEL_2;
                }
                else if ((Math.Round(SystemInfo.systemMemorySize / 1000f) >= 2 && SystemInfo.processorCount >= 4 &&
                          SystemInfo.processorFrequency >= 1500) &&
                         (Math.Round(SystemInfo.systemMemorySize / 1000f) <= 3 && SystemInfo.processorCount <= 8 &&
                          SystemInfo.processorFrequency >= 1500))
                {
                    device_level = DeviceLevel.LEVEL_3;
                }
                else
                {
                    device_level = DeviceLevel.LEVEL_L;
                }
            }

            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
            }
        }

        /* --------------------- 参考 --------------------
        // 联发科手机GPU信息表
        string[] mtk = { 
            // X系列
            "IMG PowerVR 7XTP-MT4",     // x30
            "ARM Mali-T880 MP4",        // x27 (MT6797X)
            "ARM Mali-T880 MP4",        // x25 (MT6797T)
            "ARM Mali-T880 MP4",        // x23 (MT6797D)
            "ARM Mali-T880 MP4",        // x20 (MT6797)
            "IMG PowerVR G6200",        // x10 (MT6795)
    
            // P系列
            "ARM Mali-G72 MP3",         // P60
            "ARM Mali-G71 MP2",         // P30
            "ARM Mali-T880 MP2",        // P25
            "ARM Mali-G71 MP2",         // P23
            "IMG PowerVR GE8320",       // P22 (MT6762)
            "ARM Mali-T880 MP2",        // P20
            "ARM Mali T860 MP2",        // P18 (MT6755S)
            "ARM Mali T860 MP2",        // P10 (MT6755)
    
            // A系列
            "IMG PowerVR",              // A22
            
            // 主流4G芯片
            "ARM Mali-T720",            // MT6753
            "ARM Mali-T760",            // MT6752
            "ARM Mali T860 MP2",        // MT6750
    
            // 入门4G芯片
            "IMG PowerVR GE8100",       // MT6739
            "ARM Mali T860 MP2",        // MT6738
        }
    
        // 苹果设备型号表
        if ([deviceString isEqualToString:@"iPhone3,1"])    return @"iPhone 4";
        if ([deviceString isEqualToString:@"iPhone3,2"])    return @"iPhone 4";
        if ([deviceString isEqualToString:@"iPhone3,3"])    return @"iPhone 4";
        if ([deviceString isEqualToString:@"iPhone4,1"])    return @"iPhone 4S";
        if ([deviceString isEqualToString:@"iPhone5,1"])    return @"iPhone 5";
        if ([deviceString isEqualToString:@"iPhone5,2"])    return @"iPhone 5 (GSM+CDMA)";
        if ([deviceString isEqualToString:@"iPhone5,3"])    return @"iPhone 5c (GSM)";
        if ([deviceString isEqualToString:@"iPhone5,4"])    return @"iPhone 5c (GSM+CDMA)";
        if ([deviceString isEqualToString:@"iPhone6,1"])    return @"iPhone 5s (GSM)";
        if ([deviceString isEqualToString:@"iPhone6,2"])    return @"iPhone 5s (GSM+CDMA)";
        if ([deviceString isEqualToString:@"iPhone7,1"])    return @"iPhone 6 Plus";
        if ([deviceString isEqualToString:@"iPhone7,2"])    return @"iPhone 6";
        if ([deviceString isEqualToString:@"iPhone8,1"])    return @"iPhone 6s";
        if ([deviceString isEqualToString:@"iPhone8,2"])    return @"iPhone 6s Plus";
        if ([deviceString isEqualToString:@"iPhone8,4"])    return @"iPhone SE";
        // 日行两款手机型号均为日本独占，可能使用索尼FeliCa支付方案而不是苹果支付
        if ([deviceString isEqualToString:@"iPhone9,1"])    return @"国行、日版、港行iPhone 7";
        if ([deviceString isEqualToString:@"iPhone9,2"])    return @"港行、国行iPhone 7 Plus";
        if ([deviceString isEqualToString:@"iPhone9,3"])    return @"美版、台版iPhone 7";
        if ([deviceString isEqualToString:@"iPhone9,4"])    return @"美版、台版iPhone 7 Plus";
        if ([deviceString isEqualToString:@"iPhone10,1"])   return @"国行(A1863)、日行(A1906)iPhone 8";
        if ([deviceString isEqualToString:@"iPhone10,4"])   return @"美版(Global/A1905)iPhone 8";
        if ([deviceString isEqualToString:@"iPhone10,2"])   return @"国行(A1864)、日行(A1898)iPhone 8 Plus";
        if ([deviceString isEqualToString:@"iPhone10,5"])   return @"美版(Global/A1897)iPhone 8 Plus";
        if ([deviceString isEqualToString:@"iPhone10,3"])   return @"国行(A1865)、日行(A1902)iPhone X";
        if ([deviceString isEqualToString:@"iPhone10,6"])   return @"美版(Global/A1901)iPhone X";
    
        if ([deviceString isEqualToString:@"iPod1,1"])      return @"iPod Touch 1G";
        if ([deviceString isEqualToString:@"iPod2,1"])      return @"iPod Touch 2G";
        if ([deviceString isEqualToString:@"iPod3,1"])      return @"iPod Touch 3G";
        if ([deviceString isEqualToString:@"iPod4,1"])      return @"iPod Touch 4G";
        if ([deviceString isEqualToString:@"iPod5,1"])      return @"iPod Touch (5 Gen)";
    
        if ([deviceString isEqualToString:@"iPad1,1"])      return @"iPad";
        if ([deviceString isEqualToString:@"iPad1,2"])      return @"iPad 3G";
        if ([deviceString isEqualToString:@"iPad2,1"])      return @"iPad 2 (WiFi)";
        if ([deviceString isEqualToString:@"iPad2,2"])      return @"iPad 2";
        if ([deviceString isEqualToString:@"iPad2,3"])      return @"iPad 2 (CDMA)";
        if ([deviceString isEqualToString:@"iPad2,4"])      return @"iPad 2";
        if ([deviceString isEqualToString:@"iPad2,5"])      return @"iPad Mini (WiFi)";
        if ([deviceString isEqualToString:@"iPad2,6"])      return @"iPad Mini";
        if ([deviceString isEqualToString:@"iPad2,7"])      return @"iPad Mini (GSM+CDMA)";
        if ([deviceString isEqualToString:@"iPad3,1"])      return @"iPad 3 (WiFi)";
        if ([deviceString isEqualToString:@"iPad3,2"])      return @"iPad 3 (GSM+CDMA)";
        if ([deviceString isEqualToString:@"iPad3,3"])      return @"iPad 3";
        if ([deviceString isEqualToString:@"iPad3,4"])      return @"iPad 4 (WiFi)";
        if ([deviceString isEqualToString:@"iPad3,5"])      return @"iPad 4";
        if ([deviceString isEqualToString:@"iPad3,6"])      return @"iPad 4 (GSM+CDMA)";
        if ([deviceString isEqualToString:@"iPad4,1"])      return @"iPad Air (WiFi)";
        if ([deviceString isEqualToString:@"iPad4,2"])      return @"iPad Air (Cellular)";
        if ([deviceString isEqualToString:@"iPad4,4"])      return @"iPad Mini 2 (WiFi)";
        if ([deviceString isEqualToString:@"iPad4,5"])      return @"iPad Mini 2 (Cellular)";
        if ([deviceString isEqualToString:@"iPad4,6"])      return @"iPad Mini 2";
        if ([deviceString isEqualToString:@"iPad4,7"])      return @"iPad Mini 3";
        if ([deviceString isEqualToString:@"iPad4,8"])      return @"iPad Mini 3";
        if ([deviceString isEqualToString:@"iPad4,9"])      return @"iPad Mini 3";
        if ([deviceString isEqualToString:@"iPad5,1"])      return @"iPad Mini 4 (WiFi)";
        if ([deviceString isEqualToString:@"iPad5,2"])      return @"iPad Mini 4 (LTE)";
        if ([deviceString isEqualToString:@"iPad5,3"])      return @"iPad Air 2";
        if ([deviceString isEqualToString:@"iPad5,4"])      return @"iPad Air 2";
        if ([deviceString isEqualToString:@"iPad6,3"])      return @"iPad Pro 9.7";
        if ([deviceString isEqualToString:@"iPad6,4"])      return @"iPad Pro 9.7";
        if ([deviceString isEqualToString:@"iPad6,7"])      return @"iPad Pro 12.9";
        if ([deviceString isEqualToString:@"iPad6,8"])      return @"iPad Pro 12.9";
        if ([machineString isEqualToString:@"iPad6,11"])    return @"iPad 5 (WiFi)";
        if ([machineString isEqualToString:@"iPad6,12"])    return @"iPad 5 (Cellular)";
        if ([machineString isEqualToString:@"iPad7,1"])     return @"iPad Pro 12.9 inch 2nd gen (WiFi)";
        if ([machineString isEqualToString:@"iPad7,2"])     return @"iPad Pro 12.9 inch 2nd gen (Cellular)";
        if ([machineString isEqualToString:@"iPad7,3"])     return @"iPad Pro 10.5 inch (WiFi)";
        if ([machineString isEqualToString:@"iPad7,4"])     return @"iPad Pro 10.5 inch (Cellular)";
    
       if ([deviceString isEqualToString:@"AppleTV2,1"])    return @"Apple TV 2";
       if ([deviceString isEqualToString:@"AppleTV3,1"])    return @"Apple TV 3";
       if ([deviceString isEqualToString:@"AppleTV3,2"])    return @"Apple TV 3";
       if ([deviceString isEqualToString:@"AppleTV5,3"])    return @"Apple TV 4";
    
        if ([deviceString isEqualToString:@"i386"])         return @"Simulator";
        if ([deviceString isEqualToString:@"x86_64"])       return @"Simulator";
        */
    }
}