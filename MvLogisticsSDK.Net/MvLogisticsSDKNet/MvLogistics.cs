using System;
using System.Runtime.InteropServices;

namespace MvLogisticsSDKNet;

/// <summary>
/// MvLogisticsSDK
/// </summary>
public class MvLogistics
{
	/// <summary>
	/// ch:异常消息回调 | en:Exception message callBack
	/// </summary>
	/// <param name="pstEcptInfo">ch:异常回调参数结构体 | en:Exception message structure</param>
	/// <param name="pUser">ch:用户自定义变量 | en:User defined variable</param>
	public delegate void cbExceptiondelegate(ref MVLGS_EXCEPTION_INFO pstEcptInfo, IntPtr pUser);

	/// <summary>
	/// ch:包裹回调 | en:package callback
	/// </summary>
	/// <param name="pstOutput">ch:包裹信息指针 | en:package infomation pointer</param>
	/// <param name="pUser">ch:用户自定义变量 | en:User defined variable</param>
	public delegate void cbOutputdelegate(IntPtr pstOutput, IntPtr pUser);

	/// <summary>
	/// ch:NoRead图像回调 | en:NoRead Image data callback
	/// </summary>
	/// <param name="pstImageOutPutInfo">ch:NoRead图像回调参数指针 | en:NoRead image callback parameter pointer</param>
	/// <param name="pUser">ch:用户自定义变量 | en:User defined variable</param>
	public delegate void cbNoReaddelegate(IntPtr pstImageOutPutInfo, IntPtr pUser);

	/// <summary>
	/// ch:触发回调 | en:Trigger callback
	/// </summary>
	/// <param name="pstTriggerInfo">ch:触发回调参数指针 | en:Trigger callback parameter pointer</param>
	/// <param name="pUser">ch:用户自定义变量 | en:User defined variable</param>
	public delegate void cbTriggerOutputdelegate(ref MVLGS_TRIGGER_INFO pstTriggerInfo, IntPtr pUser);

	/// <summary> ch:条码类型 | en:Code type</summary>
	public enum MVLGS_CODE_TYPE
	{
		/// <summary>ch:无可识别条码 | en:No recognizable bar code</summary>
		MVLGS_CODE_NONE = 0,
		/// <summary>ch:DM码 | en:DM code</summary>
		MVLGS_CODE_TDCR_DM = 1,
		/// <summary>ch:QR码 | en:QR code</summary>
		MVLGS_CODE_TDCR_QR = 2,
		/// <summary>ch:EAN8码 | en:EAN8 code</summary>
		MVLGS_CODE_BCR_EAN8 = 8,
		/// <summary>ch:UPCE码 | en:UPCE code</summary>
		MVLGS_CODE_BCR_UPCE = 9,
		/// <summary>ch:UPCA码 | en:UPCA code</summary>
		MVLGS_CODE_BCR_UPCA = 12,
		/// <summary>ch:EAN13码 | en:EAN13 code</summary>
		MVLGS_CODE_BCR_EAN13 = 13,
		/// <summary>ch:ISBN13码 | en:ISBN13 code</summary>
		MVLGS_CODE_BCR_ISBN13 = 14,
		/// <summary>ch:库德巴码 | en:Codabar code</summary>
		MVLGS_CODE_BCR_CODABAR = 20,
		/// <summary>ch:交叉25码 | en:ITF25 code</summary>
		MVLGS_CODE_BCR_ITF25 = 25,
		/// <summary>ch:Code 39 | en:Code 39</summary>
		MVLGS_CODE_BCR_CODE39 = 39,
		/// <summary>ch:Code 93 | en:Code 93</summary>
		MVLGS_CODE_BCR_CODE93 = 93,
		/// <summary>ch:Code 128 | en:Code 128</summary>
		MVLGS_CODE_BCR_CODE128 = 128
	}

	/// <summary>ch:图像格式 | en:Image Type</summary>
	public enum MVLGS_IMAGE_TYPE
	{
		/// <summary>ch:未定义 | en:Undefined format</summary>
		MVLGS_IMAGE_Undefined,
		/// <summary>ch:Mono8 | en:MONO8 format</summary>
		MVLGS_IMAGE_MONO8,
		/// <summary>ch:JPEG | en:JPEG format</summary>
		MVLGS_IMAGE_JPEG,
		/// <summary>ch:Bmp | en:BMP format</summary>
		MVLGS_IMAGE_BMP,
		/// <summary>ch:RGB24 | en:RGB format</summary>
		MVLGS_IMAGE_RGB24,
		/// <summary>ch:BGR24 | en:BGR format</summary>
		MVLGS_IMAGE_BGR24
	}

	/// <summary> ch:设备类型 | en:Device type</summary>
	public enum MVLGS_DEVICE_TYPE
	{
		/// <summary> ch:工业相机 | en:industrial camera</summary>
		MVLGS_IDCAM,
		/// <summary> ch:读码器 | en:Barcode reader</summary>
		MVLGS_CODEREADER,
		/// <summary> ch:线扫相机 | en:Line Scan Camera</summary>
		MVLGS_LINESCAN,
		/// <summary> ch:全景相机 | en:Panoramic camera</summary>
		MVLGS_PANORAMIC,
		/// <summary> ch:体积相机 | en:Volume Camera</summary>
		MVLGS_VOLUME,
		/// <summary> ch:称重 | en:Weighting</summary>
		MVLGS_WEIGHT
	}

