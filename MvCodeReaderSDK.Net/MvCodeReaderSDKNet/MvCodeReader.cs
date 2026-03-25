using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MvCodeReaderSDKNet;

/// <summary>
/// ch:封装的接口类 | en:Encapsulated interface class
/// </summary>
public class MvCodeReader
{
	/// 回调函数声明
	/// <summary>
	/// 异常回调函数
	/// </summary>
	/// <param name="nMsgType">异常事件类型:MV_CODEREADER_EXCEPTION_DEV_DISCONNECT(设备断开)/MV_CODEREADER_EXCEPTION_VERSION_CHECK(SDK与驱动版本不匹配)</param>
	/// <param name="pUser">用户参数</param>
	public delegate void cbExceptiondelegate(uint nMsgType, IntPtr pUser);

	/// <summary>
	/// 图像输出回调函数
	/// </summary>
	/// <param name="pData">一帧图像数据</param>
	/// <param name="pstFrameInfo">图像数据信息:MV_CODEREADER_IMAGE_OUT_INFO</param>
	/// <param name="pUser">用户参数</param>
	public delegate void cbOutputdelegate(IntPtr pData, IntPtr pstFrameInfo, IntPtr pUser);

	/// <summary>
	/// 图像输出回调函数
	/// </summary>
	/// <param name="pData">一帧图像数据</param>
	/// <param name="pstFrameInfoEx">图像数据信息扩展:MV_CODEREADER_IMAGE_OUT_INFO_EX</param>
	/// <param name="pUser">用户参数</param>
	public delegate void cbOutputExdelegate(IntPtr pData, IntPtr pstFrameInfoEx, IntPtr pUser);

	/// <summary>
	/// 图像输出回调函数
	/// </summary>
	/// <param name="pData">一帧图像数据</param>
	/// <param name="pstFrameInfoEx2">图像数据信息扩展:MV_CODEREADER_IMAGE_OUT_INFO_EX2</param>
	/// <param name="pUser">用户参数</param>
	public delegate void cbOutputEx2delegate(IntPtr pData, IntPtr pstFrameInfoEx2, IntPtr pUser);

	/// <summary>
	/// 指定一路流通道图像数据(数据包含二维码质量评级)回调
	/// </summary>
	/// <param name="pData">一帧图像数据</param>
	/// <param name="pstFrameInfoEx2">图像数据信息扩展:MV_CODEREADER_IMAGE_OUT_INFO_EX2</param>
	/// <param name="pUser">用户参数</param>
	public delegate void cbMSCOutputdelegate(IntPtr pData, IntPtr pstFrameInfoEx2, IntPtr pUser);

	/// <summary>
	/// 全部事件回调函数
	/// </summary>
	/// <param name="pEventInfo">事件回调信息:MV_CODEREADER_EVENT_OUT_INFO</param>
	/// <param name="pUser">用户参数</param>
	public delegate void cbAllEventdelegate(IntPtr pEventInfo, IntPtr pUser);

	/// <summary>
	/// 触发信息回调函数
	/// </summary>
	/// <param name="pstTriggerInfo">相机触发信息:MV_CODEREADER_TRIGGER_INFO_DATA</param>
	/// <param name="pUser">用户参数</param>
	public delegate void cbTriggerdelegate(IntPtr pstTriggerInfo, IntPtr pUser);

	/// <summary>
	/// 相机GIGE设备信息
	/// </summary>
	public struct MV_CODEREADER_GIGE_DEVICE_INFO
	{
		/// <summary>
		/// ip配置类型
		/// </summary>
		public uint nIpCfgOption;

		/// <summary>
		/// IP当前类型
		/// </summary>
		public uint nIpCfgCurrent;

		/// <summary>
		/// 当前IP地址
		/// </summary>
		public uint nCurrentIp;

		/// <summary>
		/// Mask地址
		/// </summary>
		public uint nCurrentSubNetMask;

		/// <summary>
		/// 默认网关
		/// </summary>
		public uint nDefultGateWay;

		/// <summary>
		/// 制造商名
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string chManufacturerName;

		/// <summary>
		/// 设备型号
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string chModelName;

		/// <summary>
		/// 设备版本
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string chDeviceVersion;

		/// <summary>
		/// 制造商说明
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
		public string chManufacturerSpecificInfo;

		/// <summary>
		/// 设备序列号
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string chSerialNumber;

		/// <summary>
		/// 设备用户自定义名称
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public byte[] UserDefinedName;

		/// <summary>
		/// 网口IP地址
		/// </summary>
		public uint nNetExport;

		/// <summary>
		/// 当前占用设备的用户IP
		/// </summary>
		public uint nCurUserIP;

		/// <summary>
		/// 预留字段
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public uint[] nReserved;

		/// <summary>
		/// 设备用户自定义名称
		/// </summary>
		public string chUserDefinedName
		{
			get
			{
				byte[] array = BytesTrimEnd(UserDefinedName);
				if (IsTextUTF8(array))
				{
					string text = Encoding.UTF8.GetString(array);
					char[] trimChars = new char[1];
					return text.TrimEnd(trimChars);
				}
				string text2 = Encoding.Default.GetString(array);
				char[] trimChars2 = new char[1];
				return text2.TrimEnd(trimChars2);
			}
		}
	}

	/// <summary>
	/// U3V相机设备信息
	/// </summary>
	public struct MV_CODEREADER_USB3_DEVICE_INFO
	{
		/// <summary>
		/// 控制输入端点
		/// </summary>
		public byte CrtlInEndPoint;

		/// <summary>
		/// 控制输出端点
		/// </summary>
		public byte CrtlOutEndPoint;

		/// <summary>
		/// 流端点 
		/// </summary>                     
		public byte StreamEndPoint;

		/// <summary>
		/// 事件端点
		/// </summary>
		public byte EventEndPoint;

		/// <summary>
		/// 供应商ID号
		/// </summary>
		public ushort idVendor;

		/// <summary>
		/// 产品ID号
		/// </summary>
		public ushort idProduct;

		/// <summary>
		/// 设备序列号
		/// </summary>
		public uint nDeviceNumber;

		/// <summary>
		/// 设备GUID号
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string chDeviceGUID;

		/// <summary>
		/// 供应商名字
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string chVendorName;

		/// <summary>
		/// 型号名字
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string chModelName;

		/// <summary>
		/// 家族名字
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string chFamilyName;

		/// <summary>
		/// 设备版本号
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string chDeviceVersion;

		/// <summary>
		/// 制造商名字
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string chManufacturerName;

		/// <summary>
		/// 序列号
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string chSerialNumber;

		/// <summary>
		/// 用户自定义名字
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string chUserDefinedName;

		/// <summary>
		/// 支持的USB协议
		/// </summary>
		public uint nbcdUSB;

		/// <summary>
		/// 保留字节
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public uint[] nReserved;
	}

	/// <summary>
	/// 设备信息
	/// </summary>
	public struct MV_CODEREADER_DEVICE_INFO
	{
		/// <summary>
		/// 设备信息 
		/// </summary>
		[StructLayout(LayoutKind.Explicit, Size = 540)]
		public struct SPECIAL_INFO
		{
			/// <summary>
			/// GigE设备信息
			/// </summary>
			[FieldOffset(0)]
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 216)]
			public byte[] stGigEInfo;

