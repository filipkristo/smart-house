
/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public partial class YamahaBasicStatus
{

	private YAMAHA_AVMain_Zone main_ZoneField;

	private string rspField;

	private byte rcField;

	/// <remarks/>
	public YAMAHA_AVMain_Zone Main_Zone
	{
		get
		{
			return this.main_ZoneField;
		}
		set
		{
			this.main_ZoneField = value;
		}
	}

	/// <remarks/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public string rsp
	{
		get
		{
			return this.rspField;
		}
		set
		{
			this.rspField = value;
		}
	}

	/// <remarks/>
	[System.Xml.Serialization.XmlAttributeAttribute()]
	public byte RC
	{
		get
		{
			return this.rcField;
		}
		set
		{
			this.rcField = value;
		}
	}
}

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class YAMAHA_AVMain_Zone
{

	private YAMAHA_AVMain_ZoneBasic_Status basic_StatusField;

	/// <remarks/>
	public YAMAHA_AVMain_ZoneBasic_Status Basic_Status
	{
		get
		{
			return this.basic_StatusField;
		}
		set
		{
			this.basic_StatusField = value;
		}
	}
}

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class YAMAHA_AVMain_ZoneBasic_Status
{

	private YAMAHA_AVMain_ZoneBasic_StatusPower_Control power_ControlField;

	private YAMAHA_AVMain_ZoneBasic_StatusVolume volumeField;

	private YAMAHA_AVMain_ZoneBasic_StatusInput inputField;

	private YAMAHA_AVMain_ZoneBasic_StatusSurround surroundField;

	private YAMAHA_AVMain_ZoneBasic_StatusSound_Video sound_VideoField;

	private YAMAHA_AVMain_ZoneBasic_StatusSpeaker_Preout speaker_PreoutField;

	/// <remarks/>
	public YAMAHA_AVMain_ZoneBasic_StatusPower_Control Power_Control
	{
		get
		{
			return this.power_ControlField;
		}
		set
		{
			this.power_ControlField = value;
		}
	}

	/// <remarks/>
	public YAMAHA_AVMain_ZoneBasic_StatusVolume Volume
	{
		get
		{
			return this.volumeField;
		}
		set
		{
			this.volumeField = value;
		}
	}

	/// <remarks/>
	public YAMAHA_AVMain_ZoneBasic_StatusInput Input
	{
		get
		{
			return this.inputField;
		}
		set
		{
			this.inputField = value;
		}
	}

	/// <remarks/>
	public YAMAHA_AVMain_ZoneBasic_StatusSurround Surround
	{
		get
		{
			return this.surroundField;
		}
		set
		{
			this.surroundField = value;
		}
	}

	/// <remarks/>
	public YAMAHA_AVMain_ZoneBasic_StatusSound_Video Sound_Video
	{
		get
		{
			return this.sound_VideoField;
		}
		set
		{
			this.sound_VideoField = value;
		}
	}

	/// <remarks/>
	public YAMAHA_AVMain_ZoneBasic_StatusSpeaker_Preout Speaker_Preout
	{
		get
		{
			return this.speaker_PreoutField;
		}
		set
		{
			this.speaker_PreoutField = value;
		}
	}
}

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class YAMAHA_AVMain_ZoneBasic_StatusPower_Control
{

	private string powerField;

	private string zone_B_Power_InfoField;

	private string sleepField;

	/// <remarks/>
	public string Power
	{
		get
		{
			return this.powerField;
		}
		set
		{
			this.powerField = value;
		}
	}

	/// <remarks/>
	public string Zone_B_Power_Info
	{
		get
		{
			return this.zone_B_Power_InfoField;
		}
		set
		{
			this.zone_B_Power_InfoField = value;
		}
	}

	/// <remarks/>
	public string Sleep
	{
		get
		{
			return this.sleepField;
		}
		set
		{
			this.sleepField = value;
		}
	}
}

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class YAMAHA_AVMain_ZoneBasic_StatusVolume
{

	private YAMAHA_AVMain_ZoneBasic_StatusVolumeLvl lvlField;

	private string muteField;

	private YAMAHA_AVMain_ZoneBasic_StatusVolumeSubwoofer_Trim subwoofer_TrimField;

	private string scaleField;

	private YAMAHA_AVMain_ZoneBasic_StatusVolumeZone_B zone_BField;

	/// <remarks/>
	public YAMAHA_AVMain_ZoneBasic_StatusVolumeLvl Lvl
	{
		get
		{
			return this.lvlField;
		}
		set
		{
			this.lvlField = value;
		}
	}

	/// <remarks/>
	public string Mute
	{
		get
		{
			return this.muteField;
		}
		set
		{
			this.muteField = value;
		}
	}

	/// <remarks/>
	public YAMAHA_AVMain_ZoneBasic_StatusVolumeSubwoofer_Trim Subwoofer_Trim
	{
		get
		{
			return this.subwoofer_TrimField;
		}
		set
		{
			this.subwoofer_TrimField = value;
		}
	}

	/// <remarks/>
	public string Scale
	{
		get
		{
			return this.scaleField;
		}
		set
		{
			this.scaleField = value;
		}
	}

	/// <remarks/>
	public YAMAHA_AVMain_ZoneBasic_StatusVolumeZone_B Zone_B
	{
		get
		{
			return this.zone_BField;
		}
		set
		{
			this.zone_BField = value;
		}
	}
}

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class YAMAHA_AVMain_ZoneBasic_StatusVolumeLvl
{

	private short valField;

	private byte expField;

	private string unitField;

	/// <remarks/>
	public short Val
	{
		get
		{
			return this.valField;
		}
		set
		{
			this.valField = value;
		}
	}

	/// <remarks/>
	public byte Exp
	{
		get
		{
			return this.expField;
		}
		set
		{
			this.expField = value;
		}
	}

	/// <remarks/>
	public string Unit
	{
		get
		{
			return this.unitField;
		}
		set
		{
			this.unitField = value;
		}
	}
}

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class YAMAHA_AVMain_ZoneBasic_StatusVolumeSubwoofer_Trim
{

	private byte valField;

	private byte expField;

	private string unitField;

	/// <remarks/>
	public byte Val
	{
		get
		{
			return this.valField;
		}
		set
		{
			this.valField = value;
		}
	}

	/// <remarks/>
	public byte Exp
	{
		get
		{
			return this.expField;
		}
		set
		{
			this.expField = value;
		}
	}

	/// <remarks/>
	public string Unit
	{
		get
		{
			return this.unitField;
		}
		set
		{
			this.unitField = value;
		}
	}
}

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class YAMAHA_AVMain_ZoneBasic_StatusVolumeZone_B
{

	private string feature_AvailabilityField;

	private string interlockField;

	private YAMAHA_AVMain_ZoneBasic_StatusVolumeZone_BLvl lvlField;

	private string muteField;

	/// <remarks/>
	public string Feature_Availability
	{
		get
		{
			return this.feature_AvailabilityField;
		}
		set
		{
			this.feature_AvailabilityField = value;
		}
	}

	/// <remarks/>
	public string Interlock
	{
		get
		{
			return this.interlockField;
		}
		set
		{
			this.interlockField = value;
		}
	}

	/// <remarks/>
	public YAMAHA_AVMain_ZoneBasic_StatusVolumeZone_BLvl Lvl
	{
		get
		{
			return this.lvlField;
		}
		set
		{
			this.lvlField = value;
		}
	}

	/// <remarks/>
	public string Mute
	{
		get
		{
			return this.muteField;
		}
		set
		{
			this.muteField = value;
		}
	}
}

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class YAMAHA_AVMain_ZoneBasic_StatusVolumeZone_BLvl
{

	private short valField;

	private byte expField;

	private string unitField;

	/// <remarks/>
	public short Val
	{
		get
		{
			return this.valField;
		}
		set
		{
			this.valField = value;
		}
	}

	/// <remarks/>
	public byte Exp
	{
		get
		{
			return this.expField;
		}
		set
		{
			this.expField = value;
		}
	}

	/// <remarks/>
	public string Unit
	{
		get
		{
			return this.unitField;
		}
		set
		{
			this.unitField = value;
		}
	}
}

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class YAMAHA_AVMain_ZoneBasic_StatusInput
{

	private string input_SelField;

	private YAMAHA_AVMain_ZoneBasic_StatusInputInput_Sel_Item_Info input_Sel_Item_InfoField;

	/// <remarks/>
	public string Input_Sel
	{
		get
		{
			return this.input_SelField;
		}
		set
		{
			this.input_SelField = value;
		}
	}

	/// <remarks/>
	public YAMAHA_AVMain_ZoneBasic_StatusInputInput_Sel_Item_Info Input_Sel_Item_Info
	{
		get
		{
			return this.input_Sel_Item_InfoField;
		}
		set
		{
			this.input_Sel_Item_InfoField = value;
		}
	}
}

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class YAMAHA_AVMain_ZoneBasic_StatusInputInput_Sel_Item_Info
{

	private string paramField;

	private string rwField;

	private string titleField;

	private YAMAHA_AVMain_ZoneBasic_StatusInputInput_Sel_Item_InfoIcon iconField;

	private object src_NameField;

	private byte src_NumberField;

	/// <remarks/>
	public string Param
	{
		get
		{
			return this.paramField;
		}
		set
		{
			this.paramField = value;
		}
	}

	/// <remarks/>
	public string RW
	{
		get
		{
			return this.rwField;
		}
		set
		{
			this.rwField = value;
		}
	}

	/// <remarks/>
	public string Title
	{
		get
		{
			return this.titleField;
		}
		set
		{
			this.titleField = value;
		}
	}

	/// <remarks/>
	public YAMAHA_AVMain_ZoneBasic_StatusInputInput_Sel_Item_InfoIcon Icon
	{
		get
		{
			return this.iconField;
		}
		set
		{
			this.iconField = value;
		}
	}

	/// <remarks/>
	public object Src_Name
	{
		get
		{
			return this.src_NameField;
		}
		set
		{
			this.src_NameField = value;
		}
	}

	/// <remarks/>
	public byte Src_Number
	{
		get
		{
			return this.src_NumberField;
		}
		set
		{
			this.src_NumberField = value;
		}
	}
}

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class YAMAHA_AVMain_ZoneBasic_StatusInputInput_Sel_Item_InfoIcon
{

	private string onField;

	private object offField;

	/// <remarks/>
	public string On
	{
		get
		{
			return this.onField;
		}
		set
		{
			this.onField = value;
		}
	}

	/// <remarks/>
	public object Off
	{
		get
		{
			return this.offField;
		}
		set
		{
			this.offField = value;
		}
	}
}

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class YAMAHA_AVMain_ZoneBasic_StatusSurround
{

	private YAMAHA_AVMain_ZoneBasic_StatusSurroundProgram_Sel program_SelField;

	private string _3D_Cinema_DSPField;

	/// <remarks/>
	public YAMAHA_AVMain_ZoneBasic_StatusSurroundProgram_Sel Program_Sel
	{
		get
		{
			return this.program_SelField;
		}
		set
		{
			this.program_SelField = value;
		}
	}

	/// <remarks/>
	public string _3D_Cinema_DSP
	{
		get
		{
			return this._3D_Cinema_DSPField;
		}
		set
		{
			this._3D_Cinema_DSPField = value;
		}
	}
}

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class YAMAHA_AVMain_ZoneBasic_StatusSurroundProgram_Sel
{

	private YAMAHA_AVMain_ZoneBasic_StatusSurroundProgram_SelCurrent currentField;

	/// <remarks/>
	public YAMAHA_AVMain_ZoneBasic_StatusSurroundProgram_SelCurrent Current
	{
		get
		{
			return this.currentField;
		}
		set
		{
			this.currentField = value;
		}
	}
}

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class YAMAHA_AVMain_ZoneBasic_StatusSurroundProgram_SelCurrent
{

	private string straightField;

	private string enhancerField;

	private string sound_ProgramField;

	/// <remarks/>
	public string Straight
	{
		get
		{
			return this.straightField;
		}
		set
		{
			this.straightField = value;
		}
	}

	/// <remarks/>
	public string Enhancer
	{
		get
		{
			return this.enhancerField;
		}
		set
		{
			this.enhancerField = value;
		}
	}

	/// <remarks/>
	public string Sound_Program
	{
		get
		{
			return this.sound_ProgramField;
		}
		set
		{
			this.sound_ProgramField = value;
		}
	}
}

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class YAMAHA_AVMain_ZoneBasic_StatusSound_Video
{

	private YAMAHA_AVMain_ZoneBasic_StatusSound_VideoTone toneField;

	private YAMAHA_AVMain_ZoneBasic_StatusSound_VideoDirect directField;

	private YAMAHA_AVMain_ZoneBasic_StatusSound_VideoHDMI hDMIField;

	private string extra_BassField;

	private string adaptive_DRCField;

	private YAMAHA_AVMain_ZoneBasic_StatusSound_VideoDialogue_Adjust dialogue_AdjustField;

	/// <remarks/>
	public YAMAHA_AVMain_ZoneBasic_StatusSound_VideoTone Tone
	{
		get
		{
			return this.toneField;
		}
		set
		{
			this.toneField = value;
		}
	}

	/// <remarks/>
	public YAMAHA_AVMain_ZoneBasic_StatusSound_VideoDirect Direct
	{
		get
		{
			return this.directField;
		}
		set
		{
			this.directField = value;
		}
	}

	/// <remarks/>
	public YAMAHA_AVMain_ZoneBasic_StatusSound_VideoHDMI HDMI
	{
		get
		{
			return this.hDMIField;
		}
		set
		{
			this.hDMIField = value;
		}
	}

	/// <remarks/>
	public string Extra_Bass
	{
		get
		{
			return this.extra_BassField;
		}
		set
		{
			this.extra_BassField = value;
		}
	}

	/// <remarks/>
	public string Adaptive_DRC
	{
		get
		{
			return this.adaptive_DRCField;
		}
		set
		{
			this.adaptive_DRCField = value;
		}
	}

	/// <remarks/>
	public YAMAHA_AVMain_ZoneBasic_StatusSound_VideoDialogue_Adjust Dialogue_Adjust
	{
		get
		{
			return this.dialogue_AdjustField;
		}
		set
		{
			this.dialogue_AdjustField = value;
		}
	}
}

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class YAMAHA_AVMain_ZoneBasic_StatusSound_VideoTone
{

	private YAMAHA_AVMain_ZoneBasic_StatusSound_VideoToneBass bassField;

	private YAMAHA_AVMain_ZoneBasic_StatusSound_VideoToneTreble trebleField;

	/// <remarks/>
	public YAMAHA_AVMain_ZoneBasic_StatusSound_VideoToneBass Bass
	{
		get
		{
			return this.bassField;
		}
		set
		{
			this.bassField = value;
		}
	}

	/// <remarks/>
	public YAMAHA_AVMain_ZoneBasic_StatusSound_VideoToneTreble Treble
	{
		get
		{
			return this.trebleField;
		}
		set
		{
			this.trebleField = value;
		}
	}
}

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class YAMAHA_AVMain_ZoneBasic_StatusSound_VideoToneBass
{

	private byte valField;

	private byte expField;

	private string unitField;

	/// <remarks/>
	public byte Val
	{
		get
		{
			return this.valField;
		}
		set
		{
			this.valField = value;
		}
	}

	/// <remarks/>
	public byte Exp
	{
		get
		{
			return this.expField;
		}
		set
		{
			this.expField = value;
		}
	}

	/// <remarks/>
	public string Unit
	{
		get
		{
			return this.unitField;
		}
		set
		{
			this.unitField = value;
		}
	}
}

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class YAMAHA_AVMain_ZoneBasic_StatusSound_VideoToneTreble
{

	private byte valField;

	private byte expField;

	private string unitField;

	/// <remarks/>
	public byte Val
	{
		get
		{
			return this.valField;
		}
		set
		{
			this.valField = value;
		}
	}

	/// <remarks/>
	public byte Exp
	{
		get
		{
			return this.expField;
		}
		set
		{
			this.expField = value;
		}
	}

	/// <remarks/>
	public string Unit
	{
		get
		{
			return this.unitField;
		}
		set
		{
			this.unitField = value;
		}
	}
}

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class YAMAHA_AVMain_ZoneBasic_StatusSound_VideoDirect
{

	private string modeField;

	/// <remarks/>
	public string Mode
	{
		get
		{
			return this.modeField;
		}
		set
		{
			this.modeField = value;
		}
	}
}

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class YAMAHA_AVMain_ZoneBasic_StatusSound_VideoHDMI
{

	private string standby_Through_InfoField;

	private YAMAHA_AVMain_ZoneBasic_StatusSound_VideoHDMIOutput outputField;

	/// <remarks/>
	public string Standby_Through_Info
	{
		get
		{
			return this.standby_Through_InfoField;
		}
		set
		{
			this.standby_Through_InfoField = value;
		}
	}

	/// <remarks/>
	public YAMAHA_AVMain_ZoneBasic_StatusSound_VideoHDMIOutput Output
	{
		get
		{
			return this.outputField;
		}
		set
		{
			this.outputField = value;
		}
	}
}

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class YAMAHA_AVMain_ZoneBasic_StatusSound_VideoHDMIOutput
{

	private string oUT_1Field;

	/// <remarks/>
	public string OUT_1
	{
		get
		{
			return this.oUT_1Field;
		}
		set
		{
			this.oUT_1Field = value;
		}
	}
}

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class YAMAHA_AVMain_ZoneBasic_StatusSound_VideoDialogue_Adjust
{

	private byte dialogue_LvlField;

	/// <remarks/>
	public byte Dialogue_Lvl
	{
		get
		{
			return this.dialogue_LvlField;
		}
		set
		{
			this.dialogue_LvlField = value;
		}
	}
}

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class YAMAHA_AVMain_ZoneBasic_StatusSpeaker_Preout
{

	private YAMAHA_AVMain_ZoneBasic_StatusSpeaker_PreoutSpeaker_AB speaker_ABField;

	/// <remarks/>
	public YAMAHA_AVMain_ZoneBasic_StatusSpeaker_PreoutSpeaker_AB Speaker_AB
	{
		get
		{
			return this.speaker_ABField;
		}
		set
		{
			this.speaker_ABField = value;
		}
	}
}

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class YAMAHA_AVMain_ZoneBasic_StatusSpeaker_PreoutSpeaker_AB
{

	private string speaker_AField;

	private string speaker_BField;

	/// <remarks/>
	public string Speaker_A
	{
		get
		{
			return this.speaker_AField;
		}
		set
		{
			this.speaker_AField = value;
		}
	}

	/// <remarks/>
	public string Speaker_B
	{
		get
		{
			return this.speaker_BField;
		}
		set
		{
			this.speaker_BField = value;
		}
	}
}