	/// <summary> ch:条码位置坐标 | en:Barcode position coordinates</summary>
	public struct MVLGS_POINT_I
	{
		/// <summary> ch:X坐标 | en:X coordinate</summary>
		public int nX;

		/// <summary> ch:Y坐标 | en:Y coordinate</summary>
		public int nY;
	}

	/// <summary>ch:条码信息 | en:Code information</summary>
	public struct MVLGS_CODE_INFO
	{
		/// <summary>ch:字符 | en:Character, maximum size: 4096</summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4096)]
		public string strCode;

		/// <summary>ch:字符长度 | en:Character size</summary>
		public int nLen;

		/// <summary>ch:条码类型 | en:Bar code type</summary>
		public MVLGS_CODE_TYPE enBarType;

		/// <summary>ch:条码位置 | en:Bar code location</summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public MVLGS_POINT_I[] stCornerPt;

		/// <summary>ch:条码角度（0~3600） | en:Bar code angle, range: [0, 3600°]</summary>
		public int nAngle;

		/// <summary>ch:过滤码标识(0为正常码, 1为过滤码) | en:Filter identifier: 0- normal code, 1-filter code</summary>
		public int nFilterFlag;

		/// <summary> ch:相机序列号 | en:Camera serial number</summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string strSerialNumber;

		/// <summary>ch:保留 | en:Reserved</summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 23)]
		public uint[] nReserved;
	}

	/// <summary>ch:条码信息 | en:Code information</summary>
	public struct MVLGS_CODE_INFOEx
	{
		/// <summary>ch:字符 | en:Character, maximum size: 4096</summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4096)]
		public byte[] strCode;

		/// <summary>ch:字符长度 | en:Character size</summary>
		public int nLen;

		/// <summary>ch:条码类型 | en:Bar code type</summary>         
		public MVLGS_CODE_TYPE enBarType;

		/// <summary>ch:条码位置 | en:Bar code location</summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public MVLGS_POINT_I[] stCornerPt;

		/// <summary>ch:条码角度（0~3600） | en:Bar code angle, range: [0, 3600°]</summary>
		public int nAngle;

		/// <summary>ch:过滤码标识(0为正常码, 1为过滤码) | en:Filter identifier: 0- normal code, 1-filter code</summary>           
		public int nFilterFlag;

		/// <summary> ch:相机序列号 | en:Camera serial number</summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string strSerialNumber;

		/// <summary>ch:保留 | en:Reserved</summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 23)]
		public uint[] nReserved;
	}

	/// <summary>ch:输出图像的信息 | en:Output Frame Information</summary>
	public struct MVLGS_IMAGE_INFO
	{
		/// <summary> ch:原始图像缓存，由用户传入 | en:Original image buffer</summary>
		public IntPtr pImageBuf;

		/// <summary>ch:原始图像长度 | en:Original image size</summary>
		public uint nImageLen;

		/// <summary>ch:图像格式 | en:Image Type</summary>
		public MVLGS_IMAGE_TYPE enImageType;

		/// <summary>ch:图像宽 | en:Image Width</summary>
		public ushort nWidth;

		/// <summary>ch:图像高 | en:Image Height</summary>
		public ushort nHeight;

		/// <summary> ch:触发序号，特殊场合可当做包裹号使用 | en:Trigger serial number, can be used as a parcel number in special occasions</summary>
		public uint nTriggerIndex;

		/// <summary>ch:帧号 | en:Frame No.</summary>
		public uint nFrameNum;

		/// <summary>ch:时间戳高32位 | en:Timestamp high 32 bits</summary>
		public uint nDevTimeStampHigh;

		/// <summary>ch:时间戳低32位 | en:Timestamp low 32 bits</summary>
		public uint nDevTimeStampLow;

		/// <summary>ch:用户自定义名字 | en:Custom name</summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string chUserDefinedName;

		/// <summary>ch:保留 | en:Reserved</summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public uint[] nReserved;
	}

	/// <summary> ch:条码信息列表 | en:List of barcode information</summary>
	public struct MVLGS_CODE_LIST
	{
		/// <summary> ch:条码数量 | en:Number of barcodes</summary>
		public int nCodeNum;

		/// <summary> ch:条码信息 | en:Barcode information</summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		public MVLGS_CODE_INFO[] stCodeInfo;

		/// <summary> ch:原始图像信息 | en:Original image information</summary>
		public MVLGS_IMAGE_INFO stImage;

		/// <summary> ch:抠图缓存，由SDK内部分配 | en:Cutout cache, allocated internally by the SDK</summary>
		public IntPtr pImageWaybill;

		/// <summary> ch:图像大小 | en:Image size</summary>
		public uint nImageWaybillLen;

		/// <summary> ch:抠图图像格式 | en:Matte image format</summary>
		public MVLGS_IMAGE_TYPE enWaybillImageType;

		/// <summary>ch:保留 | en:Reserved</summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
		public uint[] nReserved;
	}

	/// <summary> ch:条码信息列表 | en:List of barcode information</summary>
	public struct MVLGS_CODE_LISTEx
	{
		/// <summary> ch:条码数量 | en:Number of barcodes</summary>
		public int nCodeNum;

		/// <summary> ch:条码信息 | en:Barcode information</summary>       
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		public MVLGS_CODE_INFOEx[] stCodeInfo;

		/// <summary> ch:原始图像信息 | en:Original image information</summary>
		public MVLGS_IMAGE_INFO stImage;