			/// <summary>
			/// U3V设备信息
			/// </summary>
			[FieldOffset(0)]
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 540)]
			public byte[] stUsb3VInfo;
		}

		/// <summary>
		/// 设备主版本号
		/// </summary>
		public ushort nMajorVer;

		/// <summary>
		/// 设备次版本号
		/// </summary>
		public ushort nMinorVer;

		/// <summary>
		/// MAC 地址
		/// </summary>
		public uint nMacAddrHigh;

		/// <summary>
		/// 设备MAC地址低位
		/// </summary>
		public uint nMacAddrLow;

		/// <summary>
		/// 设备传输层协议类型，e.g. MV_GIGE_DEVICE
		/// </summary>
		public uint nTLayerType;

		/// <summary>
		/// 设备类型
		/// </summary>
		public uint nDeviceType;

		/// <summary>
		/// 选择设备
		/// 是否为指定系列型号相机
		/// true -指定系列型号相机 false- 非指定系列型号相机
		/// </summary>
		public bool bSelectDevice;

		/// <summary>
		/// 保留字节
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public uint[] nReserved;

		/// <summary>
		/// 设备信息 
		/// </summary>
		public SPECIAL_INFO SpecialInfo;
	}

	/// <summary>
	/// 设备列表信息
	/// </summary>
	public struct MV_CODEREADER_DEVICE_INFO_LIST
	{
		/// <summary>
		/// 在线设备数量
		/// </summary>
		public uint nDeviceNum;

		/// <summary>
		/// 支持最多256个设备
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		public IntPtr[] pDeviceInfo;
	}

	/// <summary>
	/// 图像数据信息
	/// </summary>
	public struct MV_CODEREADER_FRAME_OUT_INFO
	{
		/// <summary>
		/// 图像宽
		/// </summary>
		public ushort nWidth;

		/// <summary>
		/// 图像高
		/// </summary>
		public ushort nHeight;

		/// <summary>
		/// 像素格式
		/// </summary>
		public MvCodeReaderGvspPixelType enPixelType;

		/// <summary>
		/// 帧号
		/// </summary>
		public uint nFrameNum;

		/// <summary>
		/// 时间戳高32位
		/// </summary>
		public uint nDevTimeStampHigh;

		/// <summary>
		/// 时间戳低32位
		/// </summary>
		public uint nDevTimeStampLow;

		/// <summary>
		/// 保留，8字节对齐
		/// </summary>
		public uint nReserved0;

		/// <summary>
		/// 主机生成的时间戳
		/// </summary>
		public long nHostTimeStamp;

		/// <summary>
		/// 当前帧数据大小 
		/// </summary>
		public uint nFrameLen;

		/// <summary>
		/// 本帧丢包数 
		/// </summary>
		public uint nLostPacket;

		/// <summary>
		/// 保留字节
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public uint[] nReserved;
	}

	/// <summary>
	/// 图像数据信息扩展（包含水印信息）
	/// </summary>
	public struct MV_CODEREADER_FRAME_OUT_INFO_EX
	{
		/// <summary>
		/// 图像宽
		/// </summary>
		public ushort nWidth;

		/// <summary>
		/// 图像高
		/// </summary>
		public ushort nHeight;

		/// <summary>
		/// 像素格式
		/// </summary>
		public MvCodeReaderGvspPixelType enPixelType;

		/// <summary>
		/// 帧号
		/// </summary>
		public uint nFrameNum;

		/// <summary>
		/// 时间戳高32位
		/// </summary>
		public uint nDevTimeStampHigh;

		/// <summary>
		/// 时间戳低32位
		/// </summary>
		public uint nDevTimeStampLow;

		/// <summary>
		/// 保留，8字节对齐
		/// </summary>
		public uint nReserved0;

		/// <summary>
		/// 主机生成的时间戳
		/// </summary>
		public long nHostTimeStamp;

		/// <summary>
		/// 当前帧数据大小
		/// </summary>
		public uint nFrameLen;

		/// 以下为chunk新增水印信息
		/// 设备水印时标
		/// <summary>
		/// 秒数
		/// </summary>
		public uint nSecondCount;

		/// <summary>
		/// 循环计数
		/// </summary>
		public uint nCycleCount;

		/// <summary>
		/// 循环计数偏移量
		/// </summary>
		public uint nCycleOffset;

		/// <summary>
		/// 增益
		/// </summary>
		public float fGain;

		/// <summary> 
		/// 曝光时间
		/// </summary>
		public float fExposureTime;

		/// <summary>
		/// ch:平均亮度
		/// </summary>
		public uint nAverageBrightness;

		/// 白平衡相关
		/// <summary>
		/// ch:红色数据
		/// </summary>
		public uint nRed;

		/// <summary>
		/// ch:绿色数据
		/// </summary>
		public uint nGreen;

		/// <summary>
		/// ch:蓝色数据
		/// </summary>
		public uint nBlue;

		/// <summary>
		/// ch:图像数量计数
		/// </summary>
		public uint nFrameCounter;

		/// <summary>
		/// ch:触发计数
		/// </summary>
		public uint nTriggerIndex;

		/// Line 输入/输出
		///  <summary>
		///  ch:输入
		///  </summary>
		public uint nInput;

		/// <summary>
		/// ch:输出
		/// </summary>
		public uint nOutput;

		/// ROI区域
		/// <summary>
		/// ch:ROI X轴偏移
		/// </summary>
		public ushort nOffsetX;

		/// <summary>
		///  ch:ROI Y轴偏移
		/// </summary>
		public ushort nOffsetY;

		/// <summary>
		/// ch:Chunk宽度
		/// </summary>
		public ushort nChunkWidth;

		/// <summary>
		/// ch:Chunk高度
		/// </summary>
		public ushort nChunkHeight;

		/// <summary>
		/// ch:本帧丢包数
		/// </summary>
		public uint nLostPacket;

		/// <summary>
		/// ch:保留字节
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 39)]
		public uint[] nReserved;
	}

	/// <summary>
	/// 图像显示信息
	/// </summary>
	public struct MV_CODEREADER_DISPLAY_FRAME_INFO
	{
		/// <summary>
		/// 显示窗口句柄
		/// </summary>
		public IntPtr hWnd;

		/// <summary>
		/// 图像数据
		/// </summary>
		public IntPtr pData;

		/// <summary>
		/// 源图像数据长度
		/// </summary>
		public uint nDataLen;

		/// <summary>
		/// 图像宽
		/// </summary>
		public ushort nWidth;

		/// <summary>
		/// 图像高
		/// </summary>
		public ushort nHeight;

		/// <summary>
		/// 像素格式
		/// </summary>
		public MvCodeReaderGvspPixelType enPixelType;

		/// <summary>
		/// 保留字节
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public uint[] nReserved;
	}

	/// <summary>
	/// 保存图像格式信息
	/// </summary>
	public enum MV_CODEREADER_IAMGE_TYPE
	{
		/// <summary>
		/// 未定义格式
		/// </summary>
		MV_CODEREADER_Image_Undefined,
		/// <summary>
		/// Mono8格式
		/// </summary>
		MV_CODEREADER_Image_Mono8,
		/// <summary>
		/// Jpeg格式
		/// </summary>
		MV_CODEREADER_Image_Jpeg,
		/// <summary>
		/// Bmp格式
		/// </summary>
		MV_CODEREADER_Image_Bmp,
		/// <summary>
		/// RGB24格式
		/// </summary>
		MV_CODEREADER_Image_RGB24,
		/// <summary>
		/// Png图像(暂不支持)
		/// </summary>
		MV_CODEREADER_Image_Png,
		/// <summary>
		/// Tif图像(暂不支持)
		/// </summary>
		MV_CODEREADER_Image_Tif
	}

	/// <summary>
	/// 图像保存信息
	/// </summary>
	public struct MV_CODEREADER_SAVE_IMAGE_PARAM
	{
		/// <summary>
		/// [IN]     输入数据缓存
		/// </summary>
		public IntPtr pData;

		/// <summary>
		/// [IN]     输入数据大小
		/// </summary>
		public uint nDataLen;

		/// <summary>
		/// [IN]     输入数据的像素格式
		/// </summary>
		public MvCodeReaderGvspPixelType enPixelType;

		/// <summary>
		/// [IN]     图像宽
		/// </summary>
		public ushort nWidth;

		/// <summary>
		/// [IN]     图像高
		/// </summary>
		public ushort nHeight;

		/// <summary>
		/// [OUT]    输出图片缓存
		/// </summary>
		public IntPtr pImageBuffer;

		/// <summary>
		/// [OUT]    输出图片大小
		/// </summary>
		public uint nImageLen;

		/// <summary>
		/// [IN]     提供的输出缓冲区大小
		/// </summary>
		public uint nBufferSize;

		/// <summary>
		/// [IN]     输出图片格式
		/// </summary>
		public MV_CODEREADER_IAMGE_TYPE enImageType;
	}

	/// <summary>
	/// 图像保存信息扩展
	/// </summary>
	public struct MV_CODEREADER_SAVE_IMAGE_PARAM_EX
	{
		/// <summary>
		/// [IN]     输入数据缓存
		/// </summary>
		public IntPtr pData;

		/// <summary>
		/// [IN]     输入数据大小
		/// </summary>
		public uint nDataLen;

		/// <summary>
		/// [IN]     输入数据的像素格式
		/// </summary>
		public MvCodeReaderGvspPixelType enPixelType;

		/// <summary>
		/// [IN]     图像宽
		/// </summary>
		public ushort nWidth;

		/// <summary>
		/// [IN]     图像高
		/// </summary>
		public ushort nHeight;

		/// <summary>
		/// [OUT]    输出图片缓存
		/// </summary>
		public IntPtr pImageBuffer;

		/// <summary>
		/// [OUT]    输出图片大小
		/// </summary>
		public uint nImageLen;

		/// <summary>
		/// [IN]     提供的输出缓冲区大小
		/// </summary>
		public uint nBufferSize;

		/// <summary>
		/// [IN]     输出图片格式
		/// </summary>
		public MV_CODEREADER_IAMGE_TYPE enImageType;

		/// <summary>
		/// [IN]     编码质量, (50-99]
		/// </summary>
		public uint nJpgQuality;

		/// <summary>
		/// [IN]     Bayer格式转为RGB24的插值方法  0-最近邻 1-双线性 2-Hamilton
		/// </summary>
		public uint iMethodValue;

		/// <summary>
		/// 保留字节
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public uint[] nReserved;
	}

	/// <summary>
	/// 触发模式
	/// </summary>
	public enum MV_CODEREADER_TRIGGER_MODE
	{
		/// <summary>
		/// 触发模式关闭
		/// </summary>
		MV_CODEREADER_TRIGGER_MODE_OFF,
		/// <summary>
		/// 触发模式打开
		/// </summary>
		MV_CODEREADER_TRIGGER_MODE_ON
	}

	/// <summary>
	/// 触发源
	/// </summary>
	public enum MV_CODEREADER_TRIGGER_SOURCE
	{
		/// <summary>
		/// Line0 
		/// </summary>
		MV_CODEREADER_TRIGGER_SOURCE_LINE0 = 0,
		/// <summary>
		/// Line1
		/// </summary>
		MV_CODEREADER_TRIGGER_SOURCE_LINE1 = 1,
		/// <summary>
		/// Line2
		/// </summary>
		MV_CODEREADER_TRIGGER_SOURCE_LINE2 = 2,
		/// <summary>
		/// Line3
		/// </summary>
		MV_CODEREADER_TRIGGER_SOURCE_LINE3 = 3,
		/// <summary>
		/// 计数器0触发
		/// </summary>
		MV_CODEREADER_TRIGGER_SOURCE_COUNTER0 = 4,
		/// <summary>
		/// Tcp服务器触发
		/// </summary>
		MV_CODEREADER_TRIGGER_SOURCE_TCPSERVERSTART = 5,
		/// <summary>
		/// Udp触发
		/// </summary>
		MV_CODEREADER_TRIGGER_SOURCE_UDPSTART = 6,
		/// <summary>
		/// 软触发
		/// </summary>
		MV_CODEREADER_TRIGGER_SOURCE_SOFTWARE = 7,
		/// <summary>
		/// 串口触发
		/// </summary>
		MV_CODEREADER_TRIGGER_SOURCE_SERIALSTART = 8,
		/// <summary>
		/// 自触发
		/// </summary>
		MV_CODEREADER_TRIGGER_SOURCE_SELFTRIGGER = 9,
		/// <summary>
		/// 主从触发
		/// </summary>
		MV_CODEREADER_TRIGGER_SOURCE_MAINSUB = 10,
		/// <summary>
		/// Tcp客户端触发
		/// </summary>
		MV_CODEREADER_TRIGGER_SOURCE_TCPCLIENTSTART = 12
	}

	/// <summary>
	/// 条码类型信息
	/// </summary>
	public enum MV_CODEREADER_CODE_TYPE
	{
		/// <summary>
		/// 无可识别条码
		/// </summary>
		MV_CODEREADER_CODE_NONE = 0,
		/// <summary>
		/// DM码
		/// </summary>
		MV_CODEREADER_TDCR_DM = 1,
		/// <summary>
		/// QR码
		/// </summary>
		MV_CODEREADER_TDCR_QR = 2,
		/// <summary>
		/// MicroQR码
		/// </summary>
		MV_CODEREADER_TDCR_MICROQR = 140,
		/// 一维码
		/// <summary>
		/// EAN8码
		/// </summary>
		MV_CODEREADER_BCR_EAN8 = 8,
		/// <summary>
		/// UPCE码
		/// </summary>
		MV_CODEREADER_BCR_UPCE = 9,
		/// <summary>
		/// UPCA码
		/// </summary>
		MV_CODEREADER_BCR_UPCA = 12,
		/// <summary>
		/// EAN13码
		/// </summary>
		MV_CODEREADER_BCR_EAN13 = 13,
		/// <summary>
		/// ISBN13码
		/// </summary>
		MV_CODEREADER_BCR_ISBN13 = 14,
		/// <summary>
		/// 库德巴码
		/// </summary>
		MV_CODEREADER_BCR_CODABAR = 20,
		/// <summary>
		/// 交叉25码
		/// </summary>
		MV_CODEREADER_BCR_ITF25 = 25,
		/// <summary>
		/// Code 39
		/// </summary>
		MV_CODEREADER_BCR_CODE39 = 39,
		/// <summary>
		/// Code 93
		/// </summary>
		MV_CODEREADER_BCR_CODE93 = 93,
		/// <summary>
		/// Code 128
		/// </summary>
		MV_CODEREADER_BCR_CODE128 = 128,
		/// <summary>
		/// PDF417码
		/// </summary>
		MV_CODEREADER_TDCR_PDF417 = 131,
		/// <summary>
		/// MATRIX25码
		/// </summary>
		MV_CODEREADER_BCR_MATRIX25 = 26,
		/// <summary>
		/// MSI码
		/// </summary>
		MV_CODEREADER_BCR_MSI = 30,
		/// <summary>
		/// code11
		/// </summary>
		MV_CODEREADER_BCR_CODE11 = 31,
		/// <summary>
		/// industrial25
		/// </summary>
		MV_CODEREADER_BCR_INDUSTRIAL25 = 32,
		/// <summary>
		/// 中国邮政码
		/// </summary>
		MV_CODEREADER_BCR_CHINAPOST = 33,
		/// <summary>
		/// 交叉14码
		/// </summary>
		MV_CODEREADER_BCR_ITF14 = 27,
		/// <summary>
		/// Pharmacode码
		/// </summary>
		MV_CODEREADER_BCR_PHARMACODE = 36,
		/// <summary>
		/// Pharmacode Two Track码
		/// </summary>
		MV_CODEREADER_BCR_PHARMACODE2D = 37,
		/// <summary>
		/// ECC140码制
		/// </summary>
		MV_CODEREADER_TDCR_ECC140 = 133,
		/// <summary>
		/// AZTEC码制
		/// </summary>
		MV_CODEREADER_TDCR_AZTEC = 132,
		/// <summary>
		/// HANXIN码制
		/// </summary>
		MV_CODEREADER_TDCR_HANXIN = 145
	}

	/// <summary>
	/// 节点访问模式
	/// </summary>
	public enum MV_CODEREADER_XML_AccessMode
	{
		/// <summary>
		/// 节点未实现
		/// </summary>
		MV_CODEREADER_AM_NI,
		/// <summary>
		/// 节点不可达
		/// </summary>
		MV_CODEREADER_AM_NA,
		/// <summary>
		/// 节点只写
		/// </summary>
		MV_CODEREADER_AM_WO,
		/// <summary>
		/// 节点只读
		/// </summary>
		MV_CODEREADER_AM_RO,
		/// <summary>
		/// 节点可读可写
		/// </summary>
		MV_CODEREADER_AM_RW,
		/// <summary>
		/// 节点未定义
		/// </summary>
		MV_CODEREADER_AM_Undefined,
		/// <summary>
		/// 节点需周期检测 
		/// </summary>
		MV_CODEREADER_AM_CycleDetect
	}

	/// <summary>
	/// 每个节点对那个的接口类型
	/// </summary>
	public enum MV_CODEREADER_XML_InterfaceType
	{
		/// <summary>
		/// Value类型值
		/// </summary>
		MV_CODEREADER_IFT_IValue,
		/// <summary>
		/// Base类型值
		/// </summary>
		MV_CODEREADER_IFT_IBase,
		/// <summary>
		/// Integer类型值
		/// </summary>
		MV_CODEREADER_IFT_IInteger,
		/// <summary>
		/// Boolean类型值
		/// </summary>
		MV_CODEREADER_IFT_IBoolean,
		/// <summary>
		/// Command类型值
		/// </summary>
		MV_CODEREADER_IFT_ICommand,
		/// <summary>
		/// Float类型值
		/// </summary>
		MV_CODEREADER_IFT_IFloat,
		/// <summary>
		/// String类型值
		/// </summary>
		MV_CODEREADER_IFT_IString,
		/// <summary>
		/// Register类型值
		/// </summary>
		MV_CODEREADER_IFT_IRegister,
		/// <summary>
		/// Category类型值
		/// </summary>
		MV_CODEREADER_IFT_ICategory,
		/// <summary>
		/// Enumeration类型值
		/// </summary>
		MV_CODEREADER_IFT_IEnumeration,
		/// <summary>
		/// EnumEntry类型值
		/// </summary>
		MV_CODEREADER_IFT_IEnumEntry,
		/// <summary>
		/// Port类型值
		/// </summary>
		MV_CODEREADER_IFT_IPort
	}

	/// <summary>
	/// Event事件信息 
	/// </summary>
	public struct MV_CODEREADER_EVENT_OUT_INFO
	{
		/// <summary>
		/// Event名称
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string EventName;

		/// <summary>
		/// Event号
		/// </summary>
		public ushort nEventID;

		/// <summary>
		/// 流通道序号
		/// </summary>
		public ushort nStreamChannel;

		/// <summary>
		/// 帧号高位
		/// </summary>
		public uint nBlockIdHigh;

		/// <summary>
		/// 帧号低位
		/// </summary>
		public uint nBlockIdLow;

		/// <summary>
		/// 时间戳高位
		/// </summary>
		public uint nTimestampHigh;

		/// <summary>
		/// 时间戳低位
		/// </summary>
		public uint nTimestampLow;

		/// <summary>
		/// Event数据
		/// </summary>
		public IntPtr pEventData;

		/// <summary>
		/// Event数据长度
		/// </summary>
		public uint nEventDataSize;

		/// <summary>
		/// 保留字节
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public uint[] nReserved;
	}

	/// <summary>
	/// 文件存取
	/// </summary>
	public struct MV_CODEREADER_FILE_ACCESS
	{
		/// <summary>
		/// 用户文件名
		/// </summary>
		public string pUserFileName;

		/// <summary>
		/// 设备文件名
		/// </summary>
		public string pDevFileName;

		/// <summary>
		/// 保留字节
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		public uint[] nReserved;
	}

	/// <summary>
	/// 文件存取进度
	/// </summary>
	public struct MV_CODEREADER_FILE_ACCESS_PROGRESS
	{
		/// <summary>
		/// 已完成的长度
		/// </summary>
		public long nCompleted;

		/// <summary>
		/// 总长度
		/// </summary>
		public long nTotal;

		/// <summary>
		/// 保留字节
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public uint[] nReserved;
	}

	/// <summary>
	/// Int类型值（以unsigned int为类型值）
	/// </summary>
	public struct MV_CODEREADER_INTVALUE
	{
		/// <summary>
		/// 当前值
		/// </summary>
		public uint nCurValue;

		/// <summary>
		/// 最大值
		/// </summary>
		public uint nMax;

		/// <summary>
		/// 最小值
		/// </summary>
		public uint nMin;

		/// <summary>
		/// 增量值
		/// </summary>
		public uint nInc;

		/// <summary>
		/// 保留字节
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public uint[] nReserved;
	}

	/// <summary>
	/// Int类型值（以int64_t为类型值）
	/// </summary>
	public struct MV_CODEREADER_INTVALUE_EX
	{
		/// <summary>
		/// 当前值
		/// </summary>
		public long nCurValue;

		/// <summary>
		/// 最大值
		/// </summary>
		public long nMax;

		/// <summary>
		/// 最小值
		/// </summary>
		public long nMin;

		/// <summary>
		/// 增量值
		/// </summary>
		public long nInc;

		/// <summary>
		/// 保留字节
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public uint[] nReserved;
	}

	/// <summary>
	/// Float类型值
	/// </summary>
	public struct MV_CODEREADER_FLOATVALUE
	{
		/// <summary>
		/// 当前值
		/// </summary>
		public float fCurValue;

		/// <summary>
		/// 最大值
		/// </summary>
		public float fMax;

		/// <summary>
		/// 最小值
		/// </summary>
		public float fMin;

		/// <summary>
		/// 保留字节
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public uint[] nReserved;
	}

	/// <summary>
	/// Enum类型值
	/// </summary>
	public struct MV_CODEREADER_ENUMVALUE
	{
		/// <summary>
		/// 当前值
		/// </summary>
		public uint nCurValue;

		/// <summary>
		/// 有效数据个数
		/// </summary>
		public uint nSupportedNum;

		/// <summary>
		/// 支持的枚举类型
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		public uint[] nSupportValue;

		/// <summary>
		/// 保留字节
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public uint[] nReserved;
	}

	/// <summary>
	/// String类型值
	/// </summary>
	public struct MV_CODEREADER_STRINGVALUE
	{
		/// <summary>
		/// 当前值
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string chCurValue;

		/// <summary>
		/// 最大长度
		/// </summary>
		public long nMaxLength;

		/// <summary>
		/// 保留字节
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public uint[] nReserved;
	}

	/// <summary>
	/// Int型坐标
	/// </summary>
	public struct MV_CODEREADER_POINT_I
	{
		/// <summary>
		/// x坐标
		/// </summary>
		public int x;

		/// <summary>
		/// y坐标
		/// </summary>
		public int y;
	}

	/// <summary>
	/// Float型坐标
	/// </summary>
	public struct MV_CODEREADER_POINT_F
	{
		/// <summary>
		/// x坐标
		/// </summary>
		public float x;

		/// <summary>
		/// y坐标
		/// </summary>
		public float y;
	}

	/// <summary>
	/// 条码信息结构体定义
	/// </summary>
	public struct MV_CODEREADER_BCR_INFO
	{
		/// <summary>
		/// 条码ID
		/// </summary>
		public uint nID;

		/// <summary>
		/// 字符
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string chCode;

		/// <summary>
		/// 字符长度
		/// </summary>
		public uint nLen;

		/// <summary>
		/// 条码类型
		/// </summary>
		public uint nBarType;

		/// <summary>
		/// 条码位置
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public MV_CODEREADER_POINT_I[] pt;

		/// <summary>
		/// 条码角度(10倍)（0~3600）
		/// </summary>
		public int nAngle;

		/// <summary>
		/// 主包ID
		/// </summary>
		public uint nMainPackageId;

		/// <summary>
		/// 次包ID
		/// </summary>
		public uint nSubPackageId;

		/// <summary>
		/// 条码被识别的次数
		/// </summary>
		public ushort sAppearCount;

		/// <summary>
		/// PPM(10倍)
		/// </summary>
		public ushort sPPM;

		/// <summary>
		/// 算法耗时
		/// </summary>
		public ushort sAlgoCost;

		/// <summary>
		/// 图像清晰度(10倍)
		/// </summary>
		public ushort sSharpness;
	}

	/// <summary>
	/// 条码质量 质量分5等[0,4], 越高等质量越好; 1D指一维码，2D指二维码）
	/// </summary>
	public struct MV_CODEREADER_CODE_INFO
	{
		/// 等级
		/// <summary>
		/// 总体质量评分（1D/2D公用）
		/// </summary>
		public int nOverQuality;

		/// <summary>
		/// 译码评分（1D/2D公用）
		/// </summary>
		public int nDeCode;

		/// <summary>
		/// Symbol Contrast对比度质量评分（1D/2D公用）
		/// </summary>
		public int nSCGrade;

		/// <summary>
		/// modulation模块均匀性评分（1D/2D公用）
		/// </summary>
		public int nModGrade;

		/// 2D等级
		/// <summary>
		/// fixed_pattern_damage评分
		/// </summary>
		public int nFPDGrade;

		/// <summary>
		/// axial_nonuniformity码轴规整性评分
		/// </summary>
		public int nANGrade;

		/// <summary>
		/// grid_nonuniformity基础grid均匀性质量评分
		/// </summary>
		public int nGNGrade;

		/// <summary>
		/// unused_error_correction未使用纠错功能评分
		/// </summary>
		public int nUECGrade;

		/// <summary>
		/// Print Growth Horizontal 打印伸缩(水平)评分
		/// </summary>
		public int nPGHGrade;

		/// <summary>
		/// Print Growth Veritical 打印伸缩(垂直)评分
		/// </summary>
		public int nPGVGrade;

		/// 分数
		/// <summary>
		/// Symbol Contrast对比度质量分数（1D/2D公用）
		/// </summary>
		public float fSCScore;

		/// <summary>
		/// modulation模块均匀性分数（1D/2D公用）
		/// </summary>
		public float fModScore;

		/// 2D分数
		/// <summary>
		/// fixed_pattern_damage分数
		/// </summary>
		public float fFPDScore;

		/// <summary>
		/// axial_nonuniformity码轴规整性分数
		/// </summary>
		public float fAnScore;

		/// <summary>
		/// grid_nonuniformity基础grid均匀性质量分数
		/// </summary>
		public float fGNScore;

		/// <summary>
		/// unused_error_correction未使用纠错功能分数
		/// </summary>
		public float fUECScore;

		/// <summary>
		/// Print Growth Horizontal 打印伸缩(水平)分数
		/// </summary>
		public float fPGHScore;

		/// <summary>
		/// Print Growth Veritical 打印伸缩(垂直)分数
		/// </summary>
		public float fPGVScore;

		/// <summary>
		/// reflectance margin反射率余量评分
		/// </summary>
		public int nRMGrade;

		/// <summary>
		/// reflectance margin反射率余量分数
		/// </summary>
		public float fRMScore;

		/// 1D等级
		/// <summary>
		/// edge determination     边缘确定度质量等级
		/// </summary>
		public int n1DEdgeGrade;

		/// <summary>
		/// minimum reflectance    最小反射率质量等级
		/// </summary>
		public int n1DMinRGrade;

		/// <summary>
		/// minimum edge contrast  最小边缘对比度质量等级
		/// </summary>
		public int n1DMinEGrade;

		/// <summary>
		/// decodability           可译码性质量等级 
		/// </summary>
		public int n1DDcdGrade;

		/// <summary>
		/// defects                缺陷质量等级
		/// </summary>
		public int n1DDefGrade;

		/// <summary>
		/// quiet zone             静区质量等级
		/// </summary>
		public int n1DQZGrade;

		/// 1D分数
		/// /// <summary>
		/// edge determination     边缘确定度分数
		/// </summary>
		public float f1DEdgeScore;

		/// <summary>
		/// minimum reflectance    最小反射率分数
		/// </summary>
		public float f1DMinRScore;

		/// <summary>
		/// minimum edge contrast  最小边缘对比度分数
		/// </summary>
		public float f1DMinEScore;

		/// <summary>
		/// decodability           可译码性分数
		/// </summary>
		public float f1DDcdScore;

		/// <summary>
		/// defects                缺陷分数
		/// </summary>
		public float f1DDefScore;

		/// <summary>
		/// quiet zone             静区分数
		/// </summary>
		public float f1DQZScore;

		/// <summary>
		/// 保留字节
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 18)]
		public int[] nReserved;
	}

	/// <summary>
	/// 带质量信息的BCR信息
	/// </summary>
	public struct MV_CODEREADER_BCR_INFO_EX
	{
		/// <summary>
		/// 条码ID
		/// </summary>
		public uint nID;

		/// <summary>
		/// 字符
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		public byte[] chCode;

		/// <summary>
		/// 字符长度
		/// </summary>
		public uint nLen;

		/// <summary>
		/// 条码类型
		/// </summary>
		public uint nBarType;

		/// <summary>
		/// 条码位置
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public MV_CODEREADER_POINT_I[] pt;

		/// <summary>
		/// 条码质量评价
		/// </summary>
		public MV_CODEREADER_CODE_INFO stCodeQuality;

		/// <summary>
		/// 条码角度(10倍)（0~3600）
		/// </summary>
		public int nAngle;

		/// <summary>
		/// 主包ID
		/// </summary>
		public uint nMainPackageId;

		/// <summary>
		/// 次包ID
		/// </summary>
		public uint nSubPackageId;

		/// <summary>
		/// 条码被识别的次数
		/// </summary>
		public ushort sAppearCount;

		/// <summary>
		/// PPM(10倍)
		/// </summary>
		public ushort sPPM;

		/// <summary>
		/// 算法耗时
		/// </summary>
		public ushort sAlgoCost;

		/// <summary>
		/// 图像清晰度(10倍)
		/// </summary>
		public ushort sSharpness;

		/// <summary>
		/// 是否支持二维码质量评级
		/// </summary>
		public bool bIsGetQuality;

		/// <summary>
		/// 读码评分
		/// </summary>
		public uint nIDRScore;

		/// <summary>
		/// 是否支持一维码质量评级
		/// </summary>
		public uint n1DIsGetQuality;

		/// <summary>
		/// 从触发开始到APP输出时间统计(ms)
		/// </summary>
		public uint nTotalProcCost;

		/// <summary>
		/// 触发开始时间高32位(s)
		/// </summary>
		public uint nTriggerTimeTvHigh;

		/// <summary>
		/// 触发开始时间低32位(s)
		/// </summary>
		public uint nTriggerTimeTvLow;

		/// <summary>
		/// 触发开始时间高32位(us)
		/// </summary>
		public uint nTriggerTimeUtvHigh;

		/// <summary>
		/// 触发开始时间低32位(us)
		/// </summary>
		public uint nTriggerTimeUtvLow;

		/// <summary>
		/// 保留字节
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
		public uint[] nReserved;
	}

	/// <summary>
	/// 带质量信息且条码字符扩展的BCR信息
	/// </summary>
	public struct MV_CODEREADER_BCR_INFO_EX2
	{
		/// <summary>
		/// 条码ID
		/// </summary>
		public uint nID;

		/// <summary>
		/// 字符可识别长度扩展至4096
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4096)]
		public byte[] chCode;

		/// <summary>
		/// 字符长度
		/// </summary>
		public uint nLen;

		/// <summary>
		/// 条码类型
		/// </summary>
		public uint nBarType;

		/// <summary>
		/// 条码位置
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public MV_CODEREADER_POINT_I[] pt;

		/// <summary>
		/// 条码质量评价
		/// </summary>
		public MV_CODEREADER_CODE_INFO stCodeQuality;

		/// <summary>
		/// 条码角度(10倍)（0~3600）
		/// </summary>
		public int nAngle;

		/// <summary>
		/// 主包ID
		/// </summary>
		public uint nMainPackageId;

		/// <summary>
		/// 次包ID
		/// </summary>
		public uint nSubPackageId;

		/// <summary>
		/// 条码被识别的次数
		/// </summary>
		public ushort sAppearCount;

		/// <summary>
		/// PPM(10倍)
		/// </summary>
		public ushort sPPM;

		/// <summary>
		/// 算法耗时
		/// </summary>
		public ushort sAlgoCost;

		/// <summary>
		/// 图像清晰度(10倍)
		/// </summary>
		public ushort sSharpness;

		/// <summary>
		/// 是否支持二维码质量评级
		/// </summary>
		public bool bIsGetQuality;

		/// <summary>
		/// 读码评分
		/// </summary>
		public uint nIDRScore;

		/// <summary>
		/// 是否支持一维码质量评级
		/// </summary>
		public uint n1DIsGetQuality;

		/// <summary>
		/// 从触发开始到APP输出时间统计(ms)
		/// </summary>
		public uint nTotalProcCost;

		/// <summary>
		/// 触发开始时间高32位(s)
		/// </summary>
		public uint nTriggerTimeTvHigh;

		/// <summary>
		/// 触发开始时间低32位(s)
		/// </summary>
		public uint nTriggerTimeTvLow;

		/// <summary>
		/// 触发开始时间高32位(us)
		/// </summary>
		public uint nTriggerTimeUtvHigh;

		/// <summary>
		/// 触发开始时间低32位(us)
		/// </summary>
		public uint nTriggerTimeUtvLow;

		/// <summary>
		/// 保留字节
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 59)]
		public int[] nReserved;
	}

	/// <summary>
	/// ScResType_BCR 对应的结构体
	/// </summary>
	public struct MV_CODEREADER_RESULT_BCR
	{
		/// <summary>
		/// 条码数量
		/// </summary>
		public uint nCodeNum;

		/// <summary>
		/// 条码信息
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 200)]
		public MV_CODEREADER_BCR_INFO[] stBcrInfo;

		/// <summary>
		/// 保留字节
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public uint[] nReserved;
	}

	/// <summary>
	/// 条码信息加条码质量列表
	/// </summary>
	public struct MV_CODEREADER_RESULT_BCR_EX
	{
		/// <summary>
		/// 条码数量
		/// </summary>
		public uint nCodeNum;

		/// <summary>
		/// 条码信息
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 200)]
		public MV_CODEREADER_BCR_INFO_EX[] stBcrInfoEx;

		/// <summary>
		/// 保留字节
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public uint[] nReserved;
	}

	/// <summary>
	/// 条码信息字符扩展加条码质量列表
	/// </summary>
	public struct MV_CODEREADER_RESULT_BCR_EX2
	{
		/// <summary>
		/// 条码数量
		/// </summary>
		public uint nCodeNum;

		/// <summary>
		/// 条码信息
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 300)]
		public MV_CODEREADER_BCR_INFO_EX2[] stBcrInfoEx2;

		/// <summary>
		/// 保留字节
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public uint[] nReserved;
	}

	/// <summary>
	/// 输出帧信息结构体定义
	/// </summary>
	public struct MV_CODEREADER_IMAGE_OUT_INFO
	{
		/// <summary>
		/// 图像宽
		/// </summary>
		public ushort nWidth;

		/// <summary>
		/// 图像高
		/// </summary>
		public ushort nHeight;

		/// <summary>
		/// 像素或图片格式
		/// </summary>
		public MvCodeReaderGvspPixelType enPixelType;

		/// <summary>
		/// 触发序号（仅在电平触发时有效）
		/// </summary>
		public uint nTriggerIndex;

		/// <summary>
		/// 帧号
		/// </summary>
		public uint nFrameNum;

		/// <summary>
		/// 当前帧数据大小
		/// </summary>
		public uint nFrameLen;

		/// <summary>
		/// 时间戳高32位
		/// </summary>
		public uint nTimeStampHigh;

		/// <summary>
		/// 时间戳低32位
		/// </summary>
		public uint nTimeStampLow;

		/// <summary>
		/// 输出的消息类型
		/// </summary>
		public uint nResultType;

		/// <summary>
		/// 条码位置
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 65536)]
		public byte[] chResult;

		/// <summary>
		/// 是否读到条码
		/// </summary>
		public bool bIsGetCode;

		/// <summary>
		/// 面单图像
		/// </summary>
		public IntPtr pImageWaybill;

		/// <summary>
		/// 面单数据大小
		/// </summary>
		public uint nImageWaybillLen;

		/// <summary>
		/// 面单图像类型
		/// </summary>
		public MV_CODEREADER_IAMGE_TYPE enWaybillImageType;

		/// <summary>
		/// 是否误触发
		/// </summary>
		public uint bFlaseTrigger;

		/// <summary>
		/// 聚焦得分
		/// </summary>
		public uint nFocusScore;

		/// <summary>
		/// 对应stream通道序号
		/// </summary>
		public uint nChannelID;

		/// <summary>
		/// 帧图像在相机内部的处理耗时
		/// </summary>
		public uint nImageCost;

		/// <summary>
		/// 整图标记，整图标记，输出整图时标记为1
		/// </summary>
		public ushort nWholeFlag;

		/// <summary>
		/// 保留字节
		/// </summary>
		public ushort nRes;

		/// <summary>
		/// 保留字节
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
		public uint[] nReserved;
	}

	/// <summary>
	/// 面单信息结构体定义
	/// </summary>
	public struct MV_CODEREADER_WAYBILL_INFO
	{
		/// 面单坐标信息
		/// <summary>
		/// 中心点列坐标
		/// </summary>
		public float fCenterX;

		/// <summary>
		/// 中心点行坐标
		/// </summary>
		public float fCenterY;

		/// <summary>
		/// 矩形宽度，宽度为长半轴
		/// </summary>
		public float fWidth;

		/// <summary>
		/// 矩形高度，高度为短半轴
		/// </summary>
		public float fHeight;

		/// <summary>
		/// 矩形角度
		/// </summary>
		public float fAngle;

		/// <summary>
		/// 置信度
		/// </summary>
		public float fConfidence;

		/// <summary>
		/// 面单图像
		/// </summary>
		public IntPtr pImageWaybill;

		/// <summary>
		/// 面单长度
		/// </summary>
		public uint nImageLen;

		/// <summary>
		/// 当前面单内的ocr行数
		/// </summary>
		public uint nOcrRowNum;

		/// <summary>
		/// 保留字节
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
		public uint[] nReserved;
	}

	/// <summary>
	/// 面单信息列表
	/// </summary>
	public struct MV_CODEREADER_WAYBILL_LIST
	{
		/// <summary>
		/// 面单数量
		/// </summary>
		public uint nWaybillNum;

		/// <summary>
		/// 面单图像类型，可选择bmp、raw、jpg输出
		/// </summary>
		public MV_CODEREADER_IAMGE_TYPE enWaybillImageType;

		/// <summary>
		/// 条码位置
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public MV_CODEREADER_WAYBILL_INFO[] stWaybillInfo;

		/// <summary>
		/// 所有面单内的ocr总行数 面单1(ocr)+面单2(ocr)+...
		/// </summary>
		public uint nOcrAllNum;

		/// <summary>
		/// 保留字节
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public uint[] nReserved;
	}

	/// <summary>
	/// OCR基础信息
	/// </summary>
	public struct MV_CODEREADER_OCR_ROW_INFO
	{
		/// <summary>
		/// OCR ID
		/// </summary>
		public uint nID;

		/// <summary>
		/// OCR字符实际真实长度
		/// </summary>
		public uint nOcrLen;

		/// <summary>
		/// 识别到的OCR字符
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		public byte[] chOcr;

		/// <summary>
		/// 字符行整体置信度
		/// </summary>
		public float fCharConfidence;

		/// <summary>
		/// 单行OCR中心点列坐标
		/// </summary>
		public uint nOcrRowCenterX;

		/// <summary>
		/// 单行OCR中心点行坐标
		/// </summary>
		public uint nOcrRowCenterY;

		/// <summary>
		/// 单行OCR矩形宽度，宽度为长半轴
		/// </summary>
		public uint nOcrRowWidth;

		/// <summary>
		/// 单行OCR矩形高度，高度为短半轴
		/// </summary>
		public uint nOcrRowHeight;

		/// <summary>
		/// 单行OCR矩形角度
		/// </summary>
		public float fOcrRowAngle;

		/// <summary>
		/// 单行OCR定位置信度
		/// </summary>
		public float fDeteConfidence;

		/// <summary>
		/// OCR算法耗时 单位ms
		/// </summary>
		public ushort sOcrAlgoCost;

		/// <summary>
		/// 预留
		/// </summary>
		public ushort sReserved;

		/// <summary>
		/// 保留字节
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)]
		public int[] nReserved;
	}

	/// <summary>
	/// OCR信息列表
	/// </summary>
	public struct MV_CODEREADER_OCR_INFO_LIST
	{
		/// <summary>
		/// 所有面单内的OCR总行数
		/// </summary>
		public uint nOCRAllNum;

		/// <summary>
		/// OCR行基础信息
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
		public MV_CODEREADER_OCR_ROW_INFO[] stOcrRowInfo;

		/// <summary>
		/// 保留字节
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public int[] nReserved;
	}

	/// <summary>
	/// 输出帧信息结构体定义
	/// </summary>
	public struct MV_CODEREADER_IMAGE_OUT_INFO_EX
	{
		[StructLayout(LayoutKind.Explicit)]
		public struct UNPARSED_OCR_LIST
		{
			/// <summary>
			/// OCR相关信息 对应结构体MV_CODEREADER_OCR_INFO_LIST
			/// </summary>
			[FieldOffset(0)]
			public IntPtr pstOcrList;

			[FieldOffset(0)]
			public long nAligning;
		}

		/// <summary>
		/// 图像宽
		/// </summary>
		public ushort nWidth;

		/// <summary>
		/// 图像高
		/// </summary>
		public ushort nHeight;

		/// <summary>
		/// 像素或图片格式
		/// </summary>
		public MvCodeReaderGvspPixelType enPixelType;

		/// <summary>
		/// 触发序号（仅在电平触发时有效）
		/// </summary>
		public uint nTriggerIndex;

		/// <summary>
		/// 帧号
		/// </summary>
		public uint nFrameNum;

		/// <summary>
		/// 当前帧数据大小
		/// </summary>
		public uint nFrameLen;

		/// <summary>
		/// 时间戳高32位
		/// </summary>
		public uint nTimeStampHigh;

		/// <summary>
		/// 时间戳低32位
		/// </summary>
		public uint nTimeStampLow;

		/// <summary>
		/// 是否误触发
		/// </summary>
		public uint bFlaseTrigger;

		/// <summary>
		/// 聚焦得分
		/// </summary>
		public uint nFocusScore;

		/// <summary>
		/// 是否读到条码
		/// </summary>
		public bool bIsGetCode;

		/// <summary>
		/// 条码信息结构体列表
		/// </summary>
		public IntPtr pstCodeList;

		/// <summary>
		/// 面单信息
		/// </summary>
		public IntPtr pstWaybillList;

		/// <summary>
		/// 事件ID
		/// </summary>
		public uint nEventID;

		/// <summary>
		/// 对应stream通道序号
		/// </summary>
		public uint nChannelID;

		/// <summary>
		/// 帧图像在相机内部的处理耗时
		/// </summary>
		public uint nImageCost;

		public UNPARSED_OCR_LIST UnparsedOcrList;

		/// <summary>
		/// 整图标记，输出整图时标记为1
		/// </summary>
		public ushort nWholeFlag;

		/// <summary>
		/// 保留字节
		/// </summary>
		public ushort nRes;

		/// <summary>
		/// 保留字节
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public uint[] nReserved;
	}

	/// <summary>
	/// 输出帧信息结构体定义
	/// </summary>
	public struct MV_CODEREADER_IMAGE_OUT_INFO_EX2
	{
		[StructLayout(LayoutKind.Explicit)]
		public struct UNPARSED_BCR_LIST
		{
			/// <summary>
			/// 条码信息（条码字符长度扩展）对应结构体MV_CODEREADER_RESULT_BCR_EX2
			/// </summary>
			[FieldOffset(0)]
			public IntPtr pstCodeListEx2;

			[FieldOffset(0)]
			public long nAligning;
		}

		[StructLayout(LayoutKind.Explicit)]
		public struct UNPARSED_OCR_LIST
		{
			/// <summary>
			/// OCR相关信息 对应结构体MV_CODEREADER_OCR_INFO_LIST
			/// </summary>
			[FieldOffset(0)]
			public IntPtr pstOcrList;

			[FieldOffset(0)]
			public long nAligning;
		}

		/// <summary>
		/// 图像宽
		/// </summary>
		public ushort nWidth;

		/// <summary>
		/// 图像高
		/// </summary>
		public ushort nHeight;

		/// <summary>
		/// 像素或图片格式
		/// </summary>
		public MvCodeReaderGvspPixelType enPixelType;

		/// <summary>
		/// 触发序号（仅在电平触发时有效）
		/// </summary>
		public uint nTriggerIndex;

		/// <summary>
		/// 帧号
		/// </summary>
		public uint nFrameNum;

		/// <summary>
		/// 当前帧数据大小
		/// </summary>
		public uint nFrameLen;

		/// <summary>
		/// 时间戳高32位
		/// </summary>
		public uint nTimeStampHigh;

		/// <summary>
		/// 时间戳低32位
		/// </summary>
		public uint nTimeStampLow;

		/// <summary>
		/// 是否误触发
		/// </summary>
		public uint bFlaseTrigger;

		/// <summary>
		/// 聚焦得分
		/// </summary>
		public uint nFocusScore;

		/// <summary>
		/// 是否读到条码
		/// </summary>
		public bool bIsGetCode;

		/// <summary>
		/// 条码信息和条码质量结构体列表
		/// </summary>
		public IntPtr pstCodeListEx;

		/// <summary>
		/// 面单信息
		/// </summary>
		public IntPtr pstWaybillList;

		/// <summary>
		/// 事件ID
		/// </summary>
		public uint nEventID;

		/// <summary>
		/// 对应stream通道序号
		/// </summary>
		public uint nChannelID;

		/// <summary>
		/// 帧图像在相机内部的处理耗时
		/// </summary>
		public uint nImageCost;

		public UNPARSED_BCR_LIST UnparsedBcrList;

		public UNPARSED_OCR_LIST UnparsedOcrList;

		/// <summary>
		/// 整图标记，输出整图时标记为1
		/// </summary>
		public ushort nWholeFlag;

		/// <summary>
		/// 保留字节
		/// </summary>
		public ushort nRes;

		/// <summary>
		/// 保留字节
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 25)]
		public uint[] nReserved;
	}

	/// <summary>
	/// 输出信息类型
	/// </summary>
	public enum MvCodeReaderType
	{
		/// <summary>
		/// 没有结果输出
		/// </summary>
		CodeReader_ResType_NULL,
		/// <summary>
		/// 输出信息为BCR  (对应结构体 MV_SC_RESULT_BCR)
		/// </summary>
		CodeReader_ResType_BCR
	}

	/// <summary>
	/// 触发信息
	/// </summary>
	public struct MV_CODEREADER_TRIGGER_INFO_DATA
	{
		/// <summary>
		/// 触发序号 即同步触发号
		/// </summary>
		public uint nTriggerIndex;

		/// <summary>
		/// 触发状态 （1开始 0结束）
		/// </summary>
		public uint nTriggerFlag;

		/// <summary>
		/// 当前的触发状态对应的时间戳（分高、低位传输各4个字节）
		/// </summary>
		/// <summary>
		/// 触发时间高4位
		/// </summary>
		public uint nTriggerTimeHigh;

		/// <summary>
		/// 触发时间低4位
		/// </summary>
		public uint nTriggerTimeLow;

		/// <summary>
		/// 原生触发号（相机自带的触发号）
		/// </summary>
		public uint nOriginalTrigger;

		/// <summary>
		/// 是否强制结束（0--正常结束 1--强制结束 属于相机内部机制主动传输 上层无法设置生效）
		/// </summary>
		public ushort nIsForceOver;

		/// <summary>
		/// 主从标记 1--主相机 0--从相机
		/// </summary>
		public ushort nIsMainCam;

		/// <summary>
		/// 主机生成的时间戳
		/// </summary>
		public long nHostTimeStamp;

		/// <summary>
		/// 保留字节
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
		public uint[] nReserved;
	}

	/// <summary>
	/// 源图像像素格式 
	/// </summary>
	public enum MvCodeReaderGvspPixelType
	{
		/// Undefined pixel type
		PixelType_CodeReader_Gvsp_Undefined = -1,
		/// Mono buffer format defines
		PixelType_CodeReader_Gvsp_Mono1p = 16842807,
		PixelType_CodeReader_Gvsp_Mono2p = 16908344,
		PixelType_CodeReader_Gvsp_Mono4p = 17039417,
		PixelType_CodeReader_Gvsp_Mono8 = 17301505,
		PixelType_CodeReader_Gvsp_Mono8_Signed = 17301506,
		PixelType_CodeReader_Gvsp_Mono10 = 17825795,
		PixelType_CodeReader_Gvsp_Mono10_Packed = 17563652,
		PixelType_CodeReader_Gvsp_Mono12 = 17825797,
		PixelType_CodeReader_Gvsp_Mono12_Packed = 17563654,
		PixelType_CodeReader_Gvsp_Mono14 = 17825829,
		PixelType_CodeReader_Gvsp_Mono16 = 17825799,
		/// Bayer buffer format defines 
		PixelType_CodeReader_Gvsp_BayerGR8 = 17301512,
		PixelType_CodeReader_Gvsp_BayerRG8 = 17301513,
		PixelType_CodeReader_Gvsp_BayerGB8 = 17301514,
		PixelType_CodeReader_Gvsp_BayerBG8 = 17301515,
		PixelType_CodeReader_Gvsp_BayerGR10 = 17825804,
		PixelType_CodeReader_Gvsp_BayerRG10 = 17825805,
		PixelType_CodeReader_Gvsp_BayerGB10 = 17825806,
		PixelType_CodeReader_Gvsp_BayerBG10 = 17825807,
		PixelType_CodeReader_Gvsp_BayerGR12 = 17825808,
		PixelType_CodeReader_Gvsp_BayerRG12 = 17825809,
		PixelType_CodeReader_Gvsp_BayerGB12 = 17825810,
		PixelType_CodeReader_Gvsp_BayerBG12 = 17825811,
		PixelType_CodeReader_Gvsp_BayerGR10_Packed = 17563686,
		PixelType_CodeReader_Gvsp_BayerRG10_Packed = 17563687,
		PixelType_CodeReader_Gvsp_BayerGB10_Packed = 17563688,
		PixelType_CodeReader_Gvsp_BayerBG10_Packed = 17563689,
		PixelType_CodeReader_Gvsp_BayerGR12_Packed = 17563690,
		PixelType_CodeReader_Gvsp_BayerRG12_Packed = 17563691,
		PixelType_CodeReader_Gvsp_BayerGB12_Packed = 17563692,
		PixelType_CodeReader_Gvsp_BayerBG12_Packed = 17563693,
		PixelType_CodeReader_Gvsp_BayerGR16 = 17825838,
		PixelType_CodeReader_Gvsp_BayerRG16 = 17825839,
		PixelType_CodeReader_Gvsp_BayerGB16 = 17825840,
		PixelType_CodeReader_Gvsp_BayerBG16 = 17825841,
		/// RGB Packed buffer format defines 
		PixelType_CodeReader_Gvsp_RGB8_Packed = 35127316,
		PixelType_CodeReader_Gvsp_BGR8_Packed = 35127317,
		PixelType_CodeReader_Gvsp_RGBA8_Packed = 35651606,
		PixelType_CodeReader_Gvsp_BGRA8_Packed = 35651607,
		PixelType_CodeReader_Gvsp_RGB10_Packed = 36700184,
		PixelType_CodeReader_Gvsp_BGR10_Packed = 36700185,
		PixelType_CodeReader_Gvsp_RGB12_Packed = 36700186,
		PixelType_CodeReader_Gvsp_BGR12_Packed = 36700187,
		PixelType_CodeReader_Gvsp_RGB16_Packed = 36700211,
		PixelType_CodeReader_Gvsp_RGB10V1_Packed = 35651612,
		PixelType_CodeReader_Gvsp_RGB10V2_Packed = 35651613,
		PixelType_CodeReader_Gvsp_RGB12V1_Packed = 35913780,
		PixelType_CodeReader_Gvsp_RGB565_Packed = 34603061,
		PixelType_CodeReader_Gvsp_BGR565_Packed = 34603062,
		/// YUV Packed buffer format defines 
		PixelType_CodeReader_Gvsp_YUV411_Packed = 34340894,
		PixelType_CodeReader_Gvsp_YUV422_Packed = 34603039,
		PixelType_CodeReader_Gvsp_YUV422_YUYV_Packed = 34603058,
		PixelType_CodeReader_Gvsp_YUV444_Packed = 35127328,
		PixelType_CodeReader_Gvsp_YCBCR8_CBYCR = 35127354,
		PixelType_CodeReader_Gvsp_YCBCR422_8 = 34603067,
		PixelType_CodeReader_Gvsp_YCBCR422_8_CBYCRY = 34603075,
		PixelType_CodeReader_Gvsp_YCBCR411_8_CBYYCRYY = 34340924,
		PixelType_CodeReader_Gvsp_YCBCR601_8_CBYCR = 35127357,
		PixelType_CodeReader_Gvsp_YCBCR601_422_8 = 34603070,
		PixelType_CodeReader_Gvsp_YCBCR601_422_8_CBYCRY = 34603076,
		PixelType_CodeReader_Gvsp_YCBCR601_411_8_CBYYCRYY = 34340927,
		PixelType_CodeReader_Gvsp_YCBCR709_8_CBYCR = 35127360,
		PixelType_CodeReader_Gvsp_YCBCR709_422_8 = 34603073,
		PixelType_CodeReader_Gvsp_YCBCR709_422_8_CBYCRY = 34603077,
		PixelType_CodeReader_Gvsp_YCBCR709_411_8_CBYYCRYY = 34340930,
		/// RGB Planar buffer format defines 
		PixelType_CodeReader_Gvsp_RGB8_Planar = 35127329,
		PixelType_CodeReader_Gvsp_RGB10_Planar = 36700194,
		PixelType_CodeReader_Gvsp_RGB12_Planar = 36700195,
		PixelType_CodeReader_Gvsp_RGB16_Planar = 36700196,
		/// 自定义的图片格式
		PixelType_CodeReader_Gvsp_Jpeg = -2145910783,
		PixelType_CodeReader_Gvsp_Coord3D_ABC32f = 39846080,
		PixelType_CodeReader_Gvsp_Coord3D_ABC32f_Planar = 39846081,
		PixelType_CodeReader_Gvsp_Coord3D_AC32f = 37748930,
		/// 3D coordinate A-C 32-bit floating point
		PixelType_CodeReader_Gvsp_COORD3D_DEPTH_PLUS_MASK = -2112094207
	}

	/// <summary>
	/// 相机信息大小
	/// </summary>
	public const int INFO_MAX_BUFFER_SIZE = 64;

	/// <summary>
	/// 设备列表个数
	/// </summary>
	public const int MV_CODEREADER_MAX_DEVICE_NUM = 256;

	/// <summary>
	/// 静态IP类型
	/// </summary>
	public const int MV_CODEREADER_IP_CFG_STATIC = 83886080;

	/// <summary>
	/// DHCP类型
	/// </summary>
	public const int MV_CODEREADER_IP_CFG_DHCP = 100663296;

	/// <summary>
	/// LLA类型
	/// </summary>
	public const int MV_CODEREADER_IP_CFG_LLA = 67108864;

	/// <summary>
	/// 枚举型最大数量
	/// </summary>
	public const int MV_CODEREADER_MAX_XML_SYMBOLIC_NUM = 64;

	/// <summary>
	/// 异常类型信息
	/// </summary>
	/// <summary>
	/// 设备断开连接
	/// </summary>
	public const int MV_CODEREADER_EXCEPTION_DEV_DISCONNECT = 32769;

	/// <summary>
	/// SDK与驱动版本不匹配
	/// </summary>
	public const int MV_CODEREADER_EXCEPTION_VERSION_CHECK = 32770;

	/// <summary>
	/// 设备访问模式
	/// </summary>
	/// <summary>
	/// 独占权限，其他APP只允许读CCP寄存器
	/// </summary>
	public const int MV_CODEREADER_ACCESS_Exclusive = 1;

	/// <summary>
	/// 可以从5模式下抢占权限，然后以独占权限打开
	/// </summary>
	public const int MV_CODEREADER_ACCESS_ExclusiveWithSwitch = 2;

	/// <summary>
	/// 控制权限，其他APP允许读所有寄存器
	/// </summary>
	public const int MV_CODEREADER_ACCESS_Control = 3;

	/// <summary>
	/// 可以从5的模式下抢占权限，然后以控制权限打开
	/// </summary>
	public const int MV_CODEREADER_ACCESS_ControlWithSwitch = 4;

	/// <summary>
	/// 以可被抢占的控制权限打开
	/// </summary>
	public const int MV_CODEREADER_ACCESS_ControlSwitchEnable = 5;

	/// <summary>
	/// 可以从5的模式下抢占权限，然后以可被抢占的控制权限打开
	/// </summary>
	public const int MV_CODEREADER_ACCESS_ControlSwitchEnableWithKey = 6;

	/// <summary>
	/// 读模式打开设备，适用于控制权限下
	/// </summary>
	public const int MV_CODEREADER_ACCESS_Monitor = 7;

	/// <summary>
	/// 读码器Event事件名称最大长度
	/// </summary>
	public const int MV_CODEREADER_MAX_EVENT_NAME_SIZE = 128;

	/// <summary>
	/// 最大条码长度
	/// </summary>
	public const int MV_CODEREADER_MAX_BCR_CODE_LEN = 256;

	/// <summary>
	/// 扩展最大条码字符长度
	/// </summary>
	public const int MV_CODEREADER_MAX_BCR_CODE_LEN_EX = 4096;

	/// <summary>
	/// OCR字符长度
	/// </summary>
	public const int MV_CODEREADER_MAX_OCR_LEN = 128;

	/// <summary>
	/// 一次最多输出的条码个数
	/// </summary>
	public const int MAX_CODEREADER_BCR_COUNT = 200;

	/// <summary>
	/// 一次最多输出的条码个数扩展
	/// </summary>
	public const int MAX_CODEREADER_BCR_COUNT_EX = 300;

	/// <summary>
	/// 结果数据缓存的上限
	/// </summary>
	public const int MV_CODEREADER_MAX_RESULT_SIZE = 65536;

	/// <summary>
	/// 一次最多输出的抠图个数
	/// </summary>
	public const int MAX_CODEREADER_WAYBILL_COUNT = 50;

	/// <summary>
	/// 一次最多输出的OCR行数
	/// </summary>
	public const int MAX_CODEREADER_OCR_COUNT = 100;

	/// <summary>
	/// 输出协议
	/// </summary>
	/// <summary>
	/// SamrtSDK协议
	/// </summary>
	public const int CommuPtlSmartSDK = 1;

	/// <summary>
	/// TCPIP协议
	/// </summary>
	public const int CommuPtlTcpIP = 2;

	/// <summary>
	/// Serial协议
	/// </summary>
	public const int CommuPtlSerial = 3;

	/// <summary>
	/// 抠图参数定义
	/// </summary>
	/// <summary>
	/// 算法能力集，含面单提取[0x1]，图像增强[0x2]，码提取[0x4]，Box拷贝模块[0x8]，面单提取模块[0x10]，模块最大编号[0x3F]
	/// </summary>
	public const string KEY_WAYBILL_ABILITY = "WAYBILL_Ability";

	/// <summary>
	/// 算法最大宽度，默认默认5472，范围[0,65535]
	/// </summary>
	public const string KEY_WAYBILL_MAX_WIDTH = "WAYBILL_Max_Width";

	/// <summary>
	/// 算法最大高度，默认默认3648，范围[0,65535]
	/// </summary>
	public const string KEY_WAYBILL_MAX_HEIGHT = "WAYBILL_Max_Height";

	/// <summary>
	/// 面单抠图输出的图片格式，默认Jpg，范围[1,2],1为Mono8，2为Jpg，3为Bmp
	/// </summary>
	public const string KEY_WAYBILL_OUTPUTIMAGETYPE = "WAYBILL_OutputImageType";

	/// <summary>
	/// jpg编码质量，默认80，范围[1,100]
	/// </summary>
	public const string KEY_WAYBILL_JPGQUALITY = "WAYBILL_JpgQuality";

	/// <summary>
	/// 图像增强使能，默认0，范围[0,1]
	/// </summary>
	public const string KEY_WAYBILL_ENHANCEENABLE = "WAYBILL_EnhanceEnable";

	/// <summary>
	/// waybill最小宽, 宽是长边, 高是短边，默认100，范围[15,2592]
	/// </summary>
	public const string KEY_WAYBILL_MINWIDTH = "WAYBILL_MinWidth";

	/// <summary>
	/// waybill最小高，默认100，范围[10,2048]
	/// </summary>
	public const string KEY_WAYBILL_MINHEIGHT = "WAYBILL_MinHeight";

	/// <summary>
	/// waybill最大宽, 宽是长边, 高是短边，默认3072，最小值15
	/// </summary>
	public const string KEY_WAYBILL_MAXWIDTH = "WAYBILL_MaxWidth";

	/// <summary>
	/// waybill最大高，默认2048，最小值10
	/// </summary>
	public const string KEY_WAYBILL_MAXHEIGHT = "WAYBILL_MaxHeight";

	/// <summary>
	/// 膨胀次数，默认0，范围[0,10]
	/// </summary>
	public const string KEY_WAYBILL_MORPHTIMES = "WAYBILL_MorphTimes";

	/// <summary>
	/// 面单上条码和字符灰度最小值，默认0，范围[0,255]
	/// </summary>
	public const string KEY_WAYBILL_GRAYLOW = "WAYBILL_GrayLow";

	/// <summary>
	/// 面单上灰度中间值，用于区分条码和背景，默认70，范围[0,255]
	/// </summary>
	public const string KEY_WAYBILL_GRAYMID = "WAYBILL_GrayMid";

	/// <summary>
	/// 面单上背景灰度最大值，默认130，范围[0,255]
	/// </summary>
	public const string KEY_WAYBILL_GRAYHIGH = "WAYBILL_GrayHigh";

	/// <summary>
	/// 自适应二值化，默认1，范围[0,1]
	/// </summary>
	public const string KEY_WAYBILL_BINARYADAPTIVE = "WAYBILL_BinaryAdaptive";

	/// <summary>
	/// 面单抠图行方向扩边，默认10，范围[0,2000]
	/// </summary>
	public const string KEY_WAYBILL_BOUNDARYROW = "WAYBILL_BoundaryRow";

	/// <summary>
	/// 面单抠图列方向扩边，默认11，范围[0,2000]
	/// </summary>
	public const string KEY_WAYBILL_BOUNDARYCOL = "WAYBILL_BoundaryCol";

	/// <summary>
	/// 最大面单和条码高度比例，默认20，范围[1,100]
	/// </summary>
	public const string KEY_WAYBILL_MAXBILLBARHEIGTHRATIO = "WAYBILL_MaxBillBarHightRatio";

	/// <summary>
	/// 最大面单和条码宽度比例，默认5，范围[1,100]
	/// </summary>
	public const string KEY_WAYBILL_MAXBILLBARWIDTHRATIO = "WAYBILL_MaxBillBarWidthRatio";

	/// <summary>
	/// 最小面单和条码高度比例，默认5，范围[1,100]
	/// </summary>
	public const string KEY_WAYBILL_MINBILLBARHEIGTHRATIO = "WAYBILL_MinBillBarHightRatio";

	/// <summary>
	/// 最小面单和条码宽度比例，默认2，范围[1,100]
	/// </summary>
	public const string KEY_WAYBILL_MINBILLBARWIDTHRATIO = "WAYBILL_MinBillBarWidthRatio";

	/// <summary>
	/// 增强方法，最小值/默认值/不进行增强[0x1]，线性拉伸[0x2]，直方图拉伸[0x3]，直方图均衡化[0x4]，亮度校正/最大值[0x5]
	/// </summary>
	public const string KEY_WAYBILL_ENHANCEMETHOD = "WAYBILL_EnhanceMethod";

	/// <summary>
	/// 增强拉伸低阈值比例，默认1，范围[0,100]
	/// </summary>
	public const string KEY_WAYBILL_ENHANCECLIPRATIOLOW = "WAYBILL_ClipRatioLow";

	/// <summary>
	/// 增强拉伸高阈值比例，默认99，范围[0,100]
	/// </summary>
	public const string KEY_WAYBILL_ENHANCECLIPRATIOHIGH = "WAYBILL_ClipRatioHigh";

	/// <summary>
	/// 对比度系数，默认100，范围[1,10000]
	/// </summary>
	public const string KEY_WAYBILL_ENHANCECONTRASTFACTOR = "WAYBILL_ContrastFactor";

	/// <summary>
	/// 锐化系数，默认0，范围[0,10000]
	/// </summary>
	public const string KEY_WAYBILL_ENHANCESHARPENFACTOR = "WAYBILL_SharpenFactor";

	/// <summary>
	/// 锐化滤波核大小，默认3，范围[3,15]
	/// </summary>
	public const string KEY_WAYBILL_SHARPENKERNELSIZE = "WAYBILL_KernelSize";

	/// <summary>
	/// 码单抠图行方向扩边，默认0，范围[0,2000]
	/// </summary>
	public const string KEY_WAYBILL_CODEBOUNDARYROW = "WAYBILL_CodeBoundaryRow";

	/// <summary>
	/// 码单抠图列方向扩边，默认0，范围[0,2000]
	/// </summary>
	public const string KEY_WAYBILL_CODEBOUNDARYCOL = "WAYBILL_CodeBoundaryCol";

	/// 正确码定义
	/// <summary>
	/// 成功，无错误
	/// </summary>
	public const int MV_CODEREADER_OK = 0;

	/// 通用错误码定义:范围0x80020000-0x800200FF
	/// <summary>
	/// 错误或无效的句柄
	/// </summary>
	public const int MV_CODEREADER_E_HANDLE = -2147352576;

	/// <summary>
	/// 不支持的功能
	/// </summary>
	public const int MV_CODEREADER_E_SUPPORT = -2147352575;

	/// <summary>
	/// 缓存已满
	/// </summary>
	public const int MV_CODEREADER_E_BUFOVER = -2147352574;

	/// <summary>
	/// 函数调用顺序错误
	/// </summary>
	public const int MV_CODEREADER_E_CALLORDER = -2147352573;

	/// <summary>
	/// 错误的参数
	/// </summary>
	public const int MV_CODEREADER_E_PARAMETER = -2147352572;

	/// <summary>
	/// 资源申请失败
	/// </summary>
	public const int MV_CODEREADER_E_RESOURCE = -2147352571;

	/// <summary>
	/// 无数据
	/// </summary>
	public const int MV_CODEREADER_E_NODATA = -2147352570;

	/// <summary>
	/// 前置条件有误，或运行环境已发生变化
	/// </summary>
	public const int MV_CODEREADER_E_PRECONDITION = -2147352569;

	/// <summary>
	/// 版本不匹配
	/// </summary>
	public const int MV_CODEREADER_E_VERSION = -2147352568;

	/// <summary>
	/// 传入的内存空间不足
	/// </summary>
	public const int MV_CODEREADER_E_NOENOUGH_BUF = -2147352567;

	/// <summary>
	/// 异常图像，可能是丢包导致图像不完整
	/// </summary>
	public const int MV_CODEREADER_E_ABNORMAL_IMAGE = -2147352566;

	/// <summary>
	/// 动态导入DLL失败
	/// </summary>
	public const int MV_CODEREADER_E_LOAD_LIBRARY = -2147352565;

	/// <summary>
	/// 没有可输出的缓存
	/// </summary>
	public const int MV_CODEREADER_E_NOOUTBUF = -2147352564;

	/// <summary>
	/// 文件路径错误
	/// </summary>
	public const int MV_CODEREADER_E_FILE_PATH = -2147352561;

	/// <summary>
	/// 未知的错误
	/// </summary>
	public const int MV_CODEREADER_E_UNKNOW = -2147352321;

	/// GenICam系列错误:范围0x80020100-0x800201FF
	/// <summary>
	/// 通用错误
	/// </summary>
	public const int MV_CODEREADER_E_GC_GENERIC = -2147352320;

	/// <summary>
	/// 参数非法
	/// </summary>
	public const int MV_CODEREADER_E_GC_ARGUMENT = -2147352319;

	/// <summary>
	/// 值超出范围
	/// </summary>
	public const int MV_CODEREADER_E_GC_RANGE = -2147352318;

	/// <summary>
	/// 属性
	/// </summary>
	public const int MV_CODEREADER_E_GC_PROPERTY = -2147352317;

	/// <summary>
	/// 运行环境有问题
	/// </summary>
	public const int MV_CODEREADER_E_GC_RUNTIME = -2147352316;

	/// <summary>
	/// 逻辑错误
	/// </summary>
	public const int MV_CODEREADER_E_GC_LOGICAL = -2147352315;

	/// <summary>
	/// 节点访问条件有误
	/// </summary>
	public const int MV_CODEREADER_E_GC_ACCESS = -2147352314;

	/// <summary>
	/// 超时
	/// </summary>
	public const int MV_CODEREADER_E_GC_TIMEOUT = -2147352313;

	/// <summary>
	/// 转换异常
	/// </summary>
	public const int MV_CODEREADER_E_GC_DYNAMICCAST = -2147352312;

	/// <summary>
	/// GenICam未知错误
	/// </summary>
	public const int MV_CODEREADER_E_GC_UNKNOW = -2147352065;

	/// GigE_STATUS对应的错误码:范围0x80020200-0x800202FF
	/// <summary>
	/// 命令不被设备支持
	/// </summary>
	public const int MV_CODEREADER_E_NOT_IMPLEMENTED = -2147352064;

	/// <summary>
	/// 访问的目标地址不存在
	/// </summary>
	public const int MV_CODEREADER_E_INVALID_ADDRESS = -2147352063;

	/// <summary>
	/// 目标地址不可写
	/// </summary>
	public const int MV_CODEREADER_E_WRITE_PROTECT = -2147352062;

	/// <summary>
	/// 设备无访问权限
	/// </summary>
	public const int MV_CODEREADER_E_ACCESS_DENIED = -2147352061;

	/// <summary>
	/// 设备忙，或网络断开
	/// </summary>
	public const int MV_CODEREADER_E_BUSY = -2147352060;

	/// <summary>
	/// 网络包数据错误
	/// </summary>
	public const int MV_CODEREADER_E_PACKET = -2147352059;

	/// <summary>
	/// 网络相关错误
	/// </summary>
	public const int MV_CODEREADER_E_NETER = -2147352058;

	/// GigE相机特有的错误码
	/// <summary>
	/// 设备IP冲突
	/// </summary>
	public const int MV_CODEREADER_E_IP_CONFLICT = -2147352031;

	/// USB_STATUS对应的错误码:范围0x80020300-0x800203FF
	/// <summary>
	/// 读usb出错
	/// </summary>
	public const int MV_CODEREADER_E_USB_READ = -2147351808;

	/// <summary>
	/// 写usb出错
	/// </summary>
	public const int MV_CODEREADER_E_USB_WRITE = -2147351807;

	/// <summary>
	/// 设备异常
	/// </summary>
	public const int MV_CODEREADER_E_USB_DEVICE = -2147351806;

	/// <summary>
	/// GenICam相关错误
	/// </summary>
	public const int MV_CODEREADER_E_USB_GENICAM = -2147351805;

	/// <summary>
	/// 带宽不足
	/// </summary>
	public const int MV_CODEREADER_E_USB_BANDWIDTH = -2147351804;

	/// <summary>
	/// 驱动不匹配或者未装驱动
	/// </summary>
	public const int MV_CODEREADER_E_USB_DRIVER = -2147351803;

	/// <summary>
	/// USB未知的错误
	/// </summary>
	public const int MV_CODEREADER_E_USB_UNKNOW = -2147351553;

	/// 升级时对应的错误码:范围0x80020400-0x800204FF
	/// <summary>
	/// 升级模块错误码最小值
	/// </summary>
	public const int MV_CODEREADER_E_UPG_MIN_ERRCODE = -2147351552;

	/// <summary>
	/// 升级固件不匹配
	/// </summary>
	public const int MV_CODEREADER_E_UPG_FILE_MISMATCH = -2147351552;

	/// <summary>
	/// 升级固件语言不匹配
	/// </summary>
	public const int MV_CODEREADER_E_UPG_LANGUSGE_MISMATCH = -2147351551;

	/// <summary>
	/// 升级冲突（设备已经在升级了再次请求升级即返回此错误）
	/// </summary>
	public const int MV_CODEREADER_E_UPG_CONFLICT = -2147351550;

	/// <summary>
	/// 升级时相机内部出现错误
	/// </summary>
	public const int MV_CODEREADER_E_UPG_INNER_ERR = -2147351549;

	/// <summary>
	/// 获取相机型号失败
	/// </summary>
	public const int MV_CODEREADER_E_UPG_REGRESH_TYPE_ERR = -2147351548;

	/// <summary>
	/// 复制FPGA文件失败
	/// </summary>
	public const int MV_CODEREADER_E_UPG_COPY_FPGABIN_ERR = -2147351547;

	/// <summary>
	/// ZIP文件解压失败
	/// </summary>
	public const int MV_CODEREADER_E_UPG_ZIPEXTRACT_ERR = -2147351546;

	/// <summary>
	/// DAV文件解压失败
	/// </summary>
	public const int MV_CODEREADER_E_UPG_DAVEXTRACT_ERR = -2147351545;

	/// <summary>
	/// DAV文件压缩失败
	/// </summary>
	public const int MV_CODEREADER_E_UPG_DAVCOMPRESS_ERR = -2147351544;

	/// <summary>
	/// ZIP文件压缩失败
	/// </summary>
	public const int MV_CODEREADER_E_UPG_ZIPCOMPRESS_ERR = -2147351543;

	/// <summary>
	/// 获取升级进度超时
	/// </summary>
	public const int MV_CODEREADER_E_UPG_GET_PROGRESS_TIMEOUT_ERR = -2147351536;

	/// <summary>
	/// 发送进度查询指令失败
	/// </summary>
	public const int MV_CODEREADER_E_UPG_SEND_QUERY_PROGRESS_ERR = -2147351535;

	/// <summary>
	/// 接收进度查询指令失败
	/// </summary>
	public const int MV_CODEREADER_E_UPG_RECV_QUERY_PROGRESS_ERR = -2147351534;

	/// <summary>
	/// 获取查询进度失败
	/// </summary>
	public const int MV_CODEREADER_E_UPG_GET_QUERY_PROGRESS_ERR = -2147351533;

	/// <summary>
	/// 获得最大进度失败
	/// </summary>
	public const int MV_CODEREADER_E_UPG_GET_MAX_QUERY_PROGRESS_ERR = -2147351532;

	/// <summary>
	/// 文件验证失败
	/// </summary>
	public const int MV_CODEREADER_E_UPG_CHECKT_PACKET_FAILED = -2147351451;

	/// <summary>
	/// FPGA程序升级失败
	/// </summary>
	public const int MV_CODEREADER_E_UPG_FPGA_PROGRAM_FAILED = -2147351450;

	/// <summary>
	/// 看门狗升级失败
	/// </summary>
	public const int MV_CODEREADER_E_UPG_WATCHDOG_FAILED = -2147351449;

	/// <summary>
	/// 裸相机升级失败
	/// </summary>
	public const int MV_CODEREADER_E_UPG_CAMERA_AND_BARE_FAILED = -2147351448;

	/// <summary>
	/// 保留配置文件失败
	/// </summary>
	public const int MV_CODEREADER_E_UPG_RETAIN_CONFIG_FAILED = -2147351447;

	/// <summary>
	/// FPGA驱动升级失败
	/// </summary>
	public const int MV_CODEREADER_E_UPG_FPGA_DRIVER_FAILED = -2147351446;

	/// <summary>
	/// SPI驱动升级失败
	/// </summary>
	public const int MV_CODEREADER_E_UPG_SPI_DRIVER_FAILED = -2147351445;

	/// <summary>
	/// 重新启动失败
	/// </summary>
	public const int MV_CODEREADER_E_UPG_REBOOT_SYSTEM_FAILED = -2147351444;

	/// <summary>
	/// 升级服务升级失败
	/// </summary>
	public const int MV_CODEREADER_E_UPG_UPSELF_FAILED = -2147351443;

	/// <summary>
	/// 停止相关服务失败
	/// </summary>
	public const int MV_CODEREADER_E_UPG_STOP_RELATION_PROGRAM_FAILED = -2147351442;

	/// <summary>
	/// 设备类型不一致
	/// </summary>
	public const int MV_CODEREADER_E_UPG_DEVCIE_TYPE_INCONSISTENT = -2147351441;

	/// <summary>
	/// 读取加密信息失败
	/// </summary>
	public const int MV_CODEREADER_E_UPG_READ_ENCRYPT_INFO_FAILED = -2147351440;

	/// <summary>
	/// 设备平台错误
	/// </summary>
	public const int MV_CODEREADER_E_UPG_PLAT_TYPE_INCONSISTENT = -2147351439;

	/// <summary>
	/// 相机型号错误
	/// </summary>
	public const int MV_CODEREADER_E_UPG_CAMERA_TYPE_INCONSISTENT = -2147351438;

	/// <summary>
	/// 相机正在升级
	/// </summary>
	public const int MV_CODEREADER_E_UPG_DEVICE_UPGRADING = -2147351437;

	/// <summary>
	/// 升级包解压失败
	/// </summary>
	public const int MV_CODEREADER_E_UPG_UNZIP_FAILED = -2147351436;

	/// <summary>
	/// 巴枪蓝牙未连接
	/// </summary>
	public const int MV_CODEREADER_E_UPG_BLE_DISCONNECT = -2147351435;

	/// <summary>
	/// 电量不足
	/// </summary>
	public const int MV_CODEREADER_E_UPG_BATTERY_NOTENOUGH = -2147351434;

	/// <summary>
	/// 巴枪未放在底座上
	/// </summary>
	public const int MV_CODEREADER_E_UPG_RTC_NOT_PRESENT = -2147351433;

	/// <summary>
	/// APP升级失败
	/// </summary>
	public const int MV_CODEREADER_E_UPG_APP_ERR = -2147351432;

	/// <summary>
	/// L3升级失败
	/// </summary>
	public const int MV_CODEREADER_E_UPG_L3_ERR = -2147351431;

	/// <summary>
	/// MCU升级失败
	/// </summary>
	public const int MV_CODEREADER_E_UPG_MCU_ERR = -2147351430;

	/// <summary>
	/// 平台不匹配
	/// </summary>
	public const int MV_CODEREADER_E_UPG_PLATFORM_DISMATCH = -2147351429;

	/// <summary>
	/// 型号不匹配
	/// </summary>
	public const int MV_CODEREADER_E_UPG_TYPE_DISMATCH = -2147351428;

	/// <summary>
	/// 空间不匹配
	/// </summary>
	public const int MV_CODEREADER_E_UPG_SPACE_DISMATCH = -2147351427;

	/// <summary>
	/// 内存不匹配
	/// </summary>
	public const int MV_CODEREADER_E_UPG_MEM_DISMATCH = -2147351426;

	/// <summary>
	/// 网络传输异常，请重新升级
	/// </summary>
	public const int MV_CODEREADER_E_UPG_NET_TRANS_ERROR = -2147351425;

	/// <summary>
	/// 升级时未知错误
	/// </summary>
	public const int MV_CODEREADER_E_UPG_UNKNOW = -2147351297;

	/// 网络组件对应的错误码: 0x80020500-0x800205FF
	/// <summary>
	/// 创建Socket错误
	/// </summary>
	public const int MV_CODEREADER_E_CREAT_SOCKET = -2147351296;

	/// <summary>
	/// 绑定错误
	/// </summary>
	public const int MV_CODEREADER_E_BIND_SOCKET = -2147351295;

	/// <summary>
	/// 连接错误
	/// </summary>
	public const int MV_CODEREADER_E_CONNECT_SOCKET = -2147351294;

	/// <summary>
	/// 获取主机名错误
	/// </summary>
	public const int MV_CODEREADER_E_GET_HOSTNAME = -2147351293;

	/// <summary>
	/// 写入数据错误
	/// </summary>
	public const int MV_CODEREADER_E_NET_WRITE = -2147351292;

	/// <summary>
	/// 读取数据错误
	/// </summary>
	public const int MV_CODEREADER_E_NET_READ = -2147351291;

	/// <summary>
	/// Select错误
	/// </summary>
	public const int MV_CODEREADER_E_NET_SELECT = -2147351290;

	/// <summary>
	/// 超时
	/// </summary>
	public const int MV_CODEREADER_E_NET_TIMEOUT = -2147351289;

	/// <summary>
	/// 接收错误
	/// </summary>
	public const int MV_CODEREADER_E_NET_ACCEPT = -2147351288;

	/// <summary>
	/// 网络未知错误
	/// </summary>
	public const int MV_CODEREADER_E_NET_UNKNOW = -2147351041;

	/// 设备类型定义
	/// <summary>
	/// ch:未知设备类型，保留意义
	/// </summary>
	public const int MV_CODEREADER_UNKNOW_DEVICE = 0;

	/// <summary>
	/// ch:GigE设备
	/// </summary>
	public const int MV_CODEREADER_GIGE_DEVICE = 1;

	/// <summary>
	/// ch:1394-a/b 设备
	/// </summary>
	public const int MV_CODEREADER_1394_DEVICE = 2;

	/// <summary>
	/// ch:USB3.0 设备
	/// </summary>
	public const int MV_CODEREADER_USB_DEVICE = 4;

	/// <summary>
	/// ch:CameraLink设备
	/// </summary>
	public const int MV_CODEREADER_CAMERALINK_DEVICE = 8;

	/// 私有成员变量
	/// <summary>
	/// 设备句柄
	/// </summary>
	private IntPtr handle;

	public static byte[] BytesTrimEnd(byte[] inputStream)
	{
		List<byte> list = new List<byte>();
		for (int i = 0; i < inputStream.Length && inputStream[i] != 0; i++)
		{
			list.Add(inputStream[i]);
		}
		return list.ToArray();
	}

	public static bool IsTextUTF8(byte[] inputStream)
	{
		int num = 0;
		bool flag = true;
		for (int i = 0; i < inputStream.Length; i++)
		{
			byte b = inputStream[i];
			if ((b & 0x80) == 128)
			{
				flag = false;
			}
			if (num == 0)
			{
				if ((b & 0x80) != 0)
				{
					if ((b & 0xC0) != 192)
					{
						return false;
					}
					num = 1;
					b <<= 2;
					while ((b & 0x80) == 128)
					{
						b <<= 1;
						num++;
					}
				}
			}
			else
			{
				if ((b & 0xC0) != 128)
				{
					return false;
				}
				num--;
			}
		}
		if (num != 0)
		{
			return false;
		}
		return !flag;
	}

	/// <summary>
	/// ch:构造函数 | en:Constructor
	/// </summary>
	public MvCodeReader()
	{
		handle = IntPtr.Zero;
	}

	/// <summary>
	/// ch:析构函数 | en:Destructor
	/// </summary>
	~MvCodeReader()
	{
		MV_CODEREADER_DestroyHandle_NET();
	}

	/// <summary>
	/// 获取SDK的版本号
	/// </summary>
	/// <returns>始终返回4字节版本号 |主    |次    |修正  |  测试|</returns>
	public static uint MV_CODEREADER_GetSDKVersion_NET()
	{
		return MV_CODEREADER_GetSDKVersion();
	}

	/// <summary>
	/// 创建设备句柄
	/// </summary>
	/// <param name="pstDevInfo">设备信息:MV_CODEREADER_DEVICE_INFO</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_CreateHandle_NET(ref MV_CODEREADER_DEVICE_INFO pstDevInfo)
	{
		if (IntPtr.Zero != handle)
		{
			MV_CODEREADER_DestroyHandle(handle);
			handle = IntPtr.Zero;
		}
		return MV_CODEREADER_CreateHandle(ref handle, ref pstDevInfo);
	}

	/// <summary>
	/// 通过序列号创建设备句柄
	/// </summary>
	/// <param name="chSerialNumber">设备序列号</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_CreateHandleBySerialNumber_NET(string chSerialNumber)
	{
		if (IntPtr.Zero != handle)
		{
			MV_CODEREADER_DestroyHandle(handle);
			handle = IntPtr.Zero;
		}
		return MV_CODEREADER_CreateHandleBySerialNumber(ref handle, chSerialNumber);
	}

	/// <summary>
	/// 销毁设备句柄
	/// </summary>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_DestroyHandle_NET()
	{
		int result = MV_CODEREADER_DestroyHandle(handle);
		handle = IntPtr.Zero;
		return result;
	}

	/// <summary>
	/// 枚举设备
	/// </summary>
	/// <param name="pstDevList">设备列表信息</param>
	/// <param name="nTLayerType">传输层协议类型</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public static int MV_CODEREADER_EnumDevices_NET(ref MV_CODEREADER_DEVICE_INFO_LIST pstDevList, uint nTLayerType)
	{
		return MV_CODEREADER_EnumDevices(ref pstDevList, nTLayerType);
	}

	/// <summary>
	/// 枚举指定系列设备
	/// </summary>
	/// <param name="pstDevList">设备列表信息</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public static int MV_CODEREADER_EnumCodeReader_NET(ref MV_CODEREADER_DEVICE_INFO_LIST pstDevList)
	{
		return MV_CODEREADER_EnumCodeReader(ref pstDevList);
	}

	/// <summary>
	/// 枚举特定系列设备，增加校验字段，防止被第三方软件占用的设备(依赖工业相机SDK 4.0版本支持)
	/// </summary>
	/// <param name="pstDevList">设备信息列表</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public static int MV_CODEREADER_EnumIDDevices_NET(ref MV_CODEREADER_DEVICE_INFO_LIST pstDevList)
	{
		return MV_CODEREADER_EnumIDDevices(ref pstDevList);
	}

	/// <summary>
	/// 设备是否可达
	/// </summary>
	/// <param name="pstDevInfo">设备信息</param>
	/// <param name="nAccessMode">访问权限</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public static bool MV_CODEREADER_IsDeviceAccessible_NET(ref MV_CODEREADER_DEVICE_INFO pstDevInfo, uint nAccessMode)
	{
		return MV_CODEREADER_IsDeviceAccessible(ref pstDevInfo, nAccessMode);
	}

	/// <summary>
	/// 打开设备
	/// </summary>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_OpenDevice_NET()
	{
		return MV_CODEREADER_OpenDevice(handle);
	}

	/// <summary>
	/// 关闭设备
	/// </summary>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_CloseDevice_NET()
	{
		return MV_CODEREADER_CloseDevice(handle);
	}

	/// <summary>
	/// 开始取流
	/// </summary>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_StartGrabbing_NET()
	{
		return MV_CODEREADER_StartGrabbing(handle);
	}

	/// <summary>
	/// 停止取流
	/// </summary>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_StopGrabbing_NET()
	{
		return MV_CODEREADER_StopGrabbing(handle);
	}

	/// <summary>
	/// 采用超时机制获取一帧图片，SDK内部等待直到有数据时返回
	/// </summary>
	/// <param name="pData">一帧图像数据</param>
	/// <param name="pstFrameInfo">图像信息:MV_CODEREADER_IMAGE_OUT_INFO</param>
	/// <param name="nMsec">超时时间</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_GetOneFrameTimeout_NET(ref IntPtr pData, IntPtr pstFrameInfo, uint nMsec)
	{
		return MV_CODEREADER_GetOneFrameTimeout(handle, ref pData, pstFrameInfo, nMsec);
	}

	/// <summary>
	/// 采用超时机制获取一帧图片，SDK内部等待直到有数据时返回
	/// </summary>
	/// <param name="pData">一帧图像数据</param>
	/// <param name="pstFrameInfoEx">图像信息(增加OCR信息):MV_CODEREADER_IMAGE_OUT_INFO_EX</param>
	/// <param name="nMsec">超时时间</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_GetOneFrameTimeoutEx_NET(ref IntPtr pData, IntPtr pstFrameInfoEx, uint nMsec)
	{
		return MV_CODEREADER_GetOneFrameTimeoutEx(handle, ref pData, pstFrameInfoEx, nMsec);
	}

	/// <summary>
	/// 采用超时机制获取一帧图片，SDK内部等待直到有数据时返回
	/// </summary>
	/// <param name="pData">一帧图像数据</param>
	/// <param name="pstFrameInfoEx2">图像信息(扩展条码信息):MV_CODEREADER_IMAGE_OUT_INFO_EX2</param>
	/// <param name="nMsec">超时时间</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_GetOneFrameTimeoutEx2_NET(ref IntPtr pData, IntPtr pstFrameInfoEx2, uint nMsec)
	{
		return MV_CODEREADER_GetOneFrameTimeoutEx2(handle, ref pData, pstFrameInfoEx2, nMsec);
	}

	/// <summary>
	/// 采用超时机制获取一路流通道一帧图片，SDK内部等待直到有数据时返回
	/// </summary>
	/// <param name="pData">一帧图像数据</param>
	/// <param name="pstFrameInfoEx2">图像信息(包含二维码质量信息):MV_CODEREADER_IMAGE_OUT_INFO_EX2</param>
	/// <param name="nChannelID">流通道ID(单通道固件通道号为0, 多通道固件通道号为0/1(根据多通道sensor个数))</param>
	/// <param name="nMsec">超时时间</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_MSC_GetOneFrameTimeout_NET(ref IntPtr pData, IntPtr pstFrameInfoEx2, uint nChannelID, uint nMsec)
	{
		return MV_CODEREADER_MSC_GetOneFrameTimeout(handle, ref pData, pstFrameInfoEx2, nChannelID, nMsec);
	}

	/// <summary>
	/// 获取设备信息
	/// </summary>
	/// <param name="pstDevInfo">设备信息</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_GetDeviceInfo_NET(ref MV_CODEREADER_DEVICE_INFO pstDevInfo)
	{
		return MV_CODEREADER_GetDeviceInfo(handle, ref pstDevInfo);
	}

	/// <summary>
	/// 获取Integer属性值
	/// </summary>
	/// <param name="strKey">属性键值，如获取宽度信息则为"Width"</param>
	/// <param name="pIntValue">返回给调用者有关相机属性结构体</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_GetIntValue_NET(string strKey, ref MV_CODEREADER_INTVALUE_EX pIntValue)
	{
		return MV_CODEREADER_GetIntValue(handle, strKey, ref pIntValue);
	}

	/// <summary>
	/// 设置Integer型属性值
	/// </summary>
	/// <param name="strKey">属性键值，如宽度信息则为"Width"</param>
	/// <param name="nValue">想要设置的相机的属性值</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_SetIntValue_NET(string strKey, long nValue)
	{
		return MV_CODEREADER_SetIntValue(handle, strKey, nValue);
	}

	/// <summary>
	/// 获取Enum属性值
	/// </summary>
	/// <param name="strKey">属性键值，如获取像素格式信息则为"PixelFormat"</param>
	/// <param name="pEnumValue">返回给调用者有关相机属性结构体</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_GetEnumValue_NET(string strKey, ref MV_CODEREADER_ENUMVALUE pEnumValue)
	{
		return MV_CODEREADER_GetEnumValue(handle, strKey, ref pEnumValue);
	}

	/// <summary>
	/// 设置Enum型属性值
	/// </summary>
	/// <param name="strKey">属性键值，如获取像素格式信息则为"PixelFormat"</param>
	/// <param name="nValue">想要设置的相机的属性值</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_SetEnumValue_NET(string strKey, uint nValue)
	{
		return MV_CODEREADER_SetEnumValue(handle, strKey, nValue);
	}

	/// <summary>
	/// 设置Enum型属性值
	/// </summary>
	/// <param name="strKey">属性键值，如获取像素格式信息则为"PixelFormat"</param>
	/// <param name="sValue">想要设置的相机的属性字符串</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_SetEnumValueByString_NET(string strKey, string sValue)
	{
		return MV_CODEREADER_SetEnumValueByString(handle, strKey, sValue);
	}

	/// <summary>
	/// 获取Float属性值
	/// </summary>
	/// <param name="strKey">属性键值</param>
	/// <param name="pFloatValue">返回给调用者有关相机属性结构体</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_GetFloatValue_NET(string strKey, ref MV_CODEREADER_FLOATVALUE pFloatValue)
	{
		return MV_CODEREADER_GetFloatValue(handle, strKey, ref pFloatValue);
	}

	/// <summary>
	/// 设置Float属性值
	/// </summary>
	/// <param name="strKey">属性键值</param>
	/// <param name="fValue">想要设置的相机的属性值</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_SetFloatValue_NET(string strKey, float fValue)
	{
		return MV_CODEREADER_SetFloatValue(handle, strKey, fValue);
	}

	/// <summary>
	/// 获取Boolean属性值
	/// </summary>
	/// <param name="strKey">属性键值</param>
	/// <param name="pBoolValue">返回给调用者有关相机属性值</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_GetBoolValue_NET(string strKey, ref bool pBoolValue)
	{
		return MV_CODEREADER_GetBoolValue(handle, strKey, ref pBoolValue);
	}

	/// <summary>
	/// 设置Boolean型属性值
	/// </summary>
	/// <param name="strKey">属性键值</param>
	/// <param name="bValue">想要设置的相机的属性值</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_SetBoolValue_NET(string strKey, bool bValue)
	{
		return MV_CODEREADER_SetBoolValue(handle, strKey, bValue);
	}

	/// <summary>
	/// 获取String属性值
	/// </summary>
	/// <param name="strKey">属性键值</param>
	/// <param name="pStringValue">返回给调用者有关相机属性结构体指针</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_GetStringValue_NET(string strKey, ref MV_CODEREADER_STRINGVALUE pStringValue)
	{
		return MV_CODEREADER_GetStringValue(handle, strKey, ref pStringValue);
	}

	/// <summary>
	/// 设置String型属性值
	/// </summary>
	/// <param name="strKey">属性键值</param>
	/// <param name="sValue">想要设置的相机的属性值</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_SetStringValue_NET(string strKey, string sValue)
	{
		return MV_CODEREADER_SetStringValue(handle, strKey, sValue);
	}

	/// <summary>
	/// 设置Command型属性值
	/// </summary>
	/// <param name="strKey">属性键值</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_SetCommandValue_NET(string strKey)
	{
		return MV_CODEREADER_SetCommandValue(handle, strKey);
	}

	/// <summary>
	/// 获取最佳的packet size，该接口目前只支持GigE相机
	/// </summary>
	/// <returns>最佳packetsize</returns>
	public int MV_CODEREADER_GetOptimalPacketSize_NET()
	{
		return MV_CODEREADER_GetOptimalPacketSize(handle);
	}

	/// <summary>
	/// 读内存
	/// </summary>
	/// <param name="pBuffer">作为返回值使用，保存读到的内存值</param>
	/// <param name="nAddress">待读取的内存地址，该地址可以从设备的Camera.xml文件中获取，形如xxx_RegAddr的xml节点值</param>
	/// <param name="nLength">待读取的内存长度</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_ReadMemory_NET(IntPtr pBuffer, long nAddress, long nLength)
	{
		return MV_CODEREADER_ReadMemory(handle, pBuffer, nAddress, nLength);
	}

	/// <summary>
	/// 写内存
	/// </summary>
	/// <param name="pBuffer">待写入的内存值</param>
	/// <param name="nAddress">待写入的内存地址，该地址可以从设备的Camera.xml文件中获取，形如xxx_RegAddr的xml节点值</param>
	/// <param name="nLength">待写入的内存长度</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_WriteMemory_NET(IntPtr pBuffer, long nAddress, long nLength)
	{
		return MV_CODEREADER_WriteMemory(handle, pBuffer, nAddress, nLength);
	}

	/// <summary>
	/// 强制IP设置
	/// </summary>
	/// <param name="nIP">设置的IP</param>
	/// <param name="nSubNetMask">子网掩码</param>
	/// <param name="nDefaultGateWay">默认网关</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_GIGE_ForceIp_NET(uint nIP, uint nSubNetMask, uint nDefaultGateWay)
	{
		return MV_CODEREADER_GIGE_ForceIp(handle, nIP, nSubNetMask, nDefaultGateWay);
	}

	/// <summary>
	/// 配置IP方式
	/// </summary>
	/// <param name="nType">SI系列智能读码器不支持通过该接口设置IP配置类型, SI系列若设置STATIC则直接返回OK</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_GIGE_SetIpConfig_NET(uint nType)
	{
		return MV_CODEREADER_GIGE_SetIpConfig(handle, nType);
	}

	/// <summary>
	/// 从相机读取文件
	/// </summary>
	/// <param name="pstFileAccess">文件存取结构体</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_FileAccessRead_NET(ref MV_CODEREADER_FILE_ACCESS pstFileAccess)
	{
		return MV_CODEREADER_FileAccessRead(handle, ref pstFileAccess);
	}

	/// <summary>
	/// 将文件写入相机
	/// </summary>
	/// <param name="pstFileAccess">文件存取结构体</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_FileAccessWrite_NET(ref MV_CODEREADER_FILE_ACCESS pstFileAccess)
	{
		return MV_CODEREADER_FileAccessWrite(handle, ref pstFileAccess);
	}

	/// <summary>
	/// 获取文件存取的进度
	/// </summary>
	/// <param name="pstFileAccessProgress">进度内容</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_GetFileAccessProgress_NET(ref MV_CODEREADER_FILE_ACCESS_PROGRESS pstFileAccessProgress)
	{
		return MV_CODEREADER_GetFileAccessProgress(handle, ref pstFileAccessProgress);
	}

	/// <summary>
	/// 设置抠图使能
	/// </summary>
	/// <param name="bWaybillEnable">抠图使能</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_SetWayBillEnable_NET(bool bWaybillEnable)
	{
		return MV_CODEREADER_SetWayBillEnable(handle, bWaybillEnable);
	}

	/// <summary>
	/// 获取当前输入图像的面单信息（输入图像对应 MV_CODEREADER_IMAGE_OUT_INFO_EX 该结构体信息）
	/// </summary>
	/// <param name="pData">原始图像指针</param>
	/// <param name="pstFrameInfoEx">图像信息结构体:MV_CODEREADER_IMAGE_OUT_INFO_EX</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_GetWayBillInfo_NET(IntPtr pData, IntPtr pstFrameInfoEx)
	{
		return MV_CODEREADER_GetWayBillInfo(handle, pData, pstFrameInfoEx);
	}

	/// <summary>
	/// 获取当前输入图像的面单信息（输入图像对应 MV_CODEREADER_IMAGE_OUT_INFO_EX2 该结构体信息）
	/// </summary>
	/// <param name="pData">原始图像指针</param>
	/// <param name="pstFrameInfoEx2">图像信息结构体:MV_CODEREADER_IMAGE_OUT_INFO_EX2</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_GetWayBillInfoEx_NET(IntPtr pData, IntPtr pstFrameInfoEx2)
	{
		return MV_CODEREADER_GetWayBillInfoEx(handle, pData, pstFrameInfoEx2);
	}

	/// <summary>
	/// 设置整型参数
	/// </summary>
	/// <param name="strParamKeyName">属性键值</param>
	/// <param name="nValue">参数值</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_Algorithm_SetIntValue_NET(string strParamKeyName, int nValue)
	{
		return MV_CODEREADER_Algorithm_SetIntValue(handle, strParamKeyName, nValue);
	}

	/// <summary>
	/// 获取整型参数
	/// </summary>
	/// <param name="strParamKeyName">属性键值</param>
	/// <param name="pnValue">参数值</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_Algorithm_GetIntValue_NET(string strParamKeyName, ref int pnValue)
	{
		return MV_CODEREADER_Algorithm_GetIntValue(handle, strParamKeyName, ref pnValue);
	}

	/// <summary>
	/// 保存图片，支持Bmp和Jpeg.编码质量在50-99之间
	/// </summary>
	/// <param name="pSaveParam">保存图片参数结构体</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_SaveImage_NET(ref MV_CODEREADER_SAVE_IMAGE_PARAM_EX pSaveParam)
	{
		return MV_CODEREADER_SaveImage(handle, ref pSaveParam);
	}

	/// <summary>
	/// 注册异常消息回调，在打开设备之后调用
	/// </summary>
	/// <param name="cbException">异常回调函数</param>
	/// <param name="pUser">用户自定义变量</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_RegisterExceptionCallBack_NET(cbExceptiondelegate cbException, IntPtr pUser)
	{
		return MV_CODEREADER_RegisterExceptionCallBack(handle, cbException, pUser);
	}

	/// <summary>
	/// 注册全部事件回调，在打开设备之后调用(该接口需要固件支持)
	/// </summary>
	/// <param name="cbAllEvent">事件回调函数</param>
	/// <param name="pUser">用户自定义变量</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_RegisterAllEventCallBack_NET(cbAllEventdelegate cbAllEvent, IntPtr pUser)
	{
		return MV_CODEREADER_RegisterAllEventCallBack(handle, cbAllEvent, pUser);
	}

	/// <summary>
	/// 注册触发回调，在打开设备之后调用
	/// </summary>
	/// <param name="cbTrigger">触发回调函数</param>
	/// <param name="pUser">用户自定义变量</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_RegisterTriggerCallBack_NET(cbTriggerdelegate cbTrigger, IntPtr pUser)
	{
		return MV_CODEREADER_RegisterTriggerCallBack(handle, cbTrigger, pUser);
	}

	/// <summary>
	/// 注册图像数据回调
	/// </summary>
	/// <param name="cbOutput">图像回调函数</param>
	/// <param name="pUser">用户自定义参数</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_RegisterImageCallBack_NET(cbOutputdelegate cbOutput, IntPtr pUser)
	{
		return MV_CODEREADER_RegisterImageCallBack(handle, cbOutput, pUser);
	}

	/// <summary>
	/// 注册图像数据回调(包含OCR信息)，开始取流之前调用
	/// </summary>
	/// <param name="cbOutput">图像回调函数</param>
	/// <param name="pUser">用户自定义参数</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_RegisterImageCallBackEx_NET(cbOutputExdelegate cbOutput, IntPtr pUser)
	{
		return MV_CODEREADER_RegisterImageCallBackEx(handle, cbOutput, pUser);
	}

	/// <summary>
	/// 注册图像数据(数据包含二维码质量评级)回调
	/// </summary>
	/// <param name="cbOutput">图像回调函数</param>
	/// <param name="pUser">用户自定义参数</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_RegisterImageCallBackEx2_NET(cbOutputEx2delegate cbOutput, IntPtr pUser)
	{
		return MV_CODEREADER_RegisterImageCallBackEx2(handle, cbOutput, pUser);
	}

	/// <summary>
	/// 注册指定一路流通道图像数据(数据包含二维码质量评级)回调
	/// </summary>
	/// <param name="nChannelID">流通道号（单通道固件通道号为0, 多通道固件通道号为0/1(根据多通道sensor个数)）</param>
	/// <param name="cbOutput">图像回调函数</param>
	/// <param name="pUser">用户自定义参数</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_MSC_RegisterImageCallBack_NET(uint nChannelID, cbMSCOutputdelegate cbOutput, IntPtr pUser)
	{
		return MV_CODEREADER_MSC_RegisterImageCallBack(handle, nChannelID, cbOutput, pUser);
	}

	/// <summary>
	/// 设置GIGE设备超时时间
	/// </summary>
	/// <param name="nMillisec">超时时间(ms)</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_GIGE_SetGvcpTimeout_NET(uint nMillisec)
	{
		return MV_CODEREADER_GIGE_SetGvcpTimeout(handle, nMillisec);
	}

	/// <summary>
	/// 获取GIGE设备超时时间
	/// </summary>
	/// <param name="pMillisec">超时时间(ms)</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_GIGE_GetGvcpTimeout_NET(IntPtr pMillisec)
	{
		return MV_CODEREADER_GIGE_GetGvcpTimeout(handle, pMillisec);
	}

	/// <summary>
	/// 设置帧数据缓存节点数量（虚拟相机一直返回OK，但实际不支持，无效果）, 创建句柄后，连接设备前调用有效
	/// </summary>
	/// <param name="nNodeNum">缓存节点数量，范围1-12，默认3</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_SetFrameNodeNum_NET(uint nNodeNum)
	{
		return MV_CODEREADER_SetFrameNodeNum(handle, nNodeNum);
	}

	/// <summary>
	/// 清除GenICam节点缓存
	/// </summary>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_InvalidateNodes_NET()
	{
		return MV_CODEREADER_InvalidateNodes(handle);
	}

	/// <summary>
	/// 配置区域地址和值（读码相机专用）
	/// </summary>
	/// <param name="handle">设备句柄</param>
	/// <param name="nAreaAddress">区域码地址（0xa80）</param>
	/// <param name="nAreaCode">地区码（国内：0x10000  ，国外：0x20000）</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	public int MV_CODEREADER_SetAreaInfoConfig_NET(IntPtr handle, uint nAreaAddress, uint nAreaCode)
	{
		return MV_CODEREADER_SetAreaInfoConfig(handle, nAreaAddress, nAreaCode);
	}

	/// <summary>
	/// 获取设备句柄
	/// </summary>
	/// <returns>返回设备句柄</returns>
	public IntPtr GetCameraHandle()
	{
		return handle;
	}

	/// <summary>
	/// 结构体转换函数
	/// </summary>
	/// <param name="bytes">缓存</param>
	/// <param name="type">类型</param>
	/// <returns>对象值</returns>
	public static object ByteToStruct(byte[] bytes, Type type)
	{
		int num = Marshal.SizeOf(type);
		if (num > bytes.Length)
		{
			return null;
		}
		IntPtr intPtr = Marshal.AllocHGlobal(num);
		Marshal.Copy(bytes, 0, intPtr, num);
		object result = Marshal.PtrToStructure(intPtr, type);
		Marshal.FreeHGlobal(intPtr);
		return result;
	}

	/// <summary>
	/// 获取SDK版本号
	/// </summary>
	/// <returns>返回4字节版本号</returns> 
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern uint MV_CODEREADER_GetSDKVersion();

	/// <summary>
	/// 创建设备句柄（支持虚拟相机） 
	/// </summary>
	/// <param name="handle">句柄 </param>
	/// <param name="pstDevInfo">设备信息结构体: MV_CODEREADER_DEVICE_INFO </param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码 </returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_CreateHandle(ref IntPtr handle, ref MV_CODEREADER_DEVICE_INFO pstDevInfo);

	/// <summary>
	/// 使用序列号创建设备句柄（支持虚拟相机） 
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <param name="chSerialNumber">设备序列号 </param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码 </returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_CreateHandleBySerialNumber(ref IntPtr handle, string chSerialNumber);

	/// <summary>
	/// 销毁设备句柄（支持虚拟相机） 
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码 </returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_DestroyHandle(IntPtr handle);

	/// <summary>
	/// 枚举设备（支持虚拟相机）
	/// </summary>
	/// <param name="pstDevList">设备列表结构体: MV_CODEREADER_DEVICE_INFO_LIST </param>
	/// <param name="nTLayerType">传输层类型，默认为GIGE设备，具体请参考宏定义（设备类型） </param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码 </returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_EnumDevices(ref MV_CODEREADER_DEVICE_INFO_LIST pstDevList, uint nTLayerType);

	/// <summary>
	/// 枚举指定系列设备（虚拟相机可枚举，但bSelectDevice不生效，不可指定系列设备） 
	/// </summary>
	/// <param name="pstDevList">设备列表结构体: MV_CODEREADER_DEVICE_INFO_LIST</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码 </returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_EnumCodeReader(ref MV_CODEREADER_DEVICE_INFO_LIST pstDevList);

	/// <summary>
	/// 枚举读码设备
	/// </summary>
	/// <param name="pstDevList">设备列表结构体: MV_CODEREADER_DEVICE_INFO_LIST</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_EnumIDDevices(ref MV_CODEREADER_DEVICE_INFO_LIST pstDevList);

	/// <summary>
	/// 设备是否可达（虚拟相机可调用，不支持检验设备是否可达） 
	/// </summary>
	/// <param name="pstDevInfo">设备信息结构体:MV_CODEREADER_DEVICE_INFO</param>
	/// <param name="nAccessMode">访问权限，具体请参考宏定义（设备访问模式）</param>
	/// <returns>可达，返回true；不可达，返回false</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern bool MV_CODEREADER_IsDeviceAccessible(ref MV_CODEREADER_DEVICE_INFO pstDevInfo, uint nAccessMode);

	/// <summary>
	/// 打开设备（支持虚拟相机） 
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_OpenDevice(IntPtr handle);

	/// <summary>
	/// 关闭设备（支持虚拟相机） 
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_CloseDevice(IntPtr handle);

	/// <summary>
	/// 开始取流（支持虚拟相机） 
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_StartGrabbing(IntPtr handle);

	/// <summary>
	/// 停止取流（支持虚拟相机） 
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_StopGrabbing(IntPtr handle);

	/// <summary>
	/// 采用超时机制获取一帧图片（图像信息以 MV_CODEREADER_IMAGE_OUT_INFO 结构体为准） 
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <param name="pData">图像数据</param>
	/// <param name="pstFrameInfo">图像信息</param>
	/// <param name="nMsec">等待超时时间，以ms为单位</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_GetOneFrameTimeout(IntPtr handle, ref IntPtr pData, IntPtr pstFrameInfo, uint nMsec);

	/// <summary>
	/// 采用超时机制获取一帧图片（图像信息以 MV_CODEREADER_IMAGE_OUT_INFO_EX 结构体为准） 
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <param name="pData">图像数据</param>
	/// <param name="pstFrameInfoEx">图像信息</param>
	/// <param name="nMsec">等待超时时间，以ms为单位</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_GetOneFrameTimeoutEx(IntPtr handle, ref IntPtr pData, IntPtr pstFrameInfoEx, uint nMsec);

	/// <summary>
	/// 采用超时机制获取一帧图片（图像信息以 MV_CODEREADER_IMAGE_OUT_INFO_EX2 结构体为准）
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <param name="pData">图像数据</param>
	/// <param name="pstFrameInfoEx2">图像信息</param>
	/// <param name="nMsec">等待超时时间，以ms为单位</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_GetOneFrameTimeoutEx2(IntPtr handle, ref IntPtr pData, IntPtr pstFrameInfoEx2, uint nMsec);

	/// <summary>
	/// 采用超时机制获取指定流通道一帧图片 
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <param name="pData">图像数据</param>
	/// <param name="pstFrameInfoEx2">图像信息（包含二维码质量信息） </param>
	/// <param name="nChannelID">流通道号（单通道固件通道号为0，多通道固件通道号为0/1/2，根据多通道sensor个数）</param>
	/// <param name="nMsec"> 等待超时时间，以ms为单位</param>
	/// <returns></returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_MSC_GetOneFrameTimeout(IntPtr handle, ref IntPtr pData, IntPtr pstFrameInfoEx2, uint nChannelID, uint nMsec);

	/// <summary>
	/// 获取设备信息（虚拟相机可调用，不支持返回设备信息，接口返回成功）
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <param name="pstDevInfo">设备信息结构体:MV_CODEREADER_DEVICE_INFO</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_GetDeviceInfo(IntPtr handle, ref MV_CODEREADER_DEVICE_INFO pstDevInfo);

	/// <summary>
	/// 获取Int型属性值 
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <param name="strKey">参数名称，如“Width”</param>
	/// <param name="pIntValue">获取到的设备参数值列表，含最大值、最小值及当前值:MV_CODEREADER_INTVALUE_EX</param>
	/// <returns></returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_GetIntValue(IntPtr handle, string strKey, ref MV_CODEREADER_INTVALUE_EX pIntValue);

	/// <summary>
	/// 设置Int型属性值 
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <param name="strKey">参数名称，如“Width”</param>
	/// <param name="nValue">参数值</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_SetIntValue(IntPtr handle, string strKey, long nValue);

	/// <summary>
	/// 获取Enum型属性值 
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <param name="strKey">参数名称，如“LineSelector”</param>
	/// <param name="pEnumValue">获取到的设备参数值列表，含当前值及有效数据个数:MV_CODEREADER_ENUMVALUE</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_GetEnumValue(IntPtr handle, string strKey, ref MV_CODEREADER_ENUMVALUE pEnumValue);

	/// <summary>
	/// 设置Enum型属性值（属性值类型为int值类型）
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <param name="strKey">参数名称，如“LineSelector”</param>
	/// <param name="nValue">参数值</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_SetEnumValue(IntPtr handle, string strKey, uint nValue);

	/// <summary>
	/// 设置Enum型属性值（属性值类型为字符串类型）
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <param name="strKey">参数名称，如“LineSelector”</param>
	/// <param name="sValue">参数值 </param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_SetEnumValueByString(IntPtr handle, string strKey, string sValue);

	/// <summary>
	/// 获取Float型属性值
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <param name="strKey">参数名称，如“AcquisitionFrameRate”</param>
	/// <param name="pFloatValue">获取到的参数值列表，含最大值、最小值及当前值:MV_CODEREADER_FLOATVALUE</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_GetFloatValue(IntPtr handle, string strKey, ref MV_CODEREADER_FLOATVALUE pFloatValue);

	/// <summary>
	/// 设置Float型属性值
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <param name="strKey">参数名称，如“AcquisitionFrameRate” </param>
	/// <param name="fValue">参数值</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_SetFloatValue(IntPtr handle, string strKey, float fValue);

	/// <summary>
	/// 获取Bool型属性值
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <param name="strKey">参数名称，如“EnableLight”</param>
	/// <param name="pBoolValue">获取到的参数值</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_GetBoolValue(IntPtr handle, string strKey, ref bool pBoolValue);

	/// <summary>
	/// 设置Bool型属性值 
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <param name="strKey">参数名称，如“HDREnable”</param>
	/// <param name="bValue">参数值</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_SetBoolValue(IntPtr handle, string strKey, bool bValue);

	/// <summary>
	/// 获取String型属性值
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <param name="strKey">参数名称，如“DeviceUserID”</param>
	/// <param name="pStringValue">获取到的参数值:MV_CODEREADER_STRINGVALUE</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_GetStringValue(IntPtr handle, string strKey, ref MV_CODEREADER_STRINGVALUE pStringValue);

	/// <summary>
	/// 设置String型属性值
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <param name="strKey">参数名称，如“DeviceUserID”</param>
	/// <param name="sValue">参数值</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_SetStringValue(IntPtr handle, string strKey, string sValue);

	/// <summary>
	/// 设置Command型属性值
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <param name="strKey">参数名称，如“DeviceReset”</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_SetCommandValue(IntPtr handle, string strKey);

	/// <summary>
	/// 获取最佳包大小
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_GetOptimalPacketSize(IntPtr handle);

	/// <summary>
	/// 读内存（虚拟相机不支持）
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <param name="pBuffer">读到的内存值</param>
	/// <param name="nAddress">待读取的内存地址(该地址可从设备的Camera.xml文件中获取，如xxx_RegAddr的xml节点值。Camera.xml文件会在设备打开之后自动生成在应用程序的当前目录下）</param>
	/// <param name="nLength">待读取的内存长度</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_ReadMemory(IntPtr handle, IntPtr pBuffer, long nAddress, long nLength);

	/// <summary>
	/// 写内存（虚拟相机不支持）
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <param name="pBuffer">待写入的内存值 </param>
	/// <param name="nAddress">待写入的内存地址(该地址可从设备的Camera.xml文件中获取，如xxx_RegAddr的xml节点值。Camera.xml文件会在设备打开之后自动生成在应用程序的当前目录下）</param>
	/// <param name="nLength">待写入的内存长度 </param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_WriteMemory(IntPtr handle, IntPtr pBuffer, long nAddress, long nLength);

	/// <summary>
	/// 强制设置IP
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <param name="nIP">设置的IP</param>
	/// <param name="nSubNetMask">子网掩码</param>
	/// <param name="nDefaultGateWay">默认网关</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_GIGE_ForceIp(IntPtr handle, uint nIP, uint nSubNetMask, uint nDefaultGateWay);

	/// <summary>
	/// 设置设备IP配置类型
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <param name="nType"> IP配置类型</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_GIGE_SetIpConfig(IntPtr handle, uint nType);

	/// <summary>
	/// 从读码器读取文件
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <param name="pstFileAccess">文件存取:MV_CODEREADER_FILE_ACCESS</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_FileAccessRead(IntPtr handle, ref MV_CODEREADER_FILE_ACCESS pstFileAccess);

	/// <summary>
	/// 将文件写入读码器 
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <param name="pstFileAccess">文件存取:MV_CODEREADER_FILE_ACCESS</param>
	/// <returns></returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_FileAccessWrite(IntPtr handle, ref MV_CODEREADER_FILE_ACCESS pstFileAccess);

	/// <summary>
	/// 获取文件存取进度
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <param name="pstFileAccessProgress">文件存取进度</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_GetFileAccessProgress(IntPtr handle, ref MV_CODEREADER_FILE_ACCESS_PROGRESS pstFileAccessProgress);

	/// <summary>
	/// 设置抠图使能
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <param name="bWaybillEnable">抠图使能</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_SetWayBillEnable(IntPtr handle, bool bWaybillEnable);

	/// <summary>
	/// 获取当前输入图像的面单信息（输入图像对应 MV_CODEREADER_IMAGE_OUT_INFO_EX 该结构体信息）
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <param name="pData">原始图像指针 </param>
	/// <param name="pstFrameInfoEx">图像信息</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_GetWayBillInfo(IntPtr handle, IntPtr pData, IntPtr pstFrameInfoEx);

	/// <summary>
	/// 获取当前输入图像的面单信息（输入图像对应 MV_CODEREADER_IMAGE_OUT_INFO_EX2 该结构体信息）
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <param name="pData">原始图像指针 </param>
	/// <param name="pstFrameInfoEx2">图像信息</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_GetWayBillInfoEx(IntPtr handle, IntPtr pData, IntPtr pstFrameInfoEx2);

	/// <summary>
	/// 设置算法整型参数 
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <param name="strParamKeyName">参数名 </param>
	/// <param name="nValue">参数值</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_Algorithm_SetIntValue(IntPtr handle, string strParamKeyName, int nValue);

	/// <summary>
	/// 获取算法整型参数
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <param name="strParamKeyName">参数名</param>
	/// <param name="pnValue">参数值</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_Algorithm_GetIntValue(IntPtr handle, string strParamKeyName, ref int pnValue);

	/// <summary>
	/// 保存图像
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <param name="pSaveParam">图像参数结构体:MV_CODEREADER_SAVE_IMAGE_PARAM_EX</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_SaveImage(IntPtr handle, ref MV_CODEREADER_SAVE_IMAGE_PARAM_EX pSaveParam);

	/// <summary>
	/// 获取设备异常信息（虚拟相机返回OK，但实际不支持，无效果）
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <param name="cbException">用户注册异常回调函数</param>
	/// <param name="pUser">用户指针</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_RegisterExceptionCallBack(IntPtr handle, cbExceptiondelegate cbException, IntPtr pUser);

	/// <summary>
	/// 注册全部事件回调，在打开设备之后调用（虚拟相机不支持）
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <param name="cbAllEvent">用户注册事件回调函数</param>
	/// <param name="pUser">用户指针</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_RegisterAllEventCallBack(IntPtr handle, cbAllEventdelegate cbAllEvent, IntPtr pUser);

	/// <summary>
	/// 注册图像数据回调（图像信息以 MV_CODEREADER_IMAGE_OUT_INFO 结构体为准） 
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <param name="cbOutput">用户注册图像回调函数 </param>
	/// <param name="pUser">用户指针</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_RegisterImageCallBack(IntPtr handle, cbOutputdelegate cbOutput, IntPtr pUser);

	/// <summary>
	/// 注册图像扩展数据回调（图像信息以 MV_CODEREADER_IMAGE_OUT_INFO_EX 结构体为准） 
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <param name="cbOutput">用户注册图像回调函数</param>
	/// <param name="pUser">用户指针</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_RegisterImageCallBackEx(IntPtr handle, cbOutputExdelegate cbOutput, IntPtr pUser);

	/// <summary>
	/// 注册图像扩展数据回调（图像信息以 MV_CODEREADER_IMAGE_OUT_INFO_EX2 结构体为准） 
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <param name="cbOutput"> 用户注册图像（图像信息包含二维码质量评级）回调函数</param>
	/// <param name="pUser">用户指针</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_RegisterImageCallBackEx2(IntPtr handle, cbOutputEx2delegate cbOutput, IntPtr pUser);

	/// <summary>
	/// 注册指定一路流通道图像数据回调 
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <param name="nChannelID">流通道号（单通道固件通道号位0，多通道固件通道号为0/1/2，根据多通道sensor个数）</param>
	/// <param name="cbOutput">用户注册图像（图像信息包含二维码质量评级）回调函数</param>
	/// <param name="pUser">用户指针</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_MSC_RegisterImageCallBack(IntPtr handle, uint nChannelID, cbMSCOutputdelegate cbOutput, IntPtr pUser);

	/// <summary>
	/// 注册触发模式回调函数
	/// </summary>
	/// <param name="handle">句柄</param>
	/// <param name="cbTrigger">用户注册触发模式回调函数</param>
	/// <param name="pUser">用户指针</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_RegisterTriggerCallBack(IntPtr handle, cbTriggerdelegate cbTrigger, IntPtr pUser);

	/// <summary>
	/// 设置GIGE超时时间
	/// </summary>
	/// <param name="handle">设备句柄</param>
	/// <param name="nMillisec">超时时间(ms)</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_GIGE_SetGvcpTimeout(IntPtr handle, uint nMillisec);

	/// <summary>
	/// 获取GIGE超时时间
	/// </summary>
	/// <param name="handle">设备句柄</param>
	/// <param name="pMillisec">超时时间(ms)</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_GIGE_GetGvcpTimeout(IntPtr handle, IntPtr pMillisec);

	/// <summary>
	/// 设置帧数据缓存节点数量
	/// </summary>
	/// <param name="handle">设备句柄</param>
	/// <param name="nMillisec">缓存节点数量，范围1-12，默认3</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll")]
	private static extern int MV_CODEREADER_SetFrameNodeNum(IntPtr handle, uint nNodeNum);

	/// <summary>
	/// 清除GenICam节点缓存
	/// </summary>
	/// <param name="handle">设备句柄</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll", EntryPoint = " MV_CODEREADER_InvalidateNodes")]
	private static extern int MV_CODEREADER_InvalidateNodes(IntPtr handle);

	/// <summary>
	/// 配置区域地址和值（读码相机专用）
	/// </summary>
	/// <param name="handle">设备句柄</param>
	/// <param name="nAreaAddress">区域码地址（0xa80）</param>
	/// <param name="nAreaCode">地区码（国内：0x10000  ，国外：0x20000）</param>
	/// <returns>成功，返回MV_CODEREADER_OK；错误，返回错误码</returns>
	[DllImport("MvCodeReaderCtrl.dll", EntryPoint = " MV_CODEREADER_SetAreaInfoConfig")]
	private static extern int MV_CODEREADER_SetAreaInfoConfig(IntPtr handle, uint nAreaAddress, uint nAreaCode);
}
