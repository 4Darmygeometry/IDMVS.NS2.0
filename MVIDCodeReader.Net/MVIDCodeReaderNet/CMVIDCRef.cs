using System;
using System.Runtime.InteropServices;

namespace MVIDCodeReaderNet;

internal class CMVIDCRef
{
	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_GetVersion(IntPtr pstrVersion);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_CreateHandle(ref IntPtr handle, uint nCodeAbility);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_DestroyHandle(IntPtr handle);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_Algorithm_SetIntValue(IntPtr handle, string strParamKeyName, int nValue);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_Algorithm_GetIntValue(IntPtr handle, string strParamKeyName, ref int pnValue);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_Algorithm_SetFloatValue(IntPtr handle, string strParamKeyName, float fValue);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_Algorithm_GetFloatValue(IntPtr handle, string strParamKeyName, ref float pfValue);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_Algorithm_SetStringValue(IntPtr handle, string strParamKeyName, string strValue);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_Algorithm_GetStringValue(IntPtr handle, string strParamKeyName, ref byte strValue);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_Process(IntPtr handle, IntPtr pstParam, uint nCodeAbility);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_CAM_EnumDevices(ref MVIDCodeReader.MVID_CAMERA_INFO_LIST pstCamList);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_CAM_EnumDevicesByCfg(ref MVIDCodeReader.MVID_CAMERA_INFO_LIST pstCamList);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_CAM_BindDevice(IntPtr handle, IntPtr pstCamInfo);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_CAM_BindDeviceByIP(IntPtr handle, string chCurrentIp, string chNetExport);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_CAM_BindDeviceBySerialNumber(IntPtr handle, string chSerialNumber);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_CAM_RegisterImageCallBack(IntPtr handle, MVIDCodeReader.cbOutputdelegate cbOutput, IntPtr pUser);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_CAM_RegisterImageBufferCallBack(IntPtr handle, MVIDCodeReader.cbImageBufferdelegate cbOutput, IntPtr pUser);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_CAM_RegisterAllEventCallBack(IntPtr handle, MVIDCodeReader.cbEventdelegate cbEvent, IntPtr pUser);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_CAM_StartGrabbing(IntPtr handle);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_CAM_StopGrabbing(IntPtr handle);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_CAM_GetOneFrameTimeout(IntPtr handle, IntPtr pFrameInfo, uint nMsec);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_CAM_GetImageBuffer(IntPtr handle, ref MVIDCodeReader.MVID_IMAGE_INFO pFrameInfo, uint nMsec);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_CAM_SetIntValue(IntPtr handle, string strKey, long nValue);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_CAM_GetIntValue(IntPtr handle, string strKey, ref MVIDCodeReader.MVID_CAM_INTVALUE_EX pIntValue);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_CAM_SetEnumValue(IntPtr handle, string strKey, uint nValue);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_CAM_SetEnumValueByString(IntPtr handle, string strKey, string sValue);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_CAM_GetEnumValue(IntPtr handle, string strKey, ref MVIDCodeReader.MVID_CAM_ENUMVALUE pEnumValue);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_CAM_SetFloatValue(IntPtr handle, string strKey, float fValue);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_CAM_GetFloatValue(IntPtr handle, string strKey, ref MVIDCodeReader.MVID_CAM_FLOATVALUE pFloatValue);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_CAM_SetStringValue(IntPtr handle, string strKey, string sValue);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_CAM_GetStringValue(IntPtr handle, string strKey, ref MVIDCodeReader.MVID_CAM_STRINGVALUE pStringValue);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_CAM_SetBoolValue(IntPtr handle, string strKey, bool bValue);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_CAM_GetBoolValue(IntPtr handle, string strKey, ref bool pBoolValue);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_CAM_SetCommandValue(IntPtr handle, string strKey);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_CAM_SetImageNodeNum(IntPtr handle, uint nNum);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_CAM_SetImageOutPutMode(IntPtr handle, MVIDCodeReader.MVID_IMAGE_OUTPUT_MODE enImageOutPutMode);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_CAM_RegisterPreImageCallBack(IntPtr handle, MVIDCodeReader.cbPreOutputdelegate cbPreOutput, IntPtr pUser);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_RegisterExceptionCallBack(IntPtr handle, MVIDCodeReader.cbExceptiondelegate cbException, IntPtr pUser);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_SaveImage(IntPtr handle, ref MVIDCodeReader.MVID_IMAGE_INFO pstInputImage, MVIDCodeReader.MVID_IMAGE_TYPE enImageType, ref MVIDCodeReader.MVID_IMAGE_INFO pstOutputImage, uint nJpgQuality);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_GetImageFileData(IntPtr handle, string pFilePath, IntPtr pstImageParam);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_RuleLoad(IntPtr handle, string pFileName);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_ScriptLoad(IntPtr handle, string pFilePath, string pFuncName);

	[DllImport("MVIDCodeReader.dll")]
	public static extern int MVID_CR_CAM_GetAllMatchInfo(IntPtr handle, ref MVIDCodeReader.MVID_ALL_MATCH_INFO pstInfo);
}