		/// <summary> ch:抠图缓存，由SDK内部分配 | en:Cutout cache, allocated internally by the SDK</summary>
		public IntPtr pImageWaybill;

		/// <summary> ch:图像大小 | en:Image size</summary> 
		public uint nImageWaybillLen;

		/// <summary> ch:抠图图像格式 | en:Matte image format</summary>
		public MVLGS_IMAGE_TYPE enWaybillImageType;

		/// <summary>ch:保留 | en:Reserved</summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
		public uint[] nReserved;
	}

	/// <summary> ch:包裹体积字段信息 | en:Package volume field information</summary>
	public struct MVLGS_VOLUME_INFO
	{
		/// <summary> ch:包裹ID | en:Package ID</summary>
		public uint nPkgID;

		/// <summary> ch:体积 | en: volume</summary>
		public float fVolume;

		/// <summary> ch:长度 | en: length</summary>
		public float fLength;

		/// <summary> ch:宽度 | en:Width</summary>
		public float fWidth;

		/// <summary> ch:高度 | en:Height</summary>
		public float fHeight;

		/// <summary> ch:体积测量开始时间戳 | en:Volume measurement start timestamp</summary>
		public long nObjEnterSysTime;

		/// <summary> ch:体积测量结束时间戳 | en:Volume measurement end timestamp</summary>
		public long nObjLeaveSysTime;

		/// <summary>ch:保留 | en:Reserved</summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
		public uint[] nReserved;
	}

	/// <summary> ch:包裹信息 | en:Package information</summary>
	public struct MVLGS_PACKAGE_INFO
	{
		/// <summary> ch:是否包含条码信息,非0：包含条码信息 | en:Whether to include barcode information, non-zero: contains barcode information</summary>
		public bool bCodeEnable;

		/// <summary> ch:条码列表数量 | en:Number of barcode lists</summary>
		public uint nCodeListNum;

		/// <summary> ch:条码列表信息 | en:Bar code list information</summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
		public MVLGS_CODE_LIST[] stCodeList;

		/// <summary> ch:是否包含体积信息,非0：包含体积信息 | en:Whether to include volume information, non-zero: contains volume information</summary>
		public bool bVolumeEnable;

		/// <summary> ch:体积信息 | en:Volume information</summary>
		public MVLGS_VOLUME_INFO stVolumeInfo;

		/// <summary> ch:是否包含重量信息,非0：包含重量信息 | en:Whether to include weight information, non-zero: contains weight information</summary>
		public bool bWeightEnable;

		/// <summary> ch:重量信息 | en:Weight information</summary>
		public float fWeight;

		/// <summary> ch:是否包含全景图像,非0：包含全景图像 | en:Whether to include panoramic images, non-zero: include panoramic images</summary>
		public bool bPRMEnable;

		/// <summary> ch:原始图像信息 | en:Original image information</summary>
		public MVLGS_IMAGE_INFO stPRMImage;

		/// <summary> ch:是否为条码先输出 (1为第一次输出，0为第二次输出) | en:Whether to output the barcode first (1 is the first output, 0 is the second output)</summary>
		public bool bIsFirstCodeOut;

		/// <summary> ch:触发开始时间戳(13位) | en:Trigger start timestamp (13 bits)</summary>
		public long llTriggerStartTimeStamp;

		/// <summary> ch:触发结束时间戳(13位) | en:Trigger end timestamp (13 bits)</summary>
		public long llTriggerEndTimeStamp;

		/// <summary>ch:保留 | en:Reserved</summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 27)]
		public uint[] nReserved;
	}

	/// <summary> ch:包裹信息 | en:Package information</summary>
	public struct MVLGS_PACKAGE_INFOEx
	{
		/// <summary> ch:是否包含条码信息,非0：包含条码信息 | en:Whether to include barcode information, non-zero: contains barcode information</summary>
		public bool bCodeEnable;

		/// <summary> ch:条码列表数量 | en:Number of barcode lists</summary>
		public uint nCodeListNum;

		/// <summary> ch:条码列表信息 | en:Bar code list information</summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
		public MVLGS_CODE_LISTEx[] stCodeList;

		/// <summary> ch:是否包含体积信息,非0：包含体积信息 | en:Whether to include volume information, non-zero: contains volume information</summary>
		public bool bVolumeEnable;

		/// <summary> ch:体积信息 | en:Volume information</summary>
		public MVLGS_VOLUME_INFO stVolumeInfo;

		/// <summary> ch:是否包含重量信息,非0：包含重量信息 | en:Whether to include weight information, non-zero: contains weight information</summary>
		public bool bWeightEnable;

		/// <summary> ch:重量信息 | en:Weight information</summary>
		public float fWeight;

		/// <summary> ch:是否包含全景图像,非0：包含全景图像 | en:Whether to include panoramic images, non-zero: include panoramic images</summary>
		public bool bPRMEnable;

		/// <summary> ch:是否包含全景图像,非0：包含全景图像 | en:Whether to include panoramic images, non-zero: include panoramic images</summary>
		public MVLGS_IMAGE_INFO stPRMImage;

		/// <summary> ch:是否为条码先输出 (1为第一次输出，0为第二次输出) | en:Whether to output the barcode first (1 is the first output, 0 is the second output)</summary>
		public bool bIsFirstCodeOut;

		/// <summary> ch:触发开始时间戳(13位) | en:Trigger start timestamp (13 bits)</summary>
		public long llTriggerStartTimeStamp;

		/// <summary> ch:触发结束时间戳(13位) | en:Trigger end timestamp (13 bits)</summary>
		public long llTriggerEndTimeStamp;

		/// <summary>ch:保留 | en:Reserved</summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 27)]
		public uint[] nReserved;
	}

	/// <summary> ch:异常信息 | en:Exception information</summary>
	public struct MVLGS_EXCEPTION_INFO
	{
		/// <summary> ch:异常ID | en:Exception ID</summary>
		public uint nExceptionID;

		/// <summary> ch:异常设备类型 | en:Abnormal device type</summary>
		public MVLGS_DEVICE_TYPE enCamType;

		/// <summary> ch:异常设备序列号(体积相机此处为MAC地址) | en:Abnormal device serial number (the volume camera here is the MAC address)</summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string strCamSerialNum;

		/// <summary> ch:异常描述 | en:Exception description</summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string strExceptionDes;

		/// <summary>ch:保留 | en:Reserved</summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public uint[] nReserved;
	}

	/// <summary> ch:触发信息 | en:Trigger information</summary>
	public struct MVLGS_TRIGGER_INFO
	{
		/// <summary> ch:触发ID | en:Trigger ID</summary>
		public uint nTriggerIndex;

		/// <summary> ch:触发标识 与头文件触发开始为1(MV_LGS_BEGIN_TRIGGER) 触发结束为0(MV_LGS_STOP_TRIGGER) 对应映射| en:Trigger description</summary>          
		public uint nTriggerFlag;

		/// <summary>ch:保留 | en:Reserved</summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public uint[] nReserved;
	}

	/// <summary> ch:相机输出图像信息 | en:Camera output image information</summary>
	public struct MVLGS_IMAGE_OUTPUT_INFO
	{
		/// <summary> ch:图像信息 | en:Image information</summary>
		public MVLGS_IMAGE_INFO stImage;

		/// <summary> ch:相机序列号 | en:Camera serial number</summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string strSerialNumber;

		/// <summary>ch:保留 | en:Reserved</summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		public uint[] nReserved;
	}

	/// <summary> ch:设备信息 | en:Device Information</summary>
	public struct MVLGS_XML_CFG_CAM_INFO
	{
		/// <summary> ch:相机类型 | en:Camera type</summary>
		public MVLGS_DEVICE_TYPE enXmlCamType;

		/// <summary>ch:MAC 地址 高位| en:High MAC address</summary>
		public uint nMacAddrHigh;

		/// <summary>ch:MAC 地址 低位| en:Low MAC address</summary>
		public uint nMacAddrLow;

		/// <summary>ch:当前IP | en:Current IP address</summary> 
		public uint nCurrentIp;

		/// <summary>ch:制造商名字 | en:Manufacturer Name</summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string strManufacturerName;

		/// <summary>ch:型号名字 | en:Model Name</summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string strModelName;

		/// <summary>ch:设备版本号 | en:Device version No.</summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string strDeviceVersion;

		/// <summary>ch:序列号 | en:Device serial No.</summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string strSerialNumber;

		/// <summary>ch:用户自定义名字 | en:Custom name</summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string strUserDefinedName;

		/// <summary> ch:设备在线状态，true在线，false离线 | en:Device online status, true online, false offline</summary>
		[MarshalAs(UnmanagedType.I1)]
		public bool bDeviceOnline;

		/// <summary> ch:是否为主相机 | en:Whether the main camera</summary>
		[MarshalAs(UnmanagedType.I1)]
		public bool bMainCamera;

		/// <summary>ch:保留 | en:Reserved</summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public uint[] nReserved;
	}

	/// <summary> ch:XML配置文件中相机信息列表 | en:List of camera information in XML configuration file</summary>
	public struct MVLGS_XML_CFG_CAM_INFO_LIST
	{
		/// <summary> ch:XML配置文件中的相机数量 | en:Number of cameras in XML configuration file</summary>
		public uint nXmlCfgCamNum;

		/// <summary> ch:相机信息结构体数组 | en:Camera information structure array</summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		public IntPtr[] pstXmlCamInfo;
	}

	/// <summary>ch:成功, 无错误 | en:Successed, no error</summary>
	public const int MV_LGS_OK = 0;

	/// <summary> ch:错误或无效的句柄 | en:Error or invalid handle</summary>
	public const int MV_LGS_E_HANDLE = -2146369536;

	/// <summary> ch:不支持的功能 | en:The function is not supported</summary>
	public const int MV_LGS_E_SUPPORT = -2146369535;

	/// <summary> ch:缓存已满 | en:Buffer is full</summary>
	public const int MV_LGS_E_BUFOVER = -2146369534;

	/// <summary> ch:函数调用顺序错误 | en:Incorrect calling sequence</summary>
	public const int MV_LGS_E_CALLORDER = -2146369533;

	/// <summary> ch:错误的参数 | en:Incorrect parameter</summary>
	public const int MV_LGS_E_PARAMETER = -2146369532;

	/// <summary> ch:资源申请失败 | en:Applying resource failed</summary>
	public const int MV_LGS_E_RESOURCE = -2146369531;

	/// <summary> ch:无数据 | en:No data</summary>
	public const int MV_LGS_E_NODATA = -2146369530;

	/// <summary> ch:前置条件有误，或运行环境已发生变化 | en:Precondition error, or running environment changed</summary>
	public const int MV_LGS_E_PRECONDITION = -2146369529;

	/// <summary> ch:凭证错误，可能是未插加密狗，或加密狗过期 | en:Credential error, possibly because the dongle was not installed or expired</summary>
	public const int MV_LGS_E_ENCRYPT = -2146369528;

	/// <summary> ch:过滤规则相关的错误 | en:Filter rule error</summary>
	public const int MV_LGS_E_RULE = -2146369526;

	/// <summary> ch:jpg编码相关错误 | en:Jpg encoding error</summary>
	public const int MV_LGS_E_JPGENC = -2146369518;

	/// <summary> ch:输入的图像数据有损或图像格式,宽高错误 | en:Abnormal image. Incomplete image caused by packet loss or incorrect image format, width, or height</summary>
	public const int MV_LGS_E_IMAGE = -2146369517;

	/// <summary> ch:配置文件有误 | en:Config file error</summary>
	public const int MV_LGS_E_CONFIG = -2146369516;

	/// <summary> ch:未知的错误 | en:Unknown error</summary>
	public const int MV_LGS_E_UNKNOW = -2146369281;

	/// <summary> ch:相机相关的错误 | en:Camera error</summary>
	public const int MV_LGS_E_CAMERA = -2146361088;

	/// <summary> ch:一维码相关错误 | en:1D barcode error</summary>
	public const int MV_LGS_E_BCR = -2146360832;

	/// <summary> ch:二维码相关错误 | en:2D barcode error</summary>
	public const int MV_LGS_E_TDCR = -2146360576;

	/// <summary> ch:抠图相关错误 | en:Matting error</summary>
	public const int MV_LGS_E_WAYBILL = -2146360320;

	/// <summary> ch:脚本规则相关错误 | en:Script rule error</summary>
	public const int MV_LGS_E_SCRIPT = -2146360064;

	/// <summary> ch:通用错误 | en:General error</summary>
	public const int MV_LGS_E_GC_GENERIC = -2146369280;

	/// <summary> ch:参数非法 | en:Invalid parameter</summary>
	public const int MV_LGS_E_GC_ARGUMENT = -2146369279;

	/// <summary> ch:值超出范围 | en:The value is out of range</summary>
	public const int MV_LGS_E_GC_RANGE = -2146369278;

	/// <summary> ch:属性错误 | en:Attribute error</summary>
	public const int MV_LGS_E_GC_PROPERTY = -2146369277;

	/// <summary> ch:运行环境有问题 | en:Running environment error</summary>
	public const int MV_LGS_E_GC_RUNTIME = -2146369276;

	/// <summary> ch:逻辑错误 | en:Incorrect logic</summary>
	public const int MV_LGS_E_GC_LOGICAL = -2146369275;

	/// <summary> ch:节点访问条件有误 | en:Node accessing condition error</summary>
	public const int MV_LGS_E_GC_ACCESS = -2146369274;

	/// <summary> ch:超时 | en:Timeout</summary>
	public const int MV_LGS_E_GC_TIMEOUT = -2146369273;

	/// <summary> ch:转换异常 | en:Transformation exception</summary>
	public const int MV_LGS_E_GC_DYNAMICCAST = -2146369272;

	/// <summary> ch:GenICam未知错误 | en:GenICam unknown error</summary>
	public const int MV_LGS_E_GC_UNKNOW = -2146369025;

	/// <summary> ch:命令不被设备支持 | en:The command is not supported by device</summary>
	public const int MV_LGS_E_NOT_IMPLEMENTED = -2146369024;

	/// <summary> ch:访问的目标地址不存在 | en:Target address does not exist</summary>
	public const int MV_LGS_E_INVALID_ADDRESS = -2146369023;

	/// <summary> ch:目标地址不可写 | en:The target address is not writable</summary>
	public const int MV_LGS_E_WRITE_PROTECT = -2146369022;

	/// <summary> ch:设备无访问权限 | en:No access permission</summary>
	public const int MV_LGS_E_ACCESS_DENIED = -2146369021;

	/// <summary> ch:设备忙，或网络断开 | en:Device is busy, or network is disconnected</summary>
	public const int MV_LGS_E_BUSY = -2146369020;

	/// <summary> ch:网络包数据错误 | en:Network packet error</summary>
	public const int MV_LGS_E_PACKET = -2146369019;

	/// <summary> ch:网络相关错误 | en:Network error</summary>
	public const int MV_LGS_E_NETER = -2146369018;

	/// <summary> ch:设备IP冲突 | en:IP address conflicted</summary>
	public const int MV_LGS_E_IP_CONFLICT = -2146368991;

	/// <summary> ch:读usb出错 | en:USB read error</summary>
	public const int MV_LGS_E_USB_READ = -2146368768;

	/// <summary> ch:写usb出错 | en:USB write error</summary>
	public const int MV_LGS_E_USB_WRITE = -2146368767;

	/// <summary> ch:设备异常 | en:Device exception</summary>
	public const int MV_LGS_E_USB_DEVICE = -2146368766;

	/// <summary> ch:GenICam相关错误 | en:GenICam error</summary>
	public const int MV_LGS_E_USB_GENICAM = -2146368765;

	/// <summary> ch:带宽不足  该错误码新增 | en:Insufficient bandwidth, this error code is newly added</summary>
	public const int MV_LGS_E_USB_BANDWIDTH = -2146368764;

	/// <summary> ch:驱动不匹配或者未装驱动 | en:Driver is mismatched, or is not installed</summary>
	public const int MV_LGS_E_USB_DRIVER = -2146368763;

	/// <summary> ch:USB未知的错误 | en:USB unknown error</summary>
	public const int MV_LGS_E_USB_UNKNOW = -2146368513;

	/// <summary> ch:融合模块参数错误 | en:Fusion module parameter error</summary>
	public const int MV_LGS_E_FUSION_PARAM = -2146359808;

	/// <summary> ch:融合模块内存分配失败 | en:Fusion module memory allocation failed</summary>
	public const int MV_LGS_E_FUSION_MALLOC = -2146359807;

	/// <summary> ch:融合模块调用顺序错误 | en:Fusion module call sequence is wrong</summary>
	public const int MV_LGS_E_FUSION_CALLORDER = -2146359806;

	/// <summary> ch:融合模块配置文件错误 | en:Fusion module configuration file error</summary>
	public const int MV_LGS_E_FUSION_CFGFILE = -2146359805;

	/// <summary> ch:融合模块未知错误 | en:Unknown error of fusion module</summary>
	public const int MV_LGS_E_FUSION_UNKNOWN = -2146359804;

	/// <summary> ch:融合模块缓存不足 | en:Insufficient Fusion Module Cache</summary>
	public const int MV_LGS_E_FUSION_LACKBUF = -2146359803;

	/// <summary> ch:融合模块不支持 | en:Fusion module does not support</summary>
	public const int MV_LGS_E_FUSION_SUPPORT = -2146359802;

	/// <summary> ch:体积模块参数错误 | en:Volume module parameter error</summary>
	public const int MV_LGS_E_VOLMEASURE_PARAM = -2146359552;

	/// <summary> ch:体积模块内存分配失败 | en:Volume module memory allocation failed</summary>
	public const int MV_LGS_E_VOLMEASURE_MALLOC = -2146359551;

	/// <summary> ch:体积模块调用顺序错误 | en:Volume module calling sequence is wrong</summary>
	public const int MV_LGS_E_VOLMEASURE_CALLORDER = -2146359550;

	/// <summary> ch:体积模块无数据 | en:No data for volume module</summary>
	public const int MV_LGS_E_VOLMEASURE_NODATA = -2146359549;

	/// <summary> ch:体积模块配置文件错误 | en:Volume module configuration file error</summary>
	public const int MV_LGS_E_VOLMEASURE_CFGFILE = -2146359548;

	/// <summary> ch:体积模块无包裹 | en:Volume module without package</summary>
	public const int MV_LGS_E_VOLMEASURE_NOPKG = -2146359547;

	/// <summary> ch:体积模块未知错误 | en:Volume module unknown error</summary>
	public const int MV_LGS_E_VOLMEASURE_UNKNOWN = -2146359546;

	/// <summary> ch:体积模块缓存不足 | en:Insufficient volume module cache</summary>
	public const int MV_LGS_E_VOLMEASURE_LACKBUF = -2146359545;

	/// <summary> ch:体积模块不支持 | en:Volume module does not support</summary>
	public const int MV_LGS_E_VOLMEASURE_SUPPORT = -2146359544;

	/// <summary> ch:称重模块称重设备打开失败 | en:Weighing module weighing equipment failed to open</summary>
	public const int MV_LGS_E_WGHT_OPEN = -2146359296;

	/// <summary> ch:称重模块加密错误 | en:Weighing module encryption error</summary>
	public const int MV_LGS_E_WGHT_ENC = -2146359295;

	/// <summary> ch:称重模块资源初始化失败 | en:Weighing module resource initialization failed</summary>
	public const int MV_LGS_E_WGHT_RESOURCE = -2146359294;

	/// <summary> ch:称重模块调用顺序错误 | en:Weighing module call sequence error</summary>
	public const int MV_LGS_E_WGHT_CALLORDER = -2146359293;

	/// <summary> ch:称重模块指针类型参数为空 | en:Weighing module pointer type parameter is null</summary>
	public const int MV_LGS_E_WGHT_NULL = -2146359292;

	/// <summary> ch:称重模块数值类型参数范围错误 | en:Weighing module numerical type parameter range error</summary>
	public const int MV_LGS_E_WGHT_RANGE = -2146359291;

	/// <summary> ch:称重模块能力集错误 | en:Weighing module capability set error</summary>
	public const int MV_LGS_E_WGHT_ENABLE = -2146359290;

	/// <summary> ch:称重模块其他内部错误 | en:Weighing module other internal errors</summary>
	public const int MV_LGS_E_WGHT_UNKNOW = -2146359289;

	/// <summary>ch:设备重连成功 | en:The device is reconnect success</summary>
	public const int MV_LGS_EXCEPTION_RECONNECT_DEV_SUCCESS = 32768;

	/// <summary>ch:设备断开连接 | en:The device is disconnected</summary>
	public const int MV_LGS_EXCEPTION_DEV_DISCONNECT = 32769;

	/// <summary>ch:加密狗掉线 | en:The softdog is disconnected</summary>
	public const int MV_LGS_EXCEPTION_SOFTDOG_DISCONNECT = 32770;

	/// <summary>ch:开始触发 | en:Start trigger</summary>
	public const int MV_LGS_BEGIN_TRIGGER = 1;

	/// <summary>ch:结束触发 | en:Stop trigger</summary>
	public const int MV_LGS_STOP_TRIGGER = 0;

	/// <summary> ch:最大条码长度 | en:Maximum barcode length</summary>
	public const int MVLGS_MAX_CODECHARATERLEN = 4096;

	/// <summary> ch:单张图片内最大条码个数 | en:Maximum number of barcodes in a single picture</summary>
	public const int MVLGS_MAX_CODENUM = 256;

	/// <summary> ch:最大条码信息列表个数 | en:Maximum number of barcode information lists</summary>
	public const int MVLGS_MAX_CODELISTNUM = 24;

	/// <summary> ch:设备句柄 | en:Device handle</summary>
	private IntPtr handle;

	/// <summary>
	/// ch:获取SDK版本号 | en:Get SDK Version
	/// </summary>
	/// <returns>ch:返回SDK版本号 | en:SDK Version</returns>
	public static int MV_LGS_GetVersion_NET()
	{
		return MV_LGS_GetVersion();
	}

	/// <summary>
	/// ch:构造函数 | en:Constructor
	/// </summary>
	public MvLogistics()
	{
		handle = IntPtr.Zero;
	}

	/// <summary>
	/// ch:析构函数 | en:Destructor
	/// </summary>
	~MvLogistics()
	{
		MV_LGS_DestroyHandle_NET();
	}

	/// <summary>
	/// ch:创建句柄 | en:Create Handle
	/// </summary>
	/// <returns>ch:成功, 返回MV_LGS_OK; 错误, 返回错误码 | en:Success, return MV_LGS_OK. Failure, return error code</returns>
	public int MV_LGS_CreateHandle_NET()
	{
		if (IntPtr.Zero != handle)
		{
			MV_LGS_DestroyHandle(handle);
			handle = IntPtr.Zero;
		}
		return MV_LGS_CreateHandle(ref handle);
	}

	/// <summary>
	/// ch:销毁句柄 | en:Destroy Handle
	/// </summary>
	/// <returns>ch:成功, 返回MV_LGS_OK; 错误, 返回错误码 | en:Success, return MV_LGS_OK. Failure, return error code</returns>
	public int MV_LGS_DestroyHandle_NET()
	{
		int nRet = MV_LGS_DestroyHandle(handle);
		handle = IntPtr.Zero;
		return nRet;
	}

	/// <summary>
	/// ch:加载配置文件 | en:Load Config file
	/// </summary>
	/// <param name="strCfgPath">ch:配置文件路径 | en:FilePath of Config File</param>
	/// <returns>ch:成功, 返回MV_LGS_OK; 错误, 返回错误码 | en:Success, return MV_LGS_OK. Failure, return error code</returns>
	public int MV_LGS_LoadDevCfg_NET(string strCfgPath)
	{
		return MV_LGS_LoadDevCfg(handle, strCfgPath);
	}

	/// <summary>
	/// ch:注册异常消息回调 | en:Register Exception Message CallBack
	/// </summary>
	/// <param name="cbException">ch:异常回调函数指针 | en:Exception Message CallBack Function Pointer</param>
	/// <param name="pUser">ch:用户自定义变量 | en:User defined variable</param>
	/// <returns>ch:成功, 返回MV_LGS_OK; 错误, 返回错误码 | en:Success, return MV_LGS_OK. Failure, return error code</returns>
	public int MV_LGS_RegisterExceptionCB_NET(cbExceptiondelegate cbException, IntPtr pUser)
	{
		return MV_LGS_RegisterExceptionCB(handle, cbException, pUser);
	}

	/// <summary>
	/// ch:包裹消息回调 | en:Register Pakcage Message CallBack
	/// </summary>
	/// <param name="cbOutput">ch:包裹信息回调函数指针 | en:Pakcage Message CallBack Function Pointer</param>
	/// <param name="pUser">ch:用户自定义变量 | en:User defined variable</param>
	/// <returns>ch:成功, 返回MV_LGS_OK; 错误, 返回错误码 | en:Success, return MV_LGS_OK. Failure, return error code</returns>
	public int MV_LGS_RegisterPackageCB_NET(cbOutputdelegate cbOutput, IntPtr pUser)
	{
		return MV_LGS_RegisterPackageCB(handle, cbOutput, pUser);
	}

	/// <summary>
	/// ch:触发消息回调 | en:Register Trigger Message CallBack
	/// </summary>
	/// <param name="cbTriggerInfoOutput">ch:触发消息回调函数指针 | en:Trigger Message CallBack Function Pointer</param>
	/// <param name="pUser">ch:用户自定义变量 | en:User defined variable</param>
	/// <returns>ch:成功, 返回MV_LGS_OK; 错误, 返回错误码 | en:Success, return MV_LGS_OK. Failure, return error code</returns>
	public int MV_LGS_RegisterTriggerInfoCB_NET(cbTriggerOutputdelegate cbTriggerInfoOutput, IntPtr pUser)
	{
		return MV_LGS_RegisterTriggerInfoCB(handle, cbTriggerInfoOutput, pUser);
	}

	/// <summary>
	/// ch:开始取流 | en:Start Grabbing
	/// </summary>
	/// <returns>ch:成功, 返回MV_LGS_OK; 错误, 返回错误码 | en:Success, return MV_LGS_OK. Failure, return error code</returns>
	public int MV_LGS_Start_NET()
	{
		return MV_LGS_Start(handle);
	}

	/// <summary>
	/// ch:结束取流 | en:Stop Grabbing
	/// </summary>
	/// <returns>ch:成功, 返回MV_LGS_OK; 错误, 返回错误码 | en:Success, return MV_LGS_OK. Failure, return error code</returns>
	public int MV_LGS_Stop_NET()
	{
		return MV_LGS_Stop(handle);
	}

	/// <summary>
	/// ch:外部设置触发状态 | en:Set Trigger Signal
	/// </summary>
	/// <param name="nTriggerSignal">ch:触发状态 | en:Trigger Signal</param>
	/// <returns>ch:成功, 返回MV_LGS_OK; 错误, 返回错误码 | en:Success, return MV_LGS_OK. Failure, return error code</returns>
	public int MV_LGS_SetTrigger_NET(uint nTriggerSignal)
	{
		return MV_LGS_SetTrigger(handle, nTriggerSignal);
	}

	/// <summary>
	/// ch:读码器NoRead图像回调 | en:NoRead Image CallBack
	/// </summary>
	/// <param name="cbNoReadImageOutput">ch:NoRead图像输出回调函数指针 | en:NoRead Image Output CallBack Function Pointer</param>
	/// <param name="pUser">ch:用户自定义变量 | en:User defined variable</param>
	/// <returns>ch:成功, 返回MV_LGS_OK; 错误, 返回错误码 | en:Success, return MV_LGS_OK. Failure, return error code</returns>
	public int MV_LGS_RegisterNoReadImageCB_NET(cbNoReaddelegate cbNoReadImageOutput, IntPtr pUser)
	{
		return MV_LGS_RegisterNoReadImageCB(handle, cbNoReadImageOutput, pUser);
	}

	/// <summary>
	/// ch:获取XML配置中的相机信息 | en:Get XML Config Camera Info
	/// </summary>
	/// <param name="pstXmlCfgCamInfo">ch:配置文件中对应相机信息列表 | en:Camera Info Lists</param>
	/// <param name="strCfgPath">ch:配置文件路径 | en:Config File Path</param>
	/// <returns>ch:成功, 返回MV_LGS_OK; 错误, 返回错误码 | en:Success, return MV_LGS_OK. Failure, return error code</returns>
	public int MV_LGS_GetXmlCfgCamInfo_NET(ref MVLGS_XML_CFG_CAM_INFO_LIST pstXmlCfgCamInfo, string strCfgPath)
	{
		return MV_LGS_GetXmlCfgCamInfo(ref pstXmlCfgCamInfo, strCfgPath);
	}

	/// <summary>
	/// ch:获取相机句柄 | en:Get Camera Handle
	/// </summary>
	/// <returns>ch:返回相机句柄 | en:return camera handle</returns>
	public IntPtr GetCameraHandle()
	{
		return handle;
	}

	/// <summary>
	/// ch:设置流水号 | en:Set Running Number
	/// </summary>
	/// <param name="nRunNumber">ch:流水号 | en:Running Number</param>
	/// <param name="unBindTime">ch:流水号和触发号绑定的时间区间 | en:The time interval of the serial number and the trigger number binding</param>
	/// <returns>ch:成功, 返回MV_LGS_OK; 错误, 返回错误码 | en:Success, return MV_LGS_OK. Failure, return error code</returns>
	public int MV_LGS_SetRunNumber_NET(uint nRunNumber, uint unBindTime)
	{
		return MV_LGS_SetRunNumber(handle, nRunNumber, unBindTime);
	}

	[DllImport("MvLogisticsSDK.dll")]
	private static extern int MV_LGS_GetVersion();

	[DllImport("MvLogisticsSDK.dll")]
	private static extern int MV_LGS_CreateHandle(ref IntPtr handle);

	[DllImport("MvLogisticsSDK.dll")]
	private static extern int MV_LGS_DestroyHandle(IntPtr handle);

	[DllImport("MvLogisticsSDK.dll")]
	private static extern int MV_LGS_LoadDevCfg(IntPtr handle, string strCfgPath);

	[DllImport("MvLogisticsSDK.dll")]
	private static extern int MV_LGS_RegisterExceptionCB(IntPtr handle, cbExceptiondelegate cbException, IntPtr pUser);

	[DllImport("MvLogisticsSDK.dll")]
	private static extern int MV_LGS_RegisterPackageCB(IntPtr handle, cbOutputdelegate cbOutput, IntPtr pUser);

	[DllImport("MvLogisticsSDK.dll")]
	private static extern int MV_LGS_RegisterTriggerInfoCB(IntPtr handle, cbTriggerOutputdelegate cbTriggerInfoOutput, IntPtr pUser);

	[DllImport("MvLogisticsSDK.dll")]
	private static extern int MV_LGS_Start(IntPtr handle);

	[DllImport("MvLogisticsSDK.dll")]
	private static extern int MV_LGS_Stop(IntPtr handle);

	[DllImport("MvLogisticsSDK.dll")]
	private static extern int MV_LGS_SetTrigger(IntPtr handle, uint nTriggerSignal);

	[DllImport("MvLogisticsSDK.dll")]
	private static extern int MV_LGS_RegisterNoReadImageCB(IntPtr handle, cbNoReaddelegate cbNoReadImageOutput, IntPtr pUser);

	[DllImport("MvLogisticsSDK.dll")]
	private static extern int MV_LGS_GetXmlCfgCamInfo(ref MVLGS_XML_CFG_CAM_INFO_LIST pstXmlCfgCamInfo, string strCfgPath);

	[DllImport("MvLogisticsSDK.dll")]
	private static extern int MV_LGS_SetRunNumber(IntPtr handle, uint nRunNumber, uint unBindTime);
}
